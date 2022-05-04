namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	using System.Collections.Generic;

	internal class MatrixSearchTableInfo
	{
		private readonly Dictionary<int, MatrixCustomTableInfoItem> tablesWithRead;
		private readonly Dictionary<int, int> tablesWithWrite;

		internal MatrixSearchTableInfo()
		{
			tablesWithRead = new Dictionary<int, MatrixCustomTableInfoItem>();
			tablesWithWrite = new Dictionary<int, int>();
		}

		public Dictionary<int, MatrixCustomTableInfoItem> Read
		{
			get
			{
				return tablesWithRead;
			}
		}

		public Dictionary<int, int> Write
		{
			get
			{
				return tablesWithWrite;
			}
		}

		public bool TryGetValues(int key, out MatrixCustomTableInfoItem read, out int write)
		{
			if (tablesWithRead.TryGetValue(key, out read) && tablesWithWrite.TryGetValue(key, out write))
			{
				return true;
			}
			else
			{
				write = -1;
				return false;
			}
		}

		public void Remove(int key)
		{
			tablesWithRead.Remove(key);
			tablesWithWrite.Remove(key);
		}
	}
}
