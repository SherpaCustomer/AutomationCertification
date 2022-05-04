namespace Skyline.DataMiner.Library.Common
{
	/// <summary>
	/// Allows adapting the enum to other library equivalents.
	/// </summary>
	internal static class SnmpV3EncryptionAlgorithmAdapter
	{
		/// <summary>
		/// Converts SLNet flowControl string into the enum.
		/// </summary>
		/// <param name="flowControl">flowControl string received from SLNet.</param>
		/// <returns>The equivalent enum.</returns>
		public static SnmpV3EncryptionAlgorithm FromSLNetFlowControl(string flowControl)
		{
			string noCaseFlowControl = flowControl.ToUpper();
			switch (noCaseFlowControl)
			{
				case "DES":
					return SnmpV3EncryptionAlgorithm.Des;
				case "AES128":
					return SnmpV3EncryptionAlgorithm.Aes128;
				case "AES192":
					return SnmpV3EncryptionAlgorithm.Aes192;
				case "AES256":
					return SnmpV3EncryptionAlgorithm.Aes256;
				case "DEFINEDINCREDENTIALSLIBRARY":
					return SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary;
				case "NONE":
				default:
					return SnmpV3EncryptionAlgorithm.None;
			}

		}

		/// <summary>
		/// Converts the enum into the equivalent SLNet string value.
		/// </summary>
		/// <param name="value">The enum you wish to convert.</param>
		/// <returns>The equivalent string value.</returns>
		public static string ToSLNetFlowControl(SnmpV3EncryptionAlgorithm value)
		{
			switch (value)
			{
				case SnmpV3EncryptionAlgorithm.Des:
					return "DES";
				case SnmpV3EncryptionAlgorithm.Aes128:
					return "AES128";
				case SnmpV3EncryptionAlgorithm.Aes192:
					return "AES192";
				case SnmpV3EncryptionAlgorithm.Aes256:
					return "AES256";
				case SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary:
				case SnmpV3EncryptionAlgorithm.None:
				default:
					return value.ToString();
			}
		}
	}
}
