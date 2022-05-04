namespace Skyline.DataMiner.Library.Common.Properties
{
	using System;
	using System.Globalization;
	using Net.Messages;

	/// <summary>
	/// Represents a DMS view property definitions.
	/// </summary>
	internal class DmsViewPropertyDefinition : DmsPropertyDefinition, IDmsViewPropertyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DmsViewPropertyDefinition"/> class.
        /// </summary>
        /// <param name="dms">Instance of the DMS.</param>
        /// <param name="config">The configuration received from SLNet.</param>
        internal DmsViewPropertyDefinition(IDms dms, PropertyConfig config)
            : base(dms, config)
        {
        }

		/// <summary>
		/// Returns a value indicating whether the view property exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the view property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
        {
            return Dms.PropertyExists(Name, PropertyType.View);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "View property: {0}", Name);
        }
    }
}