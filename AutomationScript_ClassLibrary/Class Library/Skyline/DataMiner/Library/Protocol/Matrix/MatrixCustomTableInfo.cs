namespace Skyline.DataMiner.Library.Protocol.Matrix
{
	internal class MatrixCustomTableInfo
	{
		private readonly int tableParameterId;
		private readonly MatrixCustomTableInfoItem labelId;
		private readonly MatrixCustomTableInfoItem enabledId;
		private readonly MatrixCustomTableInfoItem lockedId;
		private readonly MatrixCustomTableInfoItem connectedId;
		private readonly int maxCount;
		private readonly bool isInput;

		internal MatrixCustomTableInfo(int tableParameterId, MatrixCustomTableInfoItem labelId, MatrixCustomTableInfoItem enabledId, MatrixCustomTableInfoItem lockedId, MatrixCustomTableInfoItem connectedId, int maxCount, bool isInput)
		{
			this.tableParameterId = tableParameterId;
			this.labelId = labelId;
			this.enabledId = enabledId;
			this.lockedId = lockedId;
			this.connectedId = connectedId;
			this.maxCount = maxCount;
			this.isInput = isInput;
		}

		internal int TableParameterId
		{
			get
			{
				return tableParameterId;
			}
		}

		internal uint LabelColumnIdx
		{
			get
			{
				return labelId.ColumnIdx;
			}
		}

		internal int LabelParameterId
		{
			get
			{
				return labelId.ParameterId;
			}
		}

		internal int LabelWriteParameterId
		{
			get
			{
				return labelId.WriteParameterId;
			}
		}

		internal uint EnabledColumnIdx
		{
			get
			{
				return enabledId.ColumnIdx;
			}
		}

		internal int EnabledParameterId
		{
			get
			{
				return enabledId.ParameterId;
			}
		}

		internal int EnabledWriteParameterId
		{
			get
			{
				return enabledId.WriteParameterId;
			}
		}

		internal uint LockedColumnIdx
		{
			get
			{
				return lockedId.ColumnIdx;
			}
		}

		internal int LockedParameterId
		{
			get
			{
				return lockedId.ParameterId;
			}
		}

		internal int LockedWriteParameterId
		{
			get
			{
				return lockedId.WriteParameterId;
			}
		}

		internal uint ConnectedColumnIdx
		{
			get
			{
				return connectedId.ColumnIdx;
			}
		}

		internal int ConnectedParameterId
		{
			get
			{
				return connectedId.ParameterId;
			}
		}

		internal int ConnectedWriteParameterId
		{
			get
			{
				return connectedId.WriteParameterId;
			}
		}

		internal int MaxCount
		{
			get
			{
				return maxCount;
			}
		}

		internal bool IsInput
		{
			get
			{
				return isInput;
			}
		}
	}
}
