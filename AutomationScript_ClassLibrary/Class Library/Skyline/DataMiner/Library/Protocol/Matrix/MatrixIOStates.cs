namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Net.Messages;

	/// <summary>
	/// Represents the matrix input or output states.
	/// </summary>
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	internal class MatrixIOStates : MatrixItems<bool>
	{
		internal MatrixIOStates(MatrixPortState portState, MatrixIOType type, ParameterInfo matrixReadParameterInfo) : base(portState, type)
		{
			ReadStates(matrixReadParameterInfo);
		}

		internal MatrixIOStates(MatrixPortState portState, MatrixIOType type, ParameterInfo matrixReadParameterInfo, Dictionary<int, bool> stateTableValues, out bool isMatch) : base(portState, type)
		{
			isMatch = ReadStates(matrixReadParameterInfo, stateTableValues);
		}

		internal MatrixIOStates(MatrixPortState portState, MatrixIOType type, Dictionary<int, bool> stateValues) : base(portState, type)
		{
			foreach (KeyValuePair<int, bool> kvp in stateValues)
			{
				OriginalItems[kvp.Key] = kvp.Value;
			}
		}

		internal bool this[int number]
		{
			get
			{
				bool state;
				if (UpdatedItems.TryGetValue(number, out state) || OriginalItems.TryGetValue(number, out state))
				{
					return state;
				}
				else
				{
					return true;
				}
			}

			set
			{
				if ((number < 0) || (number >= MaxItems))
				{
					return;
				}

				bool item;

				if (UpdatedItems.TryGetValue(number, out item))
				{
					if (item != value)
					{
						if (OriginalItems.TryGetValue(number, out item) && item == value)
						{
							UpdatedItems.Remove(number);
						}
						else
						{
							UpdatedItems[number] = value;
						}
					}
				}
				else
				{
					if (!OriginalItems.TryGetValue(number, out item) || item != value)
					{
						UpdatedItems[number] = value;
					}
				}
			}
		}

		internal void ExecuteChangedItems()
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, bool> kvp in UpdatedItems)
			{
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal void GetChangedMatrixItems(IDictionary<string, string> allChangedMatrixItems)
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, bool> kvp in UpdatedItems)
			{
				allChangedMatrixItems[Convert.ToString(kvp.Key + Offset + 1, CultureInfo.InvariantCulture)] = kvp.Value ? "enabled" : "disabled";
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal void GetChangedTableItems(IDictionary<string, string> allChangedTableItems)
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, bool> kvp in UpdatedItems)
			{
				allChangedTableItems[Convert.ToString(kvp.Key + 1, CultureInfo.InvariantCulture)] = kvp.Value ? "1" : "0";
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal void GetChangedMatrixAndTableItems(IDictionary<string, string> allChangedMatrixItems, IDictionary<string, string> allChangedTableItems)
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, bool> kvp in UpdatedItems)
			{
				allChangedMatrixItems[Convert.ToString(kvp.Key + Offset + 1, CultureInfo.InvariantCulture)] = kvp.Value ? "enabled" : "disabled";
				allChangedTableItems[Convert.ToString(kvp.Key + 1, CultureInfo.InvariantCulture)] = kvp.Value ? "1" : "0";
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal bool UpdateOriginal(int key, bool updatedValue)
		{
			bool originalValue;
			if (OriginalItems.TryGetValue(key, out originalValue) && originalValue == updatedValue)
			{
				return false;
			}
			else
			{
				OriginalItems[key] = updatedValue;
				return true;
			}
		}

		private void ReadStates(ParameterInfo matrixReadParameterInfo)
		{
			ParameterDiscreet[] discreteEntries = matrixReadParameterInfo.Discreets;
			int discreetEntryCount = 0;
			if (discreteEntries != null)
			{
				discreetEntryCount = discreteEntries.Length;
			}

			for (int i = Offset; i < Offset + MaxItems; i++)
			{
				int index = i - Offset;
				bool state = true;

				if ((i < discreetEntryCount) && discreteEntries != null && discreteEntries[i].State.Equals("disabled", StringComparison.OrdinalIgnoreCase))
				{
					state = false;
				}

				OriginalItems.Add(index, state);
			}
		}

		private bool ReadStates(ParameterInfo matrixReadParameterInfo, IDictionary<int, bool> stateTableValues)
		{
			bool isMatch = true;
			ParameterDiscreet[] discreteEntries = matrixReadParameterInfo.Discreets;
			int discreetEntryCount = 0;
			if (discreteEntries != null)
			{
				discreetEntryCount = discreteEntries.Length;
			}

			for (int i = Offset; i < Offset + MaxItems; i++)
			{
				int index = i - Offset;
				bool matrixState = true;

				if ((i < discreetEntryCount) && discreteEntries != null && discreteEntries[i].State.Equals("disabled", StringComparison.OrdinalIgnoreCase))
				{
					matrixState = false;
				}

				bool tableState;
				if (stateTableValues.TryGetValue(index, out tableState))
				{
					OriginalItems[index] = tableState;
					if (tableState != matrixState)
					{
						isMatch = false;
					}
				}
				else
				{
					OriginalItems[index] = matrixState;
				}
			}

			return isMatch;
		}
	}
}
