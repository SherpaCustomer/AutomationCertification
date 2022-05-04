﻿using Skyline.DataMiner.Net.Messages;
using System;

namespace Skyline.DataMiner.Library.Common
{
	using System.Linq;

	/// <summary>
	/// Class representing a TCP connection.
	/// </summary>
	public class Tcp : ConnectionSettings, ITcp
	{
		private string remoteHost;
		private int networkInterfaceCard;
		private int? localPort;
		private int? remotePort;
		private bool isSslTlsEnabled;
		private readonly bool isDedicated;

		internal Tcp(ElementPortInfo info)
		{
			this.remoteHost      = info.PollingIPAddress;
			if (!info.PollingIPPort.Equals(String.Empty)) remotePort = Convert.ToInt32(info.PollingIPPort);
			if (!info.LocalIPPort  .Equals(String.Empty)) localPort  = Convert.ToInt32(info.LocalIPPort);
			this.isSslTlsEnabled = info.IsSslTlsEnabled;
			this.isDedicated     = HelperClass.IsDedicatedConnection(info);

			int networkInterfaceId = String.IsNullOrWhiteSpace(info.Number) ? 0 : Convert.ToInt32(info.Number);
			this.networkInterfaceCard = networkInterfaceId;
		}

		/// <summary>
		/// Initializes a new instance, using default values for localPort (null=Auto) and NetworkInterfaceCard (0=Auto)
		/// </summary>
		/// <param name="remoteHost">The IP or name of the remote host.</param>
		/// <param name="remotePort">The port number of the remote host.</param>
		public Tcp(string remoteHost, int remotePort)
		{
			this.localPort            = null;
			this.remotePort           = remotePort;
			this.remoteHost           = remoteHost;
			this.networkInterfaceCard = 0;
			this.isDedicated          = false;
		}

		/// <summary>
		/// Default empty constructor.
		/// </summary>
		public Tcp()
		{
		}

		/// <summary>
		/// Gets or sets the IP Address or name of the remote host.
		/// </summary>
		public string RemoteHost
		{
			get { return this.remoteHost; }
			set
			{
				if (this.remoteHost != value)
				{
					ChangedPropertyList.Add(ConnectionSetting.RemoteHost);
					this.remoteHost = value;
				}
				
			}
		}

		/// <summary>
		/// Gets or sets the network interface card number.
		/// </summary>
		public int NetworkInterfaceCard
		{
			get { return this.networkInterfaceCard; }
			set
			{
				if (this.networkInterfaceCard != value)
				{
					ChangedPropertyList.Add(ConnectionSetting.NetworkInterfaceCard);
					networkInterfaceCard = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the local port.
		/// </summary>
		public int? LocalPort
		{
			get { return localPort; }
			set
			{
				if (this.localPort != value)
				{
					ChangedPropertyList.Add(ConnectionSetting.LocalPort);
					this.localPort = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the remote port.
		/// </summary>
		public int? RemotePort
		{
			get { return remotePort; }
			set
			{
				if (this.remotePort != value)
				{
					ChangedPropertyList.Add(ConnectionSetting.RemotePort);
					remotePort = value;
				}
			}
		}

		/// <summary>
		/// Indicates if SSL/TLS is enabled on the connection.
		/// </summary>
		/// <remarks>Can only be set to true on connection for protocol type Serial and port type IP.</remarks>
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
					ChangedPropertyList.Add(ConnectionSetting.IsSslTlsEnabled);
					this.isSslTlsEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets if a dedicated connection is used.
		/// </summary>
		/// <remarks>This is the "single" of <see cref="ISerialConnection"/> and <see cref="ISmartSerialConnection"/>. Cannot be configured.</remarks>
		public bool IsDedicated
		{
			get
			{
				return this.isDedicated;
			}
		}

		internal override bool IsUpdated
		{
			get
			{
				return this.ChangedPropertyList.Any();
			}
		}

		internal override void ClearUpdates()
		{
			this.ChangedPropertyList.Clear();
		}

		internal override ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
		{
			throw new NotSupportedException(
				"Method is not supported. ElementPortInfo content is directly created in corresponding connection.");
		}

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