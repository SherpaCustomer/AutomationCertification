namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Collections.Concurrent;
	using System.Globalization;

	internal class Monitor
	{
		internal Monitor(ICommunication connection, string sourceId)
		{
			Connection = connection;
			SourceIdentifier = sourceId;
		}

		internal Monitor(ICommunication connection, Element sourceElement) : this(connection, Convert.ToString(sourceElement, CultureInfo.InvariantCulture))
		{
			SourceElement = sourceElement;

			if (sourceElement != null)
			{
				ElementCleanupHandle = new SLNetWaitHandle
				{
					SetId = SourceElement + "_CLP_ElementCleanup",
					Type = WaitHandleType.Cleanup,
					Flag = new System.Threading.AutoResetEvent(false),
					TriggeredQueue = new ConcurrentQueue<object>()
				};
			}
		}

		internal ICommunication Connection { get; set; }

		internal SLNetWaitHandle ElementCleanupHandle { get; set; }

		internal Element SourceElement { get; set; }

		internal string SourceIdentifier { get; set; }

		internal void TryAddElementCleanup()
		{
			if (SourceElement != null)
			{
				ElementCleanupHandle.Handler = CreateElementCleanupHandle(ElementCleanupHandle.SetId);
				ElementCleanupHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterElement(typeof(ElementStateEventMessage), SourceElement.AgentId, SourceElement.ElementId) };

				SubscriptionManager.CreateSubscription(SourceElement.ToString(), Connection, ElementCleanupHandle, false);
			}
		}
		
		protected void TryAddDestinationElementCleanup(int agentId, int elementId)
		{
			if (SourceIdentifier != null)
			{
				if (SourceElement != null && SourceElement.AgentId == agentId && SourceElement.ElementId == elementId) return;

				var handler = CreateDestinationElementCleanupHandle(agentId, elementId);

				SLNetWaitHandle destinationElementCleanupHandle = new SLNetWaitHandle
				{
					SetId = SourceIdentifier + "-" + agentId + "/" + elementId + "_CLP_DestElemCleanup",
					Flag = new System.Threading.AutoResetEvent(false),
					Type = WaitHandleType.Cleanup,
					Destination = agentId + "/" + elementId,
					TriggeredQueue = new ConcurrentQueue<object>(),
					Handler = handler,
					Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterElement(typeof(ElementStateEventMessage), agentId, elementId) }
				};

				SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, destinationElementCleanupHandle, false);
			}
		}

		protected void TryAddDestinationServiceCleanup(int agentId, int serviceId)
		{
			if (SourceIdentifier != null)
			{
				if (SourceElement != null && SourceElement.AgentId == agentId && SourceElement.ElementId == serviceId) return;

				var handler = CreateDestinationServiceCleanupHandle(agentId, serviceId);

				SLNetWaitHandle destinationServiceCleanupHandle = new SLNetWaitHandle
				{
					SetId = SourceIdentifier + "-" + agentId + "/" + serviceId + "_CLP_DestServCleanup",
					Flag = new System.Threading.AutoResetEvent(false),
					Type = WaitHandleType.Cleanup,
					Destination = agentId + "/" + serviceId,
					TriggeredQueue = new ConcurrentQueue<object>(),
					Handler = handler,
					Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterElement(typeof(ServiceStateEventMessage), agentId, serviceId) }
				};

				SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, destinationServiceCleanupHandle, false);
			}
		}

		private static NewMessageEventHandler CreateElementCleanupHandle(string HandleGuid)
		{
			string myGuid = HandleGuid;

			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var elementStateMessage = e.Message as ElementStateEventMessage;
					if (elementStateMessage == null) return;

					var senderConn = (Connection)sender;
					System.Diagnostics.Debug.WriteLine("State Change:" + elementStateMessage.State);
					string uniqueIdentifier = elementStateMessage.DataMinerID + "/" + elementStateMessage.ElementID;

					// clear subscriptions if element is stopped or deleted
					if (elementStateMessage.State == Net.Messages.ElementState.Deleted || elementStateMessage.State == Net.Messages.ElementState.Stopped)
					{
						System.Diagnostics.Debug.WriteLine("Deleted or Stopped: Need to clean subscriptions");
						ICommunication com = new ConnectionCommunication(senderConn);

						SubscriptionManager.RemoveSubscriptions(uniqueIdentifier, com);
					}
				}
				catch(Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of Source CleanupHandle event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private NewMessageEventHandler CreateDestinationElementCleanupHandle(int agentId, int elementId)
		{
			string myGuid = SourceIdentifier + "-" + agentId + "/" + elementId + "_CLP_DestElemCleanup";

			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var elementStateMessage = e.Message as ElementStateEventMessage;
					if (elementStateMessage == null) return;

					var senderConn = (Connection)sender;
					System.Diagnostics.Debug.WriteLine("Destination Element State Change:" + elementStateMessage.State);

					string destinationIdentifier = elementStateMessage.DataMinerID + "/" + elementStateMessage.ElementID;

					// Clear subscriptions if element is deleted.
					if (elementStateMessage.State == Net.Messages.ElementState.Deleted)
					{
						System.Diagnostics.Debug.WriteLine("Deleted: Need to clean subscriptions for destination element");
						ICommunication com = new ConnectionCommunication(senderConn);

						SubscriptionManager.RemoveSubscriptions(SourceIdentifier, destinationIdentifier, com);
					}
				}
				catch(Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of Destination CleanupHandle event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private NewMessageEventHandler CreateDestinationServiceCleanupHandle(int agentId, int serviceId)
		{
			string myGuid = SourceIdentifier + "-" + agentId + "/" + serviceId + "_CLP_DestServCleanup";

			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var serviceStateMessage = e.Message as ServiceStateEventMessage;
					if (serviceStateMessage == null) return;

					var senderConn = (Connection)sender;
					System.Diagnostics.Debug.WriteLine("Destination Service State Change. IsDeleted:" + serviceStateMessage.IsDeleted);

					string destinationIdentifier = serviceStateMessage.DataMinerID + "/" + serviceStateMessage.ElementID;

					// Clear subscriptions if service is deleted.
					if (serviceStateMessage.IsDeleted)
					{
						System.Diagnostics.Debug.WriteLine("Deleted: Need to clean subscriptions for destination service");
						ICommunication com = new ConnectionCommunication(senderConn);

						SubscriptionManager.RemoveSubscriptions(SourceIdentifier, destinationIdentifier, com);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of Destination CleanupHandle event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}
	}
}