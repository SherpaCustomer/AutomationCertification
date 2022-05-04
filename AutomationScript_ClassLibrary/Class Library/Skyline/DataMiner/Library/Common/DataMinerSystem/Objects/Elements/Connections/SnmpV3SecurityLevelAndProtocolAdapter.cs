namespace Skyline.DataMiner.Library.Common
{
	/// <summary>
	/// Allows adapting the enum to other library equivalents.
	/// </summary>
	internal static class SnmpV3SecurityLevelAndProtocolAdapter
	{
		/// <summary>
		/// Converts SLNet stopBits string into the enum.
		/// </summary>
		/// <param name="stopBits">stopBits string received from SLNet.</param>
		/// <returns>The equivalent enum.</returns>
		public static SnmpV3SecurityLevelAndProtocol FromSLNetStopBits(string stopBits)
		{
			string noCaseStopBits = stopBits.ToUpper();
			switch (noCaseStopBits)
			{
				case "AUTHPRIV":
				case "AUTHENTICATIONPRIVACY":
					return SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy;
				case "AUTHNOPRIV":
				case "AUTHENTICATIONNOPRIVACY":
					return SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy;
				case "NOAUTHNOPRIV":
				case "NOAUTHENTICATIONNOPRIVACY":
					return SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy;
				case "DEFINEDINCREDENTIALSLIBRARY":
					return SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary;
				default:
					return SnmpV3SecurityLevelAndProtocol.None;
			}
		}

		/// <summary>
		/// Converts the enum into the equivalent SLNet string value.
		/// </summary>
		/// <param name="value">The enum you wish to convert.</param>
		/// <returns>The equivalent string value.</returns>
		public static string ToSLNetStopBits(SnmpV3SecurityLevelAndProtocol value)
		{
			switch (value)
			{
				case SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy:
					return "authPriv";
				case SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy:
					return "authNoPriv";
				case SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy:
					return "noAuthNoPriv";
				case SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary:
					return "DefinedInCredentialsLibrary";
				case SnmpV3SecurityLevelAndProtocol.None:
				default:
					return string.Empty;
			}
		}
	}
}
