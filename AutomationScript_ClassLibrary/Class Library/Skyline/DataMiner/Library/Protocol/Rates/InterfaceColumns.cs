namespace Skyline.DataMiner.Library.Protocol.Rates
{
	internal class InterfaceColumns
	{
		internal readonly object[] PKs;
		internal readonly object[] CurrentInput;
		internal readonly object[] CurrentOutput;
		internal readonly object[] PreviousInput;
		internal readonly object[] PreviousOutput;
		internal readonly object[] RateInput;
		internal readonly object[] RateOutput;

		internal readonly object[] Speed;
		internal readonly object[] Utilizations;
		internal readonly object[] CurrentDiscontinuity;
		internal readonly object[] PreviousDiscontinuity;

		internal InterfaceColumns(object[] ifTableColumns, bool utilization, bool discontinuity)
		{
			this.PKs = (object[])ifTableColumns[0];
			this.CurrentInput = (object[])ifTableColumns[1];
			this.CurrentOutput = (object[])ifTableColumns[2];
			this.PreviousInput = (object[])ifTableColumns[3];
			this.PreviousOutput = (object[])ifTableColumns[4];
			this.RateInput = (object[])ifTableColumns[5];
			this.RateOutput = (object[])ifTableColumns[6];

			if (utilization)
			{
				this.Speed = (object[])ifTableColumns[7];
				this.Utilizations = (object[])ifTableColumns[8];

				this.CurrentDiscontinuity = discontinuity ? (object[])ifTableColumns[9] : null;
				this.PreviousDiscontinuity = discontinuity ? (object[])ifTableColumns[10] : null;
			}
			else
			{
				this.Speed = null;
				this.Utilizations = null;

				this.CurrentDiscontinuity = discontinuity ? (object[])ifTableColumns[7] : null;
				this.PreviousDiscontinuity = discontinuity ? (object[])ifTableColumns[8] : null;
			}
		}
	}
}
