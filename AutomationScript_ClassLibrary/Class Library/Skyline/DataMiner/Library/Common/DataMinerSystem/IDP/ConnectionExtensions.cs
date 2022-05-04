namespace Skyline.DataMiner.Library.Common.Idp
{
	using System;

	/// <summary>
	///     Class containing extension methods on Connection classes to offer extra custom functionality.
	/// </summary>
	public static class ConnectionExtensions
	{

		/// <summary>
		///     Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		public static string ToCITypeJson(this IElementConnection connection, int connectionIndex)
		{
			if (connection is IHttpConnection)
			{
				return (connection as IHttpConnection).ToCITypeJson(connectionIndex);
			}
			else if (connection is ISnmpV1Connection)
			{
				return (connection as ISnmpV1Connection).ToCITypeJson(connectionIndex);

			}else if (connection is ISnmpV2Connection)
			{
				return (connection as ISnmpV2Connection).ToCITypeJson(connectionIndex);

			}else if (connection is ISnmpV3Connection)
			{
				return (connection as ISnmpV3Connection).ToCITypeJson(connectionIndex);

			}else if (connection is IVirtualConnection)
			{
				return (connection as IVirtualConnection).ToCITypeJson(connectionIndex);

			}else if (connection is ISerialConnection)
			{
				return (connection as ISerialConnection).ToCITypeJson(connectionIndex);

			}else
			{
				return (connection as IRealConnection).ToCITypeJson(connectionIndex);
			}
		}


		/// <summary>
		///     Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		private static string ToCITypeJson(this IHttpConnection connection, int connectionIndex)
		{
			string localPort = (connection.TcpConfiguration.LocalPort.HasValue ? connection.TcpConfiguration.LocalPort.ToString() : "null");

			string json = "{" +
							"\"DMAElementSnmpPortInfo\": [{" +
								"\"DeviceAddress\": \"" + connection.BusAddress + "\"," +
								"\"GetCommunity\": null," +
								"\"IPAddress\": \"" + connection.TcpConfiguration.RemoteHost + "\"," +
								"\"LocalPort\": " + localPort + "," +
								"\"Network\": \"" + connection.TcpConfiguration.NetworkInterfaceCard + "\"," +
								"\"PortId\": " + connectionIndex + "," +
								"\"PortNumber\": " + connection.TcpConfiguration.RemotePort + "," +
								"\"SetCommunity\": null," +
								"\"Type\": \"IP\"," +
								"\"TypeConnection\": \"Http\"" +
							"}" +
						  "]," +
						  "\"DMAElementSnmpV3PortInfo\": []," +
						  "\"DMASerialPortInfo\": []," +
						  "\"ElementTimeoutTime\": " + connection.ElementTimeout.Value.TotalMilliseconds + "," +
						  "\"Retries\": " + connection.Retries + "," +
						  "\"TimeoutTime\": " + connection.Timeout.TotalMilliseconds + "" +
						  "}";
			return json;
		}

		/// <summary>
		///  Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		private static string ToCITypeJson(this IRealConnection connection,int connectionIndex)
		{
			return String.Empty;
		}

		/// <summary>
		///  Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		private static string ToCITypeJson(this ISerialConnection connection,int connectionIndex)
		{
			string remoteHost;
			int? remotePort;
			int? localPort;
			int networkCard;
			string type;
			if (connection.Connection is ITcp)
			{
				var tcpConnection = connection.Connection as ITcp;
				remoteHost = tcpConnection.RemoteHost;
				remotePort = tcpConnection.RemotePort;
				localPort = tcpConnection.LocalPort;
				networkCard = tcpConnection.NetworkInterfaceCard;
				type = "IP";
			}
			else if (connection.Connection is IUdp)
			{
				var udpConnection = connection.Connection as IUdp;
				remoteHost = udpConnection.RemoteHost;
				remotePort = udpConnection.RemotePort;
				localPort = udpConnection.LocalPort;
				networkCard = udpConnection.NetworkInterfaceCard;
				type = "UDP";
			}
			else
			{
				return String.Empty;
			}

			string json = "{" +
							"\"DMAElementSnmpPortInfo\": [{" +
								"\"DeviceAddress\": \"" + connection.BusAddress + "\"," +
								"\"GetCommunity\": null," +
								"\"IPAddress\": \"" + remoteHost + "\"," +
								"\"LocalPort\": " + (localPort.HasValue ? localPort.ToString() : "null") + "," +
								"\"Network\": \"" + networkCard + "\"," +
								"\"PortId\": " + connectionIndex + "," +
								"\"PortNumber\": " + (remotePort.HasValue ? remotePort.ToString() : "null") + "," +
								"\"SetCommunity\": null," +
								"\"Type\": \"" + type + "\"," +
								"\"TypeConnection\": \"Serial\"" +
							"}" +
						  "]," +
						  "\"DMAElementSnmpV3PortInfo\": []," +
						  "\"DMASerialPortInfo\": []," +
						  "\"ElementTimeoutTime\": " + connection.ElementTimeout.Value.TotalMilliseconds + "," +
						  "\"Retries\": " + connection.Retries + "," +
						  "\"TimeoutTime\": " + connection.Timeout.TotalMilliseconds + "" +
						  "}";
			return json;
		}

		/// <summary>
		///  Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		private static string ToCITypeJson(this ISnmpV1Connection connection,int connectionIndex)
		{
			string localPort = (connection.UdpConfiguration.LocalPort.HasValue ? connection.UdpConfiguration.LocalPort.ToString() : "null");

			string json = "{" +
							"\"DMAElementSnmpPortInfo\": [{" +
								"\"DeviceAddress\": \"" + connection.DeviceAddress + "\"," +
								"\"GetCommunity\": \"" + connection.GetCommunityString + "\"," +
								"\"IPAddress\": \"" + connection.UdpConfiguration.RemoteHost + "\"," +
								"\"LocalPort\": " + localPort + "," +
								"\"Network\": \"" + connection.UdpConfiguration.NetworkInterfaceCard + "\"," +
								"\"PortId\": " + connectionIndex + "," +
								"\"PortNumber\": " + connection.UdpConfiguration.RemotePort + "," +
								"\"SetCommunity\": \"" + connection.SetCommunityString + "\"," +
								"\"Type\": \"IP\"," +
								"\"TypeConnection\": \"SnmpV1\"" +
							"}" +
						  "]," +
						  "\"DMAElementSnmpV3PortInfo\": []," +
						  "\"DMASerialPortInfo\": []," +
						  "\"ElementTimeoutTime\": " + connection.ElementTimeout.Value.TotalMilliseconds + "," +
						  "\"Retries\": " + connection.Retries + "," +
						  "\"TimeoutTime\": " + connection.Timeout.TotalMilliseconds + "" +
						  "}";
			return json;
		}

		/// <summary>
		///  Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		private static string ToCITypeJson(this ISnmpV2Connection connection,int connectionIndex)
		{
			string json = "{" +
							"\"DMAElementSnmpPortInfo\": [{" +
								"\"DeviceAddress\": \"" + connection.DeviceAddress + "\"," +
								"\"GetCommunity\": \"" + connection.GetCommunityString + "\"," +
								"\"IPAddress\": \"" + connection.UdpConfiguration.RemoteHost + "\"," +
								"\"LocalPort\": " + (connection.UdpConfiguration.LocalPort.HasValue ? connection.UdpConfiguration.LocalPort.ToString() : "null") + "," +
								"\"Network\": \"" + connection.UdpConfiguration.NetworkInterfaceCard + "\"," +
								"\"PortId\": " + connectionIndex + "," +
								"\"PortNumber\": " + connection.UdpConfiguration.RemotePort + "," +
								"\"SetCommunity\": \"" + connection.SetCommunityString + "\"," +
								"\"Type\": \"IP\"," +
								"\"TypeConnection\": \"SnmpV2\"" +
							"}" +
						  "]," +
						  "\"DMAElementSnmpV3PortInfo\": []," +
						  "\"DMASerialPortInfo\": []," +
						  "\"ElementTimeoutTime\": " + connection.ElementTimeout.Value.TotalMilliseconds + "," +
						  "\"Retries\": " + connection.Retries + "," +
						  "\"TimeoutTime\": " + connection.Timeout.TotalMilliseconds + "" +
						  "}";
			return json;
		}

		/// <summary>
		///  Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		private static string ToCITypeJson(this ISnmpV3Connection connection,int connectionIndex)
		{
			string json = "{" +
							"\"DMAElementSnmpPortInfo\": []," +
								"\"DMAElementSnmpV3PortInfo\": [{" +
								"\"AuthPassword\": \"" + connection.SecurityConfig.AuthenticationKey + "\"," +
								"\"AuthType\": \"" + ToIdpAuthType(connection.SecurityConfig.AuthenticationAlgorithm) + "\"," +
								"\"DeviceAddress\": \"" + connection.DeviceAddress + "\"," +
								"\"EncryptionAlgorithm\": \"" + ToIdpEncryption(connection.SecurityConfig.EncryptionAlgorithm) + "\"," +
								"\"IPAddress\": \"" + connection.UdpConfiguration.RemoteHost + "\"," +
								"\"Network\": \"" + connection.UdpConfiguration.NetworkInterfaceCard + "\"," +
								"\"PortId\": " + connectionIndex + "," +
								"\"PortNumber\": " + connection.UdpConfiguration.RemotePort + "," +
								"\"PrivPassword\": \"" + connection.SecurityConfig.EncryptionKey + "\"," +
								"\"SecurityLevel\": \"" + ToIdpSecurityLevel(connection.SecurityConfig.SecurityLevelAndProtocol) + "\"," +
								"\"TypeConnection\": \"SnmpV3\"," +
								"\"Username\": \"" + connection.SecurityConfig.Username + "\"" +
							"}" +
						  "]," +
						  "\"DMASerialPortInfo\": []," +
						  "\"ElementTimeoutTime\": " + connection.ElementTimeout.Value.TotalMilliseconds + "," +
						  "\"Retries\": " + connection.Retries + "," +
						  "\"TimeoutTime\": " + connection.Timeout.TotalMilliseconds + "" +
						  "}";

			return json;
		}

		/// <summary>
		///  Converts the connection into the JSON Structure expected by IDP's Configuration Item type (CIType).
		/// </summary>
		/// <param name="connection">The connection which needs to be converted to a Configuration Item Type.</param>
		/// <param name="connectionIndex">Zero-based index of the connection in an element.</param>
		/// <returns>JSON string to match CIType.</returns>
		public static string ToCITypeJson(this IVirtualConnection connection, int connectionIndex)
		{
			return String.Empty;
		}

		/// <summary>
		/// SnmpV3Encryption representation in IDP.
		/// </summary>
		private enum IdpSnmpV3EncryptionType
		{
			/// <summary>
			/// No encryption.
			/// </summary>
			None = 1,

			/// <summary>
			/// DES encryption.
			/// </summary>
			DES = 2,

			/// <summary>
			/// Triple DES encryption.
			/// </summary>
			TripleDES = 3,

			/// <summary>
			/// AES128 encryption.
			/// </summary>
			AES128 = 4,

			/// <summary>
			/// IDEA encryption.
			/// </summary>
			IDEA = 9,

			/// <summary>
			/// AES192 encryption.
			/// </summary>
			AES192 = 20,

			/// <summary>
			/// AES256 encryption.
			/// </summary>
			AES256 = 21,

			/// <summary>
			/// AES128 with 3DES Key External encryption.
			/// </summary>
			AES128W3DESKEYEXT = 22,

			/// <summary>
			/// AES192 with 3DES Key External encryption.
			/// </summary>
			AES192W3DESKEYEXT = 23,

			/// <summary>
			/// AES256 with 3DES Key External encryption.
			/// </summary>
			AES256W3DESKEYEXT = 24
		}

		/// <summary>
		/// SnmpV3SecurityLevel representation in IDP.
		/// </summary>
		private enum IdpSnmpV3SecurityLevel
		{
			/// <summary>
			/// No Authentication and no Privacy.
			/// </summary>
			None = 1,

			/// <summary>
			/// Authentication and no Privacy.
			/// </summary>
			AuthNoPriv = 2,

			/// <summary>
			/// Authentication and Privacy.
			/// </summary>
			AuthPriv = 3
		}

		/// <summary>
		/// SnmpV3Authentication algorithm representation in IDP.
		/// </summary>
		private enum IdpSnmpV3AuthType
		{
			/// <summary>
			/// No Authentication.
			/// </summary>
			None = 1,

			/// <summary>
			/// HMAC-MD5 Authentication algorithm.
			/// </summary>
			HMAC_MD5 = 2,

			/// <summary>
			/// HMAC-SHA Authentication algorithm.
			/// </summary>
			HMAC_SHA = 3,

			/// <summary>
			/// HMAC-SHA224 Authentication algorithm.
			/// </summary>
			HMAC128_SHA224 = 4,

			/// <summary>
			/// HMAC192-SHA256 Authentication algorithm.
			/// </summary>
			HMAC192_SHA256 = 5,

			/// <summary>
			/// HMAC256-SHA384 Authentication algorithm.
			/// </summary>
			HMAC256_SHA384 = 6,

			/// <summary>
			/// HMAC384-SHA512 Authentication algorithm.
			/// </summary>
			HMAC384_SHA512 = 7
		}

		/// <summary>
		/// Converts an object of type <see cref="SnmpV3AuthenticationAlgorithm"/> to the corresponding IDP type of <see cref="IdpSnmpV3AuthType"/>.
		/// </summary>
		/// <param name="input">The object to convert.</param>
		/// <returns>An instance of <see cref="IdpSnmpV3AuthType"/>.</returns>
		private static IdpSnmpV3AuthType ToIdpAuthType(SnmpV3AuthenticationAlgorithm input)
		{
			switch (input)
			{
				case SnmpV3AuthenticationAlgorithm.Md5:
					return IdpSnmpV3AuthType.HMAC_MD5;
				case SnmpV3AuthenticationAlgorithm.Sha1:
					return IdpSnmpV3AuthType.HMAC_SHA;
				case SnmpV3AuthenticationAlgorithm.Sha224:
					return IdpSnmpV3AuthType.HMAC128_SHA224;
				case SnmpV3AuthenticationAlgorithm.Sha256:
					return IdpSnmpV3AuthType.HMAC192_SHA256;
				case SnmpV3AuthenticationAlgorithm.Sha384:
					return IdpSnmpV3AuthType.HMAC256_SHA384;
				case SnmpV3AuthenticationAlgorithm.Sha512:
					return IdpSnmpV3AuthType.HMAC384_SHA512;
				case SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary:
					return IdpSnmpV3AuthType.None;
				case SnmpV3AuthenticationAlgorithm.None:
					return IdpSnmpV3AuthType.None;
				default:
					return IdpSnmpV3AuthType.None;
			}

		}

		/// <summary>
		/// Converts an object of type <see cref="SnmpV3SecurityLevelAndProtocol"/> to the corresponding IDP type of <see cref="IdpSnmpV3SecurityLevel"/>.
		/// </summary>
		/// <param name="input">The object to convert.</param>
		/// <returns>An instance of <see cref="IdpSnmpV3SecurityLevel"/>.</returns>
		private static IdpSnmpV3SecurityLevel ToIdpSecurityLevel(SnmpV3SecurityLevelAndProtocol input)
		{
			switch (input)
			{
				case SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy:
					return IdpSnmpV3SecurityLevel.AuthPriv;
				case SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy:
					return IdpSnmpV3SecurityLevel.AuthNoPriv;
				case SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy:
					return IdpSnmpV3SecurityLevel.None;
				case SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary:
					return IdpSnmpV3SecurityLevel.None;
				default:
					return IdpSnmpV3SecurityLevel.None;
			}
		}

		/// <summary>
		/// Converts an object of type <see cref="SnmpV3EncryptionAlgorithm"/> to the corresponding IDP type of <see cref="IdpSnmpV3EncryptionType"/>.
		/// </summary>
		/// <param name="input">The object to convert.</param>
		/// <returns>An instance of <see cref="IdpSnmpV3EncryptionType"/>.</returns>
		private static IdpSnmpV3EncryptionType ToIdpEncryption(SnmpV3EncryptionAlgorithm input)
		{
			switch (input)
			{
				case SnmpV3EncryptionAlgorithm.Des:
					return IdpSnmpV3EncryptionType.DES;
				case SnmpV3EncryptionAlgorithm.Aes128:
					return IdpSnmpV3EncryptionType.AES128;
				case SnmpV3EncryptionAlgorithm.Aes192:
					return IdpSnmpV3EncryptionType.AES192;
				case SnmpV3EncryptionAlgorithm.Aes256:
					return IdpSnmpV3EncryptionType.AES256;
				case SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary:
					return IdpSnmpV3EncryptionType.None;
				case SnmpV3EncryptionAlgorithm.None:
					return IdpSnmpV3EncryptionType.None;
				default:
					return IdpSnmpV3EncryptionType.None;
			}
		}
	}
}