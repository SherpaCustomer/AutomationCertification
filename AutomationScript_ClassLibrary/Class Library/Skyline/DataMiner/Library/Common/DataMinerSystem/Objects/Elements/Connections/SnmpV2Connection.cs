namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Net.Messages;

	/// <summary>
	///     Class representing an SnmpV2 Connection.
	/// </summary>
	public class SnmpV2Connection : ConnectionSettings, ISnmpV2Connection
	{
		private readonly int id;

		private readonly Guid libraryCredentials;

		private string deviceAddress;

		private TimeSpan? elementTimeout;

		private string getCommunityString;

		private int retries;

		private string setCommunityString;

		private TimeSpan timeout;

		private IUdp udpIpConfiguration;

		/// <summary>
		///     Initiates a new instance with default settings for Get Community String (public), Set Community String (private),
		///     Device Address (empty),
		///     Command Timeout (1500ms), Retries (3) and Element Timeout (30s).
		/// </summary>
		/// <param name="udpConfiguration">The UDP Connection settings.</param>
		public SnmpV2Connection(IUdp udpConfiguration)
		{
			if (udpConfiguration == null)
			{
				throw new ArgumentNullException("udpConfiguration");
			}

			this.id                 = -1;
			this.udpIpConfiguration = udpConfiguration;

			// this.udpIpConfiguration = udpIpIpConfiguration;
			this.deviceAddress      = String.Empty;
			this.getCommunityString = "public";
			this.setCommunityString = "private";
			this.timeout            = new TimeSpan(0, 0, 0, 0, 1500);
			this.retries            = 3;
			this.elementTimeout     = new TimeSpan(0, 0, 0, 30);
			this.libraryCredentials = Guid.Empty;
		}

		/// <summary>
		///     Default empty constructor
		/// </summary>
		public SnmpV2Connection()
		{
		}

		/// <summary>
		///     Initializes a new instance.
		/// </summary>
		internal SnmpV2Connection(ElementPortInfo info)
		{
			this.deviceAddress      = info.BusAddress;
			this.retries            = info.Retries;
			this.timeout            = new TimeSpan(0, 0, 0, 0, info.TimeoutTime);
			this.getCommunityString = info.GetCommunity;
			this.setCommunityString = info.SetCommunity;
			this.libraryCredentials = info.LibraryCredential;

			if (info.LibraryCredential == Guid.Empty)
			{
				this.getCommunityString = info.GetCommunity;
				this.setCommunityString = info.SetCommunity;
			}
			else
			{
				this.getCommunityString = String.Empty;
				this.setCommunityString = String.Empty;
			}

			this.id                 = info.PortID;
			this.elementTimeout     = new TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
			this.udpIpConfiguration = new Udp(info);
		}

		/// <summary>
		///     Get or Sets the device address.
		/// </summary>
		public string DeviceAddress
		{
			get
			{
				return this.deviceAddress;
			}

			set
			{
				if (this.deviceAddress != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.DeviceAddress);
					this.deviceAddress = value;
				}
			}
		}

		/// <summary>
		///     Get or Set the timespan before the element will go into timeout after not responding.
		/// </summary>
		/// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
		public TimeSpan? ElementTimeout
		{
			get
			{
				return this.elementTimeout;
			}

			set
			{
				if (this.elementTimeout != value)
				{
					if (value == null || (value.Value.TotalSeconds >= 1 && value.Value.TotalSeconds <= 120))
					{
						this.ChangedPropertyList.Add(ConnectionSetting.ElementTimeout);
						this.elementTimeout = value;
					}
					else
					{
						throw new IncorrectDataException("ElementTimeout value should be between 1 and 120 sec.");
					}
				}
			}
		}

		/// <summary>
		///     Get or Sets the Get community string.
		/// </summary>
		public string GetCommunityString
		{
			get
			{
				return this.getCommunityString;
			}

			set
			{
				if (this.getCommunityString != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.GetCommunityString);
					this.getCommunityString = value;
				}
			}
		}

		/// <summary>
		///     Gets the zero based id of the connection.
		/// </summary>
		public int Id
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		///     Gets the Library Credential settings.
		/// </summary>
		public Guid LibraryCredentials
		{
			get
			{
				return this.libraryCredentials;
			}
		}

		/// <summary>
		///     Get or Set the amount of retries.
		/// </summary>
		public int Retries
		{
			get
			{
				return this.retries;
			}

			set
			{
				if (this.retries != value)
				{
					if (value >= 0 && value <= 10)
					{
						this.ChangedPropertyList.Add(ConnectionSetting.Retries);
						this.retries = value;
					}
					else
					{
						throw new IncorrectDataException("Retries value should be between 0 and 10.");
					}
				}
			}
		}

		/// <summary>
		///     Get or Sets the Set community string.
		/// </summary>
		public string SetCommunityString
		{
			get
			{
				return this.setCommunityString;
			}

			set
			{
				if (this.setCommunityString != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.SetCommunityString);
					this.setCommunityString = value;
				}
			}
		}

		/// <summary>
		///     Get or Set the timeout value.
		/// </summary>
		public TimeSpan Timeout
		{
			get
			{
				return this.timeout;
			}

			set
			{
				if (this.timeout != value)
				{
					if (value.TotalMilliseconds >= 10 && value.TotalMilliseconds <= 120000)
					{
						this.ChangedPropertyList.Add(ConnectionSetting.Timeout);
						this.timeout = value;
					}
					else
					{
						throw new IncorrectDataException("Timeout value should be between 10 and 120 s.");
					}
				}
			}
		}

		/// <summary>
		///     Get or Sets the UDP connection settings.
		/// </summary>
		public IUdp UdpConfiguration
		{
			get
			{
				return this.udpIpConfiguration;
			}

			set
			{
				if (this.udpIpConfiguration == null || !this.udpIpConfiguration.Equals(value))
				{
					this.ChangedPropertyList.Add(ConnectionSetting.PortConnection);
					this.udpIpConfiguration = value;
				}
			}
		}

		/// <summary>
		///     Indicates if updates have been performed on the properties of the object.
		/// </summary>
		internal override bool IsUpdated
		{
			get
			{
				var udpSettings = (ConnectionSettings)this.udpIpConfiguration;
				return this.ChangedPropertyList.Any() || udpSettings.IsUpdated;
			}
		}

		/// <summary>
		///     Clear the performed update flags of the properties of the object.
		/// </summary>
		internal override void ClearUpdates()
		{
			this.ChangedPropertyList.Clear();
			var udpSettings = (ConnectionSettings)this.udpIpConfiguration;
			udpSettings.ClearUpdates();
		}

		/// <summary>
		///     Creates an ElementPortPortInfo object based on the field contents.
		/// </summary>
		/// <returns>ElementPortInfo object.</returns>
		internal override ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
		{
			var portInfo = new ElementPortInfo
			               {
				               BusAddress        = this.deviceAddress,
				               Retries           = this.retries,
				               TimeoutTime       = Convert.ToInt32(this.timeout.TotalMilliseconds),
				               LibraryCredential = this.libraryCredentials,
				               PollingIPPort     = Convert.ToString(this.udpIpConfiguration.RemotePort),
				               IsSslTlsEnabled   = this.udpIpConfiguration.IsSslTlsEnabled,
				               PollingIPAddress  = this.udpIpConfiguration.RemoteHost,
				               PortID            = portPosition,
				               ProtocolType      = Net.Messages.ProtocolType.SnmpV2,
				               LocalIPPort       = this.udpIpConfiguration.LocalPort.ToString(),
				               Number            = this.udpIpConfiguration.NetworkInterfaceCard.ToString(),
				               Type              = "ip",
				               Baudrate          = String.Empty,
				               DataBits          = String.Empty,
				               FlowControl       = String.Empty,
				               Parity            = String.Empty
			               };

			if (this.libraryCredentials == Guid.Empty)
			{
				portInfo.GetCommunity = this.getCommunityString;
				portInfo.SetCommunity = this.setCommunityString;
			}
			else
			{
				portInfo.GetCommunity = String.Empty;
				portInfo.SetCommunity = String.Empty;
			}

			return portInfo;
		}

		/// <summary>
		///     Updates the provided ElementPortInfo object with any performed changes on the object.
		/// </summary>
		/// <param name="portInfo"></param>
		/// <param name="isCompatibilityIssueDetected"></param>
		internal override void UpdateElementPortInfo(ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
		{
			foreach (ConnectionSetting property in this.ChangedPropertyList)
			{
				switch (property)
				{
					case ConnectionSetting.GetCommunityString:
						portInfo.GetCommunity = this.getCommunityString;
						break;
					case ConnectionSetting.SetCommunityString:
						portInfo.SetCommunity = this.setCommunityString;
						break;
					case ConnectionSetting.DeviceAddress:
						portInfo.BusAddress = this.deviceAddress;
						break;
					case ConnectionSetting.Timeout:
						portInfo.TimeoutTime = Convert.ToInt32(this.timeout.TotalMilliseconds);
						break;
					case ConnectionSetting.Retries:
						portInfo.Retries = this.retries;
						break;
					case ConnectionSetting.PortConnection:
						portInfo.PollingIPPort    = Convert.ToString(this.udpIpConfiguration.RemotePort);
						portInfo.IsSslTlsEnabled  = this.udpIpConfiguration.IsSslTlsEnabled;
						portInfo.PollingIPAddress = this.udpIpConfiguration.RemoteHost;
						break;
					case ConnectionSetting.ElementTimeout:
						portInfo.ElementTimeoutTime = Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds);
						break;
					default:
						continue;
				}
			}

			var udpSettings = (ConnectionSettings)this.udpIpConfiguration;
			udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
			portInfo.ProtocolType = Net.Messages.ProtocolType.SnmpV2;
		}
	}
}