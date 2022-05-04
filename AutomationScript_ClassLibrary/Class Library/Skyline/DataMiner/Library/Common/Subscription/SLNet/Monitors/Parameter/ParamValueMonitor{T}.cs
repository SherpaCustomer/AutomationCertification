namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Selectors.Data;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.IO;

	internal class ParamValueMonitor<T> : ParamValueMonitor
	{
		private Action<ParamValueChange<T>> onChange;

		internal ParamValueMonitor(ICommunication connection, string sourceId, Param selection, string handleId) : base(connection, sourceId, selection, handleId)
		{
		}

		internal ParamValueMonitor(ICommunication connection, Element sourceElement, Param selection, string handleId) : base(connection, sourceElement, selection, handleId)
		{
		}

		internal ParamValueMonitor(ICommunication connection, Element sourceElement, Param selection) : base(connection, sourceElement, selection)
		{
		}

		internal ParamValueMonitor(ICommunication connection, string sourceId, Param selection) : base(connection, sourceId, selection)
		{
		}

		internal void Start(Action<ParamValueChange<T>> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			int parameterId = Selection.ParameterId;
			this.onChange = actionOnChange;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, elementId, parameterId);

			System.Diagnostics.Debug.WriteLine("Subscribing to parameter value changes: " + agentId + "/" + elementId + "/" + parameterId);
			ActionHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterParameter(typeof(ParameterChangeEventMessage), agentId, elementId, parameterId) };

			TryAddElementCleanup();
			TryAddDestinationElementCleanup(agentId, elementId);

			SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, ActionHandle, true);
		}

		private NewMessageEventHandler CreateHandler(string HandleGuid, int dmaId, int eleId, int pid)
		{
			string myGuid = HandleGuid;
			return (sender, e) =>
			{
				try
				{

					if (!e.FromSet(myGuid)) return;
					if (!(e.Message is ParameterChangeEventMessage)) return;
					ParamValue parameterData;


					var paramChangeMessage = e.Message as ParameterChangeEventMessage;
					if (String.IsNullOrWhiteSpace(paramChangeMessage.TableIndexPK))
					{
						System.Diagnostics.Debug.WriteLine("Received Event from stand-alone.");
						parameterData = SLNetUtility.ParseStandaloneParameterChangeEventMessageString<T>(paramChangeMessage);
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Received Event from Table Cell.");
						return;
					}

					if (parameterData.Param.AgentId == dmaId && parameterData.Param.ElementId == eleId && parameterData.Param.ParameterId == pid)
					{
						System.Diagnostics.Debug.WriteLine("Match found.");
						HandleMatchingEvent(sender, myGuid, parameterData);
					}
				}
				catch (Exception ex)
				{ 
					var message = "Monitor Error: Exception during Handle of ParamValue event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, ParamValue parameterData)
		{
			string result = parameterData.ToString();
			if (onChange != null)
			{
				var senderConn = (Connection)sender;
				ICommunication com = new ConnectionCommunication(senderConn);

				Param dataSource = new Param(parameterData.Param.AgentId, parameterData.Param.ElementId, parameterData.Param.ParameterId);
				var changed = new ParamValueChange<T>(dataSource, (T)parameterData.Value, SourceIdentifier, new Dms(com));

				if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, myGuid, "Result", changed))
				{
					System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + result);
					try
					{
						onChange(changed);
					}
					catch (Exception delegateEx)
					{
						var message = "Monitor Error: Exception during Handle of ParamValue event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
						System.Diagnostics.Debug.WriteLine(message);
						Logger.Log(message);
					}
				}
			}
		}
	}
}