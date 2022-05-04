namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Library.Common;
	using Skyline.DataMiner.Library.Common.Properties;

	internal class PropertyCollection<T, U> : IPropertyCollection<T, U> where T : IDmsProperty<U> where U : IDmsPropertyDefinition
	{
		private readonly ICollection<T> collection = new List<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyCollection&lt;T,U&gt;"/> class.
		/// </summary>
		/// <param name="properties">The properties to initialize the collection with.</param>
		public PropertyCollection(IDictionary<string, T> properties)
		{
			foreach (T value in properties.Values)
			{
				collection.Add(value);
			}
		}

		/// <summary>
		/// Gets the number of properties in the collection.
		/// </summary>
		/// <value>The number of properties in this collection.</value>
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

				T property = collection.SingleOrDefault(p => p.Definition.Name.Equals(index, StringComparison.OrdinalIgnoreCase));
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