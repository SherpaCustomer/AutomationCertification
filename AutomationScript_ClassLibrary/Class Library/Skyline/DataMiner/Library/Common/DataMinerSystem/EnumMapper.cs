namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	/// <summary>
	/// Helper class to convert from enumeration value to string or vice versa.
	/// </summary>
	internal static class EnumMapper
	{
		/// <summary>
		/// The connection type map.
		/// </summary>
		private static readonly Dictionary<string, ConnectionType> ConnectionTypeMapping = new Dictionary<string, ConnectionType>
		{
			{ "SNMP", ConnectionType.SnmpV1},
			{ "SNMPV1", ConnectionType.SnmpV1},
			{ "SNMPV2", ConnectionType.SnmpV2},
			{ "SNMPV3", ConnectionType.SnmpV3},
			{ "SERIAL", ConnectionType.Serial},
			{ "SERIAL SINGLE", ConnectionType.SerialSingle},
			{ "SMART-SERIAL", ConnectionType.SmartSerial},
			{ "SMART-SERIAL SINGLE", ConnectionType.SmartSerialSingle },
			{ "HTTP", ConnectionType.Http},
			{ "GPIB", ConnectionType.Gpib},
			{ "VIRTUAL", ConnectionType.Virtual},
			{ "OPC", ConnectionType.Opc},
			{ "SLA", ConnectionType.Sla},
			{ "WEBSOCKET", ConnectionType.WebSocket }
		};

		/// <summary>
		/// Converts a string denoting a connection type to the corresponding value of the <see cref="ConnectionType"/> enumeration.
		/// </summary>
		/// <param name="type">The connection type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="type"/> is the empty string ("") or white space</exception>
		/// <exception cref="KeyNotFoundException"></exception>
		/// <returns>The corresponding <see cref="ConnectionType"/> value.</returns>
		internal static ConnectionType ConvertStringToConnectionType(string type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (String.IsNullOrWhiteSpace(type))
			{
				throw new ArgumentException("The type must not be empty.", "type");
			}

			string valueLower = type.ToUpperInvariant();

			ConnectionType result;

			if (!ConnectionTypeMapping.TryGetValue(valueLower, out result))
			{
				throw new KeyNotFoundException(String.Format(CultureInfo.InvariantCulture, "The key {0} could not be found.", valueLower));
			}

			return result;
		}
	}
}