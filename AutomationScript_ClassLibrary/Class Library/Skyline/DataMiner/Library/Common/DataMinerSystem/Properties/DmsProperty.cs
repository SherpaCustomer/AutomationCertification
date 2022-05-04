namespace Skyline.DataMiner.Library.Common.Properties
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a DMS property.
    /// </summary>
    internal class DmsProperty<T> : IDmsProperty<T> where T : IDmsPropertyDefinition
    {
        /// <summary>
        /// The definition of the property.
        /// </summary>
        protected readonly T definition;

        /// <summary>
        /// The value of the property.
        /// </summary>
        protected string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsProperty{T}"/> class.
        /// </summary>
        /// <param name="definition">The definition of the property.</param>
        /// <param name="value">The current value of the property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="definition"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        protected DmsProperty(T definition, string value)
        {
            if (EqualityComparer<T>.Default.Equals(definition, default(T)))
            {
                throw new ArgumentNullException("definition");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.definition = definition;
            this.value = value;
        }

        /// <summary>
        /// Gets the definition of the property.
        /// </summary>
        public T Definition
        {
            get
            {
                return definition;
            }
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
        }
    }
}