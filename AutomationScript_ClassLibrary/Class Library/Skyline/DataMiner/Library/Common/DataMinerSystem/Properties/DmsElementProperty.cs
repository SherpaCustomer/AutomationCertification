namespace Skyline.DataMiner.Library.Common.Properties
{
	using System;

	/// <summary>
	/// Represents a DMS element property.
	/// </summary>
	internal class DmsElementProperty : DmsProperty<IDmsElementPropertyDefinition>, IDmsElementProperty
    {
        /// <summary>
        /// The element to which the property belongs.
        /// </summary>
        private readonly IDmsElement element;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsElementProperty"/> class.
        /// </summary>
        /// <param name="element">The element to which the property is assigned.</param>
        /// <param name="definition">The definition of the property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="definition"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public DmsElementProperty(IDmsElement element, IDmsElementPropertyDefinition definition, string value)
            : base(definition, value)
        {
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

            this.element = element;
        }

        /// <summary>
        /// Gets the element to which the property is assigned.
        /// </summary>
        public IDmsElement Element
        {
            get
            {
                return element;
            }
        }
    }
}