namespace Skyline.DataMiner.Library.Common
{
	using System.Collections.Generic;
	using Net.Messages;

	/// <summary>
	/// Represents a base class for all of the components in a DmsService object.
	/// </summary>
	internal abstract class ServiceSettings
	{
		/// <summary>
		/// The list of changed properties.
		/// </summary>
		private readonly List<string> changedPropertyList = new List<string>();

		/// <summary>
		/// Instance of the DmsService class where these classes will be used for.
		/// </summary>
		private readonly DmsService dmsService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceSettings"/> class.
		/// </summary>
		/// <param name="dmsService">The reference to the <see cref="DmsService"/> instance this object is part of.</param>
		protected ServiceSettings(DmsService dmsService)
		{
			this.dmsService = dmsService;
		}

		/// <summary>
		/// Gets the service this object belongs to.
		/// </summary>
		internal DmsService DmsService
		{
			get
			{
				return dmsService;
			}
		}

		/// <summary>
		/// Gets a value indicating whether one or more properties have been updated.
		/// </summary>
		internal bool Updated
		{
			get
			{
				return changedPropertyList.Count > 0;
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
		/// Fills in the needed properties in the AddService message.
		/// </summary>
		/// <param name="message">The AddService message which will be sent to SLNet.</param>
		internal abstract void FillUpdate(AddServiceMessage message);

		/// <summary>
		/// Based on the array provided from the DmsNotify call, parse the data to the correct fields.
		/// </summary>
		/// <param name="serviceInfo">Object containing all the required information. Retrieved by DmsClass.</param>
		internal abstract void Load(ServiceInfoEventMessage serviceInfo);

		/// <summary>
		/// Clears the entries update dictionary.
		/// </summary>
		protected internal void ClearUpdates()
		{
			changedPropertyList.Clear();
		}
	}
}