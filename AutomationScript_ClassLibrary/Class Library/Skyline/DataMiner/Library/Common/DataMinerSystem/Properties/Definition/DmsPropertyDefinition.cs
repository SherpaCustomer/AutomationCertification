namespace Skyline.DataMiner.Library.Common.Properties
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using Net.Messages;

    /// <summary>
    /// Parent class for all types of DMS properties definitions.
    /// </summary>
    internal abstract class DmsPropertyDefinition : IDmsPropertyDefinition
    {
		/// <summary>
		/// Instance of the DMS class.
		/// </summary>
		protected readonly IDms dms;

		/// <summary>
		/// The name of the property.
		/// </summary>
		protected string name;

        /// <summary>
        /// The id of the property.
        /// </summary>
        protected int id;

        /// <summary>
        /// Specifies if the property is available for alarm filtering.
        /// </summary>
        protected bool isAvailableForAlarmFiltering;

        /// <summary>
        /// Specifies if the property is read only.
        /// </summary>
        protected bool isReadOnly;

        /// <summary>
        /// Specifies if the property is visible in the Surveyor.
        /// </summary>
        protected bool isVisibleInSurveyor;

        /// <summary>
        /// The regular expression.
        /// </summary>
        protected string regex;

		/// <summary>
		/// The associated discrete entries with the property.
		/// </summary>
		protected List<IDmsPropertyEntry> entries;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsPropertyDefinition"/> class.
		/// </summary>
		/// <param name="dms">Instance of DMS.</param>
		/// <param name="config">The property configuration received from SLNet.</param>
		protected DmsPropertyDefinition(IDms dms, PropertyConfig config)
        {
            if (dms == null)
            {
                throw new ArgumentNullException("dms");
            }

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            this.dms = dms;
            Parse(config);
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the ID of the property.
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the property is available for alarm filtering.
        /// </summary>
        public bool IsAvailableForAlarmFiltering
        {
            get
            {
                return isAvailableForAlarmFiltering;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the property is read only or not.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the property is visible in the surveyor.
        /// </summary>
        public bool IsVisibleInSurveyor
        {
            get
            {
                return isVisibleInSurveyor;
            }
        }

        /// <summary>
        /// Gets the regular expression of the property.
        /// </summary>
        public string Regex
        {
            get
            {
                return regex;
            }
        }

        /// <summary>
        /// Gets the discrete entries associated with the property.
        /// </summary>
        public ReadOnlyCollection<IDmsPropertyEntry> Entries
        {
            get
            {
                return entries.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the DMS instance.
        /// </summary>
        internal IDms Dms
        {
            get
            {
                return dms;
            }
        }

		/// <summary>
		/// Determines whether the object exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public abstract bool Exists();

        /// <summary>
        /// Checks if the provided input value matches the definition of the property.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns><c>true</c> if the input is valid; otherwise, <c>false</c>.</returns>
        public bool IsValidInput(string value)
        {
            if (!String.IsNullOrWhiteSpace(regex))
            {
                Regex r = new Regex(regex);
                return r.Match(value).Success;
            }

            return true;
        }

        /// <summary>
        /// Parses the SLNet object.
        /// </summary>
        /// <param name="config">Property configuration object.</param>
        internal void Parse(PropertyConfig config)
        {
            name = config.Name;
            id = config.ID;
            isAvailableForAlarmFiltering = config.IsFilterEnabled;
            isReadOnly = config.IsReadOnly;
            isVisibleInSurveyor = config.IsVisibleInSurveyor;
            regex = config.RegEx;

			if (config.Entries != null)
            {
				PropertyConfigEntry[] propertyConfigEntries = config.Entries;
				entries = new List<IDmsPropertyEntry>(propertyConfigEntries.Length);

				foreach (PropertyConfigEntry entry in propertyConfigEntries)
                {
                    DmsPropertyEntry temp = new DmsPropertyEntry(entry.Value, entry.Metric);
                    entries.Add(temp);
                }
            }
			else
			{
				entries = new List<IDmsPropertyEntry>();
			}
        }

    }
}