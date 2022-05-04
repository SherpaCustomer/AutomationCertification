namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Net.Messages;

	/// <summary>
	///     Class representing an UDP connection.
	/// </summary>
	public sealed class Udp : ConnectionSettings, IUdp
	{
		/// <summary>
		///		Compares two instances of this object by comparing the property fields.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
		/// <returns>Boolean indicating if object is equal or not.</returns>
		public bool Equals(Udp other)
		{
			return this.isDedicated == other.isDedicated
			       && this.isSslTlsEnabled == other.isSslTlsEnabled
			       && this.localPort == other.localPort
			       && this.networkInterfaceCard == other.networkInterfaceCard
			       && string.Equals(this.remoteHost, other.remoteHost, StringComparison.InvariantCulture)
			       && this.remotePort == other.remotePort;
		}

		/// <summary>Determines whether the specified object is equal to the current object.</summary>
		/// <param name="obj">The object to compare with the current object. </param>
		/// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Udp)obj);
		}

		/// <summary>Serves as the default hash function. </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.isDedicated.GetHashCode();
				hashCode = (hashCode * 397) ^ this.isSslTlsEnabled.GetHashCode();
				hashCode = (hashCode * 397) ^ this.localPort.GetHashCode();
				hashCode = (hashCode * 397) ^ this.networkInterfaceCard;
				hashCode = (hashCode * 397) ^ (this.remoteHost != null ? StringComparer.InvariantCulture.GetHashCode(this.remoteHost) : 0);
				hashCode = (hashCode * 397) ^ this.remotePort.GetHashCode();
				return hashCode;
			}
		}

		private readonly bool isDedicated;

		private bool isSslTlsEnabled;

		private int? localPort;

		private int networkInterfaceCard;

		private string remoteHost;

		private int? remotePort;

		/// <summary>
		///     Initializes a new instance, using default values for localPort (null=Auto) SslTlsEnabled (false), IsDedicated
		///     (false) and NetworkInterfaceCard (0=Auto)
		/// </summary>
		/// <param name="remoteHost">The IP or name of the remote host.</param>
		/// <param name="remotePort">The port number of the remote host.</param>
		public Udp(string remoteHost, int remotePort)
		{
			this.localPort            = null;
			this.remotePort           = remotePort;
			this.isSslTlsEnabled      = false;
			this.isDedicated          = false;
			this.remoteHost           = remoteHost;
			this.networkInterfaceCard = 0;
		}

		/// <summary>
		///     Default empty constructor
		/// </summary>
		public Udp()
		{
		}

		/// <summary>
		///     Initializes a new instance using a <see cref="ElementPortInfo" /> object.
		/// </summary>
		/// <param name="info"></param>
		internal Udp(ElementPortInfo info)
		{
			this.remoteHost      = info.PollingIPAddress;
			if (!info.PollingIPPort.Equals(String.Empty)) remotePort = Convert.ToInt32(info.PollingIPPort); 
			if (!info.LocalIPPort.Equals(String.Empty)) localPort = Convert.ToInt32(info.LocalIPPort);
			this.isSslTlsEnabled = info.IsSslTlsEnabled;
			this.isDedicated     = HelperClass.IsDedicatedConnection(info);

			int networkInterfaceId = string.IsNullOrWhiteSpace(info.Number) ? 0 : Convert.ToInt32(info.Number);
			this.networkInterfaceCard = networkInterfaceId;
		}

		/// <summary>
		///     Gets or sets if a dedicated connection is used.
		/// </summary>
		public bool IsDedicated
		{
			get
			{
				return this.isDedicated;
			}
		}

		/// <summary>
		///     Gets or sets if TLS is enabled on the connection.
		/// </summary>
		public bool IsSslTlsEnabled
		{
			get
			{
				return this.isSslTlsEnabled;
			}

			set
			{
				if (this.isSslTlsEnabled != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.IsSslTlsEnabled);
					this.isSslTlsEnabled = value;
				}
			}
		}

		/// <summary>
		///     Gets or sets the local port.
		/// </summary>
		public int? LocalPort
		{
			get
			{
				return this.localPort;
			}

			set
			{
				if (this.localPort != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.LocalPort);
					this.localPort = value;
				}
			}
		}

		/// <summary>
		///     Gets or sets the network interface card number.
		/// </summary>
		public int NetworkInterfaceCard
		{
			get
			{
				return this.networkInterfaceCard;
			}

			set
			{
				if (this.networkInterfaceCard != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.NetworkInterfaceCard);
					this.networkInterfaceCard = value;
				}
			}
		}

		/// <summary>
		///     Gets or sets the IP Address or name of the remote host.
		/// </summary>
		public string RemoteHost
		{
			get
			{
				return this.remoteHost;
			}

			set
			{
				if (this.remoteHost != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.RemoteHost);
					this.remoteHost = value;
				}
			}
		}

		/// <summary>
		///     Gets or sets the remote port.
		/// </summary>
		public int? RemotePort
		{
			get
			{
				return this.remotePort;
			}

			set
			{
				if (this.remotePort != value)
				{
					this.ChangedPropertyList.Add(ConnectionSetting.RemotePort);
					this.remotePort = value;
				}
			}
		}

		/// <summary>
		///     Indicates whether changes have been applied to the properties.
		/// </summary>
		internal override bool IsUpdated
		{
			get
			{
				return this.ChangedPropertyList.Any();
			}
		}

		/// <summary>
		///     Clear the list keeping track of all the changes performed on properties.
		/// </summary>
		internal override void ClearUpdates()
		{
			this.ChangedPropertyList.Clear();
		}

		/// <summary>
		///     Creates a new <see cref="ElementPortInfo" /> object.
		/// </summary>
		/// <returns></returns>
		internal override ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
		{
			throw new NotSupportedException(
				"Method is not supported. ElementPortInfo content is directly created in corresponding connection.");
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
					case ConnectionSetting.LocalPort:
						portInfo.LocalIPPort = Convert.ToString(this.localPort);
						break;
					case ConnectionSetting.RemotePort:
						portInfo.PollingIPPort = Convert.ToString(this.remotePort);
						break;
					case ConnectionSetting.IsSslTlsEnabled:
						portInfo.IsSslTlsEnabled = this.isSslTlsEnabled;
						break;
					case ConnectionSetting.RemoteHost:
						portInfo.PollingIPAddress = this.remoteHost;
						break;
					case ConnectionSetting.NetworkInterfaceCard:
						portInfo.Number = Convert.ToString(this.networkInterfaceCard);
						break;
					default:
						continue;
				}
			}
		}
	}
}