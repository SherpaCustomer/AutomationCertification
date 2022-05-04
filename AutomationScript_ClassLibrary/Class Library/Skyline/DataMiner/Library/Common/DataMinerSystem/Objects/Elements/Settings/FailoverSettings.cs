namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents a class containing the failover settings for an element.
	/// </summary>
	internal class FailoverSettings : ElementSettings, IFailoverSettings
	{
		/// <summary>
		/// In failover configurations, this can be used to force an element to run only on one specific agent.
		/// </summary>
		private string forceAgent = String.Empty;

		/// <summary>
		/// Is true when the element is a failover element and is online on the backup agent instead of this agent; otherwise, false.
		/// </summary>
		private bool isOnlineOnBackupAgent;

		/// <summary>
		/// Is true when the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, false.
		/// </summary>
		private bool keepOnline;

		/// <summary>
		/// Initializes a new instance of the <see cref="FailoverSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the DmsElement where this object will be used in.</param>
		internal FailoverSettings(DmsElement dmsElement)
		: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether to force agent.
		/// Local IP address of the agent which will be running the element.
		/// </summary>
		/// <value>Value indicating whether to force agent.</value>
		public string ForceAgent
		{
			get
			{
				DmsElement.LoadOnDemand();
				return forceAgent;
			}

			set
			{
				DmsElement.LoadOnDemand();

				var newValue = value == null ? String.Empty : value;
				if (!forceAgent.Equals(newValue, StringComparison.Ordinal))
				{
					ChangedPropertyList.Add("ForceAgent");
					forceAgent = newValue;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the element is a failover element and is online on the backup agent instead of this agent.
		/// </summary>
		/// <value><c>true</c> if the element is a failover element and is online on the backup agent instead of this agent; otherwise, <c>false</c>.</value>
		public bool IsOnlineOnBackupAgent
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isOnlineOnBackupAgent;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the element is a failover element that needs to keep running on the same DataMiner agent event after switching.
		/// keepOnline="true" indicates that the element needs to keep running even when the agent is offline.
		/// </summary>
		/// <value><c>true</c> if the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, <c>false</c>.</value>
		public bool KeepOnline
		{
			get
			{
				DmsElement.LoadOnDemand();
				return keepOnline;
			}

			set
			{
				DmsElement.LoadOnDemand();

				if (keepOnline != value)
				{
					ChangedPropertyList.Add("KeepOnline");
					keepOnline = value;
				}
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("FAILOVER SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendFormat(CultureInfo.InvariantCulture, "Keep online: {0}{1}", KeepOnline, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Force agent: {0}{1}", ForceAgent, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Online on backup agent: {0}{1}", IsOnlineOnBackupAgent, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message which will be sent to SLNet.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
			foreach (string property in ChangedPropertyList)
			{
				switch (property)
				{
					case "ForceAgent":
						message.ForceAgent = forceAgent;
						break;

					case "KeepOnline":
						message.KeepOnline = keepOnline;
						break;

					default:
						continue;
				}
			}
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			keepOnline = elementInfo.KeepOnline;
			forceAgent = elementInfo.ForceAgent ?? String.Empty;
			isOnlineOnBackupAgent = elementInfo.IsOnlineOnBackupAgent;
		}
	}
}