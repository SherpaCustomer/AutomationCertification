namespace Skyline.DataMiner.Library.Common
{
	using System.Collections.Generic;
	using System.Linq;

	using Net.Messages;

	/// <summary>
	/// Represents a base class for all of the components in a DmsElement object.
	/// </summary>
	internal abstract class ElementSettings
	{
		/// <summary>
		/// The list of changed properties.
		/// </summary>
		private readonly List<string> changedPropertyList = new List<string>();

		/// <summary>
		/// Instance of the DmsElement class where these classes will be used for.
		/// </summary>
		private readonly DmsElement dmsElement;

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the <see cref="DmsElement"/> instance this object is part of.</param>
		protected ElementSettings(DmsElement dmsElement)
		{
			this.dmsElement = dmsElement;
		}

		/// <summary>
		/// Gets the element this object belongs to.
		/// </summary>
		internal DmsElement DmsElement
		{
			get
			{
				return dmsElement;
			}
		}

		/// <summary>
		/// Gets a value indicating whether one or more properties have been updated.
		/// </summary>
		internal bool Updated
		{
			get
			{
				return changedPropertyList.Any();
			}
		}

		/// <summary>
		/// Gets the list of updated properties.
		/// </summary>
		protected internal List<string> ChangedPropertyList
		{
			get
			{
				return changedPropertyList;
			}
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message which will be sent to SLNet.</param>
		internal abstract void FillUpdate(AddElementMessage message);

		/// <summary>
		/// Based on the array provided from the DmsNotify call, parse the data to the correct fields.
		/// </summary>
		/// <param name="elementInfo">Object containing all the required information. Retrieved by DmsClass.</param>
		internal abstract void Load(ElementInfoEventMessage elementInfo);

		/// <summary>
		/// Clears the entries update dictionary.
		/// </summary>
		protected internal void ClearUpdates()
		{
			changedPropertyList.Clear();
		}
	}
}