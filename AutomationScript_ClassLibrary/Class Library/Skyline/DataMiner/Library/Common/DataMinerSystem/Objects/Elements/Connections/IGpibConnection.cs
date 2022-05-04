namespace Skyline.DataMiner.Library.Common
{
	interface IGpibConnection : IRealConnection
	{
		// Only supports TCP/IP.
		ITcp Connection { get; set; }

		GpibApi Api { get; set; }

		// TODO: This field is present in Cube but not in GPIBPortInfo of SLNet.
		string DeviceAddress { get; set; }
	}
}