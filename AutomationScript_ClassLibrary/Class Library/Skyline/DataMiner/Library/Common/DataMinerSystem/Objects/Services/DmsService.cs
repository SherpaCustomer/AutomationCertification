namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;
	using System.Text;

	using Net.Exceptions;
	using Net.Messages;
	using Net.Messages.Advanced;

	using Properties;

	/// <summary>
	/// Represents a DataMiner service.
	/// </summary>
	internal class DmsService : DmsObject, IDmsService
	{
		/// <summary>
		/// Contains the properties for the service.
		/// </summary>
		private readonly IDictionary<string, DmsServiceProperty> properties = new Dictionary<string, DmsServiceProperty>();

		/// <summary>
		/// A set of all updated properties.
		/// </summary>
		private readonly HashSet<string> updatedProperties = new HashSet<string>();

		/// <summary>
		/// Array of views where the service is contained in.
		/// </summary>
		private readonly ISet<IDmsView> views = new DmsViewSet();

		/// <summary>
		/// This list will be used to keep track of which views were assigned / removed during the life time of the service.
		/// </summary>
		private readonly List<int> registeredViewIds = new List<int>();

		/// <summary>
		///  Keep this message in case we need to parse the service properties when the user wants to use these.
		/// </summary>
		private ServiceInfoEventMessage serviceInfo;

		/// <summary>
		/// Specifies whether the properties of the serviceInfo object have been parsed into dedicated objects.
		/// </summary>
		private bool propertiesLoaded;

		/// <summary>
		/// The general settings.
		/// </summary>
		private GeneralServiceSettings generalSettings;

		/// <summary>
		/// The advanced settings.
		/// </summary>
		private AdvancedServiceSettings advancedSettings;

		/// <summary>
		/// The parameter settings.
		/// </summary>
		private ServiceParamsSettings parameterSettings;

		/// <summary>
		/// The replication settings.
		/// </summary>
		private ReplicationServiceSettings replicationSettings;

		/// <summary>
		/// The element components.
		/// </summary>
		private IList<ServiceSettings> settings;

		/// <summary>
		/// Specifies whether the views have been loaded.
		/// </summary>
		private bool viewsLoaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsService"/> class.
		/// </summary>
		/// <param name="dms">Object implementing <see cref="IDms"/> interface.</param>
		/// <param name="dmsServiceId">The system-wide service ID.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		internal DmsService(IDms dms, DmsServiceId dmsServiceId) :
			base(dms)
		{
			Initialize();
			generalSettings.DmsServiceId = dmsServiceId;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsService"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="serviceInfo">The service information.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serviceInfo"/> is <see langword="null"/>.</exception>
		internal DmsService(IDms dms, ServiceInfoEventMessage serviceInfo)
			: base(dms)
		{
			if (serviceInfo == null)
			{
				throw new ArgumentNullException("serviceInfo");
			}

			Initialize(serviceInfo);
			Parse(serviceInfo);
		}

		/// <summary>
		/// Gets the DataMiner Agent ID.
		/// </summary>
		/// <value>The DataMiner Agent ID.</value>
		public int AgentId
		{
			get
			{
				return DmsServiceId.AgentId;
			}
		}

		/// <summary>
		/// Gets or sets the service description.
		/// </summary>
		/// <value>The service description.</value>
		public string Description
		{
			get
			{
				return GeneralSettings.Description;
			}

			set
			{
				GeneralSettings.Description = value;
			}
		}

		/// <summary>
		/// Gets the system-wide service ID of the service.
		/// </summary>
		/// <value>The system-wide service ID of the service.</value>
		public DmsServiceId DmsServiceId
		{
			get
			{
				return generalSettings.DmsServiceId;
			}
		}

		/// <summary>
		/// Gets the DataMiner Agent that hosts this service.
		/// </summary>
		/// <value>The DataMiner Agent that hosts this service.</value>
		public IDma Host
		{
			get
			{
				return generalSettings.Host;
			}
		}

		/// <summary>
		/// Gets the service ID.
		/// </summary>
		/// <value>The service ID.</value>
		public int Id
		{
			get
			{
				return generalSettings.DmsServiceId.ServiceId;
			}
		}

		/// <summary>
		/// Gets or sets the service name.
		/// </summary>
		/// <value>The service name.</value>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is empty or white space.</exception>
		/// <exception cref="ArgumentException">The value of a set operation exceeds 200 characters.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains a forbidden character.</exception>
		/// <exception cref="ArgumentException">The value of a set operation contains more than one '%' character.</exception>
		/// <exception cref="NotSupportedException">A set operation is not supported on a generated service.</exception>
		/// <remarks>
		/// <para>The following restrictions apply to element names:</para>
		/// <list type="bullet">
		///		<item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
		///		<item><para>Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</para></item>
		///		<item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
		/// </list>
		/// </remarks>
		public string Name
		{
			get
			{
				return generalSettings.Name;
			}

			set
			{
				generalSettings.Name = InputValidator.ValidateName(value, "value");
			}
		}

		/// <summary>
		/// Gets the properties of this service.
		/// </summary>
		/// <value>The service properties.</value>
		public IPropertyCollection<IDmsServiceProperty, IDmsServicePropertyDefinition> Properties
		{
			get
			{
				LoadOnDemand();

				// Parse properties using definitions from Dms.
				if (!propertiesLoaded)
				{
					ParseServiceProperties();
				}

				IDictionary<string, IDmsServiceProperty> copy = new Dictionary<string, IDmsServiceProperty>(properties.Count);

				foreach (KeyValuePair<string, DmsServiceProperty> kvp in properties)
				{
					copy.Add(kvp.Key, kvp.Value);
				}

				return new PropertyCollection<IDmsServiceProperty, IDmsServicePropertyDefinition>(copy);
			}
		}

		/// <summary>
		/// Gets the replication settings.
		/// </summary>
		/// <value>The replication settings.</value>
		public IReplicationServiceSettings ReplicationSettings
		{
			get
			{
				return this.replicationSettings;
			}
		}

		/// <summary>
		/// Gets the views the service is part of.
		/// </summary>
		/// <value>The views the service is part of.</value>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword = "null" />.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is an empty collection.</exception>
		public ISet<IDmsView> Views
		{
			get
			{
				if (!viewsLoaded)
				{
					LoadViews();
				}

				return views;
			}
		}

		/// <summary>
		/// Gets the advanced settings of the service.
		/// </summary>
		public IAdvancedServiceSettings AdvancedSettings
		{
			get
			{
				return advancedSettings;
			}
		}

		/// <summary>
		/// Gets the Parameter settings of the service.
		/// </summary>
		public IServiceParamsSettings ParameterSettings
		{
			get
			{
				return parameterSettings;
			}
		}

		/// <summary>
		/// Gets the general settings of the service.
		/// </summary>
		internal GeneralServiceSettings GeneralSettings
		{
			get
			{
				return generalSettings;
			}
		}

		/// <summary>
		/// Gets the service state.
		/// </summary>
		/// <returns>The service state.</returns>
		/// <exception cref="ServiceNotFoundException">The service was not found in the DataMiner System.</exception>
		/// <exception cref="NotSupportedException">Alarm state messages are not supported on service templates.</exception>
		public IServiceState GetState()
		{
			if (advancedSettings.IsTemplate)
			{
				throw new NotSupportedException("Alarm state messages are not supported on service templates.");
			}

			GetServiceStateMessage getServiceStateMsg = new GetServiceStateMessage
			{
				DataMinerID = AgentId,
				HostingDataMinerID = Host.Id,
				ServiceID = Id
			};

			var response = Dms.Communication.SendSingleResponseMessage(getServiceStateMsg) as ServiceStateEventMessage;

			if (response == null)
			{
				throw new ServiceNotFoundException(DmsServiceId);
			}

			return new ServiceState(response);
		}

		/// <summary>
		/// Deletes the service.
		/// </summary>
		/// <exception cref="ServiceNotFoundException">The service was not found in the DataMiner System.</exception>
		public void Delete()
		{
			try
			{
				var deleteServiceMsg = new SetDataMinerInfoMessage
				{
					DataMinerID = AgentId,
					HostingDataMinerID = -1,
					What = 74 /* Delete Service */,
					Uia1 = new UIA(new[] { (uint)AgentId, (uint)Id })
				};

				Dms.Communication.SendSingleResponseMessage(deleteServiceMsg);
			}
			catch (DataMinerException e)
			{
				if (!this.Exists())
				{
					throw new ServiceNotFoundException(this.DmsServiceId, e);
				}

				throw;
			}
		}

		/// <summary>
		///     Duplicates the service.
		/// </summary>
		/// <param name="newServiceName">The name to assign to the duplicated service.</param>
		/// <param name="agent">The target DataMiner Agent where the duplicated service should be created.</param>
		/// <exception cref="ArgumentNullException"><paramref name="newServiceName" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="newServiceName" /> is invalid.</exception>
		/// <exception cref="NotSupportedException">The service is a service template or generated by a service template.</exception>
		/// <exception cref="AgentNotFoundException">The specified DataMiner Agent was not found in the DataMiner System.</exception>
		/// <exception cref="ServiceNotFoundException">The service was not found in the DataMiner System.</exception>
		/// <exception cref="IncorrectDataException">Invalid data.</exception>
		/// <returns>The duplicated service.</returns>
		public IDmsService Duplicate(string newServiceName, IDma agent = null)
		{
			if (this.advancedSettings.IsTemplate)
			{
				throw new NotSupportedException("Duplicating a service Template is not supported.");
			}

			if (this.advancedSettings.ParentTemplate != null)
			{
				throw new NotSupportedException("Duplicating a generated service from a service template is not supported.");
			}

			string trimmedName = InputValidator.ValidateName(newServiceName, "newServiceName");

			if (string.Equals(newServiceName, this.Name, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(
					"The new service name cannot be equal to the name of the service being duplicated.",
					"newServiceName");
			}

			try
			{
				var config = GetConfig();
				config.Name = trimmedName;
				if (agent != null)
				{
					var newServiceId = agent.CreateService(config);
					return new DmsService(dms, newServiceId);
				}
				else
				{
					var newServiceId = dms.GetAgent(DmsServiceId.AgentId).CreateService(config);
					return new DmsService(dms, newServiceId);
				}
			}
			catch (DataMinerCommunicationException e)
			{
				if (e.ErrorCode == -2147220787)
				{
					// 0x800402CD, SL_NO_CONNECTION.
					int agentId = agent == null ? this.DmsServiceId.AgentId : agent.Id;
					throw new AgentNotFoundException(
						string.Format(
							CultureInfo.InvariantCulture,
							"No DataMiner agent with ID '{0}' was found in the DataMiner system.",
							agentId),
						e);
				}

				throw;
			}
		}

		/// <summary>
		/// Determines whether this DataMiner service exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if the DataMiner service exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return Dms.ServiceExists(DmsServiceId);
		}

		/// <summary>
		/// Updates the service.
		/// </summary>
		/// <exception cref="IncorrectDataException">Invalid data was set.</exception>
		/// <exception cref="ServiceNotFoundException">The service was not found in the DataMiner system.</exception>
		public void Update()
		{
			try
			{
				// Use this flag to see if we actually have to perform an update on the service.
				if (UpdateRequired())
				{
					if (ViewsRequireUpdate() && views.Count == 0)
					{
						throw new IncorrectDataException("Views must not be empty; a service must belong to at least one view.");
					}

					AddServiceMessage message = CreateUpdateMessage();

					Communication.SendSingleResponseMessage(message);
					ClearChangeList();

					// Performed the update, so tell the service to refresh.
					IsLoaded = false;
					viewsLoaded = false;
					propertiesLoaded = false;
				}
			}
			catch (DataMinerException)
			{
				IsLoaded = false;
				viewsLoaded = false;
				propertiesLoaded = false;

				throw;
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat(CultureInfo.InvariantCulture, "Name: {0}{1}", Name, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "agent ID/service ID: {0}{1}", DmsServiceId.Value, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Description: {0}{1}", Description, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Hosting agent ID: {0}{1}", Host.Id, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Loads all the data and properties found related to the service.
		/// </summary>
		/// <exception cref="ServiceNotFoundException">The service was not found in the DataMiner system.</exception>
		internal override void Load()
		{
			try
			{
				IsLoaded = true;

				GetServiceByIDMessage message = new GetServiceByIDMessage(generalSettings.DmsServiceId.AgentId, generalSettings.DmsServiceId.ServiceId);
				ServiceInfoEventMessage response = Communication.SendSingleResponseMessage(message) as ServiceInfoEventMessage;

				Parse(response);
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -1073478960)
				{
					throw new ServiceNotFoundException(DmsServiceId, e);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Loads all the views where this service is included.
		/// </summary>
		internal void LoadViews()
		{
			GetViewsForElementMessage message = new GetViewsForElementMessage
			{
				DataMinerID = generalSettings.DmsServiceId.AgentId,
				ElementID = generalSettings.DmsServiceId.ServiceId
			};

			GetViewsForElementResponse response = Communication.SendSingleResponseMessage(message) as GetViewsForElementResponse;

			views.Clear();
			registeredViewIds.Clear();
			foreach (DataMinerObjectInfo info in response.Views)
			{
				DmsView view = new DmsView(dms, info.ID, info.Name);
				registeredViewIds.Add(info.ID);
				views.Add(view);
			}

			viewsLoaded = true;
		}

		/// <summary>
		/// Update the updataProperties HashSet with a change event.
		/// </summary>
		/// <param name="sender">The sender of the update.</param>
		/// <param name="e">The arguments to change the property.</param>
		internal void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			updatedProperties.Add(e.PropertyName);
		}

		/// <summary>
		/// Parses all of the service info.
		/// </summary>
		/// <param name="serviceInfo">The service info message.</param>
		internal void Parse(ServiceInfoEventMessage serviceInfo)
		{
			IsLoaded = true;

			try
			{
				ParseServiceInfo(serviceInfo);
			}
			catch
			{
				IsLoaded = false;
				throw;
			}
		}

		/// <summary>
		/// Creates the AddServiceMessage based on the current state of the object.
		/// </summary>
		/// <returns>The AddServiceMessage.</returns>
		internal AddServiceMessage CreateUpdateMessage()
		{
			AddServiceMessage message = new AddServiceMessage
			{
				DataMinerID = DmsServiceId.AgentId,
				HostingDataMinerID = Host.Id,
				Service = (ServiceInfoEventMessage)serviceInfo.Clone()
			};

			foreach (ServiceSettings setting in settings)
			{
				if (setting.Updated)
				{
					setting.FillUpdate(message);
				}
			}

			message.ViewIDs = views.Select(v => v.Id).ToArray();

			List<PropertyInfo> newPropertyValues = new List<PropertyInfo>();
			foreach (var property in properties.Values)
			{
				newPropertyValues.Add(new PropertyInfo
				{
					DataType = "Service",
					Value = property.Value,
					Name = property.Definition.Name,
					AccessType = PropertyAccessType.ReadWrite
				});
			}

			if (newPropertyValues.Count > 0)
			{
				message.Service.Properties = newPropertyValues.ToArray();
			}

			return message;
		}

		private ServiceConfiguration GetConfig()
		{
			var config = new ServiceConfiguration(dms, Name);
			config.Description = Description;
			if (AdvancedSettings.ServiceElement != null)
			{
				config.AdvancedSettings.IgnoreTimeouts = advancedSettings.IgnoreTimeouts;
				config.AdvancedSettings.Protocol = AdvancedSettings.ServiceElementProtocol;
				config.AdvancedSettings.AlarmTemplate = advancedSettings.ServiceElementAlarmTemplate;
				config.AdvancedSettings.TrendTemplate = advancedSettings.ServiceElementTrendTemplate;
			}

			foreach (var includedElement in ParameterSettings.IncludedParameters)
			{
				if (includedElement.IsService)
				{
					config.AddService(new DmsServiceId(includedElement.DataMinerID, includedElement.ElementID));
				}
				else
				{
					List<ElementParamFilterConfiguration> lParams = new List<ElementParamFilterConfiguration>();
					foreach (var param in includedElement.ParameterFilters)
					{
						lParams.Add(new ElementParamFilterConfiguration(param.ParameterID, param.Filter, param.IsIncluded, param.FilterType));
					}

					config.AddElement(new DmsElementId(includedElement.DataMinerID, includedElement.ElementID), lParams);
				}
			}

			foreach (var property in Properties)
			{
				if (!string.IsNullOrWhiteSpace(property.Value))
				{
					config.Properties[property.Definition.Name].Value = property.Value;
				}
			}

			if (Views.Any())
			{
				foreach (var view in Views)
				{
					config.Views.Add(view);
				}
			}
			else
			{
				config.Views.Add(new DmsView(dms, -1));
			}

			return config;
		}

		/// <summary>
		/// Clears all of the queued updated properties.
		/// </summary>
		private void ClearChangeList()
		{
			ChangedPropertyList.Clear();
			foreach (ServiceSettings setting in settings)
			{
				setting.ClearUpdates();
			}

			updatedProperties.Clear();

			// If the update passes, update the list of registered views for the element.
			registeredViewIds.Clear();
			registeredViewIds.AddRange(views.Select(v => v.Id));
		}

		/// <summary>
		/// Initializes the service.
		/// </summary>
		/// <param name="serviceInfo">The service info event returned by SLNet.</param>
		private void Initialize(ServiceInfoEventMessage serviceInfo)
		{
			this.serviceInfo = serviceInfo;

			this.generalSettings = new GeneralServiceSettings(this);
			this.advancedSettings = new AdvancedServiceSettings(this);
			this.parameterSettings = new ServiceParamsSettings(this);
			this.replicationSettings = new ReplicationServiceSettings(this);

			this.settings = new List<ServiceSettings>
			{
				this.generalSettings,
				this.advancedSettings,
				this.parameterSettings,
				this.replicationSettings
			};
		}

		/// <summary>
		/// Initializes the service.
		/// </summary>
		private void Initialize()
		{
			this.generalSettings = new GeneralServiceSettings(this);
			this.advancedSettings = new AdvancedServiceSettings(this);
			this.parameterSettings = new ServiceParamsSettings(this);
			this.replicationSettings = new ReplicationServiceSettings(this);

			settings = new List<ServiceSettings> { generalSettings, advancedSettings, parameterSettings, replicationSettings };
		}

		/// <summary>
		/// Parses the service info.
		/// </summary>
		/// <param name="serviceInfo">The service info.</param>
		private void ParseServiceInfo(ServiceInfoEventMessage serviceInfo)
		{
			// Keep this object in case properties are accessed.
			this.serviceInfo = serviceInfo;

			foreach (ServiceSettings component in settings)
			{
				component.Load(serviceInfo);
			}
		}

		/// <summary>
		/// Parses the service properties.
		/// </summary>
		private void ParseServiceProperties()
		{
			properties.Clear();
			foreach (IDmsServicePropertyDefinition definition in Dms.ServicePropertyDefinitions)
			{
				PropertyInfo info = null;
				if (serviceInfo.Properties != null)
				{
					info = serviceInfo.Properties.FirstOrDefault(p => p.Name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase));

					List<String> duplicates = serviceInfo.Properties.GroupBy(p => p.Name)
							.Where(g => g.Count() > 1)
							.Select(g => g.Key)
							.ToList();

					if (duplicates.Count > 0)
					{
						string message = "Duplicate service properties detected. Service \"" + serviceInfo.Name + "\" (" + serviceInfo.DataMinerID + "/" + serviceInfo.ElementID + "), duplicate properties: " + String.Join(", ", duplicates) + ".";
						Logger.Log(message);
					}
				}

				string propertyValue = info != null ? info.Value : String.Empty;

				if (definition.IsReadOnly)
				{
					properties.Add(definition.Name, new DmsServiceProperty(this, definition, propertyValue));
				}
				else
				{
					var property = new DmsWritableServiceProperty(this, definition, propertyValue);
					properties.Add(definition.Name, property);

					property.PropertyChanged += this.PropertyChanged;
				}
			}

			propertiesLoaded = true;
		}

		/// <summary>
		/// Specifies if the service requires an update or not.
		/// </summary>
		/// <returns><c>true</c> if an update is required; otherwise, <c>false</c>.</returns>
		private bool UpdateRequired()
		{
			bool settingsChanged = settings.Any(s => s.Updated) || updatedProperties.Count != 0 || ChangedPropertyList.Count != 0 || ViewsRequireUpdate();

			return settingsChanged;
		}

		/// <summary>
		/// Specifies if the views of the service have been updated.
		/// </summary>
		/// <returns><c>true</c> if the views have been updated; otherwise, <c>false</c>.</returns>
		private bool ViewsRequireUpdate()
		{
			if (views.Count != registeredViewIds.Count)
			{
				return true;
			}
			else
			{
				List<int> ids = views.Select(t => t.Id).ToList();

				IEnumerable<int> distinctOne = ids.Except(registeredViewIds);
				IEnumerable<int> distinctTwo = registeredViewIds.Except(ids);

				return distinctOne.Any() || distinctTwo.Any();
			}
		}
	}
}