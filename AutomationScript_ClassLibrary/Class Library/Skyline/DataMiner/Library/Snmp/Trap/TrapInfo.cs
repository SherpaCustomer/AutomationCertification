namespace Skyline.DataMiner.Library.Protocol.Snmp.Trap
{
	using System;
	using System.Net;
	using System.Text;

	/// <summary>
	/// Represents all the info as received from a trap in a protocol that has the TrapOID@setBindings attribute set to "allBindingInfo".
	/// </summary>
	public class TrapInfo
	{
		private readonly string oid;
		private readonly IPAddress source;
		private readonly DateTime receivedTime;
		private readonly ReadOnlyTrapInfoVariableBindingCollection variableBindings;

		internal TrapInfo(string oid, IPAddress source, int ticks, ReadOnlyTrapInfoVariableBindingCollection variableBindings)
		{

			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (variableBindings == null)
			{
				throw new ArgumentNullException("variableBindings");
			}

			if (String.IsNullOrWhiteSpace(oid))
			{
				throw new ArgumentException("The OID cannot be null, empty or white space.", "oid");
			}

			this.oid = oid;

			this.source = source;

			this.variableBindings = variableBindings;

			int ticksDifference;
			{
				int currentTimeTicks = Environment.TickCount & Int32.MaxValue;  // Make sure negative numbers aren't used.
				int trapTimeTicks = ticks & Int32.MaxValue; // Make sure negative numbers aren't used.

				if (currentTimeTicks >= trapTimeTicks)
				{
					ticksDifference = currentTimeTicks - trapTimeTicks;
				}
				else
				{
					ticksDifference = (int.MaxValue - trapTimeTicks) + currentTimeTicks + 1;
				}
			}

			receivedTime = DateTime.Now.AddMilliseconds(-ticksDifference);
		}

		/// <summary>
		/// Gets the variable bindings of this trap.
		/// </summary>
		/// <value>The variable bindings.</value>
		public ReadOnlyTrapInfoVariableBindingCollection VariableBindings
		{
			get { return variableBindings; }
		}

		/// <summary>
		/// Gets the trap OID.
		/// </summary>
		/// <value>The trap OID.</value>
		public string Oid
		{
			get { return oid; }
		}

		/// <summary>
		/// Gets the trap source IP address.
		/// </summary>
		/// <value>The trap source IP address.</value>
		public IPAddress Source
		{
			get { return source; }
		}

		/// <summary>
		/// Gets the time when the trap was received in the SLSNMPManager process.
		/// </summary>
		/// <value>The time when the trap was received in the SLSNMPManager process.</value>
		/// <remarks>This value is calculated based on a tick count (representing the number of milliseconds that have elapsed since the system was started at the moment the trap was received in the SLSNMPManager process).
		/// This tick count will wrap around to zero if the system is run continuously for 24.9 days.</remarks>
		public DateTime ReceivedTime
		{
			get { return receivedTime; }
		}

		/// <summary>
		/// Parses the trap object received from SLProtocol into a TrapInfo object.
		/// </summary>
		/// <param name="trapInfo">The trap info object when the TrapOID@setBindings attribute set to "allBindingInfo".</param>
		/// <exception cref="ArgumentNullException"><paramref name="trapInfo"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="trapInfo"/> is invalid.</exception>
		/// <returns>An object of type TrapInfo.</returns>
		/// <example>
		/// <code>
		/// TrapInfo trap = TrapInfo.FromTrapData(trapInfo);
		/// </code>
		/// </example>
		public static TrapInfo FromTrapData(object trapInfo)
		{
			return TrapInfoParser.Parse(trapInfo);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Trap[OID: ");
			sb.Append(Oid);
			sb.Append(", Source: ");
			sb.Append(Source);
			sb.Append(", Received: ");
			sb.Append(ReceivedTime);
			sb.Append("Variable binding count: " + VariableBindings.Count);
			sb.Append("]" + Environment.NewLine);

			return sb.ToString();
		}
	}
}