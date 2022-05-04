namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Net.Messages;

	/// <summary>
	/// Represents the matrix labels.
	/// </summary>
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	internal class MatrixLabels : MatrixItems<string>
	{
		internal MatrixLabels(MatrixPortState portState, MatrixIOType type, ParameterInfo matrixReadParameterInfo) : base(portState, type)
		{
			ReadLabels(matrixReadParameterInfo);
		}

		internal MatrixLabels(MatrixPortState portState, MatrixIOType type, ParameterInfo matrixReadParameterInfo, IDictionary<int, string> labelTableValues, out bool isMatch) : base(portState, type)
		{
			isMatch = ReadLabels(matrixReadParameterInfo, labelTableValues);
		}

		internal MatrixLabels(MatrixPortState portState, MatrixIOType type, IDictionary<int, string> labelValues) : base(portState, type)
		{
			foreach (KeyValuePair<int, string> kvp in labelValues)
			{
				OriginalItems[kvp.Key] = kvp.Value;
			}
		}

		/// <summary>
		/// Gets or sets the label of the specified input or output.
		/// </summary>
		/// <param name="number">0-based input or output number.</param>
		/// <returns>The corresponding label.</returns>
		internal string this[int number]
		{
			get
			{
				string label;

				if (UpdatedItems.TryGetValue(number, out label) || OriginalItems.TryGetValue(number, out label))
				{
					return label;
				}
				else
				{
					return (Type == MatrixIOType.Input ? "Input " : "Output ") + Convert.ToString(number + 1, CultureInfo.InvariantCulture);
				}
			}

			set
			{
				if (String.IsNullOrEmpty(value) || (number < 0) || (number >= MaxItems))
				{
					return;
				}

				string label;

				if (UpdatedItems.TryGetValue(number, out label))
				{
					if (label != value)
					{
						if (OriginalItems.TryGetValue(number, out label) && label == value)
						{
							UpdatedItems.Remove(number); // If value is again the original value, the set should not be performed.
						}
						else
						{
							UpdatedItems[number] = value;
						}
					}
				}
				else
				{
					if (!OriginalItems.TryGetValue(number, out label) || label != value)
					{
						UpdatedItems[number] = value;
					}
				}
			}
		}

		internal void ExecuteChangedLabels()
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, string> kvp in UpdatedItems)
			{
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal void GetChangedMatrixLabels(IDictionary<string, string> allChangedMatrixLabels)
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, string> kvp in UpdatedItems)
			{
				allChangedMatrixLabels[Convert.ToString(kvp.Key + Offset + 1, CultureInfo.InvariantCulture)] = kvp.Value;
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal void GetChangedTableLabels(IDictionary<string, string> allChangedTableLabels)
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, string> kvp in UpdatedItems)
			{
				allChangedTableLabels[Convert.ToString(kvp.Key + 1, CultureInfo.InvariantCulture)] = kvp.Value;
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal void GetChangedMatrixAndTableLabels(IDictionary<string, string> allChangedMatrixLabels, IDictionary<string, string> allChangedTableLabels)
		{
			List<int> keysToRemove = new List<int>();
			foreach (KeyValuePair<int, string> kvp in UpdatedItems)
			{
				allChangedMatrixLabels[Convert.ToString(kvp.Key + Offset + 1, CultureInfo.InvariantCulture)] = kvp.Value;
				allChangedTableLabels[Convert.ToString(kvp.Key + 1, CultureInfo.InvariantCulture)] = kvp.Value;
				OriginalItems[kvp.Key] = kvp.Value;
				keysToRemove.Add(kvp.Key);
			}

			foreach (int key in keysToRemove)
			{
				UpdatedItems.Remove(key);
			}
		}

		internal bool UpdateOriginal(int key, string labelName)
		{
			string originalLabel;
			if (OriginalItems.TryGetValue(key, out originalLabel) && originalLabel == labelName)
			{
				return false;
			}
			else
			{
				OriginalItems[key] = labelName;
				return true;
			}
		}

		private void ReadLabels(ParameterInfo matrixReadParameterInfo)
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
				if (i < discreetEntryCount && discreteEntries != null)
				{
					OriginalItems[index] = discreteEntries[i].Display;
				}
				else
				{
					OriginalItems[index] = (Type == MatrixIOType.Input ? "Input " : "Output ") + Convert.ToString(index + 1, CultureInfo.InvariantCulture);
				}
			}
		}

		private bool ReadLabels(ParameterInfo matrixReadParameterInfo, IDictionary<int, string> labelTableValues)
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
				string matrixLabel;
				if (i < discreetEntryCount && discreteEntries != null)
				{
					matrixLabel = discreteEntries[i].Display;
				}
				else
				{
					matrixLabel = (Type == MatrixIOType.Input ? "Input " : "Output ") + Convert.ToString(index + 1, CultureInfo.InvariantCulture);
				}

				string tableLabel;
				if (labelTableValues.TryGetValue(index, out tableLabel))
				{
					OriginalItems[index] = tableLabel;
					if (tableLabel != matrixLabel)
					{
						isMatch = false;
					}
				}
				else
				{
					OriginalItems[index] = matrixLabel;
				}
			}

			return isMatch;
		}
	}
}
