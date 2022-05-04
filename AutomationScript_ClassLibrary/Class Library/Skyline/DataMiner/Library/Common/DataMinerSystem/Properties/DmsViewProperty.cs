namespace Skyline.DataMiner.Library.Common.Properties
{
	using System;

    /// <summary>
    /// Represents a DMS property.
    /// </summary>
    internal class DmsViewProperty : DmsProperty<IDmsViewPropertyDefinition>, IDmsViewProperty
    {
        /// <summary>
        /// The view to which the property is assigned.
        /// </summary>
        private readonly IDmsView view;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsViewProperty"/> class.
        /// </summary>
        /// <param name="view">The view to which the property is assigned.</param>
        /// <param name="definition">The definition of the property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="definition"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        internal DmsViewProperty(IDmsView view, IDmsViewPropertyDefinition definition, string value)
            : base(definition, value)
        {
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}

			this.view = view;
        }

        /// <summary>
        /// Gets the view to which the property is assigned.
        /// </summary>
        public IDmsView View
        {
            get
            {
                return view;
            }
        }
    }
}