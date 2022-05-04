namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	using Net.Messages;

	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	internal class MatrixCustomTableInfoItem
	{
		private readonly bool isInput;
		private uint columnIdx;
		private int parameterId;

		internal MatrixCustomTableInfoItem(bool isInput)
		{
			columnIdx = 0;
			parameterId = 0;
			WriteParameterId = 0;
			this.isInput = isInput;
		}

		internal MatrixCustomTableInfoItem(int parameterId, uint columnIdx)
		{
			this.columnIdx = columnIdx;
			this.parameterId = parameterId;
		}

		internal uint ColumnIdx
		{
			get
			{
				return columnIdx;
			}
		}

		internal int ParameterId
		{
			get
			{
				return parameterId;
			}
		}

		internal int WriteParameterId { get; set; }

		internal void ProcessParameter(ParameterInfo parameter, string requestedName, bool isValidType, IDictionary<int, uint> columns)
		{
			if (!isValidType || parameter.Name.ToLower(CultureInfo.InvariantCulture) != requestedName)
			{
				return;
			}

			uint idx;
			if (parameter.WriteType)
			{
				WriteParameterId = parameter.ID;
			}
			else if (columns.TryGetValue(parameter.ID, out idx))
			{
				parameterId = parameter.ID;
				columnIdx = idx;
			}
			else
			{
				// Do nothing
			}
		}

		internal void ValidateParameters()
		{
			if (parameterId <= 0 || WriteParameterId <= 0)
			{
				if (isInput)
				{
					throw new ArgumentException("The specified inputsTableParameter do not contain the valid read and/or write columns.");
				}
				else
				{
					throw new ArgumentException("The specified outputsTableParameter do not contain the valid read and/or write columns.");
				}
			}
		}
	}
}
