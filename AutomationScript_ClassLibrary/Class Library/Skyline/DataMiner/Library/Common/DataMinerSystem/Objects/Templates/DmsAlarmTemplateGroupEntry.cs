namespace Skyline.DataMiner.Library.Common.Templates
{
	using System;
	using System.Globalization;

	/// <summary>
	/// Represents an alarm group entry.
	/// </summary>
	internal class DmsAlarmTemplateGroupEntry : IDmsAlarmTemplateGroupEntry
    {
        /// <summary>
        /// The template which is an entry of the alarm group.
        /// </summary>
        private readonly IDmsAlarmTemplate template;

        /// <summary>
        /// Specifies whether this entry is enabled.
        /// </summary>
        private readonly bool isEnabled;

        /// <summary>
        /// Specifies whether this entry is scheduled.
        /// </summary>
        private readonly bool isScheduled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsAlarmTemplateGroupEntry"/> class.
        /// </summary>
        /// <param name="template">The alarm template.</param>
        /// <param name="isEnabled">Specifies if the entry is enabled.</param>
        /// <param name="isScheduled">Specifies if the entry is scheduled.</param>
        internal DmsAlarmTemplateGroupEntry(IDmsAlarmTemplate template, bool isEnabled, bool isScheduled)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            this.template = template;
            this.isEnabled = isEnabled;
            this.isScheduled = isScheduled;
        }

        /// <summary>
        /// Gets the alarm template.
        /// </summary>
		public IDmsAlarmTemplate AlarmTemplate
        {
            get
            {
                return template;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the entry is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the entry is scheduled.
        /// </summary>
        public bool IsScheduled
        {
            get
            {
                return isScheduled;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Alarm template group entry:{0}", template.Name);
        }
    }
}