namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Net.Messages;

	/// <summary>
	/// Represents the matrix input or output lock states.
	/// </summary>
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	internal class MatrixLocks : MatrixItems<bool>
	{
		internal MatrixLocks(MatrixPortState portState, MatrixIOType type, ParameterInfo matrixReadParameterInfo) : base(portState, type)
		{
			ReadLocks(matrixReadParameterInfo);
		}

		internal MatrixLocks(MatrixPortState portState, MatrixIOType type, ParameterInfo matrixReadParameterInfo, IDictionary<int, bool> lockTableValues, out bool isMatch) : base(portState, type)
		{
			isMatch = ReadLocks(matrixReadParameterInfo, lockTableValues);
		}

		internal MatrixLocks(MatrixPortState portState, MatrixIOType type, IDictionary<int, bool> lockTableValues) : base(portState, type)
		{
			foreach (KeyValuePair<int, bool> kvp in lockTableValues)
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
					return false;
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

		internal static bool GetMatrixLockFromOptions(string options)
		{
			if (options.Trim().StartsWith("matrix=", StringComparison.OrdinalIgnoreCase))
			{
				options = options.Substring(7);

				foreach (string option in options.Split('~'))
				{
					if (option.Equals("locked,true", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}

			return false;
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
				allChangedMatrixItems[Convert.ToString(kvp.Key + Offset + 1, CultureInfo.InvariantCulture)] = kvp.Value ? "true" : "false";
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
				allChangedMatrixItems[Convert.ToString(kvp.Key + Offset + 1, CultureInfo.InvariantCulture)] = kvp.Value ? "true" : "false";
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

		private void ReadLocks(ParameterInfo matrixReadParameterInfo)
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
				bool isLocked = false;
				if (i < discreetEntryCount && discreteEntries != null)
				{
					isLocked = GetMatrixLockFromOptions(discreteEntries[i].Options);
				}

				OriginalItems.Add(index, isLocked);
			}
		}

		private bool ReadLocks(ParameterInfo matrixReadParameterInfo, IDictionary<int, bool> lockTableValues)
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
				bool isMatrixLocked = false;
				if (i < discreetEntryCount && discreteEntries != null)
				{
					isMatrixLocked = GetMatrixLockFromOptions(discreteEntries[i].Options);
				}

				bool isTableLocked;
				if (lockTableValues.TryGetValue(index, out isTableLocked))
				{
					OriginalItems[index] = isTableLocked;
					if (isTableLocked != isMatrixLocked)
					{
						isMatch = false;
					}
				}
				else
				{
					OriginalItems[index] = isMatrixLocked;
				}
			}

			return isMatch;
		}
	}
}
