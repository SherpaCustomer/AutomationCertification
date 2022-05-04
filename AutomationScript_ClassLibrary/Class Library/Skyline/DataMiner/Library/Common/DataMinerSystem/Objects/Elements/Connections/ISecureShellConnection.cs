namespace Skyline.DataMiner.Library.Common
{
	interface ISecureShellConnection : IRealConnection
	{
		ITcp Connection { get; set; }

		// TODO: Check if we can detect if this is a SecureShell connection.

		// TODO: Check if other properties are possible.

		// Key exchange algorithm. RN 9.5.1.

		// Currently not possible in Cube:
		// Username/Password

		// Public key.
	}
}
