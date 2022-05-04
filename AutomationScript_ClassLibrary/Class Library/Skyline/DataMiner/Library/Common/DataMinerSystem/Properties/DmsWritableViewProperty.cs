namespace Skyline.DataMiner.Library.Common.Properties
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Represents a DMS property.
    /// </summary>
    internal class DmsWritableViewProperty : DmsViewProperty, IWritableProperty
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DmsWritableViewProperty" /> class.
		/// </summary>
		/// <param name="view">The view to which this property belongs.</param>
		/// <param name="definition">The definition of the property.</param>
		/// <param name="value">The current value of the property.</param>
		/// <exception cref="ArgumentNullException"><paramref name="view"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="definition"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
		public DmsWritableViewProperty(IDmsView view, IDmsViewPropertyDefinition definition, string value)
            : base(view, definition, value)
        {
        }

		/// <summary>
		/// Occurs when the value of a property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		/// <exception cref="ArgumentException">Thrown when the value can not be added to the property.</exception>
		public new string Value
        {
            get
            {
                return value;
            }

            set
            {
                if (!definition.IsValidInput(value))
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture ,"The value:'{0}' is not valid for the property", value));
                }

                this.value = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged()
        {
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
            {
				handler.Invoke(this, new PropertyChangedEventArgs(definition.Name));
            }
        }
    }
}