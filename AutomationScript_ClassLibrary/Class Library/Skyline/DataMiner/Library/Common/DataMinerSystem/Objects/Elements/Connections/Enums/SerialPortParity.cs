namespace Skyline.DataMiner.Library.Common
{
	/// <summary>
	/// Specifies the parity bit for a <see cref="ISerial"/> object.
	/// </summary>
	internal enum SerialPortParity
	{
		/// <summary>
		/// No parity check occurs.
		/// </summary>
		No = 0,
		/// <summary>
		/// Sets the parity bit so that the count of bits set is an even number.
		/// </summary>
		Even = 1,
		/// <summary>
		/// Sets the parity bit so that the count of bits set is an odd number.
		/// </summary>
		Odd = 2,
		/// <summary>
		/// Leaves the parity bit set to 1.
		/// </summary>
		Mark = 3,
		/// <summary>
		/// Leaves the parity bit set to 0.
		/// </summary>
		Space = 4
	}
}
