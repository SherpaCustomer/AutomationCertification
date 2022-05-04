namespace Skyline.DataMiner.Library.Common.Subscription.Waiters.InterApp
{
	using Skyline.DataMiner.Library.Common.InterAppCalls.CallSingle;
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Serializing;
	using Skyline.DataMiner.Library.Common.Subscription.Monitors;

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using Skyline.DataMiner.Library.Common.InterAppCalls.CallBulk;

	internal class MessageWaiter : IDisposable
	{
		private readonly List<WaitHandle> handles;
		private readonly HashSet<string> monitoredGuids;

		private bool disposedValue;

		public MessageWaiter(ICommunication connection, ISerializer interAppSerializer, ISerializer messageSerializer, params Message[] commands)
		{
			// I need to make a single subscription for a unique dmaid/eleid/pid
			// So I first have to filter all the received commands and group them by that key.
			var commandsGrouped = commands.GroupBy(p => p.ReturnAddress.AgentId + "/" + p.ReturnAddress.ElementId + "/" + p.ReturnAddress.ParameterId);

			handles = new List<WaitHandle>();
			monitoredGuids = new HashSet<string>();
			foreach (var cmd in commands)
			{
				monitoredGuids.Add(cmd.Guid);
			}

			foreach (var commandGroup in commandsGrouped)
			{
				string uniquePid = commandGroup.Key;
				string[] splitUniquePid = uniquePid.Split('/');
				if (splitUniquePid.Length < 3) throw new FormatException("Return address needs agentId, elementId and parameterId: " + uniquePid);

				int dmaId = Convert.ToInt32(splitUniquePid[0], CultureInfo.InvariantCulture);
				int eleId = Convert.ToInt32(splitUniquePid[1], CultureInfo.InvariantCulture);
				int pid = Convert.ToInt32(splitUniquePid[2], CultureInfo.InvariantCulture);

				StartMonitor(connection, interAppSerializer, messageSerializer, dmaId, eleId, pid);
			}
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public IEnumerable<Message> WaitNext(TimeSpan timeout)
		{
			AutoResetEvent[] handleFlags = handles.Select(p => p.Flag).ToArray();
			while (monitoredGuids.Any())
			{
#pragma warning disable S2330 // Array covariance should not be used
				int trigger = AutoResetEvent.WaitAny(handleFlags, timeout);
#pragma warning restore S2330 // Array covariance should not be used
				if (trigger != System.Threading.WaitHandle.WaitTimeout)
				{
					var handle = handles[trigger];

					Message response;
					if (handle.TriggeredQueue.TryDequeue(out response))
					{
						yield return response;
						monitoredGuids.Remove(response.Guid);
					}
				}
				else
				{
					throw new TimeoutException("Timeout while waiting on responses: " + string.Join(";", monitoredGuids));
				}
			}

			foreach (var handle in handles)
			{
				handle.Monitor.Stop();
			}

			handles.Clear();
		}

		// To detect redundant calls
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach (var handle in handles)
					{
						handle.Monitor.Stop();
					}

					handles.Clear();
				}

				disposedValue = true;
			}
		}

		private void StartMonitor(ICommunication connection, ISerializer interAppSerializer, ISerializer messageSerializer, int dmaId, int eleId, int pid)
		{
			ParamValueMonitor<string> monitor = new ParamValueMonitor<string>(connection, System.Guid.NewGuid().ToString(), new Param(dmaId, eleId, pid));

			var thisHandle = new WaitHandle
			{
				Flag = new AutoResetEvent(false),
				Monitor = monitor,
				TriggeredQueue = new ConcurrentQueue<Message>()
			};

			handles.Add(thisHandle);

			monitor.Start(change =>
			{
				try
				{
					var parameterData = change.Value;
					var rawData = Convert.ToString(parameterData, CultureInfo.InvariantCulture);
					if (parameterData == null || String.IsNullOrWhiteSpace(rawData))
					{
						return;
					}
					
					var interApp = InterAppCallFactory.CreateFromRawAndAcceptMessage(rawData, interAppSerializer: interAppSerializer, messageSerializer: messageSerializer);
					if (interApp == null)
					{
						return;
					}

					foreach (Message messageReturn in interApp.Messages)
					{
						if (messageReturn != null && monitoredGuids.Contains(messageReturn.Guid))
						{
							thisHandle.TriggeredQueue.Enqueue(messageReturn);
							thisHandle.Flag.Set();
						}
					}
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine("Exception during Message Deserializing: " + e);
					// Do nothing for now, but this should eventually get logged to error logging.
				}
			});
		}

		private class WaitHandle
		{
			public AutoResetEvent Flag { get; set; }

			public Monitors.ParamValueMonitor Monitor { get; set; }

			public ConcurrentQueue<Message> TriggeredQueue { get; set; }
		}
	}
}