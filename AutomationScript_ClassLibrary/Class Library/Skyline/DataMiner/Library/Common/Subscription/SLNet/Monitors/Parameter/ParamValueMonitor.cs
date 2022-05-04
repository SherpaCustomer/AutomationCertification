namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;

	using System;
	using System.Globalization;

	internal class ParamValueMonitor : ValueMonitor
	{
		internal ParamValueMonitor(ICommunication connection, string sourceId, Param selection, string handleId) : base(connection, sourceId, selection, handleId)
		{
		}

		internal ParamValueMonitor(ICommunication connection, Element sourceElement, Param selection, string handleId) : base(connection, Convert.ToString(sourceElement, CultureInfo.InvariantCulture), selection, handleId)
		{
		}

		internal ParamValueMonitor(ICommunication connection, Element sourceElement, Param selection) : base(connection, sourceElement, selection, "-Value")
		{
		}

		internal ParamValueMonitor(ICommunication connection, string sourceId, Param selection) : base(connection, sourceId, selection, "-Value")
		{
		}
	}
}