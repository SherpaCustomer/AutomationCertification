namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Net.Messages;
	using Skyline.DataMiner.Scripting;

	/// <summary>
	/// Represents the state of a matrix port.
	/// </summary>
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	internal class MatrixPortState
	{
		private readonly bool isTableCapable;
		private readonly bool isMatrixCapable;

		private readonly MatrixConnections connections;

		private readonly MatrixDisplayType detectedDisplayType;

		private readonly MatrixLabels inputLabels;
		private readonly MatrixLabels outputLabels;

		private readonly MatrixIOStates inputStates;
		private readonly MatrixIOStates outputStates;

		private readonly MatrixLocks inputLocks;
		private readonly MatrixLocks outputLocks;

		private readonly MatrixSizeInfo maxSize;

		internal MatrixPortState(MatrixSizeInfo maxSize, ParameterInfo matrixReadParameterInfo, string connectionBuffer)
		{
			this.maxSize = maxSize;
			isTableCapable = false;
			isMatrixCapable = true;

			connections = new MatrixConnections(connectionBuffer);

			inputLabels = new MatrixLabels(this, MatrixIOType.Input, matrixReadParameterInfo);
			outputLabels = new MatrixLabels(this, MatrixIOType.Output, matrixReadParameterInfo);

			inputStates = new MatrixIOStates(this, MatrixIOType.Input, matrixReadParameterInfo);
			outputStates = new MatrixIOStates(this, MatrixIOType.Output, matrixReadParameterInfo);

			inputLocks = new MatrixLocks(this, MatrixIOType.Input, matrixReadParameterInfo);
			outputLocks = new MatrixLocks(this, MatrixIOType.Output, matrixReadParameterInfo);
		}

		internal MatrixPortState(SLProtocol protocol, MatrixSizeInfo maxSize, ParameterInfo matrixReadParameterInfo, MatrixCustomTableInfo inputTableInfo, MatrixCustomTableInfo outputTableInfo, string connectionBuffer, out MatrixSizeInfo maxFoundSize)
		{
			this.maxSize = maxSize;
			isTableCapable = true;
			isMatrixCapable = true;

			connections = new MatrixConnections(connectionBuffer);

			Dictionary<int, string> inputLabelTableCollection = new Dictionary<int, string>();
			Dictionary<int, bool> inputEnabledValuesTableCollection = new Dictionary<int, bool>();
			Dictionary<int, bool> inputLockedValuesTableCollection = new Dictionary<int, bool>();
			Dictionary<int, string> outputLabelTableCollection = new Dictionary<int, string>();
			Dictionary<int, bool> outputEnabledValuesTableCollection = new Dictionary<int, bool>();
			Dictionary<int, bool> outputLockedValuesTableCollection = new Dictionary<int, bool>();

			int maxFoundInput = -1;
			int maxFoundOutput = -1;

			if (GetColumnValues(protocol, inputTableInfo, inputLabelTableCollection, inputEnabledValuesTableCollection, inputLockedValuesTableCollection, maxSize.Inputs, out maxFoundInput) && GetColumnValues(protocol, outputTableInfo, outputLabelTableCollection, outputEnabledValuesTableCollection, outputLockedValuesTableCollection, maxSize.Outputs, out maxFoundOutput))
			{
				bool isInputLabelMatches;
				bool isOutputLabelMatches;
				bool isInputEnabledMatches;
				bool isOutputEnabledMatches;
				bool isInputLockedMatches;
				bool isOutputLockedMatches;
				inputLabels = new MatrixLabels(this, MatrixIOType.Input, matrixReadParameterInfo, inputLabelTableCollection, out isInputLabelMatches);
				outputLabels = new MatrixLabels(this, MatrixIOType.Output, matrixReadParameterInfo, outputLabelTableCollection, out isOutputLabelMatches);

				inputStates = new MatrixIOStates(this, MatrixIOType.Input, matrixReadParameterInfo, inputEnabledValuesTableCollection, out isInputEnabledMatches);
				outputStates = new MatrixIOStates(this, MatrixIOType.Output, matrixReadParameterInfo, outputEnabledValuesTableCollection, out isOutputEnabledMatches);

				inputLocks = new MatrixLocks(this, MatrixIOType.Input, matrixReadParameterInfo, inputLockedValuesTableCollection, out isInputLockedMatches);
				outputLocks = new MatrixLocks(this, MatrixIOType.Output, matrixReadParameterInfo, outputLockedValuesTableCollection, out isOutputLockedMatches);

				bool isEnabledMatch = isInputEnabledMatches && isOutputEnabledMatches;
				bool isLabelMatch = isInputLabelMatches && isOutputLabelMatches;
				bool isLockMatch = isInputLockedMatches && isOutputLockedMatches;
				if (isEnabledMatch && isLabelMatch && isLockMatch)
				{
					detectedDisplayType = MatrixDisplayType.MatrixAndTables;    // data matches in matrix and tables
				}
				else
				{
					detectedDisplayType = MatrixDisplayType.Tables; // tables have different data compared with matrix, taking tables data as source
				}
			}
			else
			{
				detectedDisplayType = MatrixDisplayType.Matrix; // no rows in tables, data of matrix is being used
				inputLabels = new MatrixLabels(this, MatrixIOType.Input, matrixReadParameterInfo);
				outputLabels = new MatrixLabels(this, MatrixIOType.Output, matrixReadParameterInfo);

				inputStates = new MatrixIOStates(this, MatrixIOType.Input, matrixReadParameterInfo);
				outputStates = new MatrixIOStates(this, MatrixIOType.Output, matrixReadParameterInfo);

				inputLocks = new MatrixLocks(this, MatrixIOType.Input, matrixReadParameterInfo);
				outputLocks = new MatrixLocks(this, MatrixIOType.Output, matrixReadParameterInfo);
			}

			maxFoundSize = new MatrixSizeInfo(maxFoundInput, maxFoundOutput);
		}

		internal MatrixPortState(SLProtocol protocol, MatrixSizeInfo maxSize, MatrixCustomTableInfo inputTableInfo, MatrixCustomTableInfo outputTableInfo, string connectionBuffer, out MatrixSizeInfo maxFoundSize)
		{
			this.maxSize = maxSize;
			isTableCapable = true;
			isMatrixCapable = false;

			connections = new MatrixConnections(connectionBuffer);

			Dictionary<int, string> inputLabelCollection = new Dictionary<int, string>();
			Dictionary<int, bool> inputEnabledValuesCollection = new Dictionary<int, bool>();
			Dictionary<int, bool> inputLockedValuesCollection = new Dictionary<int, bool>();
			Dictionary<int, string> outputLabelCollection = new Dictionary<int, string>();
			Dictionary<int, bool> outputEnabledValuesCollection = new Dictionary<int, bool>();
			Dictionary<int, bool> outputLockedValuesCollection = new Dictionary<int, bool>();
			int maxFoundInput = -1;
			int maxFoundOutput = -1;
			GetOrInitTableValues(protocol, inputTableInfo, inputLabelCollection, inputEnabledValuesCollection, inputLockedValuesCollection, out maxFoundInput);
			GetOrInitTableValues(protocol, outputTableInfo, outputLabelCollection, outputEnabledValuesCollection, outputLockedValuesCollection, out maxFoundOutput);

			inputLabels = new MatrixLabels(this, MatrixIOType.Input, inputLabelCollection);
			outputLabels = new MatrixLabels(this, MatrixIOType.Output, outputLabelCollection);

			inputStates = new MatrixIOStates(this, MatrixIOType.Input, inputEnabledValuesCollection);
			outputStates = new MatrixIOStates(this, MatrixIOType.Output, outputEnabledValuesCollection);

			inputLocks = new MatrixLocks(this, MatrixIOType.Input, inputLockedValuesCollection);
			outputLocks = new MatrixLocks(this, MatrixIOType.Output, outputLockedValuesCollection);

			maxFoundSize = new MatrixSizeInfo(maxFoundInput, maxFoundOutput);
		}

		internal MatrixConnections Connections
		{
			get { return connections; }
		}

		internal MatrixDisplayType DetectedDisplayType
		{
			get { return detectedDisplayType; }
		}

		internal MatrixLabels InputLabels
		{
			get { return inputLabels; }
		}

		internal MatrixLocks InputLocks
		{
			get { return inputLocks; }
		}

		internal MatrixIOStates InputStates
		{
			get { return inputStates; }
		}

		internal int MaxInputs
		{
			get { return maxSize.Inputs; }
		}

		internal int MaxOutputs
		{
			get { return maxSize.Outputs; }
		}

		internal bool IsTableCapable
		{
			get { return isTableCapable; }
		}

		internal bool IsMatrixCapable
		{
			get { return isMatrixCapable; }
		}

		internal MatrixLabels OutputLabels
		{
			get { return outputLabels; }
		}

		internal MatrixLocks OutputLocks
		{
			get { return outputLocks; }
		}

		internal MatrixIOStates OutputStates
		{
			get { return outputStates; }
		}

		private static void GetOrInitTableValues(SLProtocol protocol, MatrixCustomTableInfo matrixTableInfo, IDictionary<int, string> labels, IDictionary<int, bool> enabledValues, IDictionary<int, bool> lockedValues, out int maxFoundKey)
		{
			if (!GetColumnValues(protocol, matrixTableInfo, labels, enabledValues, lockedValues, matrixTableInfo.MaxCount, out maxFoundKey))
			{
				object[] indexCol = new object[matrixTableInfo.MaxCount];
				object[] labelCol = new object[matrixTableInfo.MaxCount];
				object[] enabledCol = new object[matrixTableInfo.MaxCount];
				object[] lockedCol = new object[matrixTableInfo.MaxCount];
				for (int i = 0; i < matrixTableInfo.MaxCount; i++)
				{
					string key = Convert.ToString(i + 1, CultureInfo.InvariantCulture);
					string label = (matrixTableInfo.IsInput ? "Input " : "Output ") + key;
					labels[i] = label;
					enabledValues[i] = true;
					lockedValues[i] = false;
					indexCol[i] = key;
					labelCol[i] = label;
					enabledCol[i] = "1";
					lockedCol[i] = "0";
				}

				maxFoundKey = matrixTableInfo.MaxCount - 1;
				object[] setTable = new object[4];
				setTable[0] = indexCol;
				setTable[1] = labelCol;
				setTable[2] = enabledCol;
				setTable[3] = lockedCol;
				protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, new object[] { matrixTableInfo.TableParameterId, matrixTableInfo.LabelParameterId, matrixTableInfo.EnabledParameterId, matrixTableInfo.LockedParameterId }, setTable);
			}
		}

		private static void AddMissingKeys(SLProtocol protocol, MatrixCustomTableInfo matrixTableInfo, IDictionary<int, string> labels, IDictionary<int, bool> enabledValues, IDictionary<int, bool> lockedValues, int maxKey)
		{
			HashSet<int> missingKeys = new HashSet<int>();
			for (int i = 0; i < maxKey; i++)
			{
				if (!labels.ContainsKey(i))
				{
					missingKeys.Add(i);
				}
			}

			if (missingKeys.Count == 0)
			{
				return;
			}

			// there seem to be gaps in the table keys, filling up with default values
			object[] setIndexCol = new object[missingKeys.Count];
			object[] setLabelCol = new object[missingKeys.Count];
			object[] setEnabledCol = new object[missingKeys.Count];
			object[] setLockedCol = new object[missingKeys.Count];
			int count = 0;
			foreach (int portNumber in missingKeys)
			{
				string key = Convert.ToString(portNumber + 1, CultureInfo.InvariantCulture);
				string sLabel = (matrixTableInfo.IsInput ? "Input " : "Output ") + key;
				labels[portNumber] = sLabel;
				enabledValues[portNumber] = true;
				lockedValues[portNumber] = false;
				setIndexCol[count] = key;
				setLabelCol[count] = sLabel;
				setEnabledCol[count] = "1";
				setLockedCol[count] = "0";
				count++;
			}

			object[] setTable = new object[4];
			setTable[0] = setIndexCol;
			setTable[1] = setLabelCol;
			setTable[2] = setEnabledCol;
			setTable[3] = setLockedCol;
			protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, new object[] { matrixTableInfo.TableParameterId, matrixTableInfo.LabelParameterId, matrixTableInfo.EnabledParameterId, matrixTableInfo.LockedParameterId }, setTable);
		}

		private static bool GetColumnValues(SLProtocol protocol, MatrixCustomTableInfo matrixTableInfo, IDictionary<int, string> labels, IDictionary<int, bool> enabledValues, IDictionary<int, bool> lockedValues, int maxAllowedSize, out int maxKey)
		{
			bool hasValues = false;
			maxKey = -1;
			uint[] paramIdx = new uint[4];
			paramIdx[0] = 0;
			paramIdx[1] = matrixTableInfo.LabelColumnIdx;
			paramIdx[2] = matrixTableInfo.EnabledColumnIdx;
			paramIdx[3] = matrixTableInfo.LockedColumnIdx;
			object[] tableCols = (object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, matrixTableInfo.TableParameterId, paramIdx);
			if (!CheckValidTable(tableCols, paramIdx.Length))
			{
				return hasValues;
			}

			object[] indexCol = (object[])tableCols[0];
			object[] labelCol = (object[])tableCols[1];
			object[] enabledCol = (object[])tableCols[2];
			object[] lockedCol = (object[])tableCols[3];
			HashSet<string> rowsToBeDeleted = new HashSet<string>();
			for (int i = 0; i < indexCol.Length; i++)
			{
				string key = Convert.ToString(indexCol[i], CultureInfo.InvariantCulture);
				int portNumber;
				if (Int32.TryParse(key, out portNumber))
				{
					hasValues = true;
					portNumber--;
					if (portNumber >= maxAllowedSize)
					{
						rowsToBeDeleted.Add(key);
						continue;
					}

					labels[portNumber] = Convert.ToString(labelCol[i], CultureInfo.InvariantCulture);
					enabledValues[portNumber] = Convert.ToString(enabledCol[i], CultureInfo.InvariantCulture) != "0";
					lockedValues[portNumber] = Convert.ToString(lockedCol[i], CultureInfo.InvariantCulture) != "0";
					if (portNumber > maxKey)
					{
						maxKey = portNumber;
					}
				}
				else
				{
					if (!String.IsNullOrEmpty(key))
					{
						rowsToBeDeleted.Add(key);
					}
				}
			}

			AddMissingKeys(protocol, matrixTableInfo, labels, enabledValues, lockedValues, maxKey);
			MatrixHelper.DeleteRows(protocol, matrixTableInfo.TableParameterId, rowsToBeDeleted);
			return hasValues;
		}

		private static bool CheckValidColumns(object[] columns)
		{
			for (int i = 0; i < columns.Length; i++)
			{
				if (columns[i] == null)
				{
					return false;
				}

				if (i == 0)
				{
					continue;
				}

				object[] previousCol = (object[])columns[i - 1];
				object[] currentCol = (object[])columns[i];
				if (previousCol.Length != currentCol.Length)
				{
					return false;
				}
			}

			return true;
		}

		private static bool CheckValidTable(object[] columns, int expectedSize)
		{
			if (columns == null || columns.Length < expectedSize)
			{
				return false;
			}

			return CheckValidColumns(columns);
		}
	}
}
