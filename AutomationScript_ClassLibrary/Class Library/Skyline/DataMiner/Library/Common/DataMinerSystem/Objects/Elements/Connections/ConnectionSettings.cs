namespace Skyline.DataMiner.Library.Common
{
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Messages;

	/// <summary>
	/// Base class for all connection related objects.
	/// </summary>
	public abstract class ConnectionSettings
	{
		/// <summary>
		/// Enum used to track changes on properties of classes implementing this abstract class.
		/// </summary>
		protected enum ConnectionSetting
		{
			/// <summary>
			/// GetCommunityString
			/// </summary>
			GetCommunityString = 0,
			/// <summary>
			/// SetCommunityString
			/// </summary>
			SetCommunityString = 1,
			/// <summary>
			/// DeviceAddress
			/// </summary>
			DeviceAddress = 2,
			/// <summary>
			/// Timeout
			/// </summary>
			Timeout = 3,
			/// <summary>
			/// Retries
			/// </summary>
			Retries = 4,
			/// <summary>
			/// ElementTimeout
			/// </summary>
			ElementTimeout = 5,
			/// <summary>
			/// PortConnection (e.g.Udp , Tcp)
			/// </summary>
			PortConnection = 6,
			/// <summary>
			/// SecurityConfiguration
			/// </summary>
			SecurityConfig = 7,
			/// <summary>
			/// SNMPv3 Encryption Algorithm
			/// </summary>
			EncryptionAlgorithm = 8,
			/// <summary>
			/// SNMPv3 AuthenticationProtocol
			/// </summary>
			AuthenticationProtocol = 9,
			/// <summary>
			/// SNMPv3 EncryptionKey
			/// </summary>
			EncryptionKey = 10,
			/// <summary>
			/// SNMPv3 AuthenticationKey
			/// </summary>
			AuthenticationKey = 11,
			/// <summary>
			/// SNMPv3 Username
			/// </summary>
			Username = 12,
			/// <summary>
			/// SNMPv3 Security Level and Protocol
			/// </summary>
			SecurityLevelAndProtocol = 13,
			/// <summary>
			/// Local port
			/// </summary>
			LocalPort = 14,
			/// <summary>
			/// Remote port
			/// </summary>
			RemotePort = 15,
			/// <summary>
			/// Is SSL/TLS enabled
			/// </summary>
			IsSslTlsEnabled = 16,
			/// <summary>
			/// Remote host
			/// </summary>
			RemoteHost = 17,
			/// <summary>
			/// Network interface card
			/// </summary>
			NetworkInterfaceCard = 18,
			/// <summary>
			/// Bus address
			/// </summary>
			BusAddress=19,
			/// <summary>
			/// Is BypassProxy enabled.
			/// </summary>
			IsByPassProxyEnabled
		}

		/// <summary>
		/// The list of changed properties.
		/// </summary>
		private readonly List<ConnectionSetting> changedPropertyList = new List<ConnectionSetting>();

		/// <summary>
		/// Gets a value indicating whether one or more properties have been updated.
		/// </summary>
		internal abstract bool IsUpdated{ get; }

		/// <summary>
		/// Gets the list of updated properties.
		/// </summary>
		protected List<ConnectionSetting> ChangedPropertyList
		{
			get
			{
				return changedPropertyList;
			}
		}

		/// <summary>
		/// Creates an ElementPortPortInfo object based on the field contents.
		/// </summary>
		/// <param name="portPosition">The corresponding port number.</param>
		/// <param name="isCompatibilityIssueDetected">Indicates if compatibility changes need to be taken into account.</param>
		/// <returns>ElementPortInfo object.</returns>
		internal abstract ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected);

		/// <summary>
		/// Updates the provided ElementPortInfo object with any performed changes on the object.
		/// </summary>
		/// <param name="portInfo"></param>
		/// <param name="isCompatibilityIssueDetected"></param>
		internal abstract void UpdateElementPortInfo(ElementPortInfo portInfo, bool isCompatibilityIssueDetected);

		/// <summary>
		/// Clears the entries update dictionary.
		/// </summary>
		internal abstract void ClearUpdates();
	}
}