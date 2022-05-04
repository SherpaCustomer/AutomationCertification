namespace Skyline.DataMiner.Library.Common.Subscription.Waiters.Parameter
{
	using Skyline.DataMiner.Library.Common.Selectors;
	using Skyline.DataMiner.Library.Common.Selectors.Data;

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;

	/// <summary>
	/// Class that uses SLNet subscriptions to wait for expected values.
	/// </summary>
	internal class CellWaiter : IDisposable
	{
		private readonly List<WaitHandle> handles;
		private readonly object locker = new object();
		private readonly HashSet<string> monitoredValues;
		private bool disposedValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="CellWaiter"/> class.
		/// </summary>
		/// <param name="connection">The communication interface.</param>
		/// <param name="parameters">The parameters to monitor for a particular change.</param>
		/// <exception cref="FormatException">If one of the provided parameters is missing data.</exception>
		internal CellWaiter(ICommunication connection, params CellValue[] parameters)
		{
			// I need to make a single subscription for a unique dmaid/eleid/pid
			// So I first have to filter all the received commands and group them by that key.
			var paramsGrouped = parameters.GroupBy(p => p.Cell.AgentId + "/" + p.Cell.ElementId + "/" + p.Cell.TableId + "/" + p.Cell.ParameterId + "/" + p.Cell.PrimaryKey);

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
				if (splitUniquePid.Length < 5) throw new FormatException("Cell needs dmaId, eleId, tableId, columnId and PK: " + uniquePid);

				int dmaId = Convert.ToInt32(splitUniquePid[0], CultureInfo.InvariantCulture);
				int eleId = Convert.ToInt32(splitUniquePid[1], CultureInfo.InvariantCulture);
				int tableId = Convert.ToInt32(splitUniquePid[2], CultureInfo.InvariantCulture);
				int columnId = Convert.ToInt32(splitUniquePid[3], CultureInfo.InvariantCulture);
				string pk = splitUniquePid[4];

				StartMonitor(connection, dmaId, eleId, tableId, columnId, pk);
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
		public IEnumerable<CellValue> WaitNext(TimeSpan timeout)
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

					CellValue response;
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

		private void StartMonitor(ICommunication connection, int dmaId, int eleId, int tableId, int columnId, string pk)
		{
			Cell toMonitor = new Cell(dmaId, eleId, tableId, columnId, pk);

			var monitor = new Monitors.CellValueMonitor<string>(connection, System.Guid.NewGuid().ToString(), toMonitor);

			var thisHandle = new WaitHandle
			{
				Flag = new AutoResetEvent(false),
				Monitor = monitor,
				TriggeredQueue = new ConcurrentQueue<CellValue>()
			};

			handles.Add(thisHandle);

			monitor.Start(change =>
			{
				var cellData = new CellValue(change.DataSource.AgentId, change.DataSource.ElementId, change.DataSource.TableId, change.DataSource.ParameterId, change.DataSource.PrimaryKey, change.Value);
				System.Diagnostics.Debug.WriteLine("Match found.");
				string result = cellData.ToString();
				System.Diagnostics.Debug.WriteLine("Result:" + result);
				if (monitoredValues.Contains(result))
				{
					System.Diagnostics.Debug.WriteLine("Found Monitored Value");
					thisHandle.TriggeredQueue.Enqueue(cellData);
					thisHandle.Flag.Set();
				}
			});
		}

		private class WaitHandle
		{
			public AutoResetEvent Flag { get; set; }

			public Monitors.CellValueMonitor Monitor { get; set; }

			public ConcurrentQueue<CellValue> TriggeredQueue { get; set; }
		}
	}
}