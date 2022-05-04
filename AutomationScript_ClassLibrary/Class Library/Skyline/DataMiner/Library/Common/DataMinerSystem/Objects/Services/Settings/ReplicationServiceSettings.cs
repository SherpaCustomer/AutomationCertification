namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents the replication information of an element.
	/// </summary>
	internal class ReplicationServiceSettings : ServiceSettings, IReplicationServiceSettings
	{
		/// <summary>
		/// The domain the specified user belongs to.
		/// </summary>
		private string domain = String.Empty;
		
		/// <summary>
		/// IP address of the source DataMiner Agent.
		/// </summary>
		private string ipAddressSourceDma = String.Empty;

		/// <summary>
		/// Value indicating whether this service is replicated.
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
		/// The ID of the source service.
		/// </summary>
		private DmsServiceId? sourceDmsServiceId;

		/// <summary>
		/// The user name.
		/// </summary>
		private string userName = String.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplicationServiceSettings"/> class.
		/// </summary>
		/// <param name="dmsService">The reference to the DmsService where this object will be used in.</param>
		internal ReplicationServiceSettings(DmsService dmsService)
		: base(dmsService)
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
				DmsService.LoadOnDemand();
				return domain;
			}

			private set
			{
				domain = value;
			}
		}
		
		/// <summary>
		/// Gets the IP address of the DataMiner Agent from which this service is replicated.
		/// </summary>
		/// <value>The IP address of the DataMiner Agent from which this service is replicated.</value>
		public string IPAddressSourceAgent
		{
			get
			{
				DmsService.LoadOnDemand();
				return ipAddressSourceDma;
			}

			private set
			{
				ipAddressSourceDma = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this service is replicated.
		/// </summary>
		/// <value><c>true</c> if this service is replicated; otherwise, <c>false</c>.</value>
		public bool IsReplicated
		{
			get
			{
				DmsService.LoadOnDemand();
				return isReplicated;
			}

			private set
			{
				isReplicated = value;
			}
		}

		/// <summary>
		/// Gets the additional options defined when replicating the service.
		/// </summary>
		/// <value>The additional options defined when replicating the service.</value>
		public string Options
		{
			get
			{
				DmsService.LoadOnDemand();
				return options;
			}

			private set
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
				DmsService.LoadOnDemand();
				return password;
			}

			private set
			{
				password = value;
			}
		}

		/// <summary>
		/// Gets the system-wide service ID of the source service.
		/// </summary>
		/// <value>The system-wide service ID of the source service.</value>
		public DmsServiceId? SourceDmsServiceId
		{
			get
			{
				DmsService.LoadOnDemand();
				return sourceDmsServiceId;
			}

			private set
			{
				sourceDmsServiceId = value;
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
				DmsService.LoadOnDemand();
				return userName;
			}

			private set
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
			sb.AppendFormat(CultureInfo.InvariantCulture, "Source DMA ID: {0}{1}", SourceDmsServiceId.HasValue ? sourceDmsServiceId.Value.AgentId.ToString() : "<<NULL>>", Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Source service ID: {0}{1}", SourceDmsServiceId.HasValue ? sourceDmsServiceId.Value.ServiceId.ToString() : "<<NULL>>", Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "IP address source DMA: {0}{1}", ipAddressSourceDma, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Domain: {0}{1}", domain, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "User name: {0}{1}", userName, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Password: {0}{1}", password, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Fills in the needed properties in the AddService message.
		/// </summary>
		/// <param name="message">The AddService message which will be sent to SLNet.</param>
		internal override void FillUpdate(AddServiceMessage message)
		{
			if (isReplicated)
			{
				message.Service.ReplicationActive = true;
				message.Service.ReplicationDmaIP = ipAddressSourceDma;
				message.Service.ReplicationDomain = domain;
				message.Service.ReplicationOptions = options;
				message.Service.ReplicationPwd = password;
				message.Service.ReplicationRemoteService = SourceDmsServiceId.HasValue ? SourceDmsServiceId.Value.Value : null;
				message.Service.ReplicationUser = userName;
			}
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="serviceInfo">The service information.</param>
		internal override void Load(ServiceInfoEventMessage serviceInfo)
		{
			isReplicated = serviceInfo.ReplicationActive;
			if (!isReplicated)
			{
				options = String.Empty;
				ipAddressSourceDma = String.Empty;
				password = String.Empty;
				domain = String.Empty;
				sourceDmsServiceId = null;
				userName = String.Empty;
			}

			options = serviceInfo.ReplicationOptions ?? String.Empty;
			ipAddressSourceDma = serviceInfo.ReplicationDmaIP ?? String.Empty;
			password = serviceInfo.ReplicationPwd ?? String.Empty;
			domain = serviceInfo.ReplicationDomain ?? String.Empty;

			bool isEmpty = String.IsNullOrWhiteSpace(serviceInfo.ReplicationRemoteService) || serviceInfo.ReplicationRemoteService.Equals("/", StringComparison.Ordinal);

			if (isEmpty)
			{
				sourceDmsServiceId = null;
			}
			else
			{
				try
				{
					sourceDmsServiceId = new DmsServiceId(serviceInfo.ReplicationRemoteService);
				}
				catch (Exception ex)
				{
					string logMessage = "Failed parsing replication service info for service " + Convert.ToString(serviceInfo.Name) + " (" + Convert.ToString(serviceInfo.DataMinerID) + "/" + Convert.ToString(serviceInfo.ElementID) + "). Replication remote service is: " + Convert.ToString(serviceInfo.ReplicationRemoteService) + Environment.NewLine + ex;
					Logger.Log(logMessage);
					sourceDmsServiceId = new DmsServiceId(-1, -1);
				}
			}

			userName = serviceInfo.ReplicationUser ?? String.Empty;
		}
	}
}