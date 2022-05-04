namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Collections.Concurrent;
	using System.Threading;

	internal class ElementStateMonitor : Monitor
	{
		private Action<ElementStateChange> onChange;

		internal ElementStateMonitor(ICommunication connection, string sourceId, Element selection, string handleId) : base(connection, sourceId)
		{
			Initialize(selection, handleId);
		}

		internal ElementStateMonitor(ICommunication connection, Element sourceElement, Element selection, string handleId) : base(connection, sourceElement)
		{
			Initialize(selection, handleId);
		}

		internal ElementStateMonitor(ICommunication connection, Element sourceElement, Element selection) : this(connection, sourceElement, selection, "-State")
		{
		}

		internal ElementStateMonitor(ICommunication connection, string sourceId, Element selection) : this(connection, sourceId, selection, "-State")
		{
		}

		internal SLNetWaitHandle ActionHandle { get; private set; }

		internal Element Selection { get; private set; }

		internal void Start(Action<ElementStateChange> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			this.onChange = actionOnChange;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, elementId);

			if (elementId == -1)
			{
				System.Diagnostics.Debug.WriteLine("Subscribing to Element Status of every element");
				ActionHandle.Subscriptions = new [] { new SubscriptionFilter(typeof(ElementStateEventMessage), SubscriptionFilterOptions.SkipInitialEvents) };
			}
			else
			{
				ActionHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterElement(typeof(ElementStateEventMessage), agentId, elementId) };

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

		private static Common.ElementState ParseSlnetElementState(ElementStateEventMessage elementStateMessage)
		{
			return (Common.ElementState)elementStateMessage.State;
		}

		private NewMessageEventHandler CreateHandler(string HandleGuid, int dmaId, int eleId)
		{
			string myGuid = HandleGuid;
			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var elementStateMessage = e.Message as ElementStateEventMessage;
					if (elementStateMessage == null) return;

					System.Diagnostics.Debug.WriteLine("State Event " + elementStateMessage.DataMinerID + "/" + elementStateMessage.ElementID + ":" + elementStateMessage.State + ":" + elementStateMessage.Level + ": complete=" + elementStateMessage.IsElementStartupComplete);

					bool isOnDmsLevel = dmaId == -1 && eleId == -1;
					bool isMatchWithElement = elementStateMessage.DataMinerID == dmaId && elementStateMessage.ElementID == eleId;

					if (isOnDmsLevel || isMatchWithElement)
					{
						System.Diagnostics.Debug.WriteLine("Match found.");
						HandleMatchingEvent(sender, myGuid, elementStateMessage);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of ElementState event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, ElementStateEventMessage elementStateMessage)
		{
			if (onChange == null) return;

			var senderConn = (Connection)sender;
			ICommunication com = new ConnectionCommunication(senderConn);

			Common.ElementState nonSLNetState = ParseSlnetElementState(elementStateMessage);

			if (nonSLNetState == Common.ElementState.Active && !elementStateMessage.IsElementStartupComplete) return;

			System.Diagnostics.Debug.WriteLine("executing action...");

			var changed = new ElementStateChange(new Element(elementStateMessage.DataMinerID, elementStateMessage.ElementID), SourceIdentifier, new Dms(com), nonSLNetState);
			if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, myGuid, "Result_" + elementStateMessage.DataMinerID + "/" + elementStateMessage.ElementID, changed))
			{
				System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + nonSLNetState);

				try
				{
					onChange(changed);
				}
				catch (Exception delegateEx)
				{
					var message = "Monitor Error: Exception during Handle of ElementState event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
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