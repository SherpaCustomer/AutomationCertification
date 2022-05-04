namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Library.Common.Properties;
	using Skyline.DataMiner.Library.Common.Templates;
	using Skyline.DataMiner.Net.Exceptions;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Represents a DataMiner element.
	/// </summary>
	internal class DmsElement : DmsObject, IDmsElement
	{
		/// <summary>
		///     Contains the properties for the element.
		/// </summary>
		private readonly IDictionary<string, DmsElementProperty> properties =
			new Dictionary<string, DmsElementProperty>();

		/// <summary>
		///     This list will be used to keep track of which views were assigned / removed during the life time of the element.
		/// </summary>
		private readonly List<int> registeredViewIds = new List<int>();

		/// <summary>
		///     A set of all updated properties.
		/// </summary>
		private readonly HashSet<string> updatedProperties = new HashSet<string>();

		/// <summary>
		///     Array of views where the element is contained in.
		/// </summary>
		private readonly ISet<IDmsView> views = new DmsViewSet();

		/// <summary>
		///     The advanced settings.
		/// </summary>
		private AdvancedSettings advancedSettings;

		/// <summary>
		///     The device settings.
		/// </summary>
		private DeviceSettings deviceSettings;

		/// <summary>
		///     The DVE settings.
		/// </summary>
		private DveSettings dveSettings;

		/// <summary>
		///     Collection of connections available on the element.
		/// </summary>
		private IElementConnectionCollection elementCommunicationConnections;

		// Keep this message in case we need to parse the element properties when the user wants to use these.
		private ElementInfoEventMessage elementInfo;

		/// <summary>
		///     The failover settings.
		/// </summary>
		private FailoverSettings failoverSettings;

		/// <summary>
		///     The general settings.
		/// </summary>
		private GeneralSettings generalSettings;

		/// <summary>
		///     Specifies whether the properties of the elementInfo object have been parsed into dedicated objects.
		/// </summary>
		private bool propertiesLoaded;

		/// <summary>
		///     The redundancy settings.
		/// </summary>
		private RedundancySettings redundancySettings;

		/// <summary>
		///     The replication settings.
		/// </summary>
		private ReplicationSettings replicationSettings;

		/// <summary>
		///     The element components.
		/// </summary>
		private IList<ElementSettings> settings;

		/// <summary>
		///     Specifies whether the views have been loaded.
		/// </summary>
		private bool viewsLoaded;

		/// <summary>
		///     Initializes a new instance of the <see cref="DmsElement" /> class.
		/// </summary>
		/// <param name="dms">Object implementing <see cref="IDms" /> interface.</param>
		/// <param name="dmsElementId">The system-wide element ID.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms" /> is <see langword="null" />.</exception>
		internal DmsElement(IDms dms, DmsElementId dmsElementId)
			: base(dms)
		{
			this.Initialize();
			this.generalSettings.DmsElementId = dmsElementId;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DmsElement" /> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms" /> interface.</param>
		/// <param name="elementInfo">The element information.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="elementInfo" /> is <see langword="null" />.</exception>
		internal DmsElement(IDms dms, ElementInfoEventMessage elementInfo)
			: base(dms)
		{
			if (elementInfo == null)
			{
				throw new ArgumentNullException("elementInfo");
			}

			this.Initialize(elementInfo);
			this.Parse(elementInfo);
		}

		/// <summary>
		///     Gets the advanced settings of this element.
		/// </summary>
		/// <value>The advanced settings of this element.</value>
		public IAdvancedSettings AdvancedSettings
		{
			get
			{
				return this.advancedSettings;
			}
		}

		/// <summary>
		///     Gets the DataMiner Agent ID.
		/// </summary>
		/// <value>The DataMiner Agent ID.</value>
		public int AgentId
		{
			get
			{
				return this.generalSettings.DmsElementId.AgentId;
			}
		}

		/// <summary>
		///     Gets or sets the alarm template assigned to this element.
		/// </summary>
		/// <value>The alarm template assigned to this element.</value>
		/// <exception cref="ArgumentException">
		///     The specified alarm template is not compatible with the protocol this element
		///     executes.
		/// </exception>
		public IDmsAlarmTemplate AlarmTemplate
		{
			get
			{
				return this.generalSettings.AlarmTemplate;
			}

			set
			{
				if (!InputValidator.IsCompatibleTemplate(value, this.Protocol))
				{
					throw new ArgumentException(
						"The specified alarm template is not compatible with the protocol this element executes.",
						"value");
				}

				this.generalSettings.AlarmTemplate = value;
			}
		}

		/// <summary>
		///     Gets or sets the available connections on the element.
		/// </summary>
		public IElementConnectionCollection Connections
		{
			get
			{
				return this.elementCommunicationConnections;
			}

			set
			{
				this.elementCommunicationConnections = value;
			}
		}

		/// <summary>
		///     Gets or sets the element description.
		/// </summary>
		/// <value>The element description.</value>
		public string Description
		{
			get
			{
				return this.GeneralSettings.Description;
			}

			set
			{
				this.GeneralSettings.Description = value;
			}
		}

		/// <summary>
		///     Gets the system-wide element ID of the element.
		/// </summary>
		/// <value>The system-wide element ID of the element.</value>
		public DmsElementId DmsElementId
		{
			get
			{
				return this.generalSettings.DmsElementId;
			}
		}

		/// <summary>
		///     Gets the DVE settings of this element.
		/// </summary>
		/// <value>The DVE settings of this element.</value>
		public IDveSettings DveSettings
		{
			get
			{
				return this.dveSettings;
			}
		}

		/// <summary>
		///     Gets the failover settings of this element.
		/// </summary>
		/// <value>The failover settings of this element.</value>
		public IFailoverSettings FailoverSettings
		{
			get
			{
				return this.failoverSettings;
			}
		}

		/// <summary>
		///     Gets the DataMiner Agent that hosts this element.
		/// </summary>
		/// <value>The DataMiner Agent that hosts this element.</value>
		public IDma Host
		{
			get
			{
				return this.generalSettings.Host;
			}
		}

		/// <summary>
		///     Gets the element ID.
		/// </summary>
		/// <value>The element ID.</value>
		public int Id
		{
			get
			{
				return this.generalSettings.DmsElementId.ElementId;
			}
		}

		/// <summary>
		///     Gets or sets the element name.
		/// </summary>
		/// <value>The element name.</value>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is empty or white space.</exception>
		/// <exception cref="ArgumentException">The value of a set operation exceeds 200 characters.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains a forbidden character.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains more than one '%' character.</exception>
		/// <exception cref="NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
		/// <remarks>
		///     <para>The following restrictions apply to element names:</para>
		///     <list type="bullet">
		///         <item>
		///             <para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para>
		///         </item>
		///         <item>
		///             <para>
		///                 Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|',
		///                 '°', ';'.
		///             </para>
		///         </item>
		///         <item>
		///             <para>The following characters may not occur more than once within a name: '%' (percentage).</para>
		///         </item>
		///     </list>
		/// </remarks>
		public string Name
		{
			get
			{
				return this.generalSettings.Name;
			}

			set
			{
				this.generalSettings.Name = InputValidator.ValidateName(value, "value");
			}
		}

		/// <summary>
		///     Gets the properties of this element.
		/// </summary>
		/// <value>The element properties.</value>
		public IPropertyCollection<IDmsElementProperty, IDmsElementPropertyDefinition> Properties
		{
			get
			{
				this.LoadOnDemand();

				// Parse properties using definitions from Dms.
				if (!this.propertiesLoaded)
				{
					this.ParseElementProperties();
				}

				IDictionary<string, IDmsElementProperty> copy =
					new Dictionary<string, IDmsElementProperty>(this.properties.Count);

				foreach (var kvp in this.properties)
				{
					copy.Add(kvp.Key, kvp.Value);
				}

				return new PropertyCollection<IDmsElementProperty, IDmsElementPropertyDefinition>(copy);
			}
		}

		/// <summary>
		///     Gets the protocol executed by this element.
		/// </summary>
		/// <value>The protocol executed by this element.</value>
		public IDmsProtocol Protocol
		{
			get
			{
				return this.generalSettings.Protocol;
			}
		}

		/// <summary>
		///     Gets the redundancy settings.
		/// </summary>
		/// <value>The redundancy settings.</value>
		public IRedundancySettings RedundancySettings
		{
			get
			{
				return this.redundancySettings;
			}
		}

		/// <summary>
		///     Gets the replication settings.
		/// </summary>
		/// <value>The replication settings.</value>
		public IReplicationSettings ReplicationSettings
		{
			get
			{
				return this.replicationSettings;
			}
		}

		/// <summary>
		/// Gets the spectrum component of this element.
		/// </summary>
		public IDmsSpectrumAnalyzer SpectrumAnalyzer
		{
			get
			{
				return new DmsSpectrumAnalyzer(this);
			}
		}

		/// <summary>
		///     Gets the element state.
		/// </summary>
		/// <value>The element state.</value>
		public ElementState State
		{
			get
			{
				return this.GeneralSettings.State;
			}

			internal set
			{
				this.GeneralSettings.State = value;
			}
		}

		/// <summary>
		///     Gets or sets the trend template that is assigned to this element.
		/// </summary>
		/// <value>The trend template that is assigned to this element.</value>
		/// <exception cref="ArgumentException">
		///     The specified trend template is not compatible with the protocol this element
		///     executes.
		/// </exception>
		public IDmsTrendTemplate TrendTemplate
		{
			get
			{
				return this.generalSettings.TrendTemplate;
			}

			set
			{
				if (!InputValidator.IsCompatibleTemplate(value, this.Protocol))
				{
					throw new ArgumentException(
						"The specified trend template is not compatible with the protocol this element executes.",
						"value");
				}

				this.generalSettings.TrendTemplate = value;
			}
		}

		/// <summary>
		///     Gets the type of the element.
		/// </summary>
		/// <value>The element type.</value>
		public string Type
		{
			get
			{
				return this.deviceSettings.Type;
			}
		}

		/// <summary>
		///     Gets the views the element is part of.
		/// </summary>
		/// <value>The views the element is part of.</value>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is an empty collection.</exception>
		public ISet<IDmsView> Views
		{
			get
			{
				if (!this.viewsLoaded)
				{
					this.LoadViews();
				}

				return this.views;
			}
		}

		/// <summary>
		///     Gets the general settings of the element.
		/// </summary>
		internal GeneralSettings GeneralSettings
		{
			get
			{
				return this.generalSettings;
			}
		}

		/// <summary>
		///     Deletes the element.
		/// </summary>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="NotSupportedException">The element is a DVE child or a derived element.</exception>
		public void Delete()
		{
			if (this.DveSettings.IsChild)
			{
				throw new NotSupportedException("Deleting a DVE child is not supported.");
			}

			if (this.RedundancySettings.IsDerived)
			{
				throw new NotSupportedException("Deleting a derived element is not supported.");
			}

			this.ChangeElementState(ElementState.Deleted);
		}

		/// <summary>
		///     Duplicates the element.
		/// </summary>
		/// <param name="newElementName">The name to assign to the duplicated element.</param>
		/// <param name="agent">The target DataMiner Agent where the duplicated element should be created.</param>
		/// <exception cref="ArgumentNullException"><paramref name="newElementName" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="newElementName" /> is invalid.</exception>
		/// <exception cref="NotSupportedException">The element is a DVE child or a derived element.</exception>
		/// <exception cref="AgentNotFoundException">The specified DataMiner Agent was not found in the DataMiner System.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="IncorrectDataException">Invalid data.</exception>
		/// <returns>The duplicated element.</returns>
		public IDmsElement Duplicate(string newElementName, IDma agent = null)
		{
			string trimmedName = InputValidator.ValidateName(newElementName, "newElementName");

			if (string.Equals(newElementName, this.Name, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(
					"The new element name cannot be equal to the name of the element being duplicated.",
					"newElementName");
			}

			if (this.DveSettings.IsChild)
			{
				throw new NotSupportedException("Duplicating a DVE child is not supported.");
			}

			if (this.RedundancySettings.IsDerived)
			{
				throw new NotSupportedException("Duplicating a derived element is not supported.");
			}

			try
			{
				var isCompatibilityIssueDetected = false;
				if (agent == null)
				{
					IDma targetDma = new Dma(this.Dms, this.DmsElementId.AgentId);
					isCompatibilityIssueDetected = targetDma.IsVersionHigher(Dma.SnmpV3AuthenticationChangeDMAVersion);
				}
				else
				{
					isCompatibilityIssueDetected = agent.IsVersionHigher(Dma.SnmpV3AuthenticationChangeDMAVersion);
				}

				AddElementMessage configuration = HelperClass.CreateAddElementMessage(
					this.Dms,
					this,
					isCompatibilityIssueDetected);
				configuration.DataMinerID = agent == null ? configuration.DataMinerID : agent.Id;
				configuration.ElementName = trimmedName;
				configuration.ElementID = -1;

				var response =
					(AddElementResponseMessage)this.Dms.Communication.SendSingleResponseMessage(configuration);
				int elementId = response.NewID;

				return new DmsElement(this.dms, new DmsElementId(configuration.DataMinerID, elementId));
			}
			catch (DataMinerCommunicationException e)
			{
				if (e.ErrorCode == -2147220787)
				{
					// 0x800402CD, SL_NO_CONNECTION.
					int agentId = agent == null ? this.DmsElementId.AgentId : agent.Id;
					throw new AgentNotFoundException(
						string.Format(
							CultureInfo.InvariantCulture,
							"No DataMiner agent with ID '{0}' was found in the DataMiner system.",
							agentId),
						e);
				}

				throw;
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147220959)
				{
					// 0x80040221, SL_INVALID_DATA, "Invalid data".
					if (agent == null)
					{
						string message = string.Format(
							CultureInfo.InvariantCulture,
							"Invalid data - element: '{0}', new element name: {1}",
							this.DmsElementId.Value,
							newElementName);
						throw new IncorrectDataException(message);
					}
					else
					{
						string message = string.Format(
							CultureInfo.InvariantCulture,
							"Invalid data - element: '{0}', new element name: {1}, agent: {2}",
							this.DmsElementId.Value,
							newElementName,
							agent.Id);
						throw new IncorrectDataException(message);
					}
				}

				throw;
			}
		}

		/// <summary>
		///     Determines whether this DataMiner element exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the DataMiner element exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return this.Dms.ElementExists(this.DmsElementId);
		}

		/// <summary>
		///     Gets the number of active alarms.
		/// </summary>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The number of active alarms.</returns>
		public int GetActiveAlarmCount()
		{
			var activeAlarmCountParameter = new DmsStandaloneParameter<int>(this, 65003);

			return activeAlarmCountParameter.GetValue();
		}

		/// <summary>
		///     Gets the number of critical alarms.
		/// </summary>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The number of critical alarms.</returns>
		public int GetActiveCriticalAlarmCount()
		{
			var criticalAlarmCountParameter = new DmsStandaloneParameter<int>(this, 65004);

			return criticalAlarmCountParameter.GetValue();
		}

		/// <summary>
		///     Gets the number of major alarms.
		/// </summary>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The number of major alarms.</returns>
		public int GetActiveMajorAlarmCount()
		{
			var majorAlarmCountParameter = new DmsStandaloneParameter<int>(this, 65005);

			return majorAlarmCountParameter.GetValue();
		}

		/// <summary>
		///     Gets the number of minor alarms.
		/// </summary>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The number of minor alarms.</returns>
		public int GetActiveMinorAlarmCount()
		{
			var minorAlarmCountParameter = new DmsStandaloneParameter<int>(this, 65006);

			return minorAlarmCountParameter.GetValue();
		}

		/// <summary>
		///     Gets the number of warnings.
		/// </summary>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The number of warnings.</returns>
		public int GetActiveWarningAlarmCount()
		{
			var warningCountParameter = new DmsStandaloneParameter<int>(this, 65007);
			return warningCountParameter.GetValue();
		}

		/// <summary>
		///     Gets the alarm level of the element.
		/// </summary>
		/// <returns>The element alarm level.</returns>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		public AlarmLevel GetAlarmLevel()
		{
			// Alarm state is found at 650008.
			var alarmStateParameter = new DmsStandaloneParameter<int>(this, 65008);
			int alarmState = alarmStateParameter.GetValue();

			return (AlarmLevel)alarmState;
		}

		/// <summary>
		///     Gets the specified standalone parameter.
		/// </summary>
		/// <typeparam name="T">The type of the parameter. Currently supported types: int?, double?, DateTime? and string.</typeparam>
		/// <param name="parameterId">The parameter ID.</param>
		/// <exception cref="ArgumentException"><paramref name="parameterId" /> is invalid.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <exception cref="NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
		/// <returns>The standalone parameter that corresponds with the specified ID.</returns>
		public IDmsStandaloneParameter<T> GetStandaloneParameter<T>(int parameterId)
		{
			if (parameterId < 1)
			{
				throw new ArgumentException("Invalid parameter ID.", "parameterId");
			}

			Type type = typeof(T);

			if (type != typeof(string) && type != typeof(int?) && type != typeof(double?) && type != typeof(DateTime?))
			{
				throw new NotSupportedException(
					"Only one of the following types is supported: string, int?, double? or DateTime?.");
			}

			HelperClass.CheckElementState(this);

			return new DmsStandaloneParameter<T>(this, parameterId);
		}

		/// <summary>
		///     Gets the specified table.
		/// </summary>
		/// <param name="tableId">The table parameter ID.</param>
		/// <exception cref="ArgumentException"><paramref name="tableId" /> is invalid.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		/// <returns>The table that corresponds with the specified ID.</returns>
		public IDmsTable GetTable(int tableId)
		{
			HelperClass.CheckElementState(this);

			if (tableId < 1)
			{
				throw new ArgumentException("Invalid table ID.", "tableId");
			}

			return new DmsTable(this, tableId);
		}

		/// <summary>
		///     Determines whether the element has been started up completely.
		/// </summary>
		/// <returns><c>true</c> if the element is started up; otherwise, <c>false</c>.</returns>
		/// <exception cref="ElementNotFoundException">The element was not found.</exception>
		/// <remarks>
		///     By default, the time-out value is set to 10s.
		///     This call should only be executed on elements that are in state Active.
		/// </remarks>
		public bool IsStartupComplete()
		{
			return this.NotifyElementStartupComplete();
		}

		/// <summary>
		///     Pauses the element.
		/// </summary>
		/// <exception cref="NotSupportedException">The element is a DVE child or derived element.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		public void Pause()
		{
			if (this.DveSettings.IsChild)
			{
				throw new NotSupportedException("Pausing a DVE child is not supported.");
			}

			if (this.RedundancySettings.IsDerived)
			{
				throw new NotSupportedException("Pausing a derived element is not supported.");
			}

			this.ChangeElementState(ElementState.Paused);
		}

		/// <summary>
		///     Restarts the element.
		/// </summary>
		/// <exception cref="NotSupportedException">The element is a DVE child or derived element.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		public void Restart()
		{
			if (this.DveSettings.IsChild)
			{
				throw new NotSupportedException("Pausing a DVE child is not supported.");
			}

			if (this.RedundancySettings.IsDerived)
			{
				throw new NotSupportedException("Pausing a derived element is not supported.");
			}

			this.ChangeElementState(ElementState.Restart);
		}

		/// <summary>
		///     Starts the element.
		/// </summary>
		/// <exception cref="NotSupportedException">The element is a DVE child or derived element.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		public void Start()
		{
			if (this.DveSettings.IsChild)
			{
				throw new NotSupportedException("Starting a DVE child is not supported.");
			}

			if (this.RedundancySettings.IsDerived)
			{
				throw new NotSupportedException("Starting a derived element is not supported.");
			}

			this.ChangeElementState(ElementState.Active);
		}

		/// <summary>
		///     Stops the element.
		/// </summary>
		/// <exception cref="NotSupportedException">The element is a DVE child or derived element.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		public void Stop()
		{
			if (this.DveSettings.IsChild)
			{
				throw new NotSupportedException("Stopping a DVE child is not supported.");
			}

			if (this.RedundancySettings.IsDerived)
			{
				throw new NotSupportedException("Stopping a derived element is not supported.");
			}

			this.ChangeElementState(ElementState.Stopped);
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat(CultureInfo.InvariantCulture, "Name: {0}{1}", this.Name, Environment.NewLine);
			sb.AppendFormat(
				CultureInfo.InvariantCulture,
				"agent ID/element ID: {0}{1}",
				this.DmsElementId.Value,
				Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Description: {0}{1}", this.Description, Environment.NewLine);
			sb.AppendFormat(
				CultureInfo.InvariantCulture,
				"Protocol name: {0}{1}",
				this.Protocol.Name,
				Environment.NewLine);
			sb.AppendFormat(
				CultureInfo.InvariantCulture,
				"Protocol version: {0}{1}",
				this.Protocol.Version,
				Environment.NewLine);
			sb.AppendFormat(
				CultureInfo.InvariantCulture,
				"Hosting agent ID: {0}{1}",
				this.Host.Id,
				Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		///     Updates the element.
		/// </summary>
		/// <exception cref="IncorrectDataException">Invalid data was set.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner system.</exception>
		public void Update()
		{
			if (this.generalSettings.State == ElementState.Deleted)
			{
				throw new ElementNotFoundException(
					string.Format(
						CultureInfo.InvariantCulture,
						"The element with name {0} was not found on this DataMiner agent.",
						this.Name));
			}

			try
			{
				// Use this flag to see if we actually have to perform an update on the element.
				if (this.UpdateRequired())
				{
					if (this.ViewsRequireUpdate() && this.views.Count == 0)
					{
						throw new IncorrectDataException(
							"Views must not be empty; an element must belong to at least one view.");
					}

					IDma targetDma = this.Host;
					bool isCompatibilityIssueDetected =
						targetDma.IsVersionHigher(Dma.SnmpV3AuthenticationChangeDMAVersion);

					AddElementMessage message = this.CreateUpdateMessage(isCompatibilityIssueDetected);

					this.Communication.SendSingleResponseMessage(message);
					this.ClearChangeList();

					// Performed the update, so tell the element to refresh.
					this.IsLoaded = false;
					this.viewsLoaded = false;
					this.propertiesLoaded = false;
				}
			}
			catch (DataMinerException e)
			{
				this.IsLoaded = false;
				this.viewsLoaded = false;
				this.propertiesLoaded = false;

				if (!this.Exists())
				{
					this.generalSettings.State = ElementState.Deleted;
					throw new ElementNotFoundException(this.DmsElementId, e);
				}

				throw;
			}
		}
		/// <summary>
		///     Loads all the data and properties found related to the element.
		/// </summary>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner system.</exception>
		internal override void Load()
		{
			try
			{
				this.IsLoaded = true;

				var message = new GetElementByIDMessage(
					this.generalSettings.DmsElementId.AgentId,
					this.generalSettings.DmsElementId.ElementId);
				var response = (ElementInfoEventMessage)this.Communication.SendSingleResponseMessage(message);

				this.elementCommunicationConnections = new ElementConnectionCollection(response);
				this.Parse(response);
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2146233088)
				{
					// 0x80131500, Element "[element ID]" is unavailable.
					throw new ElementNotFoundException(this.DmsElementId, e);
				}

				throw;
			}
		}

		/// <summary>
		///     Loads all the views where this element is included.
		/// </summary>
		internal void LoadViews()
		{
			var message = new GetViewsForElementMessage
			{
				DataMinerID = this.generalSettings.DmsElementId.AgentId,
				ElementID = this.generalSettings.DmsElementId.ElementId
			};

			var response = (GetViewsForElementResponse)this.Communication.SendSingleResponseMessage(message);

			this.views.Clear();
			this.registeredViewIds.Clear();
			foreach (DataMinerObjectInfo info in response.Views)
			{
				var view = new DmsView(this.dms, info.ID, info.Name);
				this.registeredViewIds.Add(info.ID);
				this.views.Add(view);
			}

			this.viewsLoaded = true;
		}

		/// <summary>
		///     Parses all of the element info.
		/// </summary>
		/// <param name="elementInfo">The element info message.</param>
		internal void Parse(ElementInfoEventMessage elementInfo)
		{
			this.IsLoaded = true;

			try
			{
				this.ParseElementInfo(elementInfo);
			}
			catch
			{
				this.IsLoaded = false;
				throw;
			}
		}

		/// <summary>
		///     Update the updataProperties HashSet with a change event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.updatedProperties.Add(e.PropertyName);
		}

		/// <summary>
		///     Changes the state of an element.
		/// </summary>
		/// <param name="newState">Specifies the state that should be assigned to the element.</param>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner system.</exception>
		private void ChangeElementState(ElementState newState)
		{
			if (this.generalSettings.State == ElementState.Deleted)
			{
				throw new ElementNotFoundException(this.DmsElementId);
			}

			try
			{
				var message = new SetElementStateMessage
				{
					BState = false,
					DataMinerID = this.generalSettings.DmsElementId.AgentId,
					ElementId = this.generalSettings.DmsElementId.ElementId,
					State = (Net.Messages.ElementState)newState
				};

				this.Communication.SendMessage(message);

				// Set the value in the element.
				this.generalSettings.State = newState == ElementState.Restart ? ElementState.Active : newState;
			}
			catch (DataMinerException e)
			{
				if (!this.Exists())
				{
					this.generalSettings.State = ElementState.Deleted;
					throw new ElementNotFoundException(this.DmsElementId, e);
				}

				throw;
			}
		}

		/// <summary>
		///     Clears all of the queued updated properties.
		/// </summary>
		private void ClearChangeList()
		{
			this.ChangedPropertyList.Clear();
			foreach (ElementSettings setting in this.settings)
			{
				setting.ClearUpdates();
			}

			this.updatedProperties.Clear();
			this.elementCommunicationConnections.Clear();

			// If the update passes, update the list of registered views for the element.
			this.registeredViewIds.Clear();
			this.registeredViewIds.AddRange(this.views.Select(v => v.Id));
		}

		/// <summary>
		///     Creates the AddElementMessage based on the current state of the object.
		/// </summary>
		/// <returns>The AddElementMessage.</returns>
		private AddElementMessage CreateUpdateMessage(bool isCompatibilityIssueDetected)
		{
			var message = new AddElementMessage
			{
				ElementID = this.DmsElementId.ElementId,
				DataMinerID = this.DmsElementId.AgentId
			};

			if (!this.dveSettings.IsChild)
			{
				message.ProtocolName = this.Protocol.Name;
				message.ProtocolVersion = this.Protocol.Version;
			}

			foreach (ElementSettings setting in this.settings)
			{
				if (setting.Updated)
				{
					setting.FillUpdate(message);
				}
			}

			// Update connection info if change has been detected.
			var connections = (ElementConnectionCollection)this.elementCommunicationConnections;

			if (connections.IsUpdateRequired())
			{
				ElementInfoEventMessage elemInfo;

				if (this.elementInfo == null)
				{
					var msg = new GetElementByIDMessage(
						this.DmsElementId.AgentId,
						this.DmsElementId.ElementId);
					elemInfo = (ElementInfoEventMessage)this.Communication.SendSingleResponseMessage(msg);
				}
				else
				{
					elemInfo = this.elementInfo;
				}

				var elementPortInfos = HelperClass.ObtainElementPortInfos(elemInfo);

				connections.UpdatePortInfo(elementPortInfos, isCompatibilityIssueDetected);
				message.Ports = elementPortInfos;
			}

			if (this.ViewsRequireUpdate())
			{
				message.ViewIDs = this.views.Select(v => v.Id).ToArray();
			}

			var newPropertyValues = new List<PropertyInfo>();
			foreach (string propertyName in this.updatedProperties)
			{
				DmsElementProperty property = this.properties[propertyName];
				newPropertyValues.Add(
					new PropertyInfo
					{
						DataType = "Element",
						Value = property.Value,
						Name = property.Definition.Name,
						AccessType = PropertyAccessType.ReadWrite
					});
			}

			if (newPropertyValues.Any())
			{
				message.Properties = newPropertyValues.ToArray();
			}

			return message;
		}

		/// <summary>
		///     Initializes the element.
		/// </summary>
		private void Initialize(ElementInfoEventMessage elementInfo)
		{
			this.elementInfo = elementInfo;

			this.Initialize();

			this.elementCommunicationConnections = new ElementConnectionCollection(this.elementInfo);
		}

		/// <summary>
		///     Initializes the element.
		/// </summary>
		private void Initialize()
		{
			this.generalSettings = new GeneralSettings(this);
			this.deviceSettings = new DeviceSettings(this);
			this.replicationSettings = new ReplicationSettings(this);
			this.advancedSettings = new AdvancedSettings(this);
			this.failoverSettings = new FailoverSettings(this);
			this.redundancySettings = new RedundancySettings(this);
			this.dveSettings = new DveSettings(this);

			this.settings = new List<ElementSettings>
			{
				this.generalSettings,
				this.deviceSettings,
				this.replicationSettings,
				this.advancedSettings,
				this.failoverSettings,
				this.redundancySettings,
				this.dveSettings
			};
		}

		/// <summary>
		///     Performs a Notify type 377 call.
		/// </summary>
		/// <exception cref="ElementNotFoundException">The element was not found.</exception>
		/// <returns><c>true</c> if startup is complete; otherwise, <c>false</c>.</returns>
		private bool NotifyElementStartupComplete()
		{
			var startupComplete = false;

			try
			{
				var message = new SetDataMinerInfoMessage
				{
					Uia1 = new UIA(
										  new[]
											  {
												  (uint)this.AgentId,
												  (uint)this.Id
											  }),
					Uia2 = null,
					What = 377
				};

				var response = (SetDataMinerInfoResponseMessage)this.Communication.SendSingleResponseMessage(message);

				if (response != null)
				{
					object result = response.RawData;

					if (result != null)
					{
						startupComplete = Convert.ToBoolean(result, CultureInfo.InvariantCulture);
					}
				}
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147220718)
				{
					// 0x80040312, Unknown destination DataMiner specified.
					throw new ElementNotFoundException(this.DmsElementId, e);
				}

				// -2147220916, 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
				// Note: When element is stopped it will throw 0x8004024 SL_NO_SUCH_ELEMENT (-2147220916).
				return false;
			}

			return startupComplete;
		}

		/// <summary>
		///     Parse an ElementPortInfo object in order to add IElementConnection objects to the ElementConnectionCollection.
		/// </summary>
		/// <param name="info">The ElementPortInfo object.</param>
		private void ParseConnection(ElementPortInfo info)
		{
			switch (info.ProtocolType)
			{
				case Net.Messages.ProtocolType.Virtual:
					var myVirtualConnection = new VirtualConnection(info);
					this.elementCommunicationConnections[info.PortID] = myVirtualConnection;
					break;

				case Net.Messages.ProtocolType.SnmpV1:
					var mySnmpV1Connection = new SnmpV1Connection(info);
					this.elementCommunicationConnections[info.PortID] = mySnmpV1Connection;
					break;

				case Net.Messages.ProtocolType.SnmpV2:
					var mySnmpv2Connection = new SnmpV2Connection(info);
					this.elementCommunicationConnections[info.PortID] = mySnmpv2Connection;
					break;

				case Net.Messages.ProtocolType.SnmpV3:
					var mySnmpV3Connection = new SnmpV3Connection(info);
					this.elementCommunicationConnections[info.PortID] = mySnmpV3Connection;
					break;

				case Net.Messages.ProtocolType.Http:
					var myHttpConnection = new HttpConnection(info);
					this.elementCommunicationConnections[info.PortID] = myHttpConnection;
					break;

				default:
					var myConnection = new RealConnection(info);
					this.elementCommunicationConnections[info.PortID] = myConnection;
					break;
			}
		}

		/// <summary>
		///     Parse an ElementInfoEventMessage object.
		/// </summary>
		/// <param name="elementInfo"></param>
		private void ParseConnections(ElementInfoEventMessage elementInfo)
		{
			// Keep this object in case properties are accessed.
			this.elementInfo = elementInfo;

			this.ParseConnection(elementInfo.MainPort);

			if (elementInfo.ExtraPorts != null)
			{
				foreach (ElementPortInfo info in elementInfo.ExtraPorts)
				{
					this.ParseConnection(info);
				}
			}
		}

		/// <summary>
		///     Parses the element info.
		/// </summary>
		/// <param name="elementInfo">The element info.</param>
		private void ParseElementInfo(ElementInfoEventMessage elementInfo)
		{
			// Keep this object in case properties are accessed.
			this.elementInfo = elementInfo;

			foreach (ElementSettings component in this.settings)
			{
				component.Load(elementInfo);
			}

			this.ParseConnections(elementInfo);
		}

		/// <summary>
		///     Parses the element properties.
		/// </summary>
		private void ParseElementProperties()
		{
			this.properties.Clear();
			foreach (IDmsElementPropertyDefinition definition in this.Dms.ElementPropertyDefinitions)
			{
				PropertyInfo info = null;
				if (this.elementInfo.Properties != null)
				{
					info = this.elementInfo.Properties.FirstOrDefault(
						p => p.Name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase));

					var duplicates = this.elementInfo.Properties.GroupBy(p => p.Name).Where(g => g.Count() > 1)
						.Select(g => g.Key).ToList();

					if (duplicates.Any())
					{
						string message = "Duplicate element properties detected. Element \""
										 + this.elementInfo.Name
										 + "\" ("
										 + this.elementInfo.DataMinerID
										 + "/"
										 + this.elementInfo.ElementID
										 + "), duplicate properties: "
										 + string.Join(", ", duplicates)
										 + ".";
						Logger.Log(message);
					}
				}

				string propertyValue = info != null ? info.Value : String.Empty;

				if (definition.IsReadOnly)
				{
					this.properties.Add(definition.Name, new DmsElementProperty(this, definition, propertyValue));
				}
				else
				{
					var property = new DmsWritableElementProperty(this, definition, propertyValue);
					this.properties.Add(definition.Name, property);

					property.PropertyChanged += this.PropertyChanged;
				}
			}

			this.propertiesLoaded = true;
		}

		/// <summary>
		///     Specifies if the element requires an update or not.
		/// </summary>
		/// <returns><c>true</c> if an update is required; otherwise, <c>false</c>.</returns>
		private bool UpdateRequired()
		{
			bool settingsChanged = this.settings.Any(s => s.Updated)
								   || this.updatedProperties.Count != 0
								   || this.ChangedPropertyList.Count != 0
								   || this.ViewsRequireUpdate();
			bool connectionInfoChanged = this.elementCommunicationConnections.IsUpdateRequired();

			return settingsChanged || connectionInfoChanged;
		}

		/// <summary>
		///     Specifies if the views of the element have been updated.
		/// </summary>
		/// <returns><c>true</c> if the views have been updated; otherwise, <c>false</c>.</returns>
		private bool ViewsRequireUpdate()
		{
			if (this.views.Count != this.registeredViewIds.Count)
			{
				return true;
			}

			var ids = this.views.Select(t => t.Id).ToList();

			var distinctOne = ids.Except(this.registeredViewIds);
			var distinctTwo = this.registeredViewIds.Except(ids);

			return distinctOne.Any() || distinctTwo.Any();
		}
	}
}