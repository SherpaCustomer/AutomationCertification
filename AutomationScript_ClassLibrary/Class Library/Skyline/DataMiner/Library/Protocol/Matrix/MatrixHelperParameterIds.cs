namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System;

	internal class MatrixHelperParameterIds
	{
		internal MatrixHelperParameterIds()
		{
			MatrixReadParameterId = -1;
			MatrixWriteParameterId = -1;
			MatrixConnectionsBufferParameterId = -1;
			MatrixSerializedParameterId = -1;
			DiscreetInfoParameterId = -1;
			InputsTableParameterId = -1;
			OutputsTableParameterId = -1;
			MaxInputCount = -1;
			MaxOutputCount = -1;
			TableSerializedParameterId = -1;
			TableVirtualSetParameterId = -1;
		}

		internal MatrixHelperParameterIds(int matrixConnectionsBufferParameterId, int matrixReadParameterId, int discreetInfoParameterId, int inputsTableParameterId, int outputsTableParameterId, int matrixSerializedParameterId)
		{
			if (inputsTableParameterId <= 0 && outputsTableParameterId > 0)
			{
				throw new ArgumentException("Invalid inputs table parameter ID.", "inputsTableParameterId");
			}

			if (inputsTableParameterId > 0 && outputsTableParameterId <= 0)
			{
				throw new ArgumentException("Invalid outputs table parameter ID.", "outputsTableParameterId");
			}

			if (matrixReadParameterId <= 0 && inputsTableParameterId <= 0 && matrixReadParameterId != -2 && inputsTableParameterId != -2)
			{
				throw new ArgumentException("Invalid matrix read parameter ID.", "matrixReadParameterId");
			}

			if (discreetInfoParameterId <= 0 && (matrixReadParameterId > 0 || matrixReadParameterId == -2))
			{
				throw new ArgumentException("Invalid discreet info parameter ID.", "discreetInfoParameterId");
			}

			if (matrixConnectionsBufferParameterId <= 0 && matrixConnectionsBufferParameterId != -2)
			{
				throw new ArgumentException("Invalid connection buffer parameter ID.", "matrixConnectionsBufferParameterId");
			}

			if (matrixSerializedParameterId <= 0 && matrixSerializedParameterId != -2)
			{
				throw new ArgumentException("Invalid matrix serialized parameter ID.", "matrixSerializedParameterId");
			}

			MatrixConnectionsBufferParameterId = matrixConnectionsBufferParameterId;
			MatrixReadParameterId = matrixReadParameterId;
			MatrixSerializedParameterId = matrixSerializedParameterId;
			DiscreetInfoParameterId = discreetInfoParameterId;
			InputsTableParameterId = inputsTableParameterId;
			OutputsTableParameterId = outputsTableParameterId;
		}

		internal int MatrixConnectionsBufferParameterId { get; set; }

		internal int MatrixReadParameterId { get; set; }

		internal int MatrixWriteParameterId { get; set; }

		internal int MatrixSerializedParameterId { get; set; }

		internal int DiscreetInfoParameterId { get; set; }

		internal int InputsTableParameterId { get; set; }

		internal int OutputsTableParameterId { get; set; }

		internal int MaxInputCount { get; set; }

		internal int MaxOutputCount { get; set; }

		internal int TableSerializedParameterId { get; set; }

		internal int TableVirtualSetParameterId { get; set; }
	}
}
