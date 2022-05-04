namespace Skyline.DataMiner.Library.Common
{
	interface IOpcConnection : IRealConnection
	{
		ITcp Connection { get; set; }
		// Only supports TCP/IP, port seems fixed.

		// TODO: TBD: Where to model this.
		string BusAddress { get; set; }
	}
}
