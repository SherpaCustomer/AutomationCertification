namespace Skyline.DataMiner.Library.Common.SLNetHelper
{
	using Skyline.DataMiner.Net;

	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Threading;

	internal enum WaitHandleType
	{
		Cleanup,
		Normal
	}

	internal class SLNetWaitHandle
	{
		/// <summary>
		/// Stores Cached Data. Mainly used in Monitors.
		/// </summary>
		internal ConcurrentDictionary<string, object> CachedData { get; set; }

		internal string Destination { get; set; }

		/// <summary>
		/// A flag that can be asynchronously triggered. Mainly used in InterApp.
		/// </summary>
		internal AutoResetEvent Flag { get; set; }

		internal NewMessageEventHandler Handler { get; set; }

		internal string SetId { get; set; }

		internal SubscriptionFilter[] Subscriptions { get; set; }

		/// <summary>
		/// Allows storing of events to be handled when AutoResetEvent Flag is triggered. Mainly used in InterApp.
		/// </summary>
		internal ConcurrentQueue<object> TriggeredQueue { get; set; }

		internal WaitHandleType Type { get; set; }

		public override bool Equals(object obj)
		{
			var handle = obj as SLNetWaitHandle;
			return handle != null && SetId == handle.SetId;
		}

		public override int GetHashCode()
		{
			return 2074477814 + EqualityComparer<string>.Default.GetHashCode(SetId);
		}
	}
}