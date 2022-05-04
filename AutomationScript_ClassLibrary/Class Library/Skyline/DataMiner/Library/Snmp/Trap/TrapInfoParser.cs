namespace Skyline.DataMiner.Library.Protocol.Snmp.Trap
{
	using System;
    using System.Globalization;
    using System.Net;

	internal class TrapInfoParser
	{
		private TrapInfoParser()
		{
		}

		/// <summary>
		/// Parses the object returned from SLProtocol when using allBindingInfo on a parameter in a DataMiner Protocol.
		/// </summary>
		/// <param name="trapInfo">The object returned from SLProtocol when using allBindingInfo on a parameter in a DataMiner Protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="trapInfo"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="trapInfo"/> is invalid.</exception>
		/// <exception cref="FormatException"><paramref name="trapInfo"/>Source is not a valid IP address.</exception>
		/// <returns>An instance of the <see cref="TrapInfo"/> class.</returns>
		/// <example>
		/// <code>TrapInfo trap = TrapInfoParser.Parse(trapInfo);</code>
		/// </example>
		public static TrapInfo Parse(object trapInfo)
		{
			if (trapInfo == null)
			{
				throw new ArgumentNullException("trapInfo");
			}

			object[] trapInfoArray = trapInfo as object[];
			if (trapInfoArray == null || trapInfoArray.Length < 1)
			{
				throw new ArgumentException("Retrieved invalid trap information.", "trapInfo");
			}

			object[] generalTrapInfo = trapInfoArray[0] as object[];
			if (generalTrapInfo == null || generalTrapInfo.Length < 3)
			{
				throw new ArgumentException("Retrieved invalid trap information: generalTrapInfo was not the correct array.", "trapInfo");
			}

			string oid = Convert.ToString(generalTrapInfo[0], CultureInfo.CurrentCulture);

			IPAddress source = IPAddress.Parse(Convert.ToString(generalTrapInfo[1], CultureInfo.CurrentCulture));
			int ticks = GetTicks(generalTrapInfo[2]);

			TrapInfoVariableBindingCollection trapBindings = GetBindings(trapInfoArray);

			return new TrapInfo(oid, source, ticks, new ReadOnlyTrapInfoVariableBindingCollection(trapBindings));
		}

		private static TrapInfoVariableBindingCollection GetBindings(object[] trapInfoArray)
		{
			TrapInfoVariableBindingCollection bindings = new TrapInfoVariableBindingCollection();

			for (int i = 1; i < trapInfoArray.Length; i++)
			{
				object[] binding = trapInfoArray[i] as object[];
				if (binding == null || binding.Length < 2)
				{
					throw new ArgumentException("Retrieved invalid trap information.");
				}
				else
				{
					string oid = Convert.ToString(binding[0], CultureInfo.CurrentCulture);
					string value = Convert.ToString(binding[1], CultureInfo.CurrentCulture);

					bindings.Add(new TrapInfoVariableBinding(oid, value));
				}
			}

			return bindings;
		}

		private static int GetTicks(object timeInfo)
		{
			int iTick;
			if (Int32.TryParse(Convert.ToString(timeInfo, CultureInfo.CurrentCulture), out iTick))
			{
				return iTick;
			}
			else
			{
				throw new ArgumentException("Retrieved invalid time ticks in trap information");
			}
		}
	}
}