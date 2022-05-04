namespace Skyline.DataMiner.Library.Common.Properties
{
	using System;
	using System.Globalization;
	using Net.Messages;

	/// <summary>
	/// Represents a DMS element property definition.
	/// </summary>
	internal class DmsElementPropertyDefinition : DmsPropertyDefinition, IDmsElementPropertyDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DmsElementPropertyDefinition"/> class.
        /// </summary>
        /// <param name="dms">Instance of the DMS.</param>
        /// <param name="config">The configuration received from SLNet.</param>
        internal DmsElementPropertyDefinition(IDms dms, PropertyConfig config)
            : base(dms, config)
        {
        }

		/// <summary>
		/// Specifies if the object exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the element property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
        {
            return Dms.PropertyExists(name, PropertyType.Element);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Element property: {0}", name);
        }
    }
}