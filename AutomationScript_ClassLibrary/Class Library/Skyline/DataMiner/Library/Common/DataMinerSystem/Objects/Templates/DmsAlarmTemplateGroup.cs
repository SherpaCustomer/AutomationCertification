namespace Skyline.DataMiner.Library.Common.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Net.Messages;

    /// <summary>
    /// Represents an alarm template group.
    /// </summary>
    internal class DmsAlarmTemplateGroup : DmsAlarmTemplate, IDmsAlarmTemplateGroup
    {
		/// <summary>
		/// The entries of the alarm group.
		/// </summary>
		private readonly List<IDmsAlarmTemplateGroupEntry> entries = new List<IDmsAlarmTemplateGroupEntry>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsAlarmTemplateGroup"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="name">The name of the alarm template.</param>
		/// <param name="protocol">The protocol this alarm template group corresponds with.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		internal DmsAlarmTemplateGroup(IDms dms, string name, IDmsProtocol protocol)
            : base(dms, name, protocol)
        {
			IsLoaded = false;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsAlarmTemplateGroup"/> class.
		/// </summary>
		/// <param name="dms">Instance of <see cref="Dms"/>.</param>
		/// <param name="alarmTemplateEventMessage">An instance of AlarmTemplateEventMessage.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="alarmTemplateEventMessage"/> is invalid.</exception>
		internal DmsAlarmTemplateGroup(IDms dms, AlarmTemplateEventMessage alarmTemplateEventMessage)
            : base(dms, alarmTemplateEventMessage.Name, alarmTemplateEventMessage.Protocol, alarmTemplateEventMessage.Version)
        {
			IsLoaded = true;

			foreach (AlarmTemplateGroupEntry entry in alarmTemplateEventMessage.GroupEntries)
			{
				IDmsAlarmTemplate template = Protocol.GetAlarmTemplate(entry.Name);
				entries.Add(new DmsAlarmTemplateGroupEntry(template, entry.IsEnabled, entry.IsScheduled));
			}
		}

		/// <summary>
		/// Gets the entries of the alarm template group.
		/// </summary>
		public ReadOnlyCollection<IDmsAlarmTemplateGroupEntry> Entries
		{
			get
			{
				LoadOnDemand();
				return entries.AsReadOnly();
			}
		}

		/// <summary>
		/// Determines whether this alarm template exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the alarm template exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
        {
            bool exists = false;
            AlarmTemplateEventMessage template = GetAlarmTemplate();

            if (template != null && template.Type == AlarmTemplateType.Group)
            {
                exists = true;
            }

            return exists;
        }

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Template Group Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
		}

		/// <summary>
		/// Parses the alarm template event message.
		/// </summary>
		/// <param name="message">The message received from the SLNet process.</param>
		internal override void Parse(AlarmTemplateEventMessage message)
        {
            IsLoaded = true;

			entries.Clear();

            foreach (AlarmTemplateGroupEntry entry in message.GroupEntries)
            {
                IDmsAlarmTemplate template = Protocol.GetAlarmTemplate(entry.Name);
                entries.Add(new DmsAlarmTemplateGroupEntry(template, entry.IsEnabled, entry.IsScheduled));
            }
        }

		/// <summary>
		/// Gets the alarm template from the SLNet process.
		/// </summary>
		/// <returns>The alarm template.</returns>
		private AlarmTemplateEventMessage GetAlarmTemplate()
		{
			GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
			{
				AsOneObject = true,
				Protocol = Protocol.Name,
				Version = Protocol.Version,
				Template = Name
			};

			AlarmTemplateEventMessage cachedAlarmTemplateMessage = (AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);

			return cachedAlarmTemplateMessage;
		}
    }
}