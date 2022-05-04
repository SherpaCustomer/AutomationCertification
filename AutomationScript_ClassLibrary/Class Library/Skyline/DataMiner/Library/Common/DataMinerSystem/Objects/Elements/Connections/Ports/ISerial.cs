namespace Skyline.DataMiner.Library.Common
{
	using System;

	/// <summary>
	/// Represents a connection using serial ports.
	/// </summary>
	internal interface ISerial : IPortConnection
	{
		/// <summary>
		/// Gets or sets the serial baud rate.
		/// </summary>
		/// <value>
		/// The baud rate.
		/// </value>
		int BaudRate { get; set; }

		/// <summary>
		/// Gets or sets the standard length of data bits per byte.
		/// </summary>
		/// <value>The data bits length.</value>
		/// <exception cref="ArgumentOutOfRangeException">The data bits value is less than 5 or more than 8.</exception>
		int DataBits { get; set; }

		/// <summary>
		/// Gets or sets the standard number of stopbits per byte.
		/// </summary>
		/// <value>The standard number of stopbits per byte.</value>
		// TODO: Use Enum? https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.stopbits?view=netframework-4.8
		// TODO: Cube does not allow to select 0 stop bits?
		double StopBits { get; set; }

		SerialPortFlowControl FlowControl { get; set; }

		/// <summary>
		/// Gets or sets the parity-checking protocol.
		/// </summary>
		/// <value>The parity-checking protocol.</value>
		SerialPortParity Parity { get; set; }

		/// <summary>
		/// Gets or sets the port for communications.
		/// </summary>
		/// <value>The communications port.</value>
		/// <exception cref="ArgumentNullException">The <see cref="PortName"/> property was set to <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">The PortName property was set to a value with a length of zero.</exception>
		string PortName { get; set; } // Maps to SerialPortInfo.PortName (See also "more serial settings" in Cube).
	}
}
