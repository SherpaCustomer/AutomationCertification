namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System.Collections.Generic;

	internal class MatrixSearchSingleSetInfo
	{
		private readonly Dictionary<int, int> tablesWithVirtualSet;
		private readonly Dictionary<int, int> tablesWithSerializedSet;

		internal MatrixSearchSingleSetInfo()
		{
			tablesWithVirtualSet = new Dictionary<int, int>();
			tablesWithSerializedSet = new Dictionary<int, int>();
		}

		public Dictionary<int, int> TablesWithVirtualSet
		{
			get
			{
				return tablesWithVirtualSet;
			}
		}

		public Dictionary<int, int> TablesWithSerializedSet
		{
			get
			{
				return tablesWithSerializedSet;
			}
		}

		public bool TryGetValues(int key, out int virtualSet, out int serializedSet)
		{
			if (tablesWithVirtualSet.TryGetValue(key, out virtualSet) && tablesWithSerializedSet.TryGetValue(key, out serializedSet))
			{
				return true;
			}
			else
			{
				serializedSet = -1;
				return false;
			}
		}

		public void Remove(int key)
		{
			tablesWithVirtualSet.Remove(key);
			tablesWithSerializedSet.Remove(key);
		}
	}
}
