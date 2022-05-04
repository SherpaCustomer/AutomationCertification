namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents the replication information of an element.
	/// </summary>
	internal class ReplicationSettings : ElementSettings, IReplicationSettings
	{
		/// <summary>
		/// The domain the specified user belongs to.
		/// </summary>
		private string domain = String.Empty;

		/// <summary>
		/// External DMP engine.
		/// </summary>
		private bool connectsToExternalDmp;

		/// <summary>
		/// IP address of the source DataMiner Agent.
		/// </summary>
		private string ipAddressSourceDma = String.Empty;

		/// <summary>
		/// Value indicating whether this element is replicated.
		/// </summary>
		private bool isReplicated;

		/// <summary>
		/// The options string.
		/// </summary>
		private string options = String.Empty;

		/// <summary>
		/// The password.
		/// </summary>
		private string password = String.Empty;

		/// <summary>
		/// The ID of the source element.
		/// </summary>
		private DmsElementId sourceDmsElementId = new DmsElementId(-1, -1);

		/// <summary>
		/// The user name.
		/// </summary>
		private string userName = String.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplicationSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the DmsElement where this object will be used in.</param>
		internal ReplicationSettings(DmsElement dmsElement)
		: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets the domain the user belongs to.
		/// </summary>
		/// <value>The domain the user belongs to.</value>
		public string Domain
		{
			get
			{
				DmsElement.LoadOnDemand();
				return domain;
			}

			internal set
			{
				domain = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether it is allowed to perform logic of a protocol on the replicated element instead of only showing the data received on the original element.
		/// By Default, some functionality is not allowed on replicated elements (get, set, QAs, triggers etc.).
		/// </summary>
		/// <value><c>true</c> if it is allowed to perform the logic of a protocol on the replicated element; otherwise, <c>false</c>.</value>
		public bool ConnectsToExternalProbe
		{
			get
			{
				DmsElement.LoadOnDemand();
				return connectsToExternalDmp;
			}
		}

		/// <summary>
		/// Gets the IP address of the DataMiner Agent from which this element is replicated.
		/// </summary>
		/// <value>The IP address of the DataMiner Agent from which this element is replicated</value>
		public string IPAddressSourceAgent
		{
			get
			{
				DmsElement.LoadOnDemand();
				return ipAddressSourceDma;
			}

			internal set
			{
				ipAddressSourceDma = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this element is replicated.
		/// </summary>
		/// <value><c>true</c> if this element is replicated; otherwise, <c>false</c>.</value>
		public bool IsReplicated
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isReplicated;
			}

			internal set
			{
				isReplicated = value;
			}
		}

		/// <summary>
		/// Gets the additional options defined when replicating the element.
		/// </summary>
		/// <value>The additional options defined when replicating the element.</value>
		public string Options
		{
			get
			{
				DmsElement.LoadOnDemand();
				return options;
			}

			internal set
			{
				options = value;
			}
		}

		/// <summary>
		/// Gets the password corresponding with the user name to log in on the source DataMiner Agent.
		/// </summary>
		/// <value>The password corresponding with the user name.</value>
		public string Password
		{
			get
			{
				DmsElement.LoadOnDemand();
				return password;
			}

			internal set
			{
				password = value;
			}
		}

		/// <summary>
		/// Gets the system-wide element ID of the source element.
		/// </summary>
		/// <value>The system-wide element ID of the source element.</value>
		public DmsElementId SourceDmsElementId
		{
			get
			{
				DmsElement.LoadOnDemand();
				return sourceDmsElementId;
			}

			internal set
			{
				sourceDmsElementId = value;
			}
		}

		/// <summary>
		/// Gets the user name used to log in on the source DataMiner Agent.
		/// </summary>
		/// <value>The user name used to log in on the source DataMiner Agent.</value>
		public string UserName
		{
			get
			{
				DmsElement.LoadOnDemand();
				return userName;
			}

			internal set
			{
				userName = value;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("REPLICATION SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendFormat(CultureInfo.InvariantCulture, "Replicated: {0}{1}", isReplicated, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Source DMA ID: {0}{1}", sourceDmsElementId.AgentId, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Source element ID: {0}{1}", sourceDmsElementId.ElementId, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "IP address source DMA: {0}{1}", ipAddressSourceDma, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Domain: {0}{1}", domain, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "User name: {0}{1}", userName, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Password: {0}{1}", password, Environment.NewLine);
			//sb.AppendFormat(CultureInfo.InvariantCulture, "Options: {0}{1}", options, Environment.NewLine);
			//sb.AppendFormat(CultureInfo.InvariantCulture, "Replication DMP engine: {0}{1}", connectsToExternalDmp, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message which will be sent to SLNet.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			isReplicated = elementInfo.ReplicationActive;
			if (!isReplicated)
			{
				options = String.Empty;
				ipAddressSourceDma = String.Empty;
				password = String.Empty;
				domain = String.Empty;
				sourceDmsElementId = new DmsElementId(-1, -1);
				userName = String.Empty;
				connectsToExternalDmp = false;
			}

			options = elementInfo.ReplicationOptions ?? String.Empty;
			ipAddressSourceDma = elementInfo.ReplicationDmaIP ?? String.Empty;
			password = elementInfo.ReplicationPwd ?? String.Empty;
			domain = elementInfo.ReplicationDomain ?? String.Empty;

			bool isEmpty = String.IsNullOrWhiteSpace(elementInfo.ReplicationRemoteElement) || elementInfo.ReplicationRemoteElement.Equals("/", StringComparison.Ordinal);

			if (isEmpty)
			{
				sourceDmsElementId = new DmsElementId(-1, -1);
			}
			else
			{
				try
				{
					sourceDmsElementId = new DmsElementId(elementInfo.ReplicationRemoteElement);
				}
				catch (Exception ex)
				{
					string logMessage = "Failed parsing replication element info for element " + Convert.ToString(elementInfo.Name) + " (" + Convert.ToString(elementInfo.DataMinerID) + "/" + Convert.ToString(elementInfo.ElementID) + "). Replication remote element is: " + Convert.ToString(elementInfo.ReplicationRemoteElement) + Environment.NewLine + ex;
					Logger.Log(logMessage);
					sourceDmsElementId = new DmsElementId(-1, -1);
				}
			}

			userName = elementInfo.ReplicationUser ?? String.Empty;
			connectsToExternalDmp = elementInfo.ReplicationIsExternalDMP;
		}
	}
}