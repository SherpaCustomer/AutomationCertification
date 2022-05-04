namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Selectors.Data;
	using Skyline.DataMiner.Library.Common.SLNetHelper;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Diagnostics;

	internal class CellValueMonitor<T> : CellValueMonitor
	{
		private Action<CellValueChange<T>> onChange;

		internal CellValueMonitor(ICommunication connection, string sourceId, Cell selection, string handleId) : base(connection, sourceId, selection, handleId)
		{
		}

		internal CellValueMonitor(ICommunication connection, Element sourceElement, Cell selection, string handleId) : base(connection, sourceElement, selection, handleId)
		{
		}

		internal CellValueMonitor(ICommunication connection, Element sourceElement, Cell selection) : base(connection, sourceElement, selection)
		{
		}

		internal CellValueMonitor(ICommunication connection, string sourceId, Cell selection) : base(connection, sourceId, selection)
		{
		}

		internal void Start(Action<CellValueChange<T>> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			int parameterId = Selection.ParameterId;

			this.onChange = actionOnChange;

			Cell cell = (Cell)Selection;
			string pk = cell.PrimaryKey;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId, agentId, elementId, parameterId, cell.TableId, pk);

			System.Diagnostics.Debug.WriteLine("Subscribing to Table Cell Value Changes: " + agentId + "/" + elementId + "/" + parameterId + "/" + pk);
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
					if (!(e.Message is ParameterChangeEventMessage)) return;

					CellValue cellData;

					var paramChangeMessage = e.Message as ParameterChangeEventMessage;
					if (String.IsNullOrWhiteSpace(paramChangeMessage.TableIndexPK))
					{
						Debug.WriteLine("Received Event from stand-alone.");
						return;
					}
					else
					{
						Debug.WriteLine("Received Event from Table Cell.");

						cellData = SLNetUtility.ParseCellParameterChangeEventMessage<T>(paramChangeMessage, tablePid);
					}

					if (cellData.Cell.AgentId == dmaId && cellData.Cell.ElementId == eleId && cellData.Cell.ParameterId == pid && cellData.Cell.PrimaryKey == pk)
					{
						Debug.WriteLine("Match found.");
						HandleMatchingEvent(sender, myGuid, cellData);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of CellValue event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, CellValue cellData)
		{
			string result = cellData.ToString();
			if (onChange != null)
			{
				var senderConn = (Connection)sender;
				ICommunication com = new ConnectionCommunication(senderConn);

				var cell = new Cell(cellData.Cell.AgentId, cellData.Cell.ElementId, cellData.Cell.TableId, cellData.Cell.ParameterId, cellData.Cell.PrimaryKey);
				var changed = new CellValueChange<T>(cell, (T)cellData.Value, SourceIdentifier, new Dms(com));

				if (SubscriptionManager.ReplaceIfDifferentCachedData(SourceIdentifier, myGuid, "Result", changed))
				{
					System.Diagnostics.Debug.WriteLine("Trigger Action - Different Result:" + result);
					try
					{
						onChange(changed);
					}
					catch (Exception delegateEx)
					{
						var message = "Monitor Error: Exception during Handle of CellValue event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
						System.Diagnostics.Debug.WriteLine(message);
						Logger.Log(message);
					}
				}
			}
		}
	}
}