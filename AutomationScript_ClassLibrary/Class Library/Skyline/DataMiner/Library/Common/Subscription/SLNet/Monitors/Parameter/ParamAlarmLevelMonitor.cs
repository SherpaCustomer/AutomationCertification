namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Selectors.Data;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Globalization;

	internal class ParamAlarmLevelMonitor : ValueMonitor
	{
		private Action<ParamAlarmLevelChange> onChange;

		internal ParamAlarmLevelMonitor(ICommunication connection, string sourceId, Param selection, string handleId) : base(connection, sourceId, selection, handleId + "-AlarmLevel")
		{
		}

		internal ParamAlarmLevelMonitor(ICommunication connection, Element sourceElement, Param selection, string handleId) : base(connection, Convert.ToString(sourceElement, CultureInfo.InvariantCulture), selection, handleId + "-AlarmLevel")
		{
		}

		internal ParamAlarmLevelMonitor(ICommunication connection, Element sourceElement, Param selection) : base(connection, sourceElement, selection, "-AlarmLevel")
		{
		}

		internal ParamAlarmLevelMonitor(ICommunication connection, string sourceId, Param selection) : base(connection, sourceId, selection, "-AlarmLevel")
		{
		}

		internal void Start(Action<ParamAlarmLevelChange> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			int parameterId = Selection.ParameterId;

			this.onChange = actionOnChange;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, elementId, parameterId);
			ActionHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterParameter(typeof(ParameterChangeEventMessage), agentId, elementId, parameterId) };

			TryAddElementCleanup();
			TryAddDestinationElementCleanup(agentId, elementId);

			System.Diagnostics.Debug.WriteLine("Subscribing to Parameter Alarm State Changes: " + agentId + "/" + elementId + "/" + parameterId);
			SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, ActionHandle, true);
		}

		private NewMessageEventHandler CreateHandler(string handleGuid, int agentId, int elementId, int pid)
		{
			string myGuid = handleGuid;

			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var paramChangeMessage = e.Message as ParameterChangeEventMessage;

					if (paramChangeMessage == null) return;

					ParamAlarmLevel parameterAlarmStateData;
					if (String.IsNullOrWhiteSpace(paramChangeMessage.TableIndexPK))
					{
						System.Diagnostics.Debug.WriteLine("Received event from stand-alone parameter.");

						parameterAlarmStateData = SLNetUtility.ParseAlarmStateParameterChangeEvent(paramChangeMessage);
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Received event from table cell.");
						return;
					}

					if (parameterAlarmStateData.Param.AgentId == agentId && parameterAlarmStateData.Param.ElementId == elementId && parameterAlarmStateData.Param.ParameterId == pid)
					{
						System.Diagnostics.Debug.WriteLine("Match found.");
						HandleMatchingEvent(sender, myGuid, parameterAlarmStateData);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of ParamAlarmLevel event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, ParamAlarmLevel parameterAlarmStateData)
		{
			string result = parameterAlarmStateData.ToString();

			if (onChange != null)
			{
				var senderConn = (Connection)sender;
				ICommunication com = new ConnectionCommunication(senderConn);

				var changed = new ParamAlarmLevelChange(new Param(parameterAlarmStateData.Param.AgentId, parameterAlarmStateData.Param.ElementId, parameterAlarmStateData.Param.ParameterId), parameterAlarmStateData.AlarmLevel, SourceIdentifier, new Dms(com));

				if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, myGuid, "AlarmState", changed))
				{
					System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + result);
					try
					{
						onChange(changed);
					}
					catch (Exception delegateEx)
					{
						var message = "Monitor Error: Exception during Handle of ParamAlarmLevel event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
						System.Diagnostics.Debug.WriteLine(message);
						Logger.Log(message);
					}
				}
			}
		}
	}
}