namespace Skyline.DataMiner.Library.Common.Properties
{
	using System;
	using System.Globalization;

	/// <summary>
	/// Entry class for the discrete entries associated with a property definition.
	/// </summary>
	internal class DmsPropertyEntry : IDmsPropertyEntry
    {
        /// <summary>
        /// The value of the property.
        /// </summary>
        private string value;

        /// <summary>
        /// The numeric value attached with this discrete.
        /// </summary>
        private int metric;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsPropertyEntry"/> class.
		/// </summary>
		/// <param name="value">The display value of the entry.</param>
		/// <param name="metric">The internal value of the entry.</param>
        internal DmsPropertyEntry(string value, int metric)
        {
            Value = value;
            Metric = metric;
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        public string Value
        {
            get
            {
                return value;
            }

            internal set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets the numeric value attached with the discrete.
        /// </summary>
        public int Metric
        {
            get
            {
                return metric;
            }

            internal set
            {
                metric = value;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Property Entry:<{0};{1}>", value, metric);
        }
    }
}