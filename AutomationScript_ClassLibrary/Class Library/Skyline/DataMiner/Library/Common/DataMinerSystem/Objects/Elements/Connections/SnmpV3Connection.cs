namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Net.Messages;

	/// <summary>
	///     Class representing a SNMPv3 class.
	/// </summary>
	public class SnmpV3Connection : ConnectionSettings, ISnmpV3Connection
	{
		private readonly int id;

		private readonly Guid libraryCredentials;

		private string deviceAddress;

		private TimeSpan? elementTimeout;

		private int retries;

		private ISnmpV3SecurityConfig securityConfig;

		private TimeSpan timeout;

		private IUdp udpIpConfiguration;

		/// <summary>
		///     Initializes a new instance.
		/// </summary>
		/// <param name="udpConfiguration">The udp configuration settings.</param>
		/// <param name="securityConfig">The SNMPv3 security configuration.</param>
		public SnmpV3Connection(IUdp udpConfiguration, SnmpV3SecurityConfig securityConfig)
		{
			if (udpConfiguration == null)
			{
				throw new ArgumentNullException("udpConfiguration");
			}

			if (securityConfig == null)
			{
				throw new ArgumentNullException("securityConfig");
			}

			this.libraryCredentials = Guid.Empty;
			this.id                 = -1;
			this.udpIpConfiguration = udpConfiguration;
			this.deviceAddress      = String.Empty;
			this.securityConfig     = securityConfig;
			this.timeout            = new TimeSpan(0, 0, 0, 0, 1500);
			this.retries            = 3;
			this.elementTimeout     = new TimeSpan(0, 0, 0, 30);
		}

		/// <summary>
		///     Default empty constructor
		/// </summary>
		public SnmpV3Connection()
		{
		}

		/// <summary>
		///     Initializes a new instance.
		/// </summary>
		internal SnmpV3Connection(ElementPortInfo info)
		{
			this.deviceAddress  = info.BusAddress;
			this.retries        = info.Retries;
			this.timeout        = new TimeSpan(0, 0, 0, 0, info.TimeoutTime);
			this.elementTimeout = new TimeSpan(0, 0, info.ElementTimeoutTime / 1000);

			if (this.libraryCredentials == Guid.Empty)
			{
				var securityLevelAndProtocol = SnmpV3SecurityLevelAndProtocolAdapter.FromSLNetStopBits(info.StopBits);
				var encryptionAlgorithm = SnmpV3EncryptionAlgorithmAdapter.FromSLNetFlowControl(info.FlowControl);
				var authenticationProtocol = SnmpV3AuthenticationAlgorithmAdapter.FromSLNetParity(info.Parity);
	
				string authenticationKey = info.GetCommunity;
				string encryptionKey = info.SetCommunity;
				string username = info.DataBits;

				this.securityConfig = new SnmpV3SecurityConfig(
					securityLevelAndProtocol,
					username,
					authenticationKey,
					encryptionKey,
					authenticationProtocol,
					encryptionAlgorithm);
			}
			else
			{
				this.SecurityConfig = new SnmpV3SecurityConfig(
					SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary,
					String.Empty,
					String.Empty,
					String.Empty,
					SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary,
					SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary);
			}

			this.id                 = info.PortID;
			this.elementTimeout     = new TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
			this.udpIpConfiguration = new Udp(info);
		}

		/// <summary>
		///     Gets or Sets the device address.
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
		/// Gets the zero based id of the connection.
		/// </summary>
		public int Id
		{
			get
			{
				return this.id;
			}

			// set
			// {
			// 	ChangedPropertyList.Add("Id");
			// 	id = value;
			// }
		}

		/// <summary>
		///     Get the libraryCredentials.
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
		///     Gets or sets the SNMPv3 security configuration.
		/// </summary>
		public ISnmpV3SecurityConfig SecurityConfig
		{
			get
			{
				return this.securityConfig;
			}

			set
			{
				this.ChangedPropertyList.Add(ConnectionSetting.SecurityConfig);
				this.securityConfig = value;
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
						throw new IncorrectDataException("Timeout value should be between 10 and 120 sec.");
					}
				}
			}
		}

		/// <summary>
		///     Get or Set the UDP Connection settings
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
				var mySecurityConfig = (ConnectionSettings)this.securityConfig;
				return this.ChangedPropertyList.Any() || udpSettings.IsUpdated || mySecurityConfig.IsUpdated;
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
			var mySecurityConfig = (ConnectionSettings)this.securityConfig;
			mySecurityConfig.ClearUpdates();
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
				               LibraryCredential = Guid.Empty,
				               StopBits = SnmpV3SecurityLevelAndProtocolAdapter.ToSLNetStopBits(this.SecurityConfig.SecurityLevelAndProtocol),
				               FlowControl = SnmpV3EncryptionAlgorithmAdapter.ToSLNetFlowControl(this.SecurityConfig.EncryptionAlgorithm),
				               Parity =  SnmpV3AuthenticationAlgorithmAdapter.ToSLNetParity(this.SecurityConfig.AuthenticationAlgorithm),
				               GetCommunity = RSA.Encrypt(this.SecurityConfig.AuthenticationKey),
				               SetCommunity = RSA.Encrypt(this.SecurityConfig.EncryptionKey),
				               DataBits = this.SecurityConfig.Username,
				               PollingIPPort = Convert.ToString(this.udpIpConfiguration.RemotePort),
				               IsSslTlsEnabled = this.udpIpConfiguration.IsSslTlsEnabled,
				               PollingIPAddress = this.udpIpConfiguration.RemoteHost,
				               PortID = portPosition,
				               ProtocolType = Net.Messages.ProtocolType.SnmpV3,
				               Baudrate = String.Empty,
				               LocalIPPort = this.udpIpConfiguration.LocalPort.ToString(),
				               Number = this.udpIpConfiguration.NetworkInterfaceCard.ToString(),
				               Type = "ip"
			               };

			return portInfo;
		}

		/// <summary>
		///     Updates the provided ElementPortInfo object with any performed changes on the object.
		/// </summary>
		/// <param name="portInfo"></param>
		/// <param name="isCompatibilityIssueDetected"></param>
		internal override void UpdateElementPortInfo(ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
		{
			var mySecurityConfig = (ConnectionSettings)this.securityConfig;
			var udpSettings = (ConnectionSettings)this.udpIpConfiguration;

			foreach (ConnectionSetting property in this.ChangedPropertyList)
			{
				switch (property)
				{
					case ConnectionSetting.DeviceAddress:
						portInfo.BusAddress = this.deviceAddress;
						break;
					case ConnectionSetting.Timeout:
						portInfo.TimeoutTime = Convert.ToInt32(this.timeout.TotalMilliseconds);
						break;
					case ConnectionSetting.ElementTimeout:
						portInfo.ElementTimeoutTime = Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds);
						break;
					case ConnectionSetting.Retries:
						portInfo.Retries = this.retries;
						break;
					case ConnectionSetting.PortConnection:
						udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
						break;
					case ConnectionSetting.SecurityConfig:
						mySecurityConfig.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
						break;
					default:
						continue;
				}
			}

			portInfo.ProtocolType = Net.Messages.ProtocolType.SnmpV3;

			if (mySecurityConfig.IsUpdated)
			{
				mySecurityConfig.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
			}

			if (udpSettings.IsUpdated)
			{
				udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
			}
		}
	}
}