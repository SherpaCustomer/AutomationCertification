namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System.Collections.Generic;
	using System.Globalization;
	using Net.Messages;
	using Skyline.DataMiner.Scripting;

	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
	[Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
	internal class MatrixHelperParameterNames
	{
		internal string MatrixParameterName { get; set; }

		internal string InputsLabelParameterName { get; set; }

		internal string InputsIsEnabledParameterName { get; set; }

		internal string InputsIsLockedParameterName { get; set; }

		internal string OutputsLabelParameterName { get; set; }

		internal string OutputsIsEnabledParameterName { get; set; }

		internal string OutputsIsLockedParameterName { get; set; }

		internal string OutputsConnectedInputParameterName { get; set; }

		internal string OutputsTableVirtualSetParameterName { get; set; }

		internal string OutputsTableSerializedWritesParameterName { get; set; }

		internal void SetInputNames(ParameterInfo parameter, IDictionary<int, uint> inputColumns)
		{
			string tableName = parameter.Name.ToLower(CultureInfo.InvariantCulture);
			InputsLabelParameterName = tableName + "label";
			InputsIsEnabledParameterName = tableName + "isenabled";
			InputsIsLockedParameterName = tableName + "islocked";
			uint idx = 0;
			foreach (TableColumnDefinition tableColumnDefinition in parameter.TableColumnDefinitions)
			{
				inputColumns[tableColumnDefinition.ParameterID] = idx++;
			}
		}

		internal void SetOutputNames(ParameterInfo parameter, IDictionary<int, uint> outputColumns)
		{
			string tableName = parameter.Name.ToLower(CultureInfo.InvariantCulture);
			OutputsLabelParameterName = tableName + "label";
			OutputsIsEnabledParameterName = tableName + "isenabled";
			OutputsIsLockedParameterName = tableName + "islocked";
			OutputsConnectedInputParameterName = tableName + "connectedinput";
			OutputsTableVirtualSetParameterName = tableName + "virtualsets";
			OutputsTableSerializedWritesParameterName = tableName + "serializedsets";
			uint idx = 0;
			foreach (TableColumnDefinition tableColumnDefinition in parameter.TableColumnDefinitions)
			{
				outputColumns[tableColumnDefinition.ParameterID] = idx++;
			}
		}
	}
}
