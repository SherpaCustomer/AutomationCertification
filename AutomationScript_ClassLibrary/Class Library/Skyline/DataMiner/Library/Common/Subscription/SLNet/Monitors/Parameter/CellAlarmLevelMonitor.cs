namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Selectors.Data;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Globalization;

	internal class CellAlarmLevelMonitor : ValueMonitor
	{
		private Action<CellAlarmLevelChange> onChange;

		internal CellAlarmLevelMonitor(ICommunication connection, string sourceId, Cell selection, string handleId) : base(connection, sourceId, selection, handleId + "-AlarmLevel")
		{
		}

		internal CellAlarmLevelMonitor(ICommunication connection, Element sourceElement, Cell selection, string handleId) : base(connection, Convert.ToString(sourceElement, CultureInfo.InvariantCulture), selection, handleId + "-AlarmLevel")
		{
		}

		internal CellAlarmLevelMonitor(ICommunication connection, Element sourceElement, Cell selection) : base(connection, sourceElement, selection, "-AlarmLevel")
		{
		}

		internal CellAlarmLevelMonitor(ICommunication connection, string sourceId, Cell selection) : base(connection, sourceId, selection, "-AlarmLevel")
		{
		}

		internal void Start(Action<CellAlarmLevelChange> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			int parameterId = Selection.ParameterId;
			this.onChange = actionOnChange;

			Cell cell = (Cell)Selection;
			string pk = cell.PrimaryKey;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, elementId, parameterId, cell.TableId, pk);

			System.Diagnostics.Debug.WriteLine("Subscribing to Table Cell Alarm Level Changes: " + agentId + "/" + elementId + "/" + parameterId + "/" + pk);
			ActionHandle.Subscriptions = new SubscriptionFilter[] {
						new SubscriptionFilterParameter(typeof(ParameterTableUpdateEventMessage), agentId, elementId, parameterId, "^pk^"+pk),
						new SubscriptionFilterParameter(typeof(ParameterChangeEventMessage), agentId, elementId, parameterId, "^pk^"+pk)
					};

			TryAddElementCleanup();
			TryAddDestinationElementCleanup(agentId, elementId);

			SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, ActionHandle, true);
		}

		private NewMessageEventHandler CreateHandler(string HandleGuid, int dmaId, int eleId, int pid, int tablePid, string pk)
		{
			string myGuid = HandleGuid;
			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var paramChangeMessage = e.Message as ParameterChangeEventMessage;

					if (paramChangeMessage == null) return;

					CellAlarmLevel cellAlarmLevel;

					if (String.IsNullOrWhiteSpace(paramChangeMessage.TableIndexPK))
					{
						System.Diagnostics.Debug.WriteLine("Received Event from stand-alone.");
						return;
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Received Event from Table Cell.");
						cellAlarmLevel = SLNetUtility.ParseCellParameterChangeEventMessageAlarmLevel(paramChangeMessage, tablePid);
					}

					if (cellAlarmLevel.Cell.AgentId == dmaId && cellAlarmLevel.Cell.ElementId == eleId && cellAlarmLevel.Cell.ParameterId == pid && cellAlarmLevel.Cell.PrimaryKey == pk)
					{
						System.Diagnostics.Debug.WriteLine("Match found.");
						HandleMatchingEvent(sender, myGuid, cellAlarmLevel);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of CellAlarmLevel event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, CellAlarmLevel cellAlarmLevel)
		{
			string result = cellAlarmLevel.ToString();
			if (onChange != null)
			{
				var senderConn = (Connection)sender;
				ICommunication com = new ConnectionCommunication(senderConn);

				var changed = new CellAlarmLevelChange(new Cell(cellAlarmLevel.Cell.AgentId, cellAlarmLevel.Cell.ElementId, cellAlarmLevel.Cell.TableId, cellAlarmLevel.Cell.ParameterId, cellAlarmLevel.Cell.PrimaryKey), cellAlarmLevel.AlarmLevel, SourceIdentifier, new Dms(com));

				if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, myGuid, "Alarm Level", changed))
				{
					System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + result);
					try
					{
						onChange(changed);
					}
					catch (Exception delegateEx)
					{
						var message = "Monitor Error: Exception during Handle of CellAlarmLevel event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
						System.Diagnostics.Debug.WriteLine(message);
						Logger.Log(message);
					}
				}
			}
		}
	}
}