namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using System;

	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.SLNetHelper;

	using System.Collections.Concurrent;
	using System.Threading;

	internal class ValueMonitor : Monitor
	{
		internal ValueMonitor(ICommunication connection, string sourceId, Param selection, string handleId) : base(connection, sourceId)
		{
			Initialize(selection, handleId);
		}

		internal ValueMonitor(ICommunication connection, Element sourceElement, Param selection, string handleId) : base(connection, sourceElement)
		{
			Initialize(selection, handleId);
		}

		internal ValueMonitor(ICommunication connection, Element sourceElement, Param selection) : this(connection, sourceElement, selection, null)
		{
		}

		internal ValueMonitor(ICommunication connection, string sourceId, Param selection) : this(connection, sourceId, selection, null)
		{
		}

		internal SLNetWaitHandle ActionHandle { get; set; }

		internal Param Selection { get; private set; }

		internal void Stop(bool force = false)
		{
			if (ActionHandle != null)
			{
				SubscriptionManager.RemoveSubscription(SourceIdentifier, Connection, ActionHandle, force);
				SubscriptionManager.TryRemoveCleanupSubs(SourceIdentifier, Connection);
			}
		}

		private void Initialize(Param selection, string typeId)
		{
			Selection = selection;

			ActionHandle = new SLNetWaitHandle
			{
				Flag = new AutoResetEvent(false),
				SetId = SourceIdentifier + "-" + Selection + typeId ?? String.Empty,
				Type = WaitHandleType.Normal,
				Destination = Selection.AgentId + "/" + Selection.ElementId,
				TriggeredQueue = new ConcurrentQueue<object>(),
				CachedData = new ConcurrentDictionary<string, object>()
			};
		}
	}
}