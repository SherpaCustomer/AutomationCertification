namespace Skyline.DataMiner.Library.Common.Templates
{
	using System;
	using System.Globalization;
	using Net.Messages;

	/// <summary>
	/// Represents a standalone alarm template.
	/// </summary>
	internal class DmsStandaloneAlarmTemplate : DmsAlarmTemplate, IDmsStandaloneAlarmTemplate
	{
		/// <summary>
		/// The description of the alarm definition.
		/// </summary>
		private string description;

		/// <summary>
		/// Indicates whether this alarm template is used in a group.
		/// </summary>
		private bool isUsedInGroup;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsStandaloneAlarmTemplate"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="name">The name of the alarm template.</param>
		/// <param name="protocol">The protocol this standalone alarm template corresponds with.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		internal DmsStandaloneAlarmTemplate(IDms dms, string name, IDmsProtocol protocol)
			: base(dms, name, protocol)
		{
			IsLoaded = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsStandaloneAlarmTemplate"/> class.
		/// </summary>
		/// <param name="dms">The DataMiner system reference.</param>
		/// <param name="alarmTemplateEventMessage">An instance of AlarmTemplateEventMessage.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="dms"/> is invalid.</exception>
		internal DmsStandaloneAlarmTemplate(IDms dms, AlarmTemplateEventMessage alarmTemplateEventMessage)
			: base(dms, alarmTemplateEventMessage.Name, alarmTemplateEventMessage.Protocol, alarmTemplateEventMessage.Version)
		{
			IsLoaded = true;

			description = alarmTemplateEventMessage.Description;
			isUsedInGroup = alarmTemplateEventMessage.IsUsedInGroup;
		}

		/// <summary>
		/// Gets or sets the alarm template description.
		/// </summary>
		public string Description
		{
			get
			{
				LoadOnDemand();
				return description;
			}

			set
			{
				LoadOnDemand();

				ChangedPropertyList.Add("Description");
				description = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the alarm template is used in a group.
		/// </summary>
		public bool IsUsedInGroup
		{
			get
			{
				LoadOnDemand();
				return isUsedInGroup;
			}
		}

		/// <summary>
		/// Determines whether this alarm template exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the alarm template exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return Protocol.StandaloneAlarmTemplateExists(Name);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Alarm Template Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
		}

		/// <summary>
		/// Parses the alarm template event message.
		/// </summary>
		/// <param name="message">The message received from SLNet.</param>
		internal override void Parse(AlarmTemplateEventMessage message)
		{
			IsLoaded = true;

			description = message.Description;
			isUsedInGroup = message.IsUsedInGroup;
		}
	}
}