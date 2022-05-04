namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Collections.Concurrent;
	using System.Threading;

	internal class ElementNameMonitor : Monitor
	{
		private Action<ElementNameChange> onChange;

		internal ElementNameMonitor(ICommunication connection, string sourceId, Element selection, string handleId) : base(connection, sourceId)
		{
			Initialize(selection, handleId);
		}

		internal ElementNameMonitor(ICommunication connection, Element sourceElement, Element selection, string handleId) : base(connection, sourceElement)
		{
			Initialize(selection, handleId);
		}

		internal ElementNameMonitor(ICommunication connection, Element sourceElement, Element selection) : this(connection, sourceElement, selection, "-Name")
		{
		}

		internal ElementNameMonitor(ICommunication connection, string sourceId, Element selection) : this(connection, sourceId, selection, "-Name")
		{
		}

		internal SLNetWaitHandle ActionHandle { get; private set; }

		internal Element Selection { get; private set; }

		internal void Start(Action<ElementNameChange> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			this.onChange = actionOnChange;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, elementId);

			if (elementId == -1)
			{
				System.Diagnostics.Debug.WriteLine("Subscribing to Element Name Changes of every element");
				ActionHandle.Subscriptions = new [] { new SubscriptionFilter(typeof(LiteElementInfoEvent), SubscriptionFilterOptions.SkipInitialEvents) };
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Subscribing to Element Name Changes of " + Selection);
				ActionHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterElement(typeof(LiteElementInfoEvent), agentId, elementId) };

				TryAddDestinationElementCleanup(agentId, elementId);
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

		private NewMessageEventHandler CreateHandler(string HandleGuid, int dmaId, int eleId)
		{
			string myGuid = HandleGuid;
			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;
					var elementInfoEvent = e.Message as LiteElementInfoEvent;
					if (elementInfoEvent == null) return;
					System.Diagnostics.Debug.WriteLine("Name Event " + elementInfoEvent.DataMinerID + "/" + elementInfoEvent.ElementID + ":" + elementInfoEvent.Name);

					bool isOnDmsLevel = dmaId == -1 && eleId == -1;
					bool isMatchWithElement = elementInfoEvent.DataMinerID == dmaId && elementInfoEvent.ElementID == eleId;
					if (isOnDmsLevel || isMatchWithElement)
					{
						System.Diagnostics.Debug.WriteLine("Match found.");

						HandleMatchingEvent(sender, myGuid, elementInfoEvent);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of ElementName event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, LiteElementInfoEvent elementInfoEvent)
		{
			if (onChange == null) return;

			var senderConn = (Connection)sender;
			ICommunication com = new ConnectionCommunication(senderConn);

			System.Diagnostics.Debug.WriteLine("executing action...");
			var changed = new ElementNameChange(new Element(elementInfoEvent.DataMinerID, elementInfoEvent.ElementID), SourceIdentifier, new Dms(com), elementInfoEvent.Name);
			if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, myGuid, "Result_" + elementInfoEvent.DataMinerID + "/" + elementInfoEvent.ElementID, changed))
			{
				System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + elementInfoEvent.Name);
				try
				{
					onChange(changed);
				}
				catch (Exception delegateEx)
				{
					var message = "Monitor Error: Exception during Handle of ElementName event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			}
		}

		private void Initialize(Element selection, string handleId)
		{
			Selection = selection;

			ActionHandle = new SLNetWaitHandle
			{
				Flag = new AutoResetEvent(false),
				SetId = SourceIdentifier + "-" + Selection + handleId,
				Type = WaitHandleType.Normal,
				Destination = Selection.AgentId + "/" + Selection.ElementId,
				TriggeredQueue = new ConcurrentQueue<object>(),
				CachedData = new ConcurrentDictionary<string, object>()
			};
		}
	}
}