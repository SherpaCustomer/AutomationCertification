namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using System;
	using System.Collections.Concurrent;
	using System.Threading;
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;
	
	internal class ServiceAlarmLevelMonitor : Monitor
	{
		private Action<ServiceAlarmLevelChange> onchange;

		internal ServiceAlarmLevelMonitor(ICommunication connection, string sourceId, Service selection, string handleId) : base(connection, sourceId)
		{
			Initialize(selection, handleId);
		}

		internal ServiceAlarmLevelMonitor(ICommunication connection, Element sourceElement, Service selection, string handleId) : base(connection, sourceElement)
		{
			Initialize(selection, handleId);
		}

		internal ServiceAlarmLevelMonitor(ICommunication connection, Element sourceElement, Service selection) : this(connection, sourceElement, selection, "-ServiceAlarmLevel")
		{
		}

		internal ServiceAlarmLevelMonitor(ICommunication connection, string sourceId, Service selection) : this(connection, sourceId, selection, "-ServiceAlarmLevelState")
		{
		}

		internal SLNetWaitHandle ActionHandle { get; private set; }

		internal Service Selection { get; private set; }

		internal void Start(Action<ServiceAlarmLevelChange> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int serviceId = Selection.ServiceId;
			this.onchange = actionOnChange;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, serviceId);

			if (serviceId == -1)
			{
				System.Diagnostics.Debug.WriteLine("Subscribing to Service Status of every service");
				ActionHandle.Subscriptions = new[] { new SubscriptionFilter(typeof(ServiceStateEventMessage), SubscriptionFilterOptions.SkipInitialEvents) };
			}
			else
			{
				ActionHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterElement(typeof(ServiceStateEventMessage), agentId, serviceId) };

				TryAddDestinationElementCleanup(agentId, serviceId);
			}

			TryAddElementCleanup();
			SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, ActionHandle, true);
		}

		internal void Stop(bool force = false)
		{
			if (ActionHandle != null)
			{
				SubscriptionManager.RemoveSubscription(SourceIdentifier, Connection, ActionHandle, force);
				SubscriptionManager.TryRemoveCleanupSubs(SourceIdentifier, Connection);
			}
		}

		private static Common.AlarmLevel ParseSlnetServiceAlarmLevel(ServiceStateEventMessage serviceStateMessage)
		{
			return (Common.AlarmLevel)serviceStateMessage.Level;
		}

		private NewMessageEventHandler CreateHandler(string handleGuid, int dmaId, int eleId)
		{
			string thisGuid = handleGuid;
			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(thisGuid)) return;

					var serviceStateMessage = e.Message as ServiceStateEventMessage;
					if (serviceStateMessage == null) return;

					System.Diagnostics.Debug.WriteLine("State Event " + serviceStateMessage.DataMinerID + "/" + serviceStateMessage.ElementID + ":" + serviceStateMessage.Level);

					bool isOnDmsLevel = dmaId == -1 && eleId == -1;
					bool isMatchWithElement = serviceStateMessage.DataMinerID == dmaId && serviceStateMessage.ElementID == eleId;

					if (isOnDmsLevel || isMatchWithElement)
					{
						System.Diagnostics.Debug.WriteLine("Match found.");
						HandleMatchingEvent(sender, thisGuid, serviceStateMessage);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of ElementState event (Class Library Side): " + thisGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string guid, ServiceStateEventMessage serviceStateMessage)
		{
			if (onchange == null) return;

			var senderConn = (Connection)sender;
			ICommunication com = new ConnectionCommunication(senderConn);

			Common.AlarmLevel nonSLNetState = ParseSlnetServiceAlarmLevel(serviceStateMessage);

			System.Diagnostics.Debug.WriteLine("executing action...");

			var changed = new ServiceAlarmLevelChange(new Service(serviceStateMessage.DataMinerID, serviceStateMessage.ElementID), SourceIdentifier, new Dms(com), nonSLNetState);
			if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, guid, "Result_" + serviceStateMessage.DataMinerID + "/" + serviceStateMessage.ElementID, changed))
			{
				System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + nonSLNetState);

				try
				{
					onchange(changed);
				}
				catch (Exception delegateEx)
				{
					var message = "Monitor Error: Exception during Handle of ElementState event (check provided action): " + guid + "-- With exception: " + delegateEx;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			}
		}

		private void Initialize(Service selection, string handleId)
		{
			Selection = selection;

			ActionHandle = new SLNetWaitHandle
			{
				Flag = new AutoResetEvent(false),
				SetId = SourceIdentifier + "-" + Selection + handleId,
				Type = WaitHandleType.Normal,
				Destination = Selection.AgentId + "/" + Selection.ServiceId,
				TriggeredQueue = new ConcurrentQueue<object>(),
				CachedData = new ConcurrentDictionary<string, object>()
			};
		}
	}
}