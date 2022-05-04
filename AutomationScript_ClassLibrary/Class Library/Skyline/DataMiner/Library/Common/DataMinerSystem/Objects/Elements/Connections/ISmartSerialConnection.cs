namespace Skyline.DataMiner.Library.Common
{
	interface ISmartSerialConnection : IRealConnection
	{
		/// <summary>
		/// Gets or sets the underlying connection.
		/// </summary>
		/// <value>The underlying connection.</value>
		IIpBased Connection { get; set; }

		// TODO: "BypassProxy" and possible other uses? Where to model this?: Feedback LMN: No as soon as "bypassproxy" was specified, then no other value can be put in this field. (at least not supported currently)
		string BusAddress { get; set; }

		bool IsSecure { get; set; }
	}

	// TODO: model client and server. ISmartSerialServerConnection, ISmartSerialClientConnection?
	// TODO: Info message allows to retrieve allowed IPs: http://devcore3/documentation/server/RC/html/e7dbdb35-9528-5b65-8436-6b3242a8076f.htm
}