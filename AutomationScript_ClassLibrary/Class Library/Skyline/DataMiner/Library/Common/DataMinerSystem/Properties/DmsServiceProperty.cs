namespace Skyline.DataMiner.Library.Common.Properties
{
	using System;

	/// <summary>
	/// Represents a DMS service property.
	/// </summary>
	internal class DmsServiceProperty : DmsProperty<IDmsServicePropertyDefinition>, IDmsServiceProperty
    {
        /// <summary>
        /// The service to which the property belongs.
        /// </summary>
        private readonly IDmsService service;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsServiceProperty"/> class.
		/// </summary>
		/// <param name="service">The service to which the property is assigned.</param>
		/// <param name="definition">The definition of the property.</param>
		/// <param name="value">The current value of the property.</param>
		/// <exception cref="ArgumentNullException"><paramref name="service"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="definition"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
		public DmsServiceProperty(IDmsService service, IDmsServicePropertyDefinition definition, string value)
            : base(definition, value)
        {
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}

            this.service = service;
        }

        /// <summary>
        /// Gets the service to which the property is assigned.
        /// </summary>
        public IDmsService Service
        {
            get
            {
                return service;
            }
        }
    }
}