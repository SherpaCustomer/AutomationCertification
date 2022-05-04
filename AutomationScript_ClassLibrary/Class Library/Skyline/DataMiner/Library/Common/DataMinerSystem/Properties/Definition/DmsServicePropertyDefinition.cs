namespace Skyline.DataMiner.Library.Common.Properties
{
	using System.Globalization;

	using Skyline.DataMiner.Net.Messages;

	internal class DmsServicePropertyDefinition : DmsPropertyDefinition, IDmsServicePropertyDefinition
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DmsServicePropertyDefinition" /> class.
		/// </summary>
		/// <param name="dms">Instance of the DMS.</param>
		/// <param name="config">The configuration received from SLNet.</param>
		internal DmsServicePropertyDefinition(IDms dms, PropertyConfig config)
			: base(dms, config)
		{
		}

		/// <summary>
		///     Returns a value indicating whether the service property exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the service property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return this.Dms.PropertyExists(this.Name, PropertyType.Service);
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "Service property: {0}", this.Name);
		}
	}
}