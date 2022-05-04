namespace Skyline.DataMiner.Library.Common.Rates
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Class that contains the data of all the interfaces. This can be looked to as an object that contains all the data of a table.
	/// </summary>
	public class InterfaceTable
	{
		private readonly ICollection<InterfaceRow> interfaceRows;
		private readonly TimeSpan? delta;
		private readonly int bufferedDeltaValue;
		private readonly int minDelta;

		/// <summary>
		/// Initializes a new instance of the <see cref="InterfaceTable"/> class.
		/// </summary>
		/// <param name="interfaceRows">Collection that contains the data of all interfaces.</param>
		/// <param name="delta">Time span between this and previous executed poll group.</param>
		/// <param name="bufferedDeltaValue">Value of how long ago a group got valid polled results before the previous executed poll group.</param>
		/// <param name="minDelta">The minimum value <paramref name="delta" /> must have (in ms).</param>
		public InterfaceTable(ICollection<InterfaceRow> interfaceRows, TimeSpan? delta, int bufferedDeltaValue, int minDelta)
		{
			this.interfaceRows = interfaceRows;
			this.delta = delta;
			this.minDelta = minDelta;
			this.bufferedDeltaValue = bufferedDeltaValue;
		}

		/// <summary>
		/// Gets the collection containing all the interface rows.
		/// </summary>
		public ICollection<InterfaceRow> InterfaceRows
		{
			get
			{
				return interfaceRows;
			}
		}

		/// <summary>
		/// Gets the time span between this and previous executed poll group.
		/// </summary>
		public TimeSpan? Delta
		{
			get
			{
				return delta;
			}
		}

		/// <summary>
		/// Gets the value of how long ago a group got valid polled results before the previous executed poll group.
		/// </summary>
		public int BufferedDeltaValue
		{
			get
			{
				return bufferedDeltaValue;
			}
		}

		/// <summary>
		/// Gets the minimum value the Delta must have (in ms).
		/// </summary>
		public int MinDelta
		{
			get
			{
				return minDelta;
			}
		}
	}
}
