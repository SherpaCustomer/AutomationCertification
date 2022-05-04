namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents a component of a matrix.
	/// </summary>
	/// <typeparam name="T">Type of the matrix item.</typeparam>
	internal class MatrixItems<T>
	{
		private readonly MatrixIOType type;

		private readonly int offset;
		private readonly int maxItems;

		private readonly Dictionary<int, T> originalItems;
		private readonly Dictionary<int, T> updatedItems;

		internal MatrixItems(MatrixPortState portState, MatrixIOType type)
		{
			this.type = type;

			if (type == MatrixIOType.Input)
			{
				maxItems = portState.MaxInputs;
				offset = 0;
			}
			else
			{
				maxItems = portState.MaxOutputs;
				offset = portState.MaxInputs;
			}

			originalItems = new Dictionary<int, T>();
			updatedItems = new Dictionary<int, T>();
		}

		internal MatrixIOType Type
		{
			get { return type; }
		}

		internal int Offset
		{
			get { return offset; }
		}

		internal int MaxItems
		{
			get { return maxItems; }
		}

		internal Dictionary<int, T> OriginalItems
		{
			get { return originalItems; }
		}

		internal Dictionary<int, T> UpdatedItems
		{
			get { return updatedItems; }
		}
	}
}
