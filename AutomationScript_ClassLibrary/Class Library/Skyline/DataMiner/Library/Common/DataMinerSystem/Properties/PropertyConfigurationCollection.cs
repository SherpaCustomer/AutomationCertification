namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Represents a property configuration collection.
	/// </summary>
	internal class PropertyConfigurationCollection : IPropertConfigurationCollection
	{
		private readonly ICollection<PropertyConfiguration> collection = new List<PropertyConfiguration>();

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyConfigurationCollection"/> class.
		/// </summary>
		/// <param name="properties">The available property configurations.</param>
		public PropertyConfigurationCollection(IDictionary<string, PropertyConfiguration> properties)
		{
			foreach (PropertyConfiguration value in properties.Values)
			{
				collection.Add(value);
			}
		}

		/// <summary>
		/// Gets the amount of configurations in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return collection.Count;
			}
		}

		/// <summary>
		/// Gets the configuration based on the name.
		/// </summary>
		/// <param name="propertyName">The name of the configuration.</param>
		/// <returns>The matching configuration object.</returns>
		public PropertyConfiguration this[string propertyName]
		{
			get
			{
				if (propertyName == null)
				{
					throw new ArgumentNullException("propertyName");
				}

				PropertyConfiguration property = collection.SingleOrDefault(p => p.Definition.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
				if (property == null)
				{
					throw new ArgumentOutOfRangeException("propertyName");
				}
				else
				{
					return property;
				}
			}
		}

		/// <summary>
		/// Returns a reference to an enumerator object which is used to iterate over the collection.
		/// </summary>
		/// <returns>Returns a reference to an enumerator object, which is used to iterate over a Collection object.</returns>
		public IEnumerator<PropertyConfiguration> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		/// <summary>
		/// Returns a reference to an enumerator object which is used to iterate over the collection.
		/// </summary>
		/// <returns>Returns a reference to an enumerator object, which is used to iterate over a Collection object.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)collection).GetEnumerator();
		}
	}
}