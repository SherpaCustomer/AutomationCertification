namespace Skyline.DataMiner.Library.Protocol.Rates
{
	using Skyline.DataMiner.Scripting;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// This class is used to load the parameter values into objects or set the object results back to the parameters.
	/// </summary>
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
	public class RateCalculator
	{
		private readonly ValuesToRatesColumns inputColumns;
		private readonly ValuesToRatesColumns outputColumns;
		private readonly SpeedType speedType;
		private readonly Common.Rates.DataConversionType dataConversionType;
		private readonly int tablePid;
		private readonly int bufferedDeltaPid;
		private readonly HashSet<string> discontinuityTimes;
		private readonly Dictionary<string, string> discontinuityTimeValues;
		private Common.Rates.InterfaceTable interfaceTable;
		private bool isAgentRestarted;
		private bool isBufferedDeltaPidRead;
		private int bufferedDeltaValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="RateCalculator"/> class. All used parameter IDs and column indexes are passed in this constructor.
		/// </summary>
		/// <param name="tablePid">Parameter ID of the table to interact with.</param>
		/// <param name="bufferedDeltaPid">Parameter ID where the buffered delta value can be found.</param>
		/// <param name="speedType">Indicates whether these are 32 bit counters (Low Speed) or 64 bit counters (High Speed) are used.</param>
		/// <param name="dataConversionType">Indicates if the rate calculation should be multiplied with eight (OctetsToBits) or not (NoConversion).</param>
		/// <param name="inputColumns">Collection with all used input parameter columns.</param>
		/// <param name="outputColumns">Collection with all used output parameter columns.</param>
		public RateCalculator(int tablePid, int bufferedDeltaPid, SpeedType speedType, DataConversionType dataConversionType, ValuesToRatesColumns inputColumns, ValuesToRatesColumns outputColumns)
		{
			this.tablePid = tablePid;
			this.inputColumns = inputColumns;
			this.outputColumns = outputColumns;
			this.speedType = speedType;
			this.dataConversionType = (Common.Rates.DataConversionType)(int)dataConversionType;
			this.bufferedDeltaPid = bufferedDeltaPid;
			Utilization = null;
			Discontinuity = null;
			isAgentRestarted = false;
			isBufferedDeltaPidRead = false;
			bufferedDeltaValue = 0;
			CurrentSysUptimePid = 0;
			PreviousSysUptimePid = 0;
			discontinuityTimes = new HashSet<string>();
			discontinuityTimeValues = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets a boolean indicating if the SNMP Agent of the device has been restarted since the previous poll cycle. The value is only valid if the CurrentSysUptimePid and PreviousSysUptimePid have been filled in and CalculateAndSetTable method has been executed.
		/// </summary>
		public bool IsAgentRestarted
		{
			get
			{
				return isAgentRestarted;
			}
		}

		/// <summary>
		/// Gets or sets the parameter information of the utilization column.
		/// </summary>
		public UtilizationColumns Utilization
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parameter information of the discontinuity time column.
		/// </summary>
		public ValueColumns Discontinuity
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parameter ID of the polled SysUptime.
		/// </summary>
		public uint CurrentSysUptimePid
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parameter ID where the previous SysUptime value is stored.
		/// </summary>
		public uint PreviousSysUptimePid
		{
			get;
			set;
		}

		/// <summary>
		/// This method gets called when a timeout occurred.
		/// </summary>
		/// <param name="protocol">Link with SLProtocol.</param>
		/// <param name="pidBufferedDelta">Parameter ID of the BufferedDelta.</param>
		/// <param name="groupId">The ID of the SNMP group.</param>
		public static void TimeoutOrError(SLProtocol protocol, int pidBufferedDelta, int groupId)
		{
			TimeSpan? lastDelta = GetSnmpGroupExecutionDelta(protocol, groupId);
			if (lastDelta != null)
			{
				double lastDeltaInMs = lastDelta.Value.TotalMilliseconds;

				double bufferedDelta = Convert.ToDouble(protocol.GetParameter(pidBufferedDelta));
				double totalDelta = bufferedDelta + lastDeltaInMs;

				protocol.SetParameter(pidBufferedDelta, totalDelta);
			}
		}

		/// <summary>
		/// Retrieves the delta between two consecutive executions of the specified SNMP group.
		/// </summary>
		/// <param name="protocol">SLProtocol instance.</param>
		/// <param name="groupId">The ID of the SNMP group.</param>
		/// <exception cref="ArgumentException">The group ID is negative.</exception>
		/// <returns>The delta between two consecutive executions of the group.</returns>
		public static TimeSpan? GetSnmpGroupExecutionDelta(SLProtocol protocol, int groupId)
		{
			if (groupId < 0)
			{
				throw new ArgumentException("The group ID must not be negative.", "groupId");
			}

			int result = Convert.ToInt32(protocol.NotifyProtocol(269 /*NT_GET_BITRATE_DELTA*/, groupId, null));

			if (result != -1)
			{
				return TimeSpan.FromMilliseconds(result);
			}

			return null;
		}

		/// <summary>
		/// This method will read out the current table values, perform the calculations, and sets the result in the table.
		/// </summary>
		/// <param name="protocol">Link with SLProtocol.</param>
		/// <param name="delta">Time span between this and previous executed poll group.</param>
		/// <param name="minDelta">The minimum value <paramref name="delta" /> must have (in ms).</param>
		public void CalculateAndSetTable(SLProtocol protocol, TimeSpan? delta, int minDelta)
		{
			// Gets
			GetAgentRestarted(protocol, delta);

			if (!isBufferedDeltaPidRead)
			{
				bufferedDeltaValue = Convert.ToInt32(protocol.GetParameter(bufferedDeltaPid));
			}

			GetData(protocol, delta, minDelta, Utilization != null, Discontinuity != null);

			// Calculate
			Common.Rates.RateCalculator.CalculateRate(interfaceTable, isAgentRestarted, discontinuityTimes);
			if (Utilization != null)
			{
				Common.Rates.RateCalculator.CalculateUtilization(interfaceTable);
			}

			// Sets
			SetTable(protocol, Utilization != null, Discontinuity != null);

			if (bufferedDeltaValue > 0)
			{
				protocol.SetParameter(bufferedDeltaPid, 0);
			}
		}

		private void GetAgentRestarted(SLProtocol protocol, TimeSpan? delta)
		{
			if (CurrentSysUptimePid < 1 || PreviousSysUptimePid < 1)
			{
				isAgentRestarted = false;
				return;
			}

			bufferedDeltaValue = Convert.ToInt32(protocol.GetParameter(bufferedDeltaPid));
			isBufferedDeltaPidRead = true;

			object[] uptimes = (object[])protocol.GetParameters(new[] { CurrentSysUptimePid, PreviousSysUptimePid });
			double currentSysUptime = Convert.ToDouble(uptimes[0]);
			double previousSysUptime = Convert.ToDouble(uptimes[1]);
			protocol.SetParameter(Convert.ToInt32(PreviousSysUptimePid), currentSysUptime);
			if (currentSysUptime >= previousSysUptime || delta == null)
			{
				isAgentRestarted = false;
				return;
			}

			// if the previousUptime + the buffered delta + delta results in an overflow, there was a wrap around.
			// Otherwise, there was an SNMP Agent restart.
			double result = (previousSysUptime * 100) + ((bufferedDeltaValue + delta.Value.TotalMilliseconds) / 10);
			isAgentRestarted = result <= UInt32.MaxValue;
		}

		private void SetTable(SLProtocol protocol, bool utilization, bool discontinuity)
		{
			// Sanity Check
			if (interfaceTable.InterfaceRows.Count == 0)
			{
				return;
			}

			// Basic Data
			List<object> setTablePids = new List<object>(7)
			{
				tablePid,
				inputColumns.PreviousValuesColumn.Pid,
				outputColumns.PreviousValuesColumn.Pid,
				inputColumns.RateColumn.Pid,
				outputColumns.RateColumn.Pid,
			};

			List<object> setTableData = SetTableBasicData();

			// Utilization Data
			if (utilization)
			{
				setTablePids.Add(Utilization.UtilizationColumn.Pid);
				setTableData.Add(SetTableUtilizationData());
			}

			// Discontinuity Data
			if (discontinuity)
			{
				setTablePids.Add(Discontinuity.PreviousValuesColumn.Pid);
				setTableData.Add(SetTableDiscontinuityData());
			}

			protocol.NotifyProtocol(220 /*NT_FILL_ARRAY_WITH_COLUMN*/, setTablePids.ToArray(), setTableData.ToArray());
		}

		private List<object> SetTableBasicData()
		{
			object[] pkeyCol = new object[interfaceTable.InterfaceRows.Count];
			object[] prevIn = new object[interfaceTable.InterfaceRows.Count];
			object[] prevOut = new object[interfaceTable.InterfaceRows.Count];
			object[] rateIn = new object[interfaceTable.InterfaceRows.Count];
			object[] rateOut = new object[interfaceTable.InterfaceRows.Count];

			int iCount = 0;

			foreach (Common.Rates.InterfaceRow interfaceDataItem in interfaceTable.InterfaceRows)
			{
				pkeyCol[iCount] = interfaceDataItem.Key;
				rateIn[iCount] = interfaceDataItem.InputBitRate.NewRate;
				rateOut[iCount] = interfaceDataItem.OutputBitRate.NewRate;

				if (interfaceDataItem.InputBitRate.GetType() == typeof(Common.Rates.InterfaceRate32BitCounters))
				{
					prevIn[iCount] = ((Common.Rates.InterfaceRate32BitCounters)interfaceDataItem.InputBitRate).CurrentCounter;
					prevOut[iCount] = ((Common.Rates.InterfaceRate32BitCounters)interfaceDataItem.OutputBitRate).CurrentCounter;
				}
				else if (interfaceDataItem.InputBitRate.GetType() == typeof(Common.Rates.InterfaceRate64BitCounters))
				{
					prevIn[iCount] = ((Common.Rates.InterfaceRate64BitCounters)interfaceDataItem.InputBitRate).CurrentCounter;
					prevOut[iCount] = ((Common.Rates.InterfaceRate64BitCounters)interfaceDataItem.OutputBitRate).CurrentCounter;
				}
				else
				{
					// do nothing, there are no other inherited classes yet
				}

				iCount++;
			}

			List<object> setTableData = new List<object>(7)
			{
				pkeyCol,
				prevIn,
				prevOut,
				rateIn,
				rateOut,
			};

			return setTableData;
		}

		private object[] SetTableUtilizationData()
		{
			object[] utilizationCol = new object[interfaceTable.InterfaceRows.Count];

			int iCount = 0;
			foreach (Common.Rates.InterfaceRow interfaceDataItem in interfaceTable.InterfaceRows)
			{
				utilizationCol[iCount] = interfaceDataItem.NewUtilization;

				iCount++;
			}

			return utilizationCol;
		}

		private object[] SetTableDiscontinuityData()
		{
			object[] discontinuityCol = new object[interfaceTable.InterfaceRows.Count];

			int iCount = 0;
			foreach (Common.Rates.InterfaceRow interfaceDataItem in interfaceTable.InterfaceRows)
			{
				discontinuityCol[iCount] = discontinuityTimeValues.ContainsKey(interfaceDataItem.Key) ?
					discontinuityTimeValues[interfaceDataItem.Key] : "0";

				iCount++;
			}

			return discontinuityCol;
		}

		private Dictionary<string, Common.Rates.DuplexStatus> GetDuplexStatus(SLProtocol protocol)
		{
			object[] columns = (object[])protocol.NotifyProtocol(321 /*NT_GET_TABLE_COLUMNS*/, Utilization.DuplexColumn.TablePid, new uint[] { 0, Utilization.DuplexColumn.Idx });

			Dictionary<string, Common.Rates.DuplexStatus> duplexStatus = new Dictionary<string, Common.Rates.DuplexStatus>();

			if (columns.Length == 2 && ((object[])columns[0]).Length == ((object[])columns[1]).Length)
			{
				object[] pkeyColumn = (object[])columns[0];
				object[] duplexStateColumn = (object[])columns[1];

				for (int i = 0; i < pkeyColumn.Length; i++)
				{
					duplexStatus[Convert.ToString(pkeyColumn[i])] = (Common.Rates.DuplexStatus)Convert.ToInt32(duplexStateColumn[i]);
				}
			}

			return duplexStatus;
		}

		private void GetData(SLProtocol protocol, TimeSpan? delta, int minDelta, bool utilization, bool discontinuity)
		{
			List<Common.Rates.InterfaceRow> interfaceRows = new List<Common.Rates.InterfaceRow>();

			object[] ifTableColumns;
			if (!TryGetTable(protocol, utilization, discontinuity, out ifTableColumns))
			{
				interfaceTable = new Common.Rates.InterfaceTable(interfaceRows, delta, bufferedDeltaValue, minDelta);
				return;
			}

			InterfaceColumns interfaceColumns = new InterfaceColumns(ifTableColumns, utilization, discontinuity);
			for (int i = 0; i < interfaceColumns.PKs.Length; i++)
			{
				string sPK = Convert.ToString(interfaceColumns.PKs[i]);

				Common.Rates.InterfaceRate inputBitRate;
				Common.Rates.InterfaceRate outputBitRate;
				if (speedType == SpeedType.Low)
				{
					uint inputCurrent = Convert.ToUInt32(interfaceColumns.CurrentInput[i]);
					uint inputPrevious = Convert.ToUInt32(interfaceColumns.PreviousInput[i]);
					double inputRate = Convert.ToDouble(interfaceColumns.RateInput[i]);
					inputBitRate = new Common.Rates.InterfaceRate32BitCounters(sPK, dataConversionType, inputCurrent, inputPrevious, inputRate);

					uint outputCurrent = Convert.ToUInt32(interfaceColumns.CurrentOutput[i]);
					uint outputPrevious = Convert.ToUInt32(interfaceColumns.PreviousOutput[i]);
					double outputRate = Convert.ToDouble(interfaceColumns.RateOutput[i]);
					outputBitRate = new Common.Rates.InterfaceRate32BitCounters(sPK, dataConversionType, outputCurrent, outputPrevious, outputRate);
				}
				else
				{
					UInt64 inputCurrent = Convert.ToUInt64(interfaceColumns.CurrentInput[i]);
					UInt64 inputPrevious = Convert.ToUInt64(interfaceColumns.PreviousInput[i]);
					double inputRate = Convert.ToDouble(interfaceColumns.RateInput[i]);
					inputBitRate = new Common.Rates.InterfaceRate64BitCounters(sPK, dataConversionType, inputCurrent, inputPrevious, inputRate);

					UInt64 outputCurrent = Convert.ToUInt64(interfaceColumns.CurrentOutput[i]);
					UInt64 outputPrevious = Convert.ToUInt64(interfaceColumns.PreviousOutput[i]);
					double outputRate = Convert.ToDouble(interfaceColumns.RateOutput[i]);
					outputBitRate = new Common.Rates.InterfaceRate64BitCounters(sPK, dataConversionType, outputCurrent, outputPrevious, outputRate);
				}

				double utilizationValue;
				double speedValue;
				Common.Rates.DuplexStatus duplexStatus;
				if (utilization)
				{
					utilizationValue = Convert.ToDouble(interfaceColumns.Utilizations[i]);

					uint uiSpeedValue = Convert.ToUInt32(interfaceColumns.Speed[i]);
					GetDataUtilization(protocol, uiSpeedValue, sPK, out speedValue, out duplexStatus);
				}
				else
				{
					utilizationValue = -1;

					speedValue = -1;
					duplexStatus = Common.Rates.DuplexStatus.NotInitialized;
				}

				interfaceRows.Add(new Common.Rates.InterfaceRow(sPK, inputBitRate, outputBitRate, utilizationValue, speedValue, duplexStatus));

				if (discontinuity)
				{
					string currentDiscontinuityTime = Convert.ToString(interfaceColumns.CurrentDiscontinuity[i]);
					string previousDiscontinuityTime = Convert.ToString(interfaceColumns.PreviousDiscontinuity[i]);
					discontinuityTimeValues[sPK] = currentDiscontinuityTime;
					if (!String.IsNullOrEmpty(previousDiscontinuityTime) && currentDiscontinuityTime != previousDiscontinuityTime)
					{
						discontinuityTimes.Add(sPK);
					}
				}
			}

			interfaceTable = new Common.Rates.InterfaceTable(interfaceRows, delta, bufferedDeltaValue, minDelta);
		}

		private void GetDataUtilization(SLProtocol protocol, uint uiSpeedValue, string sPK, out double speedValue, out Common.Rates.DuplexStatus duplexStatus)
		{
			if (speedType == SpeedType.Low)
			{
				speedValue = uiSpeedValue == UInt32.MaxValue ? -1.0 : Convert.ToDouble(uiSpeedValue);
			}
			else
			{
				speedValue = Convert.ToDouble(uiSpeedValue) * 1000000;    // High speed is expressed in Mbps.
			}

			Dictionary<string, Common.Rates.DuplexStatus> duplexValues = GetDuplexStatus(protocol);
			if (!duplexValues.TryGetValue(sPK, out duplexStatus))
			{
				duplexStatus = Common.Rates.DuplexStatus.NotInitialized;
			}
		}

		private bool TryGetTable(SLProtocol protocol, bool utilization, bool discontinuity, out object[] ifTableColumns)
		{
			List<uint> columnIdxs = new List<uint>(11)
			{
				0,
				inputColumns.CurrentValuesColumnIdx,
				outputColumns.CurrentValuesColumnIdx,
				inputColumns.PreviousValuesColumn.Idx,
				outputColumns.PreviousValuesColumn.Idx,
				inputColumns.RateColumn.Idx,
				outputColumns.RateColumn.Idx,
			};

			if (utilization)
			{
				columnIdxs.Add(Utilization.SpeedColumn.Idx);
				columnIdxs.Add(Utilization.UtilizationColumn.Idx);
			}

			if (discontinuity)
			{
				columnIdxs.Add(Discontinuity.CurrentValuesColumn.Idx);
				columnIdxs.Add(Discontinuity.PreviousValuesColumn.Idx);
			}

			ifTableColumns = (object[])protocol.NotifyProtocol(321 /*NT_GET_TABLE_COLUMNS*/, tablePid, columnIdxs.ToArray());
			if (ifTableColumns == null || ifTableColumns.Length != columnIdxs.Count)
			{
				return false;
			}

			return true;
		}
	}
}