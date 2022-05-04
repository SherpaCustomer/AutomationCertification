namespace Skyline.DataMiner.Library.Common.Subscription.Waiters.Parameter
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Selectors.Data;
	using Skyline.DataMiner.Library.Common.Subscription.Monitors;

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;

	/// <summary>
	/// Class that uses subscriptions to wait for expected values.
	/// </summary>
	internal class ParamWaiter : IDisposable
	{
		private readonly List<WaitHandle> handles;
		private readonly object locker = new object();
		private readonly HashSet<string> monitoredValues;
		private bool disposedValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="ParamWaiter"/> class.
		/// </summary>
		/// <param name="connection">The raw SLNet connection.</param>
		/// <param name="parameters">A list of parameters to monitor for a particular change.</param>
		/// <exception cref="FormatException">If one of the provided parameters is missing data.</exception>
		internal ParamWaiter(ICommunication connection, params ParamValue[] parameters)
		{
			// I need to make a single subscription for a unique dmaid/eleid/pid
			// So I first have to filter all the received commands and group them by that key.
			var paramsGrouped = parameters.GroupBy(p => p.Param.AgentId + "/" + p.Param.ElementId + "/" + p.Param.ParameterId);

			handles = new List<WaitHandle>();
			monitoredValues = new HashSet<string>();
			foreach (var param in parameters)
			{
				monitoredValues.Add(param.ToString());
				System.Diagnostics.Debug.WriteLine("Monitoring: " + param);
			}

			foreach (var param in paramsGrouped)
			{
				string uniquePid = param.Key;
				string[] splitUniquePid = uniquePid.Split('/');
				if (splitUniquePid.Length < 3) throw new FormatException("Param needs dmaId, eleId and pid: " + uniquePid);

				int dmaId = Convert.ToInt32(splitUniquePid[0], CultureInfo.InvariantCulture);
				int eleId = Convert.ToInt32(splitUniquePid[1], CultureInfo.InvariantCulture);
				int pid = Convert.ToInt32(splitUniquePid[2], CultureInfo.InvariantCulture);

				StartMonitor(connection, splitUniquePid, dmaId, eleId, pid);
			}
		}

		/// <summary>
		/// This code added to correctly implement the disposable pattern.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Waits on all requested changes or timeout time.
		/// </summary>
		/// <param name="timeout">Maximum amount of time to wait on each change.</param>
		/// <returns>The changed value once it has reached the expected change.</returns>
		/// <exception cref="TimeoutException">Expected change took too long.</exception>
		public IEnumerable<ParamValue> WaitNext(TimeSpan timeout)
		{
			AutoResetEvent[] handleFlags = handles.Select(p => p.Flag).ToArray();
			while (monitoredValues.Any())
			{
#pragma warning disable S2330 // Array covariance should not be used
				int trigger = AutoResetEvent.WaitAny(handleFlags, timeout);
#pragma warning restore S2330 // Array covariance should not be used
				if (trigger != System.Threading.WaitHandle.WaitTimeout)
				{
					var handle = handles[trigger];

					ParamValue response;
					if (!handle.TriggeredQueue.TryDequeue(out response)) continue;

					lock (locker)
					{
						if (monitoredValues.Remove(response.ToString()))
						{
							System.Diagnostics.Debug.WriteLine("Returning Change: " + response);
							yield return response;
						}
					}
				}
				else
				{
					throw new TimeoutException("Timeout while waiting on expected values: " + string.Join(";", monitoredValues));
				}
			}

			foreach (var handle in handles)
			{
				handle.Monitor.Stop();
			}
		}

		/// <summary>
		/// To detect redundant calls.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach (var handle in handles)
					{
						handle.Monitor.Stop(true);
					}
				}

				disposedValue = true;
			}
		}

		private void StartMonitor(ICommunication connection, string[] splitUniquePid, int dmaId, int eleId, int pid)
		{
			Param toMonitor;
			if (splitUniquePid.Length > 3)
			{
				int tableId = Convert.ToInt32(splitUniquePid[3], CultureInfo.InvariantCulture);
				string pk = splitUniquePid[4];
				toMonitor = new Cell(dmaId, eleId, tableId, pid, pk);
			}
			else
			{
				toMonitor = new Param(dmaId, eleId, pid);
			}

			var monitor = new ParamValueMonitor<string>(connection, Guid.NewGuid().ToString(), toMonitor);

			var thisHandle = new WaitHandle
			{
				Flag = new AutoResetEvent(false),
				Monitor = monitor,
				TriggeredQueue = new ConcurrentQueue<ParamValue>()
			};

			handles.Add(thisHandle);

			monitor.Start(change =>
			{
				var parameterData = new ParamValue(change.DataSource.AgentId, change.DataSource.ElementId, change.DataSource.ParameterId, change.Value);
				System.Diagnostics.Debug.WriteLine("Match found.");
				string result = parameterData.ToString();
				System.Diagnostics.Debug.WriteLine("Result:" + result);
				if (monitoredValues.Contains(result))
				{
					System.Diagnostics.Debug.WriteLine("Found Monitored Value");
					thisHandle.TriggeredQueue.Enqueue(parameterData);
					thisHandle.Flag.Set();
				}
			});
		}

		private class WaitHandle
		{
			public AutoResetEvent Flag { get; set; }

			public Monitors.ParamValueMonitor Monitor { get; set; }

			public ConcurrentQueue<ParamValue> TriggeredQueue { get; set; }
		}
	}
}