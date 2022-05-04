namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;

	using System;
	using System.Globalization;

	internal class CellValueMonitor : ValueMonitor
	{
		internal CellValueMonitor(ICommunication connection, string sourceId, Cell selection, string handleId) : base(connection, sourceId, selection, handleId)
		{
		}

		internal CellValueMonitor(ICommunication connection, Element sourceElement, Cell selection, string handleId) : base(connection, Convert.ToString(sourceElement, CultureInfo.InvariantCulture), selection, handleId)
		{
		}

		internal CellValueMonitor(ICommunication connection, Element sourceElement, Cell selection) : base(connection, sourceElement, selection, "-Value")
		{
		}

		internal CellValueMonitor(ICommunication connection, string sourceId, Cell selection) : base(connection, sourceId, selection, "-Value")
		{
		}
	}
}