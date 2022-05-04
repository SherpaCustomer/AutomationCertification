namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents the parent for every type of object that can be present on a DataMiner system.
	/// </summary>
	internal abstract class DmsObject
	{
		/// <summary>
		/// The DataMiner system the object belongs to.
		/// </summary>
		protected readonly IDms dms;

		/// <summary>
		/// List containing all of the properties that were changed.
		/// </summary>
		private readonly List<string> changedPropertyList = new List<string>();

		/// <summary>
		/// Flag stating whether the DataMiner system object has been loaded.
		/// </summary>
		private bool isLoaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsObject"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		protected DmsObject(IDms dms)
		{
			if (dms == null)
			{
				throw new ArgumentNullException("dms");
			}

			this.dms = dms;
		}

		/// <summary>
		/// Gets the DataMiner system this object belongs to.
		/// </summary>
		public IDms Dms
		{
			get
			{
				return dms;
			}
		}

		/// <summary>
		/// Gets the list containing all of the names of the properties that are changed.
		/// </summary>
		internal List<string> ChangedPropertyList
		{
			get
			{
				return changedPropertyList;
			}
		}

		/// <summary>
		/// Gets the communication object.
		/// </summary>
		internal ICommunication Communication
		{
			get
			{
				return dms.Communication;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not the DMS object has been loaded.
		/// </summary>
		internal bool IsLoaded
		{
			get
			{
				return isLoaded;
			}

			set
			{
				isLoaded = value;
			}
		}

		/// <summary>
		/// Returns a value indicating whether the object exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the object exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public abstract bool Exists();

		/// <summary>
		/// Loads DMS object data in case the object exists and is not already loaded.
		/// </summary>
		internal void LoadOnDemand()
		{
			if (!IsLoaded)
			{
				Load();
			}
		}

		/// <summary>
		/// Loads the object.
		/// </summary>
		internal abstract void Load();
	}
}