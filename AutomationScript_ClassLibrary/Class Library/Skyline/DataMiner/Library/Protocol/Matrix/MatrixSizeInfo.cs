namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	internal class MatrixSizeInfo
	{
		private readonly int inputs;
		private readonly int outputs;

		internal MatrixSizeInfo(int inputs, int outputs)
		{
			this.inputs = inputs;
			this.outputs = outputs;
		}

		internal int Inputs
		{
			get
			{
				return inputs;
			}
		}

		internal int Outputs
		{
			get
			{
				return outputs;
			}
		}
	}
}
