namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Net.Messages;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Scripting;

	/// <summary>
	/// Represents a matrix UI control in a DataMiner protocol.
	/// Do not use this class directly. Instead, please use one of the inherited classes, depending on the type of available parameters in the protocol: <see cref="MatrixHelperForMatrix"/>, <see cref="MatrixHelperForTables"/>, or <see cref="MatrixHelperForMatrixAndTables"/>.
	/// </summary>
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("Newtonsoft.Json.dll")]
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	public class MatrixHelper
	{
		private readonly bool isFullSendToDisplayNeeded;
		private readonly int agentId;
		private readonly int elementId;
		private readonly int discreetInfoParameterId;
		private readonly int matrixConnectionsBufferParameterId;
		private readonly int matrixReadParameterId;
		private readonly int matrixWriteParameterId;
		private readonly int matrixSerializedParameterId;
		private readonly int tableSerializedParameterId;
		private readonly int tableVirtualSetParameterId;
		private readonly string elementName;
		private readonly MatrixPortState portState;
		private readonly MatrixInputs inputs;
		private readonly MatrixOutputs outputs;
		private bool changeMatrixSize;
		private int displayedInputs;
		private int displayedOutputs;
		private MatrixCustomTableInfo inputTableInfo;
		private MatrixCustomTableInfo outputTableInfo;
		private MatrixDisplayType displayType;
		private MatrixDisplayType originalDisplayType;
		private ParameterInfo matrixReadParameterInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="MatrixHelper"/> class.
		/// </summary>
		/// <param name="protocol">Link with SLProtocol process.</param>
		/// <param name="matrixHelperParameters">Object that contains all used Parameter IDs.</param>
		/// <param name="maxInputCount">The maximum amount of inputs. The <see cref="MatrixHelper.DisplayedInputs"/> property cannot be larger than this value.</param>
		/// <param name="maxOutputCount">The maximum amount of outputs. The <see cref="MatrixHelper.DisplayedOutputs"/> property cannot be larger than this value.</param>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Invalid matrix read parameter ID.
		/// -or-
		/// The specified matrix read parameter ID does not have a corresponding matrix parameter of type write with the same name.
		/// -or-
		/// Invalid discreet info parameter ID.
		/// -or-
		/// Invalid connection buffer parameter ID.
		/// </exception>
		internal MatrixHelper(SLProtocol protocol, MatrixHelperParameterIds matrixHelperParameters, int maxInputCount, int maxOutputCount)
		{
			matrixHelperParameters.MaxInputCount = maxInputCount;
			matrixHelperParameters.MaxOutputCount = maxOutputCount;
			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			agentId = protocol.DataMinerID;
			elementId = protocol.ElementID;
			elementName = protocol.ElementName;
			discreetInfoParameterId = matrixHelperParameters.DiscreetInfoParameterId;
			matrixConnectionsBufferParameterId = matrixHelperParameters.MatrixConnectionsBufferParameterId;
			matrixSerializedParameterId = matrixHelperParameters.MatrixSerializedParameterId;

			if (matrixHelperParameters.MatrixReadParameterId > 0 || matrixHelperParameters.MatrixReadParameterId == -2)
			{
				GetAgentBuildInfo buildInfoMessage = new GetAgentBuildInfo(agentId);
				var buildInfoResponse = (BuildInfoResponse)protocol.SLNet.SendSingleResponseMessage(buildInfoMessage);
				isFullSendToDisplayNeeded = ParseBuildInfo(buildInfoResponse);  // Boolean and method can be removed if DCP task 86217 is fixed, when 9.6.0 Main release is the minimum supported DMA version
			}
			else
			{
				isFullSendToDisplayNeeded = false;
			}

			GetElementProtocolMessage message = new GetElementProtocolMessage(agentId, elementId);
			var elementProtocolResponseMessage = (GetElementProtocolResponseMessage)protocol.SLNet.SendSingleResponseMessage(message);
			MatrixHelperParameterIds foundParameterIds = new MatrixHelperParameterIds();
			ParseProtocolInfo(elementProtocolResponseMessage, matrixHelperParameters, foundParameterIds);

			matrixReadParameterId = foundParameterIds.MatrixReadParameterId;
			matrixWriteParameterId = foundParameterIds.MatrixWriteParameterId;
			matrixConnectionsBufferParameterId = foundParameterIds.MatrixConnectionsBufferParameterId;
			matrixSerializedParameterId = foundParameterIds.MatrixSerializedParameterId;
			tableSerializedParameterId = foundParameterIds.TableSerializedParameterId;
			tableVirtualSetParameterId = foundParameterIds.TableVirtualSetParameterId;
			string connectionBuffer = Convert.ToString(protocol.GetParameter(foundParameterIds.MatrixConnectionsBufferParameterId), CultureInfo.CurrentCulture);
			MatrixSizeInfo maxSize = new MatrixSizeInfo(MaxInputs, MaxOutputs);
			MatrixSizeInfo maxFoundSize;
			if (matrixReadParameterId > 0 && foundParameterIds.InputsTableParameterId > 0)
			{
				portState = new MatrixPortState(protocol, maxSize, matrixReadParameterInfo, inputTableInfo, outputTableInfo, connectionBuffer, out maxFoundSize);    // matrix and table parameters exist
				displayType = portState.DetectedDisplayType;
				originalDisplayType = portState.DetectedDisplayType;
				if (displayType == MatrixDisplayType.Tables)
				{
					displayedInputs = maxFoundSize.Inputs + 1;
					displayedOutputs = maxFoundSize.Outputs + 1;
				}
			}
			else if (matrixReadParameterId > 0)
			{
				portState = new MatrixPortState(maxSize, matrixReadParameterInfo, connectionBuffer);
				displayType = MatrixDisplayType.Matrix;
				originalDisplayType = MatrixDisplayType.Matrix;
			}
			else
			{
				portState = new MatrixPortState(protocol, maxSize, inputTableInfo, outputTableInfo, connectionBuffer, out maxFoundSize);
				displayType = MatrixDisplayType.Tables;
				originalDisplayType = MatrixDisplayType.Tables;
				displayedInputs = maxFoundSize.Inputs + 1;
				displayedOutputs = maxFoundSize.Outputs + 1;
			}

			inputs = new MatrixInputs(portState);
			outputs = new MatrixOutputs(portState);
		}

		private enum ChangeType
		{
			Label = 0,
			State = 1,
			Page = 3,
			Allowed = 4,
			Lock = 6,
			MasterInfo = 7,
			FollowMode = 8,
			Size = 9
		}

		/// <summary>
		/// Gets or sets the number of displayed inputs.
		/// </summary>
		/// <value>The number of displayed inputs.</value>
		/// <exception cref="ArgumentOutOfRangeException">The value of a set operation is not in the range [1, MaxInputs].</exception>
		public int DisplayedInputs
		{
			get
			{
				return displayedInputs;
			}

			set
			{
				if (value > 0 && value <= MaxInputs)
				{
					if (displayedInputs != value)
					{
						displayedInputs = value;
						changeMatrixSize = true;
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("The number of displayed inputs must be in the range [1, " + MaxInputs + "].");
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of displayed outputs.
		/// </summary>
		/// <value>The number of displayed outputs.</value>
		/// <exception cref="ArgumentOutOfRangeException">The value of a set operation is not in the range [1, MaxOutputs].</exception>
		public int DisplayedOutputs
		{
			get
			{
				return displayedOutputs;
			}

			set
			{
				if (value > 0 && value <= MaxOutputs)
				{
					if (displayedOutputs != value)
					{
						displayedOutputs = value;
						changeMatrixSize = true;
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("The number of displayed outputs must be in the range [1, " + MaxOutputs + "].");
				}
			}
		}

		/// <summary>
		/// Gets the inputs of this matrix.
		/// </summary>
		/// <value>The inputs of this matrix.</value>
		public MatrixInputs Inputs
		{
			get { return inputs; }
		}

		/// <summary>
		/// Gets the maximum number of connected inputs per output.
		/// </summary>
		/// <value>The maximum number of connected inputs per output.</value>
		public int MaxConnectedInputsPerOutput { get; private set; }

		/// <summary>
		/// Gets the maximum number of connected outputs per input.
		/// </summary>
		/// <value>The maximum number of connected outputs per input.</value>
		public int MaxConnectedOutputsPerInput { get; private set; }

		/// <summary>
		/// Gets the maximum number of inputs this matrix supports.
		/// </summary>
		/// <value>The maximum number of inputs this matrix supports.</value>
		public int MaxInputs { get; private set; }

		/// <summary>
		/// Gets the maximum number of outputs this matrix supports.
		/// </summary>
		/// <value>The maximum number of outputs this matrix supports.</value>
		public int MaxOutputs { get; private set; }

		/// <summary>
		/// Gets the minimum number of connected inputs per output.
		/// </summary>
		/// <value>The minimum number of connected inputs per output.</value>
		public int MinConnectedInputsPerOutput { get; private set; }

		/// <summary>
		/// Gets the minimum number of connected outputs per input.
		/// </summary>
		/// <value>The minimum number of connected outputs per input.</value>
		public int MinConnectedOutputsPerInput { get; private set; }

		/// <summary>
		/// Gets the outputs of this matrix.
		/// </summary>
		/// <value>The outputs of this matrix.</value>
		public MatrixOutputs Outputs
		{
			get { return outputs; }
		}

		/// <summary>
		/// Processes write parameter changes on matrix, discreet info, or table.
		/// </summary>
		/// <param name="protocol">Link with SLProtocol process. This needs to be the SLProtocol object that triggered the QAction as the <see cref="SLProtocol.GetTriggerParameter"/> method is internally being called.</param>
		/// <param name="triggerParameter">ID of the parameter that triggered the QAction.</param>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		public void ProcessParameterSetFromUI(SLProtocol protocol, int triggerParameter = 0)
		{
			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			int triggerId = triggerParameter <= 0 ? protocol.GetTriggerParameter() : triggerParameter;
			if (triggerId == matrixWriteParameterId || triggerId == tableVirtualSetParameterId)
			{
				ProcessAllCrosspointWriteParameterChange(protocol, triggerId);
			}
			else if (triggerId == discreetInfoParameterId)
			{
				ProcessMatrixDiscreetInfoParameterChange(protocol);
			}
			else if (triggerId == tableSerializedParameterId)
			{
				ProcessTableWriteParameterChange(protocol);
			}
			else
			{
				// Do nothing
			}
		}

		/// <summary>
		/// Applies all changes on the parameter controls (connection changes, label changes, etc.).
		/// </summary>
		/// <param name="protocol">Link with SLProtocol process.</param>
		/// <exception cref="ArgumentException">Matrix cannot be retrieved. Exception could only occur when needing to switch DisplayType between tables and matrix.</exception>
		public void ApplyChanges(SLProtocol protocol)
		{
			if (originalDisplayType == displayType)
			{
				ApplyChangesNoDisplayTypeChanged(protocol);
			}
			else
			{
				ApplyChangesDisplayTypeChanged(protocol);
			}
		}

		internal static void DeleteRows(SLProtocol protocol, int tablePid, ICollection<string> rowsToBeDeleted)
		{
			if (rowsToBeDeleted.Count == 0)
			{
				return;
			}

			string[] deleteKeys = new string[rowsToBeDeleted.Count];
			int count = 0;
			foreach (string key in rowsToBeDeleted)
			{
				deleteKeys[count++] = key;
			}

			protocol.NotifyProtocol((int)NotifyType.DeleteRow, tablePid, deleteKeys);
		}

		internal MatrixDisplayType GetDisplayType()
		{
			return displayType;
		}

		internal void SetDisplayType(MatrixDisplayType displayType)
		{
			switch (displayType)
			{
				case MatrixDisplayType.Matrix:
					if (portState.IsMatrixCapable)
					{
						this.displayType = displayType;
					}
					else
					{
						throw new InvalidOperationException("MatrixDisplayType Matrix was requested but only Table is supported.");
					}

					break;
				case MatrixDisplayType.Tables:
					if (portState.IsTableCapable)
					{
						this.displayType = displayType;
					}
					else
					{
						throw new InvalidOperationException("MatrixDisplayType Table was requested but only Matrix is supported.");
					}

					break;
				case MatrixDisplayType.MatrixAndTables:
					if (portState.IsMatrixCapable && portState.IsTableCapable)
					{
						this.displayType = displayType;
					}
					else
					{
						throw new InvalidOperationException("MatrixDisplayType Matrix and Table was requested, but only one type is supported.");
					}

					break;
				default:
					throw new InvalidOperationException("Unknown MatrixDisplayType was provided.");
			}
		}

		/// <summary>
		/// Gets triggered when crosspoint connections are changed.
		/// </summary>
		/// <param name="set">Information about the changed crosspoint connections.</param>
		protected virtual void OnCrossPointsSetFromUI(MatrixCrossPointsSetFromUIMessage set)
		{
		}

		/// <summary>
		/// Gets triggered when the label of an input or output is changed.
		/// </summary>
		/// <param name="set">Information about the changed label.</param>
		protected virtual void OnLabelSetFromUI(MatrixLabelSetFromUIMessage set)
		{
		}

		/// <summary>
		/// Gets triggered when an input or output is locked or unlocked.
		/// </summary>
		/// <param name="set">Information about the changed lock.</param>
		protected virtual void OnLockSetFromUI(MatrixLockSetFromUIMessage set)
		{
		}

		/// <summary>
		/// Gets triggered when an input or output is enabled or disabled.
		/// </summary>
		/// <param name="set">Information about the changed state.</param>
		protected virtual void OnStateSetFromUI(MatrixIOStateSetFromUIMessage set)
		{
		}

		private static void ClearTable(SLProtocol protocol, int tableParameterId)
		{
			object[] tableCols = (object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, tableParameterId, new uint[] { 0 });
			if (tableCols != null && tableCols.Length > 0)
			{
				object[] indexCol = (object[])tableCols[0];
				if (indexCol != null && indexCol.Length > 0)
				{
					string[] rowsToBeDeleted = new string[indexCol.Length];
					for (int i = 0; i < indexCol.Length; i++)
					{
						rowsToBeDeleted[i] = Convert.ToString(indexCol[i], CultureInfo.CurrentCulture);
					}

					protocol.NotifyProtocol((int)NotifyType.DeleteRow, tableParameterId, rowsToBeDeleted);
				}
			}
		}

		private static void GetNeededKeys(ISet<string> neededKeys, Dictionary<string, string> allChangedItems, int displayedItems)
		{
			foreach (string key in allChangedItems.Keys)
			{
				int pk;
				if (Int32.TryParse(key, out pk) && pk <= displayedItems)
				{
					neededKeys.Add(key);
				}
			}
		}

		private static void GetNeededKeys(ISet<string> neededKeys, Dictionary<int, HashSet<int>> allChangedItems, int displayedItems)
		{
			foreach (int pk in allChangedItems.Keys)
			{
				if (pk <= displayedItems)
				{
					neededKeys.Add(Convert.ToString(pk + 1, CultureInfo.InvariantCulture));
				}
			}
		}

		private static void ProcessTableExpanded(string pk, MatrixPort matrixPort, IDictionary<string, string> allChangedLabels, IDictionary<string, string> allChangedLocks, IDictionary<string, string> allChangedStates)
		{
			if (!allChangedLabels.ContainsKey(pk))
			{
				allChangedLabels[pk] = matrixPort.Label;
			}

			if (!allChangedLocks.ContainsKey(pk))
			{
				allChangedLocks[pk] = matrixPort.IsLocked ? "1" : "0";
			}

			if (!allChangedStates.ContainsKey(pk))
			{
				allChangedStates[pk] = matrixPort.IsEnabled ? "1" : "0";
			}
		}

		private static int ProcessPkColumn(ISet<string> rowsToBeDeleted, object[] indexCol, int displayedItems)
		{
			int maxFoundPK = 0;
			for (int i = 0; i < indexCol.Length; i++)
			{
				string key = Convert.ToString(indexCol[i], CultureInfo.CurrentCulture);
				int portNumber;
				if (Int32.TryParse(key, out portNumber))
				{
					if (portNumber > displayedItems)
					{
						rowsToBeDeleted.Add(key);
					}
					else if (portNumber > maxFoundPK)
					{
						maxFoundPK = portNumber;
					}
					else
					{
						// Do nothing
					}
				}
				else
				{
					rowsToBeDeleted.Add(key);
				}
			}

			return maxFoundPK;
		}

		private static void SetColumnValue(string key, int count, MatrixPort matrixPort, object[] setCol, IDictionary<string, string> allChangedItems, string type)
		{
			string item;
			if (allChangedItems.TryGetValue(key, out item))
			{
				setCol[count] = item;
			}
			else
			{
				switch (type)
				{
					case "Label":
						setCol[count] = matrixPort.Label;
						break;
					case "IsLocked":
						setCol[count] = matrixPort.IsLocked ? "1" : "0";
						break;
					case "IsEnabled":
						setCol[count] = matrixPort.IsEnabled ? "1" : "0";
						break;
					default:
						return;
				}
			}
		}

		private static bool ParseBuildInfo(BuildInfoResponse response)
		{
			if (response == null || response.Agents == null || response.Agents.Length == 0)
			{
				return false;
			}

			string rawVersion = response.Agents[0].RawVersion;
			string[] splitDot = new[] { "." };
			string[] versionParts = rawVersion.Split(splitDot, StringSplitOptions.None);
			int versionPartMajor = 0;
			int versionPartMinor = 0;
			int versionPartMonth = 0;
			int versionPartWeek = 0;
			if (versionParts.Length != 4)
			{
				return false;
			}

			if (!Int32.TryParse(versionParts[0], out versionPartMajor) ||
				!Int32.TryParse(versionParts[2], out versionPartMonth) ||
				!Int32.TryParse(versionParts[1], out versionPartMinor) ||
				!Int32.TryParse(versionParts[3], out versionPartWeek))
			{
				return false;
			}

			if (versionPartMajor != 9)
			{
				return false;
			}

			switch (versionPartMinor)
			{
				case 0:
					return true;
				case 5:
					return !(versionPartMonth == 0 && versionPartWeek == 0) || response.Agents[0].UpgradeBuildID < 7952;
				case 6:
					return !(versionPartMonth == 0 && versionPartWeek == 0) && versionPartMonth < 3;
				default:
					return false;
			}
		}

		private static void ParseProtocolTableParameter(ParameterInfo parameter, bool isAutoDetectTable, MatrixHelperParameterIds requestParameters, MatrixHelperParameterNames matrixHelperParameterNames, IDictionary<int, uint> inputColumns, IDictionary<int, uint> outputColumns, IDictionary<int, ParameterInfo> potentialTables)
		{
			if (!parameter.ArrayType || parameter.IsMatrix)
			{
				return;
			}

			if (isAutoDetectTable)
			{
				if (parameter.TableColumnDefinitions != null && parameter.TableColumnDefinitions.Length > 3)
				{
					potentialTables[parameter.ID] = parameter;
				}
			}
			else
			{
				if (requestParameters.InputsTableParameterId > 0 && parameter.ID == requestParameters.InputsTableParameterId)
				{
					matrixHelperParameterNames.SetInputNames(parameter, inputColumns);
				}

				if (requestParameters.OutputsTableParameterId > 0 && parameter.ID == requestParameters.OutputsTableParameterId)
				{
					matrixHelperParameterNames.SetOutputNames(parameter, outputColumns);
				}
			}
		}

		private static void SearchPotentialTables(string postValue, string parameterName, int parameterId, IDictionary<int, ParameterInfo> potentialTables, IDictionary<int, MatrixCustomTableInfoItem> foundReadTables, IDictionary<int, int> foundWriteTables, bool isWrite)
		{
			if (!parameterName.EndsWith(postValue, StringComparison.Ordinal))
			{
				return;
			}

			foreach (KeyValuePair<int, ParameterInfo> kvp in potentialTables)
			{
				if ((kvp.Value.Name.ToLower(CultureInfo.InvariantCulture) + postValue) != parameterName)
				{
					continue;
				}

				if (isWrite)
				{
					foundWriteTables[kvp.Key] = parameterId;
					continue;
				}

				uint parameterIdx = 0;
				foreach (TableColumnDefinition tableColumnDefinition in kvp.Value.TableColumnDefinitions)
				{
					if (tableColumnDefinition.ParameterID == parameterId && foundReadTables != null)
					{
						foundReadTables[kvp.Key] = new MatrixCustomTableInfoItem(parameterId, parameterIdx);
						return;
					}

					parameterIdx++;
				}
			}
		}

		private static int GetMatrixWriteParameterId(string matrixParameterName, GetElementProtocolResponseMessage response)
		{
			int id = -1;

			var parameters = response.Parameters;

			foreach (var parameter in parameters)
			{
				if (parameter.IsMatrix && parameter.WriteType && parameter.Name == matrixParameterName)
				{
					id = parameter.ID;
					break;
				}
			}

			return id;
		}

		private void ApplyChangesNoDisplayTypeChanged(SLProtocol protocol)
		{
			bool isMatrixChanged = false;
			bool isTableChanged = false;
			bool isCrosspointChanged = false;
			if (displayType == MatrixDisplayType.Matrix)
			{
				Dictionary<string, string> allChangedLabels = new Dictionary<string, string>();
				Dictionary<string, string> allChangedLocks = new Dictionary<string, string>();
				Dictionary<string, string> allChangedStates = new Dictionary<string, string>();
				portState.InputLabels.GetChangedMatrixLabels(allChangedLabels);
				portState.OutputLabels.GetChangedMatrixLabels(allChangedLabels);
				portState.InputLocks.GetChangedMatrixItems(allChangedLocks);
				portState.OutputLocks.GetChangedMatrixItems(allChangedLocks);
				portState.InputStates.GetChangedMatrixItems(allChangedStates);
				portState.OutputStates.GetChangedMatrixItems(allChangedStates);
				isMatrixChanged = ApplyMatrixChanges(protocol, allChangedLabels, allChangedLocks, allChangedStates, true);
				isCrosspointChanged = ExecuteChangedMatrixSets(protocol, true);
			}
			else if (displayType == MatrixDisplayType.Tables)
			{
				Dictionary<string, string> allChangedInputLabels = new Dictionary<string, string>();
				Dictionary<string, string> allChangedInputLocks = new Dictionary<string, string>();
				Dictionary<string, string> allChangedInputStates = new Dictionary<string, string>();
				Dictionary<string, string> allChangedOutputLabels = new Dictionary<string, string>();
				Dictionary<string, string> allChangedOutputLocks = new Dictionary<string, string>();
				Dictionary<string, string> allChangedOutputStates = new Dictionary<string, string>();
				portState.InputLabels.GetChangedTableLabels(allChangedInputLabels);
				portState.OutputLabels.GetChangedTableLabels(allChangedOutputLabels);
				portState.InputLocks.GetChangedTableItems(allChangedInputLocks);
				portState.OutputLocks.GetChangedTableItems(allChangedOutputLocks);
				portState.InputStates.GetChangedTableItems(allChangedInputStates);
				portState.OutputStates.GetChangedTableItems(allChangedOutputStates);
				isTableChanged = ApplyTableChanges(protocol, allChangedInputLabels, allChangedOutputLabels, allChangedInputLocks, allChangedOutputLocks, allChangedInputStates, allChangedOutputStates);
			}
			else if (displayType == MatrixDisplayType.MatrixAndTables)
			{
				Dictionary<string, string> allChangedMatrixLabels = new Dictionary<string, string>();
				Dictionary<string, string> allChangedMatrixLocks = new Dictionary<string, string>();
				Dictionary<string, string> allChangedMatrixStates = new Dictionary<string, string>();
				Dictionary<string, string> allChangedInputLabels = new Dictionary<string, string>();
				Dictionary<string, string> allChangedInputLocks = new Dictionary<string, string>();
				Dictionary<string, string> allChangedInputStates = new Dictionary<string, string>();
				Dictionary<string, string> allChangedOutputLabels = new Dictionary<string, string>();
				Dictionary<string, string> allChangedOutputLocks = new Dictionary<string, string>();
				Dictionary<string, string> allChangedOutputStates = new Dictionary<string, string>();
				portState.InputLabels.GetChangedMatrixAndTableLabels(allChangedMatrixLabels, allChangedInputLabels);
				portState.OutputLabels.GetChangedMatrixAndTableLabels(allChangedMatrixLabels, allChangedOutputLabels);
				portState.InputLocks.GetChangedMatrixAndTableItems(allChangedMatrixLocks, allChangedInputLocks);
				portState.OutputLocks.GetChangedMatrixAndTableItems(allChangedMatrixLocks, allChangedOutputLocks);
				portState.InputStates.GetChangedMatrixAndTableItems(allChangedMatrixStates, allChangedInputStates);
				portState.OutputStates.GetChangedMatrixAndTableItems(allChangedMatrixStates, allChangedOutputStates);
				isMatrixChanged = ApplyMatrixChanges(protocol, allChangedMatrixLabels, allChangedMatrixLocks, allChangedMatrixStates, false);
				isCrosspointChanged = ExecuteChangedMatrixSets(protocol, false);
				isTableChanged = ApplyTableChanges(protocol, allChangedInputLabels, allChangedOutputLabels, allChangedInputLocks, allChangedOutputLocks, allChangedInputStates, allChangedOutputStates);
			}
			else
			{
				// Do nothing
			}

			if (isMatrixChanged || isCrosspointChanged || isTableChanged)
			{
				UpdateSerializedMatrixParameter(protocol);
			}
		}

		private void ApplyChangesDisplayTypeChanged(SLProtocol protocol)
		{
			if (displayType == MatrixDisplayType.MatrixAndTables)
			{
				// previous was matrix OR table, now need to add changes to the previous one and then copy all info to the other one
				if (originalDisplayType == MatrixDisplayType.Matrix)
				{
					Dictionary<string, string> allChangedLabels = new Dictionary<string, string>();
					Dictionary<string, string> allChangedLocks = new Dictionary<string, string>();
					Dictionary<string, string> allChangedStates = new Dictionary<string, string>();
					portState.InputLabels.GetChangedMatrixLabels(allChangedLabels);
					portState.OutputLabels.GetChangedMatrixLabels(allChangedLabels);
					portState.InputLocks.GetChangedMatrixItems(allChangedLocks);
					portState.OutputLocks.GetChangedMatrixItems(allChangedLocks);
					portState.InputStates.GetChangedMatrixItems(allChangedStates);
					portState.OutputStates.GetChangedMatrixItems(allChangedStates);
					ApplyMatrixChanges(protocol, allChangedLabels, allChangedLocks, allChangedStates, true);
					ExecuteChangedMatrixSets(protocol, true);
					ApplyTableFullSet(protocol);
				}
				else
				{
					// originalDisplayType == MatrixDisplayType.Table
					Dictionary<string, string> allChangedInputLabels = new Dictionary<string, string>();
					Dictionary<string, string> allChangedInputLocks = new Dictionary<string, string>();
					Dictionary<string, string> allChangedInputStates = new Dictionary<string, string>();
					Dictionary<string, string> allChangedOutputLabels = new Dictionary<string, string>();
					Dictionary<string, string> allChangedOutputLocks = new Dictionary<string, string>();
					Dictionary<string, string> allChangedOutputStates = new Dictionary<string, string>();
					portState.InputLabels.GetChangedTableLabels(allChangedInputLabels);
					portState.OutputLabels.GetChangedTableLabels(allChangedOutputLabels);
					portState.InputLocks.GetChangedTableItems(allChangedInputLocks);
					portState.OutputLocks.GetChangedTableItems(allChangedOutputLocks);
					portState.InputStates.GetChangedTableItems(allChangedInputStates);
					portState.OutputStates.GetChangedTableItems(allChangedOutputStates);
					ApplyTableChanges(protocol, allChangedInputLabels, allChangedOutputLabels, allChangedInputLocks, allChangedOutputLocks, allChangedInputStates, allChangedOutputStates);
					ApplyMatrixFullSet(protocol);
				}
			}
			else if (originalDisplayType == MatrixDisplayType.MatrixAndTables)
			{
				// previous contained both matrix AND table, now only need to do changes on one item and clear the other item
				if (displayType == MatrixDisplayType.Matrix)
				{
					// do changes on matrix, clear table
					ClearOriginalTableSets(protocol);
					Dictionary<string, string> allChangedLabels = new Dictionary<string, string>();
					Dictionary<string, string> allChangedLocks = new Dictionary<string, string>();
					Dictionary<string, string> allChangedStates = new Dictionary<string, string>();
					portState.InputLabels.GetChangedMatrixLabels(allChangedLabels);
					portState.OutputLabels.GetChangedMatrixLabels(allChangedLabels);
					portState.InputLocks.GetChangedMatrixItems(allChangedLocks);
					portState.OutputLocks.GetChangedMatrixItems(allChangedLocks);
					portState.InputStates.GetChangedMatrixItems(allChangedStates);
					portState.OutputStates.GetChangedMatrixItems(allChangedStates);
					ApplyMatrixChanges(protocol, allChangedLabels, allChangedLocks, allChangedStates, true);
					ExecuteChangedMatrixSets(protocol, true);
				}
				else
				{
					// displayType == MatrixDisplayType.Table
					// do changes on table, clear matrix
					ClearOriginalMatrixSets(protocol);
					Dictionary<string, string> allChangedInputLabels = new Dictionary<string, string>();
					Dictionary<string, string> allChangedInputLocks = new Dictionary<string, string>();
					Dictionary<string, string> allChangedInputStates = new Dictionary<string, string>();
					Dictionary<string, string> allChangedOutputLabels = new Dictionary<string, string>();
					Dictionary<string, string> allChangedOutputLocks = new Dictionary<string, string>();
					Dictionary<string, string> allChangedOutputStates = new Dictionary<string, string>();
					portState.InputLabels.GetChangedTableLabels(allChangedInputLabels);
					portState.OutputLabels.GetChangedTableLabels(allChangedOutputLabels);
					portState.InputLocks.GetChangedTableItems(allChangedInputLocks);
					portState.OutputLocks.GetChangedTableItems(allChangedOutputLocks);
					portState.InputStates.GetChangedTableItems(allChangedInputStates);
					portState.OutputStates.GetChangedTableItems(allChangedOutputStates);
					ApplyTableChanges(protocol, allChangedInputLabels, allChangedOutputLabels, allChangedInputLocks, allChangedOutputLocks, allChangedInputStates, allChangedOutputStates);
				}
			}
			else
			{
				// complete swap needed
				if (displayType == MatrixDisplayType.Matrix)
				{
					// displayType == MatrixDisplayType.Matrix, originalDisplayType = MatrixDisplayType.Table
					ClearOriginalTableSets(protocol);
					portState.InputLabels.ExecuteChangedLabels();
					portState.OutputLabels.ExecuteChangedLabels();
					portState.InputLocks.ExecuteChangedItems();
					portState.OutputLocks.ExecuteChangedItems();
					portState.InputStates.ExecuteChangedItems();
					portState.OutputStates.ExecuteChangedItems();
					GetChangedConnections(protocol);
					ApplyMatrixFullSet(protocol);
				}
				else
				{
					// displayType == MatrixDisplayType.Table, originalDisplayType = MatrixDisplayType.Matrix
					ClearOriginalMatrixSets(protocol);
					portState.InputLabels.ExecuteChangedLabels();
					portState.OutputLabels.ExecuteChangedLabels();
					portState.InputLocks.ExecuteChangedItems();
					portState.OutputLocks.ExecuteChangedItems();
					portState.InputStates.ExecuteChangedItems();
					portState.OutputStates.ExecuteChangedItems();
					GetChangedConnections(protocol);
					ApplyTableFullSet(protocol);
				}
			}

			originalDisplayType = displayType;
			UpdateSerializedMatrixParameter(protocol);
		}

		private void ProcessAllCrosspointWriteParameterChange(SLProtocol protocol, int parameterId)
		{
			List<MatrixCrossPointSetFromUI> updatedCrossPoints = new List<MatrixCrossPointSetFromUI>();
			HashSet<int> connectedOutputs = new HashSet<int>();
			Dictionary<int, int> pendingDisconnects = new Dictionary<int, int>();

			string writeValue = Convert.ToString(protocol.GetParameter(parameterId), CultureInfo.CurrentCulture);

			foreach (string set in writeValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
				string[] setItems = set.Split(',');

				if (setItems.Length < 3)
				{
					return;
				}

				int inputNumber;
				int outputNumber;
				int connectionType;

				bool validInput = Int32.TryParse(setItems[0], out inputNumber);
				bool validOutput = Int32.TryParse(setItems[1], out outputNumber) && (outputNumber > MaxInputs) && (outputNumber <= MaxInputs + MaxOutputs);
				bool validConnectionType = Int32.TryParse(setItems[2], out connectionType) && (connectionType == 0 || connectionType == 1);

				if (!validInput || !validOutput || !validConnectionType)
				{
					return;
				}

				int inputKey = inputNumber - 1;
				int outputKey = outputNumber - MaxInputs - 1;
				MatrixCrossPointConnectionState crossPointState = connectionType == 0 ? MatrixCrossPointConnectionState.Disconnected : MatrixCrossPointConnectionState.Connected;
				ProcessCrosspointWriteParameterChange(inputKey, outputKey, crossPointState, updatedCrossPoints, connectedOutputs, pendingDisconnects);
			}

			if (updatedCrossPoints.Any())
			{
				OnCrossPointsSetFromUI(new MatrixCrossPointsSetFromUIMessage(updatedCrossPoints));
			}
		}

		private void ProcessCrosspointWriteParameterChange(int inputKey, int outputKey, MatrixCrossPointConnectionState crossPointState, ICollection<MatrixCrossPointSetFromUI> updatedCrossPoints, ISet<int> connectedOutputs, IDictionary<int, int> pendingDisconnects)
		{
			bool isCrosspointLocked = inputs[inputKey].IsLocked || outputs[outputKey].IsLocked;
			bool isCrosspointEnabled = inputs[inputKey].IsEnabled && outputs[outputKey].IsEnabled;
			if (crossPointState == MatrixCrossPointConnectionState.Connected)
			{
				if (isCrosspointLocked || !isCrosspointEnabled)
				{
					return;
				}

				updatedCrossPoints.Add(new MatrixCrossPointSetFromUI(inputKey, outputKey, crossPointState));
				connectedOutputs.Add(outputKey);
				int disconnectInput;
				if (pendingDisconnects.TryGetValue(outputKey, out disconnectInput))
				{
					updatedCrossPoints.Add(new MatrixCrossPointSetFromUI(disconnectInput, outputKey, MatrixCrossPointConnectionState.Disconnected));
					pendingDisconnects.Remove(outputKey);
				}
			}
			else
			{
				if ((!isCrosspointLocked && isCrosspointEnabled) || connectedOutputs.Contains(outputKey))
				{
					updatedCrossPoints.Add(new MatrixCrossPointSetFromUI(inputKey, outputKey, crossPointState));
				}
				else
				{
					pendingDisconnects[outputKey] = inputKey;
				}
			}
		}

		private void ProcessMatrixDiscreetInfoParameterChange(SLProtocol protocol)
		{
			if (displayType != MatrixDisplayType.Tables)
			{
				string writeValue = Convert.ToString(protocol.GetParameter(discreetInfoParameterId), CultureInfo.CurrentCulture);
				string[] parts = writeValue.Split(';');

				int type;
				if ((parts.Length > 3) && (parts[1] == Convert.ToString(matrixReadParameterId, CultureInfo.InvariantCulture)) && Int32.TryParse(parts[0], out type))
				{
					ChangeType changeType = (ChangeType)type;

					switch (changeType)
					{
						case ChangeType.Label:
							ProcessLabelChange(parts, protocol);
							break;
						case ChangeType.Lock:
							ProcessLockChange(parts, protocol);
							break;
						case ChangeType.State:
							ProcessStateChange(parts, protocol);
							break;
						default:
							return;
					}
				}
			}
		}

		private void ProcessTableWriteParameterChange(SLProtocol protocol)
		{
			if (displayType == MatrixDisplayType.Matrix)
			{
				return;
			}

			string writeValue = Convert.ToString(protocol.GetParameter(tableSerializedParameterId), CultureInfo.CurrentCulture);
			string[] parts = writeValue.Split(';');
			int writeParameterId;
			int key;
			if (parts.Length <= 2 || !Int32.TryParse(parts[0], out writeParameterId) || !Int32.TryParse(parts[1], out key))
			{
				return;
			}

			key--;
			string setValue = parts[2];
			if (!VerifyInputWriteParameterValue(writeParameterId, key, setValue) && !VerifyOutputWriteParameterValue(writeParameterId, key, setValue))
			{
				throw new ArgumentException("Unknown incoming write parameter id " + Convert.ToString(writeParameterId, CultureInfo.InvariantCulture));
			}
		}

		private bool VerifyInputWriteParameterValue(int writeParameterId, int key, string setValue)
		{
			if (writeParameterId == inputTableInfo.LabelWriteParameterId)
			{
				if (key < displayedInputs && !String.IsNullOrEmpty(setValue) && inputs[key].Label != setValue)
				{
					OnLabelSetFromUI(new MatrixLabelSetFromUIMessage(key, MatrixIOType.Input, setValue));
				}

				return true;
			}

			bool isValidSetValue = setValue == "0" || setValue == "1";
			bool isSetValue = setValue == "1";
			if (writeParameterId == inputTableInfo.LockedWriteParameterId)
			{
				if (isValidSetValue && key < displayedInputs && inputs[key].IsLocked != isSetValue && inputs[key].IsEnabled)
				{
					OnLockSetFromUI(new MatrixLockSetFromUIMessage(key, MatrixIOType.Input, isSetValue));
				}

				return true;
			}

			if (writeParameterId == inputTableInfo.EnabledWriteParameterId)
			{
				if (isValidSetValue && key < displayedInputs && inputs[key].IsEnabled != isSetValue)
				{
					OnStateSetFromUI(new MatrixIOStateSetFromUIMessage(key, MatrixIOType.Input, isSetValue));
				}

				return true;
			}

			return false;
		}

		private bool VerifyOutputWriteParameterValue(int writeParameterId, int key, string setValue)
		{
			if (writeParameterId == outputTableInfo.LabelWriteParameterId)
			{
				if (key < displayedOutputs && !String.IsNullOrEmpty(setValue) && outputs[key].Label != setValue)
				{
					OnLabelSetFromUI(new MatrixLabelSetFromUIMessage(key, MatrixIOType.Output, setValue));
				}

				return true;
			}

			bool isValidSetValue = setValue == "0" || setValue == "1";
			bool isSetValue = setValue == "1";
			if (writeParameterId == outputTableInfo.LockedWriteParameterId)
			{
				if (isValidSetValue && key < displayedOutputs && outputs[key].IsLocked != isSetValue && outputs[key].IsEnabled)
				{
					OnLockSetFromUI(new MatrixLockSetFromUIMessage(key, MatrixIOType.Output, isSetValue));
				}

				return true;
			}

			if (writeParameterId == outputTableInfo.EnabledWriteParameterId)
			{
				if (isValidSetValue && key < displayedOutputs && outputs[key].IsEnabled != isSetValue)
				{
					OnStateSetFromUI(new MatrixIOStateSetFromUIMessage(key, MatrixIOType.Output, isSetValue));
				}

				return true;
			}

			if (writeParameterId == outputTableInfo.ConnectedWriteParameterId)
			{
				ProcessConnectedWriteParameterValue(key, setValue);
				return true;
			}

			return false;
		}

		private void ProcessConnectedWriteParameterValue(int output, string setValue)
		{
			if (output >= displayedOutputs)
			{
				return;
			}

			int input = 0;

			bool isValidInput = String.IsNullOrEmpty(setValue) ? true : Int32.TryParse(setValue, out input);

			if (!isValidInput || input > displayedInputs || outputs[output].IsLocked || !outputs[output].IsEnabled)
			{
				return;
			}

			IList<int> connectedInputs = outputs[output].ConnectedInputs;
			List<MatrixCrossPointSetFromUI> updatedCrossPoints = new List<MatrixCrossPointSetFromUI>();
			if (input > 0)
			{
				input--;
				if (connectedInputs.Contains(input) || inputs[input].IsLocked || !inputs[input].IsEnabled)
				{
					return; // input already connected or not available on UI
				}
				else
				{
					updatedCrossPoints.Add(new MatrixCrossPointSetFromUI(input, output, MatrixCrossPointConnectionState.Connected));
				}
			}

			DisconnectConnectedInputs(connectedInputs, output, updatedCrossPoints);

			if (updatedCrossPoints.Any())
			{
				OnCrossPointsSetFromUI(new MatrixCrossPointsSetFromUIMessage(updatedCrossPoints));
			}
		}

		private void DisconnectConnectedInputs(ICollection<int> connectedInputs, int output, ICollection<MatrixCrossPointSetFromUI> updatedCrossPoints)
		{
			// disconnect all connected inputs, except when the input was locked
			foreach (int connectedInput in connectedInputs)
			{
				if (connectedInput >= 0 && connectedInput < MaxInputs)
				{
					if (inputs[connectedInput].IsLocked)
					{
						updatedCrossPoints.Clear();
						return;
					}

					updatedCrossPoints.Add(new MatrixCrossPointSetFromUI(connectedInput, output, MatrixCrossPointConnectionState.Disconnected));
				}
			}
		}

		private bool ApplyMatrixChanges(SLProtocol protocol, IDictionary<string, string> allChangedLabels, IDictionary<string, string> allChangedLocks, IDictionary<string, string> allChangedStates, bool isChangeOriginalConnection)
		{
			int totalChangeCount = allChangedLabels.Count + allChangedLocks.Count + allChangedStates.Count;
			if (changeMatrixSize)
			{
				totalChangeCount++;
			}

			if (totalChangeCount == 0)
			{
				return false;
			}

			object[] elementInfo = new object[totalChangeCount];
			object[] sourceValue = new object[totalChangeCount];

			int changeCount = 0;
			const uint disableDiscreetInfoTrigger = 1;    // value 1 will disable triggering the discreet info parameter. We are sending the notify and need to be able to distinguish with the changes coming from the operator sets.

			foreach (KeyValuePair<string, string> kvp in allChangedLabels)
			{
				elementInfo[changeCount] = new[] { (uint)ChangeType.Label, (uint)elementId, (uint)matrixReadParameterId, (uint)agentId, disableDiscreetInfoTrigger };
				sourceValue[changeCount] = new[] { kvp.Key, kvp.Value };

				changeCount++;
			}

			foreach (KeyValuePair<string, string> kvp in allChangedLocks)
			{
				elementInfo[changeCount] = new[] { (uint)ChangeType.Lock, (uint)elementId, (uint)matrixReadParameterId, (uint)agentId, disableDiscreetInfoTrigger };
				sourceValue[changeCount] = new[] { kvp.Key, kvp.Value };

				changeCount++;
			}

			foreach (KeyValuePair<string, string> kvp in allChangedStates)
			{
				elementInfo[changeCount] = new[] { (uint)ChangeType.State, (uint)elementId, (uint)matrixReadParameterId, (uint)agentId, disableDiscreetInfoTrigger };
				sourceValue[changeCount] = new[] { kvp.Key, kvp.Value };

				changeCount++;
			}

			if (changeMatrixSize)
			{
				elementInfo[changeCount] = new[] { (uint)ChangeType.Size, (uint)elementId, (uint)matrixReadParameterId, (uint)agentId, disableDiscreetInfoTrigger };
				sourceValue[changeCount] = new[] { Convert.ToString(displayedInputs, CultureInfo.InvariantCulture), Convert.ToString(displayedOutputs, CultureInfo.InvariantCulture) };
				if (isChangeOriginalConnection)
				{
					changeMatrixSize = false;
				}
			}

			protocol.NotifyDataMinerQueued((int)NotifyType.UpdatePortsXml, elementInfo, sourceValue);
			return true;
		}

		private void ExecuteConnectSets(SLProtocol protocol, int outputNumber, ICollection<int> originalInputs, ISet<int> updatedInputs, ICollection<int[]> updatedCrossPoints, bool originalInputsFound)
		{
			foreach (int inputIndex in updatedInputs)
			{
				if (inputIndex != -1 && (!originalInputsFound || !originalInputs.Contains(inputIndex)))
				{
					int inputNumber = inputIndex + 1;

					protocol.SetParameterIndex(matrixReadParameterId, inputNumber, outputNumber, 1);
					updatedCrossPoints.Add(new[] { inputNumber, outputNumber });
				}
			}
		}

		private bool ExecuteDisconnectSets(SLProtocol protocol, int outputNumber, ICollection<int> originalInputs, ISet<int> updatedInputs, ICollection<int[]> updatedCrossPoints)
		{
			bool wasConnected = false;
			foreach (int inputIndex in originalInputs)
			{
				if (inputIndex != -1 && !updatedInputs.Contains(inputIndex))
				{
					int inputNumber = inputIndex + 1;

					protocol.SetParameterIndex(matrixReadParameterId, inputNumber, outputNumber, 0);

					updatedCrossPoints.Add(new[] { inputNumber, outputNumber });
					wasConnected = true;
				}
			}

			return wasConnected;
		}

		private void ExecuteSendToDisplay(SLProtocol protocol, bool fullSendToDisplay, bool isFullSendToDisplayNeeded, ICollection<int[]> updatedCrossPoints)
		{
			if (fullSendToDisplay && isFullSendToDisplayNeeded)
			{
				protocol.SendToDisplay(matrixReadParameterId);
			}
			else
			{
				int[] changedInputs = new int[updatedCrossPoints.Count];
				int[] changedOutputs = new int[updatedCrossPoints.Count];
				int changedCount = 0;

				foreach (int[] changedPoint in updatedCrossPoints)
				{
					changedInputs[changedCount] = changedPoint[0];
					changedOutputs[changedCount] = changedPoint[1];
					changedCount++;
				}

				protocol.SendToDisplay(matrixReadParameterId, changedInputs, changedOutputs); // If version before 9.0 CU[2] then replace with protocol.SendToDisplay(matrixPid)
			}
		}

		private bool ExecuteChangedMatrixSets(SLProtocol protocol, bool isChangeOriginalConnection)
		{
			var updatedConnections = portState.Connections.UpdatedConnections;

			if (updatedConnections.Count <= 0)
			{
				return false;
			}

			List<int[]> updatedCrossPoints = new List<int[]>();

			bool fullSendToDisplay = false; // Can be removed if DCP task 86217 is fixed.

			foreach (KeyValuePair<int, HashSet<int>> kvp in updatedConnections)
			{
				int outputNumber = kvp.Key + 1;

				HashSet<int> originalInputs;
				bool originalInputsFound = false;
				bool wasConnected = false; // Can be removed if DCP task 86217 is fixed.

				if (portState.Connections.OriginalConnections.TryGetValue(kvp.Key, out originalInputs))
				{
					originalInputsFound = true;
					wasConnected = ExecuteDisconnectSets(protocol, outputNumber, originalInputs, kvp.Value, updatedCrossPoints);
				}

				ExecuteConnectSets(protocol, outputNumber, originalInputs, kvp.Value, updatedCrossPoints, originalInputsFound);

				if (!wasConnected)
				{
					fullSendToDisplay = true;
				}

				if (isChangeOriginalConnection)
				{
					portState.Connections.OriginalConnections[kvp.Key] = kvp.Value;
				}
			}

			if (isChangeOriginalConnection)
			{
				portState.Connections.UpdatedConnections.Clear();
			}

			if (updatedCrossPoints.Any())
			{
				ExecuteSendToDisplay(protocol, fullSendToDisplay, isFullSendToDisplayNeeded, updatedCrossPoints);
			}

			if (isChangeOriginalConnection)
			{
				UpdateConnectionsBufferParameter(protocol);
				return true;
			}

			return false;
		}

		private void GetChangedConnections(SLProtocol protocol)
		{
			if (portState.Connections.UpdatedConnections.Any())
			{
				foreach (KeyValuePair<int, HashSet<int>> kvp in portState.Connections.UpdatedConnections)
				{
					portState.Connections.OriginalConnections[kvp.Key] = kvp.Value;
				}

				portState.Connections.UpdatedConnections.Clear();
				UpdateConnectionsBufferParameter(protocol);
			}
		}

		private void ExecuteFullMatrixSets(SLProtocol protocol)
		{
			if (portState.Connections.OriginalConnections.Count == 0)
			{
				return;
			}

			List<int[]> updatedCrossPoints = new List<int[]>();
			foreach (KeyValuePair<int, HashSet<int>> kvp in portState.Connections.OriginalConnections)
			{
				int outputNumber = kvp.Key + 1;
				foreach (int inputIndex in kvp.Value)
				{
					if (inputIndex != -1)
					{
						int inputNumber = inputIndex + 1;
						protocol.SetParameterIndex(matrixReadParameterId, inputNumber, outputNumber, 1);
						updatedCrossPoints.Add(new[] { inputNumber, outputNumber });
					}
				}
			}

			if (updatedCrossPoints.Count == 0)
			{
				return;
			}

			if (isFullSendToDisplayNeeded)
			{
				protocol.SendToDisplay(matrixReadParameterId); // Can be removed if DCP task 86217 is fixed.
			}
			else
			{
				int[] changedInputs = new int[updatedCrossPoints.Count];
				int[] changedOutputs = new int[updatedCrossPoints.Count];
				int changedCount = 0;

				foreach (int[] changedPoint in updatedCrossPoints)
				{
					changedInputs[changedCount] = changedPoint[0];
					changedOutputs[changedCount] = changedPoint[1];
					changedCount++;
				}

				protocol.SendToDisplay(matrixReadParameterId, changedInputs, changedOutputs); // If version before 9.0 CU[2] then replace with protocol.SendToDisplay(matrixPid)
			}
		}

		private void ClearOriginalMatrixSets(SLProtocol protocol)
		{
			if (portState.Connections.OriginalConnections.Count == 0)
			{
				return;
			}

			List<int[]> updatedCrossPoints = new List<int[]>();
			foreach (KeyValuePair<int, HashSet<int>> kvp in portState.Connections.OriginalConnections)
			{
				int outputNumber = kvp.Key + 1;
				foreach (int inputIndex in kvp.Value)
				{
					if (inputIndex != -1)
					{
						int inputNumber = inputIndex + 1;
						protocol.SetParameterIndex(matrixReadParameterId, inputNumber, outputNumber, 0);
						updatedCrossPoints.Add(new[] { inputNumber, outputNumber });
					}
				}
			}

			if (updatedCrossPoints.Count == 0)
			{
				return;
			}

			if (isFullSendToDisplayNeeded)
			{
				protocol.SendToDisplay(matrixReadParameterId);
			}
			else
			{
				int[] changedInputs = new int[updatedCrossPoints.Count];
				int[] changedOutputs = new int[updatedCrossPoints.Count];
				int changedCount = 0;

				foreach (int[] changedPoint in updatedCrossPoints)
				{
					changedInputs[changedCount] = changedPoint[0];
					changedOutputs[changedCount] = changedPoint[1];
					changedCount++;
				}

				protocol.SendToDisplay(matrixReadParameterId, changedInputs, changedOutputs); // If version before 9.0 CU[2] then replace with protocol.SendToDisplay(matrixPid)
			}
		}

		private void ClearOriginalTableSets(SLProtocol protocol)
		{
			ClearTable(protocol, inputTableInfo.TableParameterId);
			ClearTable(protocol, outputTableInfo.TableParameterId);
		}

		private void ApplyTableFullSet(SLProtocol protocol)
		{
			if (displayedInputs > 0)
			{
				ApplyTableInputSet(protocol);
			}

			if (displayedOutputs > 0)
			{
				ApplyTableOutputSet(protocol);
			}
		}

		private void ApplyTableInputSet(SLProtocol protocol)
		{
			object[] setInputKeyCol = new object[displayedInputs];
			object[] setInputLabelCol = new object[displayedInputs];
			object[] setInputLockCol = new object[displayedInputs];
			object[] setInputStateCol = new object[displayedInputs];
			for (int i = 0; i < displayedInputs; i++)
			{
				setInputKeyCol[i] = Convert.ToString(i + 1, CultureInfo.InvariantCulture);
				setInputLabelCol[i] = inputs[i].Label;
				setInputLockCol[i] = inputs[i].IsLocked ? "1" : "0";
				setInputStateCol[i] = inputs[i].IsEnabled ? "1" : "0";
			}

			object[] setTable = new object[4];
			setTable[0] = setInputKeyCol;
			setTable[1] = setInputLabelCol;
			setTable[2] = setInputStateCol;
			setTable[3] = setInputLockCol;
			protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, new object[] { inputTableInfo.TableParameterId, inputTableInfo.LabelParameterId, inputTableInfo.EnabledParameterId, inputTableInfo.LockedParameterId }, setTable);
		}

		private void ApplyTableOutputSet(SLProtocol protocol)
		{
			object[] setOutputKeyCol = new object[displayedOutputs];
			object[] setOutputLabelCol = new object[displayedOutputs];
			object[] setOutputLockCol = new object[displayedOutputs];
			object[] setOutputStateCol = new object[displayedOutputs];
			object[] setOutputConnectedInputCol = new object[displayedOutputs];
			for (int i = 0; i < displayedOutputs; i++)
			{
				setOutputKeyCol[i] = Convert.ToString(i + 1, CultureInfo.InvariantCulture);
				setOutputLabelCol[i] = outputs[i].Label;
				setOutputLockCol[i] = outputs[i].IsLocked ? "1" : "0";
				setOutputStateCol[i] = outputs[i].IsEnabled ? "1" : "0";
				setOutputConnectedInputCol[i] = ApplyTableOutputConnectedInputSet(i);
			}

			object[] setTable = new object[5];
			setTable[0] = setOutputKeyCol;
			setTable[1] = setOutputLabelCol;
			setTable[2] = setOutputStateCol;
			setTable[3] = setOutputLockCol;
			setTable[4] = setOutputConnectedInputCol;
			protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, new object[] { outputTableInfo.TableParameterId, outputTableInfo.LabelParameterId, outputTableInfo.EnabledParameterId, outputTableInfo.LockedParameterId, outputTableInfo.ConnectedParameterId }, setTable);
		}

		private string ApplyTableOutputConnectedInputSet(int outputNumber)
		{
			HashSet<int> connectedInputs;
			if (!portState.Connections.OriginalConnections.TryGetValue(outputNumber, out connectedInputs))
			{
				return null;
			}

			foreach (int connectedInput in connectedInputs)
			{
				return connectedInput == -1 ? String.Empty : Convert.ToString(connectedInput + 1, CultureInfo.InvariantCulture);
			}

			return String.Empty;
		}

		private void ApplyMatrixFullSet(SLProtocol protocol)
		{
			GetElementProtocolMessage message = new GetElementProtocolMessage(agentId, elementId);
			GetElementProtocolResponseMessage elementProtocolResponseMessage = (GetElementProtocolResponseMessage)protocol.SLNet.SendSingleResponseMessage(message);
			if (elementProtocolResponseMessage == null)
			{
				throw new ArgumentException("Matrix cannot be retrieved");
			}

			foreach (ParameterInfo paramInfo in elementProtocolResponseMessage.Parameters)
			{
				if (paramInfo.ID != matrixReadParameterId || !paramInfo.IsMatrix || paramInfo.WriteType)
				{
					continue;
				}

				bool isChangeSize = true;
				if (!String.IsNullOrEmpty(paramInfo.PhysicalSize))
				{
					string[] dimensions = paramInfo.PhysicalSize.Split(';');
					if (dimensions.Length > 1)
					{
						int currentDisplayedInputs = Convert.ToInt32(dimensions[0], CultureInfo.InvariantCulture);
						int currentDisplayedOutputs = Convert.ToInt32(dimensions[1], CultureInfo.InvariantCulture);
						isChangeSize = !(displayedInputs == currentDisplayedInputs && displayedOutputs == currentDisplayedOutputs);
					}
				}

				changeMatrixSize = isChangeSize;
				ApplyMatrixFullSetProcessParam(protocol, paramInfo);
				break;
			}

			ExecuteFullMatrixSets(protocol);
		}

		private void ApplyMatrixFullSetProcessDiscreet(ParameterDiscreet discreetEntry, int key, IDictionary<string, string> allChangedLabels, IDictionary<string, string> allChangedLocks, IDictionary<string, string> allChangedStates)
		{
			bool isOutput = key > MaxInputs;
			key--;
			string label = discreetEntry.Display;
			bool isEnabled = !discreetEntry.State.Equals("disabled", StringComparison.OrdinalIgnoreCase);
			bool isLocked = MatrixLocks.GetMatrixLockFromOptions(discreetEntry.Options);

			MatrixPort matrixPort;
			if (isOutput)
			{
				key -= MaxInputs;
				matrixPort = outputs[key];
			}
			else
			{
				matrixPort = inputs[key];
			}

			if (matrixPort.Label != label)
			{
				allChangedLabels[discreetEntry.Value] = matrixPort.Label;
			}

			if (matrixPort.IsEnabled != isEnabled)
			{
				allChangedStates[discreetEntry.Value] = matrixPort.IsEnabled ? "enabled" : "disabled";
			}

			if (matrixPort.IsLocked != isLocked)
			{
				allChangedLocks[discreetEntry.Value] = matrixPort.IsLocked ? "true" : "false";
			}
		}

		private void ApplyMatrixFullSetProcessParam(SLProtocol protocol, ParameterInfo paramInfo)
		{
			Dictionary<string, string> allChangedLabels = new Dictionary<string, string>();
			Dictionary<string, string> allChangedLocks = new Dictionary<string, string>();
			Dictionary<string, string> allChangedStates = new Dictionary<string, string>();
			foreach (ParameterDiscreet discreetEntry in paramInfo.Discreets)
			{
				int key;
				if (!Int32.TryParse(discreetEntry.Value, out key))
				{
					continue;
				}

				ApplyMatrixFullSetProcessDiscreet(discreetEntry, key, allChangedLabels, allChangedLocks, allChangedStates);
			}

			ApplyMatrixChanges(protocol, allChangedLabels, allChangedLocks, allChangedStates, true);
		}

		private void UpdateConnectionsBufferParameter(SLProtocol protocol)
		{
			StringBuilder bufferValue = new StringBuilder();

			for (int i = 0; i < MaxOutputs; i++)
			{
				if (i != 0)
				{
					bufferValue.Append(";");
				}

				HashSet<int> originalInputs;
				if (portState.Connections.OriginalConnections.TryGetValue(i, out originalInputs))
				{
					bufferValue.Append(String.Join(":", originalInputs));
				}
				else
				{
					bufferValue.Append("-1");
				}
			}

			bufferValue.Append("|");
			bufferValue.Append(agentId);
			bufferValue.Append("/");
			bufferValue.Append(elementId);
			bufferValue.Append("|");
			bufferValue.Append(tableVirtualSetParameterId > 0 ? Convert.ToString(tableVirtualSetParameterId, CultureInfo.InvariantCulture) : Convert.ToString(matrixWriteParameterId, CultureInfo.InvariantCulture));
			bufferValue.Append("|");
			bufferValue.Append(MaxInputs);

			protocol.SetParameter(matrixConnectionsBufferParameterId, bufferValue.ToString());
		}

		private void UpdateSerializedMatrixParameter(SLProtocol protocol)
		{
			if (matrixSerializedParameterId < 1)
			{
				return;
			}

			MatrixStatus matrixStatus = new MatrixStatus(inputs, outputs)
			{
				DmaId = agentId,
				DisplayType = displayType,
				DisplayedInputs = displayedInputs,
				DisplayedOutputs = displayedOutputs,
				ElementId = elementId,
				ElementName = elementName,
				MatrixWritePid = tableVirtualSetParameterId > 0 ? tableVirtualSetParameterId : matrixWriteParameterId,
				MaxConnectedInputsPerOutput = MaxConnectedInputsPerOutput,
				MaxConnectedOutputsPerInput = MaxConnectedOutputsPerInput,
				MaxInputs = MaxInputs,
				MaxOutputs = MaxOutputs,
				MinConnectedInputsPerOutput = MinConnectedInputsPerOutput,
				MinConnectedOutputsPerInput = MinConnectedOutputsPerInput
			};

			string serialized = JsonConvert.SerializeObject(matrixStatus);
			protocol.SetParameter(matrixSerializedParameterId, serialized);
		}

		private bool ApplyTableChanges(SLProtocol protocol, Dictionary<string, string> allChangedInputLabels, Dictionary<string, string> allChangedOutputLabels, Dictionary<string, string> allChangedInputLocks, Dictionary<string, string> allChangedOutputLocks, Dictionary<string, string> allChangedInputStates, Dictionary<string, string> allChangedOutputStates)
		{
			// remark: will not be using protocol.Leave yet because of RN 19707, will be changed later when 9.6.0 Main release is the minimum supported DMA version
			bool isTableChanged = false;
			bool isInputItemChanged = allChangedInputLabels.Any() || allChangedInputLocks.Any() || allChangedInputStates.Any();
			if (isInputItemChanged || changeMatrixSize)
			{
				ApplyTableChangesInput(protocol, allChangedInputLabels, allChangedInputLocks, allChangedInputStates);
				isTableChanged = true;
			}

			bool isOutputItemChanged = allChangedOutputLabels.Any() || allChangedOutputLocks.Any() || allChangedOutputStates.Any() || portState.Connections.UpdatedConnections.Any();
			if (isOutputItemChanged || changeMatrixSize)
			{
				ApplyTableChangesOutput(protocol, allChangedOutputLabels, allChangedOutputLocks, allChangedOutputStates);
				isTableChanged = true;
			}

			changeMatrixSize = false;
			return isTableChanged;
		}

		private void ApplyTableChangesInput(SLProtocol protocol, Dictionary<string, string> allChangedInputLabels, Dictionary<string, string> allChangedInputLocks, Dictionary<string, string> allChangedInputStates)
		{
			if (changeMatrixSize)
			{
				ApplyTableChangesInputSizeChange(protocol, allChangedInputLabels, allChangedInputLocks, allChangedInputStates);
			}

			HashSet<string> neededInputKeys = new HashSet<string>();
			GetNeededKeys(neededInputKeys, allChangedInputLabels, displayedInputs);
			GetNeededKeys(neededInputKeys, allChangedInputLocks, displayedInputs);
			GetNeededKeys(neededInputKeys, allChangedInputStates, displayedInputs);
			if (neededInputKeys.Count == 0)
			{
				return;
			}

			object[] setInputKeyCol = new object[neededInputKeys.Count];
			object[] setInputLabelCol = new object[neededInputKeys.Count];
			object[] setInputLockCol = new object[neededInputKeys.Count];
			object[] setInputStateCol = new object[neededInputKeys.Count];
			int count = 0;
			foreach (string key in neededInputKeys)
			{
				setInputKeyCol[count] = key;
				MatrixPort matrixPort = inputs[Convert.ToInt32(key, CultureInfo.InvariantCulture) - 1];
				SetColumnValue(key, count, matrixPort, setInputLabelCol, allChangedInputLabels, "Label");
				SetColumnValue(key, count, matrixPort, setInputLockCol, allChangedInputLocks, "IsLocked");
				SetColumnValue(key, count, matrixPort, setInputStateCol, allChangedInputStates, "IsEnabled");
				count++;
			}

			object[] setTable = new object[4];
			setTable[0] = setInputKeyCol;
			setTable[1] = setInputLabelCol;
			setTable[2] = setInputStateCol;
			setTable[3] = setInputLockCol;
			protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, new object[] { inputTableInfo.TableParameterId, inputTableInfo.LabelParameterId, inputTableInfo.EnabledParameterId, inputTableInfo.LockedParameterId }, setTable);
		}

		private void ApplyTableChangesInputSizeChange(SLProtocol protocol, IDictionary<string, string> allChangedInputLabels, IDictionary<string, string> allChangedInputLocks, IDictionary<string, string> allChangedInputStates)
		{
			object[] tableCols = (object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, inputTableInfo.TableParameterId, new uint[] { 0 });
			if (tableCols == null || tableCols.Length <= 0 || tableCols[0] == null)
			{
				return;
			}

			HashSet<string> rowsToBeDeleted = new HashSet<string>();
			int maxFoundPK = ProcessPkColumn(rowsToBeDeleted, (object[])tableCols[0], displayedInputs);

			for (int i = maxFoundPK + 1; i <= displayedInputs; i++)
			{
				// when table has grown, then keys need to be added, flagged as changed items
				ProcessTableExpanded(Convert.ToString(i, CultureInfo.InvariantCulture), inputs[i - 1], allChangedInputLabels, allChangedInputLocks, allChangedInputStates);
			}

			DeleteRows(protocol, inputTableInfo.TableParameterId, rowsToBeDeleted);   // when table has shrunk, then keys need to be removed
		}

		private void SetColumnValue(string key, int count, object[] setCol)
		{
			int outputNumber = Convert.ToInt32(key, CultureInfo.InvariantCulture) - 1;
			bool isProcessInputs;
			bool isReplaceOriginalConnections;
			HashSet<int> connectedInputs;
			if (portState.Connections.UpdatedConnections.TryGetValue(outputNumber, out connectedInputs))
			{
				isProcessInputs = true;
				isReplaceOriginalConnections = true;
			}
			else
			{
				isProcessInputs = portState.Connections.OriginalConnections.TryGetValue(outputNumber, out connectedInputs);
				isReplaceOriginalConnections = false;
			}

			if (!isProcessInputs)
			{
				return;
			}

			int foundInputs = 0;
			foreach (int connectedInput in connectedInputs)
			{
				foundInputs++;

				setCol[count] = connectedInput == -1 ? String.Empty : Convert.ToString(connectedInput + 1, CultureInfo.InvariantCulture);

				if (foundInputs == 1)
				{
					break;  // only one input connected per output is supported
				}
			}

			if (foundInputs == 0)
			{
				setCol[count] = String.Empty;
			}

			if (isReplaceOriginalConnections)
			{
				portState.Connections.OriginalConnections[outputNumber] = connectedInputs;
			}
		}

		private void ApplyTableChangesOutput(SLProtocol protocol, Dictionary<string, string> allChangedOutputLabels, Dictionary<string, string> allChangedOutputLocks, Dictionary<string, string> allChangedOutputStates)
		{
			if (changeMatrixSize)
			{
				ApplyTableChangesOutputSizeChange(protocol, allChangedOutputLabels, allChangedOutputLocks, allChangedOutputStates);
			}

			bool isUpdateConnectionsBufferParameter = portState.Connections.UpdatedConnections.Any();
			HashSet<string> neededOutputKeys = new HashSet<string>();
			GetNeededKeys(neededOutputKeys, allChangedOutputLabels, displayedOutputs);
			GetNeededKeys(neededOutputKeys, allChangedOutputLocks, displayedOutputs);
			GetNeededKeys(neededOutputKeys, allChangedOutputStates, displayedOutputs);
			GetNeededKeys(neededOutputKeys, portState.Connections.UpdatedConnections, displayedOutputs);

			if (neededOutputKeys.Count == 0)
			{
				return;
			}

			object[] setOutputKeyCol = new object[neededOutputKeys.Count];
			object[] setOutputLabelCol = new object[neededOutputKeys.Count];
			object[] setOutputLockCol = new object[neededOutputKeys.Count];
			object[] setOutputStateCol = new object[neededOutputKeys.Count];
			object[] setOutputConnectedInputCol = new object[neededOutputKeys.Count];
			int count = 0;
			foreach (string key in neededOutputKeys)
			{
				setOutputKeyCol[count] = key;
				MatrixPort matrixPort = outputs[Convert.ToInt32(key, CultureInfo.InvariantCulture) - 1];
				SetColumnValue(key, count, matrixPort, setOutputLabelCol, allChangedOutputLabels, "Label");
				SetColumnValue(key, count, matrixPort, setOutputLockCol, allChangedOutputLocks, "IsLocked");
				SetColumnValue(key, count, matrixPort, setOutputStateCol, allChangedOutputStates, "IsEnabled");
				SetColumnValue(key, count, setOutputConnectedInputCol);
				count++;
			}

			portState.Connections.UpdatedConnections.Clear();
			if (isUpdateConnectionsBufferParameter)
			{
				UpdateConnectionsBufferParameter(protocol);
			}

			object[] setTable = new object[5];
			setTable[0] = setOutputKeyCol;
			setTable[1] = setOutputLabelCol;
			setTable[2] = setOutputStateCol;
			setTable[3] = setOutputLockCol;
			setTable[4] = setOutputConnectedInputCol;
			protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, new object[] { outputTableInfo.TableParameterId, outputTableInfo.LabelParameterId, outputTableInfo.EnabledParameterId, outputTableInfo.LockedParameterId, outputTableInfo.ConnectedParameterId }, setTable);
		}

		private void ApplyTableChangesOutputSizeChange(SLProtocol protocol, IDictionary<string, string> allChangedOutputLabels, IDictionary<string, string> allChangedOutputLocks, IDictionary<string, string> allChangedOutputStates)
		{
			object[] tableCols = (object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, outputTableInfo.TableParameterId, new uint[] { 0 });
			if (tableCols == null || tableCols.Length <= 0 || tableCols[0] == null)
			{
				return;
			}

			HashSet<string> rowsToBeDeleted = new HashSet<string>();
			int maxFoundPK = ProcessPkColumn(rowsToBeDeleted, (object[])tableCols[0], displayedOutputs);

			for (int i = maxFoundPK + 1; i <= displayedOutputs; i++)
			{
				// when table has grown, then keys need to be added, flagged as changed items
				ProcessTableExpanded(Convert.ToString(i, CultureInfo.InvariantCulture), outputs[i - 1], allChangedOutputLabels, allChangedOutputLocks, allChangedOutputStates);

				HashSet<int> connectedInputs;
				if (!portState.Connections.UpdatedConnections.ContainsKey(i - 1) && portState.Connections.OriginalConnections.TryGetValue(i - 1, out connectedInputs))
				{
					portState.Connections.UpdatedConnections[i - 1] = new HashSet<int>(connectedInputs);
				}
			}

			DeleteRows(protocol, outputTableInfo.TableParameterId, rowsToBeDeleted);
		}

		private void ProcessLabelChange(string[] discreteInfoParts, SLProtocol protocol)
		{
			int portNumber; // One-based port number in range [1, maxInputs+maxOutputs].
			bool validPortNumber = Int32.TryParse(discreteInfoParts[2], out portNumber);
			string newLabel = discreteInfoParts[3];

			if (!validPortNumber || portNumber <= 0 || String.IsNullOrEmpty(newLabel))
			{
				return;
			}

			portNumber--; // Internally, zero-based indexes are used.

			if (portNumber >= MaxInputs)
			{
				ProcessLabelChangeOutput(protocol, portNumber - MaxInputs, newLabel);
			}
			else
			{
				ProcessLabelChangeInput(protocol, portNumber, newLabel);
			}
		}

		private void ProcessLabelChangeInput(SLProtocol protocol, int inputIndex, string newLabel)
		{
			if (!portState.InputLabels.UpdateOriginal(inputIndex, newLabel))
			{
				return;
			}

			if (displayType != MatrixDisplayType.Matrix && inputTableInfo.TableParameterId > 0 && inputTableInfo.LabelColumnIdx > 0)
			{
				string key = Convert.ToString(inputIndex + 1, CultureInfo.InvariantCulture);
				if (protocol.GetKeyPosition(inputTableInfo.TableParameterId, key) != 0)
				{
					protocol.SetParameterIndexByKey(inputTableInfo.TableParameterId, key, Convert.ToInt32(inputTableInfo.LabelColumnIdx) + 1, newLabel);
				}
			}

			OnLabelSetFromUI(new MatrixLabelSetFromUIMessage(inputIndex, MatrixIOType.Input, newLabel));
		}

		private void ProcessLabelChangeOutput(SLProtocol protocol, int outputIndex, string newLabel)
		{
			if (!portState.OutputLabels.UpdateOriginal(outputIndex, newLabel))
			{
				return;
			}

			if (displayType != MatrixDisplayType.Matrix && outputTableInfo.TableParameterId > 0 && outputTableInfo.LabelColumnIdx > 0)
			{
				string key = Convert.ToString(outputIndex + 1, CultureInfo.InvariantCulture);
				if (protocol.GetKeyPosition(outputTableInfo.TableParameterId, key) != 0)
				{
					protocol.SetParameterIndexByKey(outputTableInfo.TableParameterId, key, Convert.ToInt32(outputTableInfo.LabelColumnIdx) + 1, newLabel);
				}
			}

			OnLabelSetFromUI(new MatrixLabelSetFromUIMessage(outputIndex, MatrixIOType.Output, newLabel));
		}

		private void ProcessLockChange(string[] discreteInfoParts, SLProtocol protocol)
		{
			int portNumber; // One-based port number in range [1, maxInputs+maxOutputs].
			bool validPortNumber = Int32.TryParse(discreteInfoParts[2], out portNumber);

			string portLocks = discreteInfoParts[3].ToLower(CultureInfo.InvariantCulture);

			if (!validPortNumber || portNumber <= 0 || String.IsNullOrEmpty(portLocks))
			{
				return;
			}

			portNumber--;

			bool isLocked = portLocks.Contains("true");

			if (portNumber >= MaxInputs)
			{
				ProcessLockChangeOutput(protocol, portNumber - MaxInputs, isLocked);
			}
			else
			{
				ProcessLockChangeInput(protocol, portNumber, isLocked);
			}
		}

		private void ProcessLockChangeInput(SLProtocol protocol, int inputIndex, bool isLocked)
		{
			if (!portState.InputLocks.UpdateOriginal(inputIndex, isLocked))
			{
				return;
			}

			if (displayType != MatrixDisplayType.Matrix && inputTableInfo.TableParameterId > 0 && inputTableInfo.LockedColumnIdx > 0)
			{
				string key = Convert.ToString(inputIndex + 1, CultureInfo.InvariantCulture);
				if (protocol.GetKeyPosition(inputTableInfo.TableParameterId, key) != 0)
				{
					protocol.SetParameterIndexByKey(inputTableInfo.TableParameterId, key, Convert.ToInt32(inputTableInfo.LockedColumnIdx) + 1, isLocked ? "1" : "0");
				}
			}

			OnLockSetFromUI(new MatrixLockSetFromUIMessage(inputIndex, MatrixIOType.Input, isLocked));
		}

		private void ProcessLockChangeOutput(SLProtocol protocol, int outputIndex, bool isLocked)
		{
			if (!portState.OutputLocks.UpdateOriginal(outputIndex, isLocked))
			{
				return;
			}

			if (displayType != MatrixDisplayType.Matrix && outputTableInfo.TableParameterId > 0 && outputTableInfo.LockedColumnIdx > 0)
			{
				string key = Convert.ToString(outputIndex + 1, CultureInfo.InvariantCulture);
				if (protocol.GetKeyPosition(outputTableInfo.TableParameterId, key) != 0)
				{
					protocol.SetParameterIndexByKey(outputTableInfo.TableParameterId, key, Convert.ToInt32(outputTableInfo.LockedColumnIdx) + 1, isLocked ? "1" : "0");
				}
			}

			OnLockSetFromUI(new MatrixLockSetFromUIMessage(outputIndex, MatrixIOType.Output, isLocked));
		}

		private void ProcessStateChange(string[] discreteInfoParts, SLProtocol protocol)
		{
			int portNumber; // One-based port number in range [1, maxInputs+maxOutputs].
			bool validPortNumber = Int32.TryParse(discreteInfoParts[2], out portNumber);

			string portStateValue = discreteInfoParts[3].ToLower(CultureInfo.InvariantCulture);

			if (!validPortNumber || portNumber <= 0 || String.IsNullOrEmpty(portStateValue))
			{
				return;
			}

			portNumber--;

			bool isEnabled = portStateValue.Contains("enabled");

			if (portNumber >= MaxInputs)
			{
				ProcessStateChangeOutput(protocol, portNumber - MaxInputs, isEnabled);
			}
			else
			{
				ProcessStateChangeInput(protocol, portNumber, isEnabled);
			}
		}

		private void ProcessStateChangeInput(SLProtocol protocol, int inputIndex, bool isEnabled)
		{
			if (!portState.InputStates.UpdateOriginal(inputIndex, isEnabled))
			{
				return;
			}

			if (displayType != MatrixDisplayType.Matrix && inputTableInfo.TableParameterId > 0 && inputTableInfo.EnabledColumnIdx > 0)
			{
				string key = Convert.ToString(inputIndex + 1, CultureInfo.InvariantCulture);
				if (protocol.GetKeyPosition(inputTableInfo.TableParameterId, key) != 0)
				{
					protocol.SetParameterIndexByKey(inputTableInfo.TableParameterId, key, Convert.ToInt32(inputTableInfo.EnabledColumnIdx) + 1, isEnabled ? "1" : "0");
				}
			}

			OnStateSetFromUI(new MatrixIOStateSetFromUIMessage(inputIndex, MatrixIOType.Input, isEnabled));
		}

		private void ProcessStateChangeOutput(SLProtocol protocol, int outputIndex, bool isEnabled)
		{
			if (!portState.OutputStates.UpdateOriginal(outputIndex, isEnabled))
			{
				return;
			}

			if (displayType != MatrixDisplayType.Matrix && outputTableInfo.TableParameterId > 0 && outputTableInfo.EnabledColumnIdx > 0)
			{
				string key = Convert.ToString(outputIndex + 1, CultureInfo.InvariantCulture);
				if (protocol.GetKeyPosition(outputTableInfo.TableParameterId, key) != 0)
				{
					protocol.SetParameterIndexByKey(outputTableInfo.TableParameterId, key, Convert.ToInt32(outputTableInfo.EnabledColumnIdx) + 1, isEnabled ? "1" : "0");
				}
			}

			OnStateSetFromUI(new MatrixIOStateSetFromUIMessage(outputIndex, MatrixIOType.Output, isEnabled));
		}

		private void ParseProtocolMatrixParameter(ParameterInfo parameter, bool isAutoDetectMatrix, MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters, MatrixHelperParameterNames matrixHelperParameterNames)
		{
			bool isMatrixParameter = isAutoDetectMatrix || (requestParameters.MatrixReadParameterId > 0 && parameter.ID == requestParameters.MatrixReadParameterId);
			if (!parameter.IsMatrix || !isMatrixParameter)
			{
				return;
			}

			if (isAutoDetectMatrix)
			{
				if (foundParameters.MatrixReadParameterId == -1)
				{
					foundParameters.MatrixReadParameterId = parameter.ID;
				}
				else
				{
					throw new ArgumentException("Automatic detection of the matrix parameter can only be done when there is only one matrix parameter, at least 2 have been discovered: " + Convert.ToString(foundParameters.MatrixReadParameterId, CultureInfo.InvariantCulture) + " and " + Convert.ToString(parameter.ID, CultureInfo.InvariantCulture));
				}
			}
			else
			{
				foundParameters.MatrixReadParameterId = parameter.ID;
			}

			// Obtain matrix parameter name.
			matrixHelperParameterNames.MatrixParameterName = parameter.Name;
			matrixReadParameterInfo = parameter;

			ParseMatrixSize(parameter);
			ParseMatrixSettings(parameter, requestParameters.OutputsTableParameterId);
		}

		private MatrixHelperParameterNames ParseProtocolGetParameterNames(ParameterInfo[] parameters, MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters, IDictionary<int, uint> inputColumns, IDictionary<int, uint> outputColumns, IDictionary<int, ParameterInfo> potentialTables)
		{
			MatrixHelperParameterNames matrixHelperParameterNames = new MatrixHelperParameterNames();
			bool isValidBufferParameter = false;
			bool isValidSerializedParameter = false;
			bool isAutoDetectMatrix = requestParameters.MatrixReadParameterId == -2;
			bool isAutoDetectMatrixConnectionsBuffer = requestParameters.MatrixConnectionsBufferParameterId == -2;
			bool isAutoDetectMatrixSerialized = requestParameters.MatrixSerializedParameterId == -2;
			bool isAutoDetectTable = requestParameters.InputsTableParameterId == -2 || requestParameters.OutputsTableParameterId == -2;
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameter = parameters[i];
				if (parameter.WriteType)
				{
					continue;
				}

				ParseProtocolMatrixParameter(parameter, isAutoDetectMatrix, requestParameters, foundParameters, matrixHelperParameterNames);
				ParseProtocolTableParameter(parameter, isAutoDetectTable, requestParameters, matrixHelperParameterNames, inputColumns, outputColumns, potentialTables);
				bool isMatrixConnectionsBufferParameter = parameter.ID == requestParameters.MatrixConnectionsBufferParameterId || (isAutoDetectMatrixConnectionsBuffer && parameter.Name.ToLower(CultureInfo.InvariantCulture) == "matrixconnectionsbuffer");
				if (!isValidBufferParameter && parameter.ParameterType == ParameterMeasurementType.String && isMatrixConnectionsBufferParameter)
				{
					foundParameters.MatrixConnectionsBufferParameterId = parameter.ID;
					isValidBufferParameter = true;
					continue;
				}

				bool isMatrixSerializedParameter = parameter.ID == requestParameters.MatrixSerializedParameterId || (isAutoDetectMatrixSerialized && parameter.Name.ToLower(CultureInfo.InvariantCulture) == "matrixserialized");
				if (!isValidSerializedParameter && parameter.ParameterType == ParameterMeasurementType.String && isMatrixSerializedParameter)
				{
					foundParameters.MatrixSerializedParameterId = parameter.ID;
					isValidSerializedParameter = true;
				}
			}

			VerifyValidParameters(isValidBufferParameter, isValidSerializedParameter, requestParameters.MatrixSerializedParameterId);
			return matrixHelperParameterNames;
		}

		private static void VerifyValidParameters(bool isValidBufferParameter, bool isValidSerializedParameter, int requestSerializedParameterId)
		{
			if (!isValidBufferParameter)
			{
				throw new ArgumentException("Invalid connection buffer parameter. Looking either for specified parameter ID or for single parameter name 'MatrixConnectionsBuffer'. This needs to be a displayed read parameter with String Measurement Type.");
			}

			if (!isValidSerializedParameter && requestSerializedParameterId > 0)
			{
				throw new ArgumentException("Invalid serialized matrix parameter id. Looking for specified parameter ID " + Convert.ToString(requestSerializedParameterId) + " but it does not seem to exist. This needs to be a displayed read parameter with String Measurement Type.");
			}
		}

		private void ParseProtocolAutoDetectTables(ParameterInfo[] parameters, MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters, IDictionary<int, ParameterInfo> potentialTables)
		{
			AssignCustomNumbersForTable(requestParameters);
			MatrixSearchTableInfo tablesWithLabel = new MatrixSearchTableInfo();
			MatrixSearchTableInfo tablesWithEnabled = new MatrixSearchTableInfo();
			MatrixSearchTableInfo tablesWithLocked = new MatrixSearchTableInfo();
			MatrixSearchTableInfo tablesWithConnected = new MatrixSearchTableInfo();
			MatrixSearchSingleSetInfo tablesWithSet = new MatrixSearchSingleSetInfo();

			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameter = parameters[i];
				if (parameter.ArrayType)
				{
					continue;
				}

				string parameterName = parameter.Name.ToLower(CultureInfo.InvariantCulture);
				if (parameter.ParameterType == ParameterMeasurementType.String)
				{
					SearchPotentialTables("label", parameterName, parameter.ID, potentialTables, tablesWithLabel.Read, tablesWithLabel.Write, parameter.WriteType);
					SearchPotentialTables("connectedinput", parameterName, parameter.ID, potentialTables, tablesWithConnected.Read, tablesWithConnected.Write, parameter.WriteType);
					if (parameter.WriteType)
					{
						SearchPotentialTables("virtualsets", parameterName, parameter.ID, potentialTables, null, tablesWithSet.TablesWithVirtualSet, parameter.WriteType);
						SearchPotentialTables("serializedsets", parameterName, parameter.ID, potentialTables, null, tablesWithSet.TablesWithSerializedSet, parameter.WriteType);
					}
				}
				else if ((!parameter.WriteType && parameter.ParameterType == ParameterMeasurementType.Discreet) || (parameter.WriteType && parameter.ParameterType == ParameterMeasurementType.ToggleButton))
				{
					SearchPotentialTables("isenabled", parameterName, parameter.ID, potentialTables, tablesWithEnabled.Read, tablesWithEnabled.Write, parameter.WriteType);
					SearchPotentialTables("islocked", parameterName, parameter.ID, potentialTables, tablesWithLocked.Read, tablesWithLocked.Write, parameter.WriteType);
				}
				else
				{
					// Do nothing
				}
			}

			ParseProtocolAutoDetectOutputTable(requestParameters, foundParameters, tablesWithLabel, tablesWithEnabled, tablesWithLocked, tablesWithConnected, tablesWithSet);
			ParseProtocolAutoDetectInputTable(requestParameters, foundParameters, tablesWithLabel, tablesWithEnabled, tablesWithLocked);
		}

		private void ParseProtocolAutoDetectOutputTable(MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters, MatrixSearchTableInfo tablesWithLabel, MatrixSearchTableInfo tablesWithEnabled, MatrixSearchTableInfo tablesWithLocked, MatrixSearchTableInfo tablesWithConnected, MatrixSearchSingleSetInfo tablesWithSet)
		{
			foreach (KeyValuePair<int, MatrixCustomTableInfoItem> kvp in tablesWithConnected.Read)
			{
				int searchTable = kvp.Key;
				int writeConnected;
				if (!tablesWithConnected.Write.TryGetValue(searchTable, out writeConnected))
				{
					continue;
				}

				int writeEnabled;
				int writeLabel;
				int writeLocked;
				int writeVirtualSet;
				int writeSerializedSet;
				MatrixCustomTableInfoItem readConnected = kvp.Value;
				MatrixCustomTableInfoItem readEnabled;
				MatrixCustomTableInfoItem readLabel;
				MatrixCustomTableInfoItem readLocked;
				bool isLabelPresent = tablesWithLabel.TryGetValues(searchTable, out readLabel, out writeLabel);
				bool isLockedPresent = tablesWithLocked.TryGetValues(searchTable, out readLocked, out writeLocked);
				bool isEnabledPresent = tablesWithEnabled.TryGetValues(searchTable, out readEnabled, out writeEnabled);
				bool isSetPresent = tablesWithSet.TryGetValues(searchTable, out writeVirtualSet, out writeSerializedSet);
				if (!isLabelPresent || !isLockedPresent || !isEnabledPresent || !isSetPresent)
				{
					continue;
				}

				if (foundParameters.OutputsTableParameterId != -1)
				{
					throw new ArgumentException("Found two possible outputsTableParameterIds: " + Convert.ToString(foundParameters.OutputsTableParameterId, CultureInfo.InvariantCulture) + " and " + Convert.ToString(searchTable, CultureInfo.InvariantCulture) + ". There can't be multiple tables with the same name format when performing an auto detect, please use the constructor without auto detect instead.");
				}
				else
				{
					foundParameters.OutputsTableParameterId = searchTable;
					requestParameters.OutputsTableParameterId = searchTable;
					readLabel.WriteParameterId = writeLabel;
					readEnabled.WriteParameterId = writeEnabled;
					readLocked.WriteParameterId = writeLocked;
					readConnected.WriteParameterId = writeConnected;
					outputTableInfo = new MatrixCustomTableInfo(requestParameters.OutputsTableParameterId, readLabel, readEnabled, readLocked, readConnected, MaxOutputs, false);
					foundParameters.TableVirtualSetParameterId = writeVirtualSet;
					foundParameters.TableSerializedParameterId = writeSerializedSet;
					tablesWithEnabled.Remove(searchTable);
					tablesWithLabel.Remove(searchTable);
					tablesWithLocked.Remove(searchTable);
					tablesWithSet.Remove(searchTable);
					tablesWithConnected.Write.Remove(searchTable);
				}
			}

			if (foundParameters.OutputsTableParameterId == -1)
			{
				throw new ArgumentException("Could not auto detect outputsTable. Please use the correct parameter name format and don't forget write virtualsets and serializedsets single parameters.");
			}
		}

		private void ParseProtocolAutoDetectInputTable(MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters, MatrixSearchTableInfo tablesWithLabel, MatrixSearchTableInfo tablesWithEnabled, MatrixSearchTableInfo tablesWithLocked)
		{
			foreach (KeyValuePair<int, MatrixCustomTableInfoItem> kvp in tablesWithLabel.Read)
			{
				int searchTable = kvp.Key;
				int writeEnabled;
				int writeLabel;
				int writeLocked;
				MatrixCustomTableInfoItem readEnabled;
				MatrixCustomTableInfoItem readLabel;
				MatrixCustomTableInfoItem readLocked;
				bool isLabelPresent = tablesWithLabel.TryGetValues(searchTable, out readLabel, out writeLabel);
				bool isEnabledPresent = tablesWithEnabled.TryGetValues(searchTable, out readEnabled, out writeEnabled);
				bool isLockedPresent = tablesWithLocked.TryGetValues(searchTable, out readLocked, out writeLocked);
				if (!isLabelPresent || !isEnabledPresent || !isLockedPresent)
				{
					continue;
				}

				if (foundParameters.InputsTableParameterId != -1)
				{
					throw new ArgumentException("Found two possible inputsTableParameterIds: " + Convert.ToString(foundParameters.InputsTableParameterId, CultureInfo.InvariantCulture) + " and " + Convert.ToString(searchTable, CultureInfo.InvariantCulture) + ". There can't be multiple tables with the same name format when performing an auto detect, please use the constructor without auto detect instead.");
				}
				else
				{
					foundParameters.InputsTableParameterId = searchTable;
					requestParameters.InputsTableParameterId = searchTable;
					readLabel.WriteParameterId = writeLabel;
					readEnabled.WriteParameterId = writeEnabled;
					readLocked.WriteParameterId = writeLocked;
					MatrixCustomTableInfoItem readConnected = new MatrixCustomTableInfoItem(true);
					inputTableInfo = new MatrixCustomTableInfo(requestParameters.InputsTableParameterId, readLabel, readEnabled, readLocked, readConnected, MaxInputs, true);
					tablesWithEnabled.Read.Remove(searchTable);
					tablesWithLocked.Read.Remove(searchTable);
					tablesWithEnabled.Write.Remove(searchTable);
					tablesWithLabel.Write.Remove(searchTable);
					tablesWithLocked.Write.Remove(searchTable);
				}
			}

			if (foundParameters.InputsTableParameterId == -1)
			{
				throw new ArgumentException("Could not auto detect inputsTable. Please use the correct parameter name format.");
			}
		}

		private void ParseProtocolManualTables(ParameterInfo[] parameters, MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters, Dictionary<int, uint> inputColumns, Dictionary<int, uint> outputColumns, MatrixHelperParameterNames matrixHelperParameterNames)
		{
			foundParameters.InputsTableParameterId = requestParameters.InputsTableParameterId;
			foundParameters.OutputsTableParameterId = requestParameters.OutputsTableParameterId;
			if (inputColumns.Count == 0 || outputColumns.Count == 0)
			{
				throw new ArgumentException("The specified inputTableParameterId or outputsTableParameterId is not valid.");
			}

			MatrixCustomTableInfoItem inputLabel = new MatrixCustomTableInfoItem(true);
			MatrixCustomTableInfoItem inputIsEnabled = new MatrixCustomTableInfoItem(true);
			MatrixCustomTableInfoItem inputIsLocked = new MatrixCustomTableInfoItem(true);
			MatrixCustomTableInfoItem inputIsConnected = new MatrixCustomTableInfoItem(true);
			MatrixCustomTableInfoItem outputLabel = new MatrixCustomTableInfoItem(false);
			MatrixCustomTableInfoItem outputIsEnabled = new MatrixCustomTableInfoItem(false);
			MatrixCustomTableInfoItem outputIsLocked = new MatrixCustomTableInfoItem(false);
			MatrixCustomTableInfoItem outputConnectedInput = new MatrixCustomTableInfoItem(false);

			for (int i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				bool isParameterString = parameter.ParameterType == ParameterMeasurementType.String;
				bool isParameterDiscreet = parameter.ParameterType == ParameterMeasurementType.Discreet || parameter.ParameterType == ParameterMeasurementType.ToggleButton;
				inputLabel.ProcessParameter(parameter, matrixHelperParameterNames.InputsLabelParameterName, isParameterString, inputColumns);
				inputIsEnabled.ProcessParameter(parameter, matrixHelperParameterNames.InputsIsEnabledParameterName, isParameterDiscreet, inputColumns);
				inputIsLocked.ProcessParameter(parameter, matrixHelperParameterNames.InputsIsLockedParameterName, isParameterDiscreet, inputColumns);
				outputLabel.ProcessParameter(parameter, matrixHelperParameterNames.OutputsLabelParameterName, isParameterString, outputColumns);
				outputIsEnabled.ProcessParameter(parameter, matrixHelperParameterNames.OutputsIsEnabledParameterName, isParameterDiscreet, outputColumns);
				outputIsLocked.ProcessParameter(parameter, matrixHelperParameterNames.OutputsIsLockedParameterName, isParameterDiscreet, outputColumns);
				outputConnectedInput.ProcessParameter(parameter, matrixHelperParameterNames.OutputsConnectedInputParameterName, isParameterString, outputColumns);

				string parameterName = parameter.Name.ToLower(CultureInfo.InvariantCulture);
				if (parameterName == matrixHelperParameterNames.OutputsTableVirtualSetParameterName && isParameterString && parameter.WriteType)
				{
					foundParameters.TableVirtualSetParameterId = parameter.ID;
				}
				else if (parameterName == matrixHelperParameterNames.OutputsTableSerializedWritesParameterName && isParameterString && parameter.WriteType)
				{
					foundParameters.TableSerializedParameterId = parameter.ID;
				}
				else
				{
					// Do nothing
				}
			}

			inputLabel.ValidateParameters();
			inputIsEnabled.ValidateParameters();
			inputIsLocked.ValidateParameters();
			outputLabel.ValidateParameters();
			outputIsEnabled.ValidateParameters();
			outputIsLocked.ValidateParameters();
			outputConnectedInput.ValidateParameters();
			if (foundParameters.TableVirtualSetParameterId < 1)
			{
				throw new ArgumentException("The parameter for virtual sets cannot be found. This needs to be a displayed write string parameter with the same name as the output table appended with 'VirtualSets'");
			}

			if (foundParameters.TableSerializedParameterId < 1)
			{
				throw new ArgumentException("The parameter for serialized sets cannot be found. This needs to be a displayed write string parameter with the same name as the output table appended with 'SerializedSets'");
			}

			AssignCustomNumbersForTable(requestParameters);
			inputTableInfo = new MatrixCustomTableInfo(requestParameters.InputsTableParameterId, inputLabel, inputIsEnabled, inputIsLocked, inputIsConnected, MaxInputs, true);
			outputTableInfo = new MatrixCustomTableInfo(requestParameters.OutputsTableParameterId, outputLabel, outputIsEnabled, outputIsLocked, outputConnectedInput, MaxOutputs, false);
		}

		private void AssignCustomNumbersForTable(MatrixHelperParameterIds requestParameters)
		{
			if (requestParameters.MatrixReadParameterId <= 0 && requestParameters.MatrixReadParameterId != -2)
			{
				// when there is no matrix parameter provided then the custom numbers apply
				if (requestParameters.MaxInputCount > 0 && requestParameters.MaxOutputCount > 0)
				{
					MaxInputs = requestParameters.MaxInputCount;
					MaxOutputs = requestParameters.MaxOutputCount;
					displayedInputs = requestParameters.MaxInputCount;
					displayedOutputs = requestParameters.MaxOutputCount;
					MinConnectedInputsPerOutput = 0;
					MaxConnectedInputsPerOutput = 1;
					MinConnectedOutputsPerInput = 0;
					MaxConnectedOutputsPerInput = requestParameters.MaxOutputCount;
				}
				else
				{
					throw new ArgumentException("A valid maxInputCount and maxOutputCount needs to be provided when there is no matrix parameter");
				}
			}
		}

		private void ParseProtocolInfo(GetElementProtocolResponseMessage response, MatrixHelperParameterIds requestParameters, MatrixHelperParameterIds foundParameters)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			var parameters = response.Parameters;
			bool isAutoDetectTable = requestParameters.InputsTableParameterId == -2 || requestParameters.OutputsTableParameterId == -2;
			Dictionary<int, uint> inputColumns = new Dictionary<int, uint>();
			Dictionary<int, uint> outputColumns = new Dictionary<int, uint>();
			Dictionary<int, ParameterInfo> potentialTables = new Dictionary<int, ParameterInfo>();
			MatrixHelperParameterNames matrixHelperParameterNames = ParseProtocolGetParameterNames(parameters, requestParameters, foundParameters, inputColumns, outputColumns, potentialTables);

			if (isAutoDetectTable)
			{
				ParseProtocolAutoDetectTables(parameters, requestParameters, foundParameters, potentialTables);
			}
			else
			{
				if (requestParameters.InputsTableParameterId > 0 && requestParameters.OutputsTableParameterId > 0)
				{
					ParseProtocolManualTables(parameters, requestParameters, foundParameters, inputColumns, outputColumns, matrixHelperParameterNames);
				}
			}

			if (requestParameters.MatrixReadParameterId > 0 || requestParameters.MatrixReadParameterId == -2)
			{
				if (!String.IsNullOrEmpty(matrixHelperParameterNames.MatrixParameterName))
				{
					foundParameters.MatrixWriteParameterId = GetMatrixWriteParameterId(matrixHelperParameterNames.MatrixParameterName, response);
					if (foundParameters.MatrixWriteParameterId == -1)
					{
						throw new ArgumentException("The specified matrix read parameter ID does not have a corresponding matrix parameter of type write with the same name.");
					}
				}
				else
				{
					// This means the specified matrix read parameter does not exist or is not a matrix read parameter.
					throw new ArgumentException("Invalid read matrix parameter ID.");
				}
			}
		}

		private void ParseMatrixSize(ParameterInfo parameter)
		{
			// Obtain maximum dimensions.
			MaxInputs = parameter.XDimension;
			MaxOutputs = parameter.YDimension;

			// Obtain physical size.
			string physicalSize = parameter.PhysicalSize;

			if (!String.IsNullOrWhiteSpace(physicalSize))
			{
				string[] dimensions = physicalSize.Split(';');
				if (dimensions.Length > 1)
				{
					displayedInputs = Convert.ToInt32(dimensions[0], CultureInfo.InvariantCulture);
					displayedOutputs = Convert.ToInt32(dimensions[1], CultureInfo.InvariantCulture);
				}
			}
			else
			{
				displayedInputs = MaxInputs;
				displayedOutputs = MaxOutputs;
			}
		}

		private void ParseMatrixSettings(ParameterInfo parameter, int tableId)
		{
			string matrixOptions = parameter.MatrixOptions;

			if (!String.IsNullOrWhiteSpace(matrixOptions))
			{
				string[] options = matrixOptions.Split(',');

				if (options.Length > 5)
				{
					MinConnectedInputsPerOutput = Convert.ToInt32(options[2], CultureInfo.InvariantCulture);
					MaxConnectedInputsPerOutput = Convert.ToInt32(options[3], CultureInfo.InvariantCulture);
					MinConnectedOutputsPerInput = Convert.ToInt32(options[4], CultureInfo.InvariantCulture);
					MaxConnectedOutputsPerInput = Convert.ToInt32(options[5], CultureInfo.InvariantCulture);

					if (tableId > 0 && MaxConnectedInputsPerOutput != 1)
					{
						throw new InvalidOperationException("There can be maximum one input connected to an output when there are tables defined.");
					}
				}
			}
		}
	}
}