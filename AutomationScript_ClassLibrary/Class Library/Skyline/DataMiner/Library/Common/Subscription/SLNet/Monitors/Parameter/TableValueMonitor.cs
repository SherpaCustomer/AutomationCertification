namespace Skyline.DataMiner.Library.Common.Subscription.Monitors
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Subscription.Monitors.Parameter.Helpers;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	internal class TableValueMonitor : ParamValueMonitor
	{
		private readonly TableCache cache = new TableCache();

		private Action<TableValueChange> onChange;
		private int primaryKeyColumnIdx;

		internal TableValueMonitor(ICommunication connection, string sourceId, Param selection, string handleId) : base(connection, sourceId, selection, handleId)
		{
		}

		internal TableValueMonitor(ICommunication connection, Element sourceElement, Param selection, string handleId) : base(connection, sourceElement, selection, handleId)
		{
		}

		internal TableValueMonitor(ICommunication connection, Element sourceElement, Param selection) : base(connection, sourceElement, selection)
		{
		}
		internal TableValueMonitor(ICommunication connection, string sourceId, Param selection) : base(connection, sourceId, selection)
		{
		}

		internal void Start(int primaryKeyColumnIdx, Action<TableValueChange> actionOnChange)
		{
			int agentId = Selection.AgentId;
			int elementId = Selection.ElementId;
			int parameterId = Selection.ParameterId;

			this.primaryKeyColumnIdx = primaryKeyColumnIdx;
			this.onChange = actionOnChange;

			ActionHandle.Handler = CreateHandler(ActionHandle.SetId);

			System.Diagnostics.Debug.WriteLine("Subscribing to table value changes: " + agentId + "/" + elementId + "/" + parameterId);
			ActionHandle.Subscriptions = new SubscriptionFilter[] { new SubscriptionFilterParameter(typeof(ParameterChangeEventMessage), agentId, elementId, parameterId) };

			TryAddElementCleanup();
			TryAddDestinationElementCleanup(agentId, elementId);

			SubscriptionManager.CreateSubscription(SourceIdentifier, Connection, ActionHandle, true);
		}

		private NewMessageEventHandler CreateHandler(string handleGuid)
		{
			string myGuid = handleGuid;
			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(myGuid)) return;

					var parameterChange = e.Message as ParameterChangeEventMessage;
					if (parameterChange != null)
					{
						var dataSource = new Param(parameterChange.DataMinerID, parameterChange.ElementID, parameterChange.ParameterID);

						bool isInitial = false;
						int idxColumnID = this.primaryKeyColumnIdx;
						string[] deletedRows = null;
						IDictionary<string, object[]> updatedRows;

						var parameterTableUpdate = parameterChange as ParameterTableUpdateEventMessage;
						if (parameterTableUpdate != null)
						{
							updatedRows = BuildDictionary(parameterTableUpdate);
							idxColumnID = parameterTableUpdate.IndexColumnID;
							deletedRows = parameterTableUpdate.DeletedRows;
						}
						else
						{
							// format of initial data is different, but we already have a method that can parse it
							updatedRows = DmsTable.BuildDictionary(parameterChange, 0);
							isInitial = true;
						}

						var senderConn = (Connection)sender;
						ICommunication com = new ConnectionCommunication(senderConn);

						var change = new TableValueChange(dataSource, SourceIdentifier, new Dms(com), idxColumnID, updatedRows, deletedRows, isInitial);
						HandleMatchingEvent(sender, myGuid, change);
					}
				}
				catch (Exception ex)
				{
					var message = "Monitor Error: Exception during Handle of ParameterTableUpdate event (Class Library Side): " + myGuid + " -- " + e + " With exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private void HandleMatchingEvent(object sender, string myGuid, TableValueChange change)
		{
			bool isChanged = cache.ApplyUpdate(change);

			if (isChanged || change.IsInitialData)
			{
				System.Diagnostics.Debug.WriteLine("Trigger Action - Changed data:" + change);
				try
				{
					if (onChange != null)
					{
						onChange(change);
					}
				}
				catch (Exception delegateEx)
				{
					var message = "Monitor Error: Exception during Handle of ParameterTableUpdate event (check provided action): " + myGuid + "-- With exception: " + delegateEx;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			}
		}

		private static IDictionary<string, object[]> BuildDictionary(ParameterTableUpdateEventMessage data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			var result = new Dictionary<string, object[]>();
			int keyColumnIndex = data.IndexColumnID;

			if (data.NewValue == null || data.NewValue.ArrayValue == null)
			{
				return result;
			}

			foreach (var ur in data.UpdatedRows)
			{
				object[] row = new object[ur.ArrayValue.Length];

				for (int i = 0; i < ur.ArrayValue.Length; i++)
				{
					var av = ur.ArrayValue[i];
					row[i] = av.CellValue.InteropValue;
				}

				string key = Convert.ToString(row[keyColumnIndex]);
				result[key] = row;
			}

			return result;
		}
	}
}