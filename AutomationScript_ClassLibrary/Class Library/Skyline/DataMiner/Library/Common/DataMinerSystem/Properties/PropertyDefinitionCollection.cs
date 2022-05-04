namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Library.Common.Properties;

	/// <summary>
	/// Represents a collection of property definitions.
	/// </summary>
	/// <typeparam name="T">The property type.</typeparam>
	internal class PropertyDefinitionCollection<T> : IPropertyDefinitionCollection<T> where T : IDmsPropertyDefinition
	{
		private readonly ICollection<T> collection = new List<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyDefinitionCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="properties">The properties to initialize the collection with.</param>
		public PropertyDefinitionCollection(IDictionary<string, T> properties)
		{
			foreach (T value in properties.Values)
			{
				collection.Add(value);
			}
		}

		/// <summary>
		/// Gets tne number of items in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return collection.Count;
			}
		}

		/// <summary>
		/// Gets the item at the specified index.
		/// </summary>
		/// <param name="index">The name of the property.</param>
		/// <exception cref="ArgumentNullException"><paramref name="index"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">An invalid value that is not a member of the set of values.</exception>
		/// <returns>The property with the specified name.</returns>
		public T this[string index]
		{
			get
			{
				if (index == null)
				{
					throw new ArgumentNullException("index");
				}

				T property = collection.SingleOrDefault(p => p.Name.Equals(index, StringComparison.OrdinalIgnoreCase));

				if (EqualityComparer<T>.Default.Equals(property, default(T)))
				{
					throw new ArgumentOutOfRangeException("index");
				}
				else
				{
					return property;
				}
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)collection).GetEnumerator();
		}
	}
}