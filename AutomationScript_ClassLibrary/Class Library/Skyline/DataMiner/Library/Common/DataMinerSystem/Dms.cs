namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	using Skyline.DataMiner.Library.Common.Properties;
	using Skyline.DataMiner.Library.Common.Templates;
	using Skyline.DataMiner.Net.Exceptions;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	/// <summary>
	/// Represents a DataMiner System.
	/// </summary>
	internal class Dms : IDms
	{
		/// <summary>
		/// Dictionary for all of the element properties found on the DataMiner System.
		/// </summary>
		private readonly IDictionary<string, IDmsElementPropertyDefinition> elementProperties = new Dictionary<string, IDmsElementPropertyDefinition>();

		/// <summary>
		/// Dictionary for all of the service properties found on the DataMiner System.
		/// </summary>
		private readonly IDictionary<string, IDmsServicePropertyDefinition> serviceProperties = new Dictionary<string, IDmsServicePropertyDefinition>();

		/// <summary>
		/// Dictionary for all of the view properties found on the DataMiner System.
		/// </summary>
		private readonly IDictionary<string, IDmsViewPropertyDefinition> viewProperties = new Dictionary<string, IDmsViewPropertyDefinition>();
		
		/// <summary>
		/// Specifies is the DataMiner System object has been loaded.
		/// </summary>
		private bool isLoaded;

		/// <summary>
		/// Cached element information message.
		/// </summary>
		private ElementInfoEventMessage cachedElementInfoMessage;

		/// <summary>
		/// Cached service information message.
		/// </summary>
		private ServiceInfoEventMessage cachedServiceInfoMessage;

		/// <summary>
		/// Cached view information message.
		/// </summary>
		private ViewInfoEventMessage cachedViewInfoMessage;

		/// <summary>
		/// Cached alarm template information message.
		/// </summary>
		private AlarmTemplateEventMessage cachedAlarmTemplateMessage;

		/// <summary>
		/// Cached trend template information message.
		/// </summary>
		private GetTrendingTemplateInfoResponseMessage cachedTrendTemplateMessage;

		/// <summary>
		/// Cached DataMiner information message.
		/// </summary>
		private GetDataMinerInfoResponseMessage cachedDataMinerAgentMessage;

		/// <summary>
		/// Cached protocol information message.
		/// </summary>
		private GetProtocolInfoResponseMessage cachedProtocolMessage;

		/// <summary>
		/// Cached protocol requested version.
		/// </summary>
		private string cachedProtocolRequestedVersion;

		/// <summary>
		/// The object used for DMS communication.
		/// </summary>
		private ICommunication communication;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dms"/> class.
		/// </summary>
		/// <param name="communication">An object implementing the ICommunication interface.</param>
		/// <exception cref="ArgumentNullException"><paramref name="communication"/> is <see langword="null"/>.</exception>
		internal Dms(ICommunication communication)
		{
			if (communication == null)
			{
				throw new ArgumentNullException("communication");
			}

			this.communication = communication;
		}

		/// <summary>
		/// Gets the communication interface.
		/// </summary>
		/// <value>The communication interface.</value>
		public ICommunication Communication
		{
			get
			{
				return communication;
			}
		}

		/// <summary>
		/// Gets the element property definitions available in the DataMiner System.
		/// </summary>
		/// <value>The element property definitions available in the DataMiner System.</value>
		public IPropertyDefinitionCollection<IDmsElementPropertyDefinition> ElementPropertyDefinitions
		{
			get
			{
				if (!isLoaded)
				{
					LoadDmsProperties();
				}

				return new PropertyDefinitionCollection<IDmsElementPropertyDefinition>(elementProperties);
			}
		}
		
		/// <summary>
		/// Gets the view property definitions available in the DataMiner System.
		/// </summary>
		/// <value>The view property definitions available in the DataMiner System.</value>
		public IPropertyDefinitionCollection<IDmsViewPropertyDefinition> ViewPropertyDefinitions
		{
			get
			{
				if (!isLoaded)
				{
					LoadDmsProperties();
				}

				return new PropertyDefinitionCollection<IDmsViewPropertyDefinition>(viewProperties);
			}
		}

		/// <summary>
		///     Gets the service property definitions available in the DataMiner System.
		/// </summary>
		/// <value>The service property definitions available in the DataMiner System.</value>
		public IPropertyDefinitionCollection<IDmsServicePropertyDefinition> ServicePropertyDefinitions
		{
			get
			{
				if (!this.isLoaded)
				{
					this.LoadDmsProperties();
				}

				return new PropertyDefinitionCollection<IDmsServicePropertyDefinition>(this.serviceProperties);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the object is loaded.
		/// </summary>
		/// <value><c>true</c> if the object is loaded; otherwise, <c>false</c>.</value>
		internal bool IsLoaded
		{
			get
			{
				return isLoaded;
			}
		}

		/// <summary>
		/// Gets the cached alarm template message.
		/// </summary>
		internal AlarmTemplateEventMessage CachedAlarmTemplate
		{
			get
			{
				return cachedAlarmTemplateMessage;
			}
		}

		/// <summary>
		/// Gets the cached trend template message.
		/// </summary>
		internal GetTrendingTemplateInfoResponseMessage CachedTrendTemplate
		{
			get
			{
				return cachedTrendTemplateMessage;
			}
		}

		/// <summary>
		/// Gets the cached element info message.
		/// </summary>
		internal ElementInfoEventMessage CachedElementInfo
		{
			get
			{
				return cachedElementInfoMessage;
			}
		}

		/// <summary>
		/// Determines whether a DataMiner Agent with the specified ID is present in the DataMiner System.
		/// </summary>
		/// <param name="agentId">The DataMiner Agent ID.</param>
		/// <exception cref="ArgumentException"><paramref name="agentId"/> is invalid.</exception>
		/// <returns><c>true</c> if the DataMiner Agent ID is valid; otherwise, <c>false</c>.</returns>
		public bool AgentExists(int agentId)
		{
			if (agentId < 1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "DataMiner agent ID: {0} is invalid", agentId), "agentId");
			}

			try
			{
				GetDataMinerByIDMessage message = new GetDataMinerByIDMessage(agentId);
				cachedDataMinerAgentMessage = communication.SendSingleResponseMessage(message) as GetDataMinerInfoResponseMessage;

				return cachedDataMinerAgentMessage != null;
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2146233088)
				{
					// 0x80131500, No agent available with ID.
					return false;
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Creates a property with the specified configuration.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="type">Specifies the property type.</param>
		/// <param name="isFilterEnabled">Specifies if the filter is enabled.</param>
		/// <param name="isReadOnly">Specifies if the property is read-only.</param>
		/// <param name="isVisibleInSurveyor">Specifies if the property is visible in the surveyor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="type" /> is invalid.</exception>
		/// <returns>The ID of the created property. If it fails to create property -1 is returned.</returns>
		public int CreateProperty(string name, PropertyType type, bool isFilterEnabled, bool isReadOnly, bool isVisibleInSurveyor)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("The name of the property is the empty string (\"\") or white space.", "name");
			}

			if (type != PropertyType.Element && type != PropertyType.View)
			{
				throw new ArgumentException("Invalid property type specified.", "type");
			}

			try
			{
				if (!PropertyExists(name, type))
				{
					AddPropertyConfigMessage message = new AddPropertyConfigMessage
					{
						Config = new PropertyConfig
						{
							ID = -1,
							IsFilterEnabled = isFilterEnabled,
							IsReadOnly = isReadOnly,
							IsVisibleInSurveyor = isVisibleInSurveyor,
							Name = name,
							Type = Enum.GetName(typeof(PropertyType), type)
						}
					};

					UpdatePropertyConfigResponse response = (UpdatePropertyConfigResponse)communication.SendSingleResponseMessage(message);
					return response.ID;
				}
				else
				{
					return -1;
				}
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147220959)
				{
					// 0x80040221, SL_INVALID_DATA, "Invalid data".
					string message = String.Format(CultureInfo.InvariantCulture, "Invalid data: '{0}', '{1}', '{2}', '{3}', '{4}'", name, type, isFilterEnabled, isReadOnly, isVisibleInSurveyor);

					throw new IncorrectDataException(message);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Creates a view with the specified configuration.
		/// </summary>
		/// <param name="configuration">The view to be created.</param>
		/// <exception cref="ArgumentNullException"><paramref name="configuration"/> is <see langword="null"/>.</exception>
		/// <exception cref="IncorrectDataException"><paramref name="configuration"/> is invalid.</exception>
		/// <returns>The ID of the created view.</returns>
		public int CreateView(ViewConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}

			try
			{
				SetDataMinerInfoMessage message = HelperClass.CreateAddOrUpdateMessage(configuration);
				SetDataMinerInfoResponseMessage response = (SetDataMinerInfoResponseMessage)communication.SendSingleResponseMessage(message);

				return response.iRet;
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147220959)
				{
					// 0x80040221, SL_INVALID_DATA, "Invalid data".
					string message = String.Format(CultureInfo.InvariantCulture, "Invalid data: '{0}'", configuration);

					throw new IncorrectDataException(message);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Deletes the property with the specified ID.
		/// </summary>
		/// <param name="propertyId">The ID of the property.</param>
		/// <exception cref="ArgumentException"><paramref name="propertyId"/> is invalid.</exception>
		public void DeleteProperty(int propertyId)
		{
			if (propertyId < 1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid property ID: {0}", propertyId), "propertyId");
			}

			try
			{
				communication.SendSingleResponseMessage(new DeletePropertyConfigMessage
				{
					ID = propertyId,
					OwnerType = PropertyOwningObjectType.Undefined
				});
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2147220959)
				{
					// 0x80040221, SL_INVALID_DATA, "Invalid data".
					string message = String.Format(CultureInfo.InvariantCulture, "Invalid data: '{0}'", propertyId);

					throw new IncorrectDataException(message);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Determines whether an element with the specified Agent ID/element ID exists in the DataMiner System.
		/// </summary>
		/// <param name="dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
		/// <returns><c>true</c> if the element exists; otherwise, <c>false</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="dmsElementId"/> is invalid.</exception>
		public bool ElementExists(DmsElementId dmsElementId)
		{
			int dmaId = dmsElementId.AgentId;
			int elementId = dmsElementId.ElementId;

			if (dmaId < 1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid DataMiner agent ID: {0}", dmaId), "dmsElementId");
			}

			if (elementId < 1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid DataMiner element ID: {0}", elementId), "dmsElementId");
			}

			try
			{
				GetElementByIDMessage message = new GetElementByIDMessage(dmaId, elementId);
				ElementInfoEventMessage response = (ElementInfoEventMessage)Communication.SendSingleResponseMessage(message);

				// Cache the response of SLNet.
				// Could be useful when this call is used within a "GetElement" this makes sure that we do not double call SLNet.
				if (response != null)
				{
					cachedElementInfoMessage = response;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2146233088)
				{
					// 0x80131500, Element "[element name]" is unavailable.
					return false;
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Determines whether an element with the specified name exists in the DataMiner System.
		/// </summary>
		/// <param name="elementName">The name of the element.</param>
		/// <exception cref="ArgumentNullException"><paramref name="elementName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="elementName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the element exists; otherwise, <c>false</c>.</returns>
		public bool ElementExists(string elementName)
		{
			if (elementName == null)
			{
				throw new ArgumentNullException("elementName");
			}

			if (String.IsNullOrWhiteSpace(elementName))
			{
				throw new ArgumentException("The element name is the empty string (\"\") or white space.", "elementName");
			}

			try
			{
				GetElementByNameMessage message = new GetElementByNameMessage(elementName);
				ElementInfoEventMessage response = (ElementInfoEventMessage)communication.SendSingleResponseMessage(message);

				// Cache the response of SLNet.
				// Could be useful when this call is used within a "GetElement" this makes sure that we do not double call SLNet.
				if (response != null)
				{
					cachedElementInfoMessage = response;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -2146233088)
				{
					// 0x80131500, Element "[element name]" is unavailable.
					return false;
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Determines whether a service with the specified Agent ID/service ID exists in the DataMiner System.
		/// </summary>
		/// <param name="dmsServiceId">The DataMiner Agent ID/service ID of the service.</param>
		/// <returns><c>true</c> if the service exists; otherwise, <c>false</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="dmsServiceId"/> is invalid.</exception>
		public bool ServiceExists(DmsServiceId dmsServiceId)
		{
			int dmaId = dmsServiceId.AgentId;
			int serviceId = dmsServiceId.ServiceId;

			if (dmaId < 1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid DataMiner agent ID: {0}", dmaId), "dmsServiceId");
			}

			if (serviceId < 1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid DataMiner service ID: {0}", serviceId), "dmsServiceId");
			}

			try
			{
				GetServiceByIDMessage message = new GetServiceByIDMessage(dmaId, serviceId);
				ServiceInfoEventMessage response = Communication.SendSingleResponseMessage(message) as ServiceInfoEventMessage;

				// Cache the response of SLNet.
				// Could be useful when this call is used within a "GetService" this makes sure that we do not double call SLNet.
				if (response != null)
				{
					cachedServiceInfoMessage = response;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -1073478960)
				{
					return false;
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Determines whether a service with the specified name exists in the DataMiner System.
		/// </summary>
		/// <param name="serviceName">The name of the service.</param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="serviceName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the service exists; otherwise, <c>false</c>.</returns>
		public bool ServiceExists(string serviceName)
		{
			if (serviceName == null)
			{
				throw new ArgumentNullException("serviceName");
			}

			if (String.IsNullOrWhiteSpace(serviceName))
			{
				throw new ArgumentException("The service name is the empty string (\"\") or white space.", "serviceName");
			}

			try
			{
				GetServiceByNameMessage message = new GetServiceByNameMessage(serviceName);
				ServiceInfoEventMessage response = communication.SendSingleResponseMessage(message) as ServiceInfoEventMessage;

				// Cache the response of SLNet.
				// Could be useful when this call is used within a "GetService" this makes sure that we do not double call SLNet.
				if (response != null)
				{
					cachedServiceInfoMessage = response;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (DataMinerException e)
			{
				if (e.ErrorCode == -1073478960)
				{
					return false;
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Retrieves all standalone alarm templates from the DataMiner System.
		/// </summary>
		/// <returns>The alarm templates.</returns>
		public ICollection<IDmsStandaloneAlarmTemplate> GetStandaloneAlarmTemplates()
		{
			List<IDmsStandaloneAlarmTemplate> alarmTemplates = new List<IDmsStandaloneAlarmTemplate>();

			GetAvailableAlarmTemplatesMessage message = new GetAvailableAlarmTemplatesMessage
			{
				IncludeGroups = false,
				IncludeTemplates = true
			};

			GetAvailableAlarmTemplatesResponse responses = (GetAvailableAlarmTemplatesResponse)communication.SendSingleResponseMessage(message);

			foreach (AlarmTemplateMetaInfo response in responses.Templates)
			{
				DmsProtocol protocol = new DmsProtocol(this, response.ProtocolName, response.ProtocolVersion);
				DmsStandaloneAlarmTemplate template = new DmsStandaloneAlarmTemplate(this, response.Name, protocol);
				alarmTemplates.Add(template);
			}

			return alarmTemplates;
		}

		/// <summary>
		/// Retrieves all alarm group templates from the DataMiner System.
		/// </summary>
		/// <returns>The alarm groups.</returns>
		public ICollection<IDmsAlarmTemplateGroup> GetAlarmTemplateGroups()
		{
			List<IDmsAlarmTemplateGroup> alarmGroups = new List<IDmsAlarmTemplateGroup>();

			GetAvailableAlarmTemplatesMessage message = new GetAvailableAlarmTemplatesMessage
			{
				IncludeGroups = true,
				IncludeTemplates = false
			};

			GetAvailableAlarmTemplatesResponse responses = (GetAvailableAlarmTemplatesResponse)communication.SendSingleResponseMessage(message);

			foreach (AlarmTemplateMetaInfo response in responses.Templates)
			{
				DmsProtocol protocol = new DmsProtocol(this, response.ProtocolName, response.ProtocolVersion);
				DmsAlarmTemplateGroup template = new DmsAlarmTemplateGroup(this, response.Name, protocol);
				alarmGroups.Add(template);
			}

			return alarmGroups;
		}

		/// <summary>
		/// Retrieves all alarm templates (standalone or groups) from the DataMiner System.
		/// </summary>
		/// <returns>The list of alarm definitions.</returns>
		public ICollection<IDmsAlarmTemplate> GetAlarmTemplates()
		{
			List<IDmsAlarmTemplate> alarmDefinitions = new List<IDmsAlarmTemplate>();

			GetAvailableAlarmTemplatesMessage message = new GetAvailableAlarmTemplatesMessage
			{
				IncludeGroups = true,
				IncludeTemplates = true
			};

			GetAvailableAlarmTemplatesResponse responses = (GetAvailableAlarmTemplatesResponse)communication.SendSingleResponseMessage(message);

			foreach (AlarmTemplateMetaInfo response in responses.Templates)
			{
				DmsProtocol protocol = new DmsProtocol(this, response.ProtocolName, response.ProtocolVersion);
				DmsAlarmTemplate definition = response.IsGroup ?
					(DmsAlarmTemplate)new DmsAlarmTemplateGroup(this, response.Name, protocol) :
					new DmsStandaloneAlarmTemplate(this, response.Name, protocol);
				alarmDefinitions.Add(definition);
			}

			return alarmDefinitions;
		}

		/// <summary>
		/// Gets the DataMiner Agent with the specified DataMiner Agent ID.
		/// </summary>
		/// <param name="agentId">The ID of the DataMiner Agent.</param>
		/// <returns>The DataMiner agent that corresponds with the specified DataMiner Agent ID.</returns>
		/// <exception cref="ArgumentException">The DataMiner Agent ID is negative.</exception>
		/// <exception cref="AgentNotFoundException">There exists no DataMiner Agent in the DataMiner System with the specified DataMiner Agent ID.</exception>
		public IDma GetAgent(int agentId)
		{
			if (!AgentExists(agentId))
			{
				throw new AgentNotFoundException(agentId);
			}

			return new Dma(this, agentId);
		}

		/// <summary>
		/// Gets the DataMiner Agents found on the DataMiner System.
		/// </summary>
		/// <returns>The DataMiner Agents in the DataMiner System.</returns>
		public ICollection<IDma> GetAgents()
		{
			List<IDma> dataMinerAgents = new List<IDma>();

			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.DataMinerInfo
			};

			DMSMessage[] responses = communication.SendMessage(message);
			foreach (DMSMessage response in responses)
			{
				GetDataMinerInfoResponseMessage info = (GetDataMinerInfoResponseMessage)response;

				if (info.ID > 0)
				{
					dataMinerAgents.Add(new Dma(this, info.ID));
				}
			}

			return dataMinerAgents;
		}

		/// <summary>
		/// Retrieves the element with the specified element name.
		/// </summary>
		/// <param name="elementName">The name of the element.</param>
		/// <exception cref="ArgumentNullException"><paramref name="elementName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="elementName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner System.</exception>
		/// <returns>The element with the specified name.</returns>
		public IDmsElement GetElement(string elementName)
		{
			if (elementName == null)
			{
				throw new ArgumentNullException("elementName");
			}

			if (String.IsNullOrWhiteSpace(elementName))
			{
				throw new ArgumentException("The element name is the empty string (\"\") or white space.", "elementName");
			}

			if (!ElementExists(elementName))
			{
				throw new ElementNotFoundException(elementName);
			}

			return new DmsElement(this, cachedElementInfoMessage);
		}

		/// <summary>
		/// Retrieves the element with the specified ID.
		/// </summary>
		/// <param name="dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
		/// <exception cref="ArgumentException"><paramref name="dmsElementId"/> is invalid.</exception>
		/// <exception cref="ElementNotFoundException">The element with the specified ID was not found in the DataMiner System.</exception>
		/// <returns>The element with the specified ID.</returns>
		public IDmsElement GetElement(DmsElementId dmsElementId)
		{
			if (!ElementExists(dmsElementId))
			{
				throw new ElementNotFoundException(dmsElementId);
			}

			return new DmsElement(this, cachedElementInfoMessage);
		}

		/// <summary>
		/// Gets the number of elements present on the DataMiner system.
		/// </summary>
		/// <returns>The number of elements present on the DataMiner system.</returns>
		public int GetElementCount()
		{
			return GetElements().Count;
		}

		/// <summary>
		/// Retrieves all elements from the DataMiner System.
		/// </summary>
		/// <returns>The elements present on the DataMiner System.</returns>
		public ICollection<IDmsElement> GetElements()
		{
			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.ElementInfo
			};

			DMSMessage[] responses = communication.SendMessage(message);

			List<IDmsElement> elements = new List<IDmsElement>();

			foreach (DMSMessage response in responses)
			{
				ElementInfoEventMessage elementInfo = (ElementInfoEventMessage)response;
				if (elementInfo.DataMinerID == -1 || elementInfo.ElementID == -1)
				{
					continue;
				}

				try
				{
					DmsElement element = new DmsElement(this, elementInfo);
					elements.Add(element);
				}
				catch (Exception ex)
				{
					string logMessage = "Failed parsing element info for element " + Convert.ToString(elementInfo.Name) + " (" + Convert.ToString(elementInfo.DataMinerID) + "/" + Convert.ToString(elementInfo.ElementID) + ")." + Environment.NewLine + ex;
					Logger.Log(logMessage);
				}
			}

			return elements;
		}

		/// <summary>
		/// Retrieves the service with the specified service name.
		/// </summary>
		/// <param name="serviceName">The name of the service.</param>
		/// <exception cref="ArgumentException"><paramref name="serviceName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ServiceNotFoundException">The service was not found in the DataMiner System.</exception>
		/// <returns>The service with the specified name.</returns>
		public IDmsService GetService(string serviceName)
		{
			if (String.IsNullOrWhiteSpace(serviceName))
			{
				throw new ArgumentException("The service name is the empty string (\"\") or white space.", "serviceName");
			}

			if (!ServiceExists(serviceName))
			{
				throw new ServiceNotFoundException(serviceName);
			}

			return new DmsService(this, cachedServiceInfoMessage);
		}

		/// <summary>
		/// Retrieves the service with the specified ID.
		/// </summary>
		/// <param name="dmsServiceId">The DataMiner Agent ID/service ID of the service.</param>
		/// <exception cref="ArgumentException"><paramref name="dmsServiceId"/> is invalid.</exception>
		/// <exception cref="ServiceNotFoundException">The service with the specified ID was not found in the DataMiner System.</exception>
		/// <returns>The service with the specified ID.</returns>
		public IDmsService GetService(DmsServiceId dmsServiceId)
		{
			if (!ServiceExists(dmsServiceId))
			{
				throw new ServiceNotFoundException(dmsServiceId);
			}

			return new DmsService(this, cachedServiceInfoMessage);
		}

		/// <summary>
		/// Gets the number of services present on the DataMiner system.
		/// </summary>
		/// <returns>The number of services present on the DataMiner system.</returns>
		public int GetServiceCount()
		{
			return GetServices().Count;
		}

		/// <summary>
		/// Retrieves all services from the DataMiner System.
		/// </summary>
		/// <returns>The services present on the DataMiner System.</returns>
		public ICollection<IDmsService> GetServices()
		{
			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.ServiceInfo
			};

			DMSMessage[] responses = communication.SendMessage(message);

			List<IDmsService> services = new List<IDmsService>();

			foreach (DMSMessage response in responses)
			{
				ServiceInfoEventMessage serviceInfo = response as ServiceInfoEventMessage;
				if (serviceInfo.DataMinerID == -1 || serviceInfo.ElementID == -1)
				{
					continue;
				}

				try
				{
					DmsService service = new DmsService(this, serviceInfo);
					services.Add(service);
				}
				catch (Exception ex)
				{
					string logMessage = "Failed parsing service info for service " + Convert.ToString(serviceInfo.Name) + " (" + Convert.ToString(serviceInfo.DataMinerID) + "/" + Convert.ToString(serviceInfo.ElementID) + ")." + Environment.NewLine + ex;
					Logger.Log(logMessage);
				}
			}

			return services;
		}

		/// <summary>
		/// Retrieves the protocol with the given name and version.
		/// </summary>
		/// <param name="name">The name of the protocol.</param>
		/// <param name="version">The version of the protocol.</param>
		/// <returns>An instance of the protocol.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="version"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="version"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ProtocolNotFoundException">No protocol with the specified name and version exists in the DataMiner System.</exception>
		public IDmsProtocol GetProtocol(string name, string version)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (version == null)
			{
				throw new ArgumentNullException("version");
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "name");
			}

			if (String.IsNullOrWhiteSpace(version))
			{
				throw new ArgumentException("The name of the version is the empty string (\"\") or white space.", "version");
			}

			if (!ProtocolExists(name, version))
			{
				throw new ProtocolNotFoundException(name, version);
			}

			return new DmsProtocol(this, cachedProtocolMessage, DmsProtocol.CheckIsProduction(cachedProtocolRequestedVersion));
		}

		/// <summary>
		/// Retrieves all protocols from the DataMiner System.
		/// </summary>
		/// <returns>The protocols available on the DataMiner System.</returns>
		public ICollection<IDmsProtocol> GetProtocols()
		{
			List<IDmsProtocol> protocols = new List<IDmsProtocol>();

			GetInfoMessage message = new GetInfoMessage
			{
				DataMinerID = -1,
				Type = InfoType.Protocols
			};

			DMSMessage[] responses = communication.SendMessage(message);
			foreach (DMSMessage response in responses)
			{
				GetProtocolsResponseMessage protocolInfo = (GetProtocolsResponseMessage)response;
				foreach (ProtocolVersionDetails versionDetail in protocolInfo.VersionDetails)
				{
					string version = versionDetail.Version;
					string referencedProtocol = versionDetail.ReferencedProtocol;
					if (!String.IsNullOrEmpty(version))
					{
						DmsProtocol protocol = new DmsProtocol(this, protocolInfo.Protocol, version, (ProtocolType)protocolInfo.Type, referencedProtocol);
						protocols.Add(protocol);
					}
				}
			}

			return protocols;
		}

		/// <summary>
		/// Retrieves all trend templates from the DataMiner System.
		/// </summary>
		/// <returns>The trend templates.</returns>
		public ICollection<IDmsTrendTemplate> GetTrendTemplates()
		{
			List<IDmsTrendTemplate> trendTemplates = new List<IDmsTrendTemplate>();

			GetAvailableTrendTemplatesMessage message = new GetAvailableTrendTemplatesMessage();
			GetAvailableTrendTemplatesResponse responses = (GetAvailableTrendTemplatesResponse)communication.SendSingleResponseMessage(message);

			foreach (TrendTemplateMetaInfo response in responses.Templates)
			{
				DmsTrendTemplate template = new DmsTrendTemplate(this, response);
				trendTemplates.Add(template);
			}

			return trendTemplates;
		}

		/// <summary>
		/// Gets the view with the specified ID.
		/// </summary>
		/// <param name="viewId">The view ID.</param>
		/// <exception cref="ArgumentException"><paramref name="viewId"/> is invalid.</exception>
		/// <exception cref="ViewNotFoundException">No view with the specified id exists in the DataMiner System.</exception>
		/// <returns>The view with the specified ID.</returns>
		public IDmsView GetView(int viewId)
		{
			if (viewId < -1)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid view ID: {0}", viewId), "viewId");
			}

			if (!ViewExists(viewId))
			{
				throw new ViewNotFoundException(viewId);
			}

			return new DmsView(this, cachedViewInfoMessage);
		}

		/// <summary>
		/// Retrieves the view with the specified name.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <exception cref="ArgumentNullException"><paramref name="viewName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="viewName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ViewNotFoundException">No view with the specified name exists in the DataMiner System.</exception>
		/// <returns>The view with the specified name.</returns>
		public IDmsView GetView(string viewName)
		{
			if (viewName == null)
			{
				throw new ArgumentNullException("viewName");
			}

			if (String.IsNullOrWhiteSpace(viewName))
			{
				throw new ArgumentException("The name of the view is the empty string (\"\") or white space.", "viewName");
			}

			if (!ViewExists(viewName))
			{
				throw new ViewNotFoundException(viewName);
			}

			return new DmsView(this, cachedViewInfoMessage);
		}

		/// <summary>
		/// Retrieves all views from the DataMiner System.
		/// </summary>
		/// <returns>The views.</returns>
		public ICollection<IDmsView> GetViews()
		{
			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.ViewInfo
			};

			DMSMessage[] responses = communication.SendMessage(message);

			List<IDmsView> views = new List<IDmsView>();
			foreach (DMSMessage response in responses)
			{
				ViewInfoEventMessage viewInfo = (ViewInfoEventMessage)response;
				DmsView view = new DmsView(this, viewInfo);
				views.Add(view);
			}

			return views;
		}

		/// <summary>
		/// Determines whether the specified version of the specified protocol exists.
		/// </summary>
		/// <param name="protocolName">The protocol name.</param>
		/// <param name="protocolVersion">The protocol version.</param>
		/// <exception cref="ArgumentNullException"><paramref name="protocolName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocolVersion"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="protocolName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="protocolVersion"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the protocol is valid; otherwise, <c>false</c>.</returns>
		public bool ProtocolExists(string protocolName, string protocolVersion)
		{
			if (protocolName == null)
			{
				throw new ArgumentNullException("protocolName");
			}

			if (protocolVersion == null)
			{
				throw new ArgumentNullException("protocolVersion");
			}

			if (String.IsNullOrWhiteSpace(protocolName))
			{
				throw new ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "protocolName");
			}

			if (String.IsNullOrWhiteSpace(protocolVersion))
			{
				throw new ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "protocolVersion");
			}

			cachedProtocolRequestedVersion = protocolVersion;
			GetProtocolMessage message = new GetProtocolMessage
			{
				Protocol = protocolName,
				Version = cachedProtocolRequestedVersion
			};

			cachedProtocolMessage = (GetProtocolInfoResponseMessage)communication.SendSingleResponseMessage(message);
			return cachedProtocolMessage != null;
		}

		/// <summary>
		/// Determines whether the specified property exists in the DataMiner System.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="type">Specifies the property type.</param>
		/// <returns>Value indicating whether the specified property exists in the DataMiner System.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="type" /> is invalid.</exception>
		public bool PropertyExists(string name, PropertyType type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("The name of the property is the empty string (\"\") or white space.", "name");
			}

			if (type != PropertyType.Element && type != PropertyType.View && type != PropertyType.Service)
			{
				throw new ArgumentException("Invalid property type specified.", "type");
			}

			LoadDmsProperties();

			switch (type)
			{
				case PropertyType.Element:
					try
					{
						IDmsPropertyDefinition def = ElementPropertyDefinitions[name];
						return def != null;
					}
					catch (ArgumentOutOfRangeException)
					{
						return false;
					}

				case PropertyType.View:
					try
					{
						IDmsViewPropertyDefinition def = ViewPropertyDefinitions[name];
						return def != null;
					}
					catch (ArgumentOutOfRangeException)
					{
						return false;
					}

				case PropertyType.Service:
					try
					{
						IDmsServicePropertyDefinition def = ServicePropertyDefinitions[name];
						return def != null;
					}
					catch (ArgumentOutOfRangeException)
					{
						return false;
					}

				default:
					return false;
			}
		}

		/// <summary>
		/// Updates the communication interface.
		/// </summary>
		/// <param name="communication">The communication interface.</param>
		/// <exception cref="ArgumentNullException"><paramref name="communication"/> is <see langword="null"/>.</exception>
		public void Refresh(ICommunication communication)
		{
			if (communication == null)
			{
				throw new ArgumentNullException("communication");
			}

			this.communication = communication;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return "DataMiner System";
		}

		/// <summary>
		/// Determines whether the view with the specified ID exists.
		/// </summary>
		/// <param name="viewId">The view ID.</param>
		/// <exception cref="ArgumentException"><paramref name="viewId"/> is invalid.</exception>
		/// <returns><c>true</c> if the view exists; otherwise, <c>false</c>.</returns>
		public bool ViewExists(int viewId)
		{
			if (viewId < -1 || viewId == 0)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Invalid view ID: {0}", viewId), "viewId");
			}

			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.ViewInfo
			};

			DMSMessage[] responses = communication.SendMessage(message);

			foreach (DMSMessage response in responses)
			{
				ViewInfoEventMessage viewInfo = (ViewInfoEventMessage)response;
				if (viewInfo.ID.Equals(viewId))
				{
					cachedViewInfoMessage = viewInfo;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the view with the specified name exists.
		/// </summary>
		/// <param name="viewName">The view name.</param>
		/// <exception cref="ArgumentNullException"><paramref name="viewName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="viewName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the view exists; otherwise, <c>false</c>.</returns>
		public bool ViewExists(string viewName)
		{
			if (viewName == null)
			{
				throw new ArgumentNullException("viewName");
			}

			if (String.IsNullOrWhiteSpace(viewName))
			{
				throw new ArgumentException("The name of the view is the empty string (\"\") or white space.", "viewName");
			}

			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.ViewInfo
			};

			DMSMessage[] responses = communication.SendMessage(message);
			foreach (DMSMessage response in responses)
			{
				ViewInfoEventMessage viewInfo = (ViewInfoEventMessage)response;

				if (viewInfo.Name.Equals(viewName, StringComparison.OrdinalIgnoreCase))
				{
					cachedViewInfoMessage = viewInfo;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether an alarm template with the specified name exists for the specified protocol.
		/// </summary>
		/// <param name="templateName">The name of the alarm template.</param>
		/// <param name="protocol">The corresponding protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if an alarm template with the specified name exists for the specified protocol; otherwise, <c>false</c>.</returns>
		internal bool AlarmTemplateExists(string templateName, IDmsProtocol protocol)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			bool exists = false;
			AlarmTemplateEventMessage template = GetAlarmTemplate(templateName, protocol);

			if (template != null)
			{
				exists = true;
			}

			return exists;
		}

		/// <summary>
		/// Determines whether the specified alarm template exists.
		/// </summary>
		/// <param name="templateName">The name of the template.</param>
		/// <param name="protocol">Instance of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the alarm template exists; otherwise, <c>false</c>.</returns>
		internal bool StandaloneAlarmTemplateExists(string templateName, IDmsProtocol protocol)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			bool exists = false;
			AlarmTemplateEventMessage template = GetAlarmTemplate(templateName, protocol);

			if (template != null && template.Type == AlarmTemplateType.Template)
			{
				exists = true;
			}

			return exists;
		}

		/// <summary>
		/// Determines whether the specified alarm template exists.
		/// </summary>
		/// <param name="templateName">The name of the template.</param>
		/// <param name="protocol">Instance of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the alarm template exists; otherwise, <c>false</c>.</returns>
		internal bool AlarmTemplateGroupExists(string templateName, IDmsProtocol protocol)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			bool exists = false;
			AlarmTemplateEventMessage template = GetAlarmTemplate(templateName, protocol);

			if (template != null && template.Type == AlarmTemplateType.Group)
			{
				exists = true;
			}

			return exists;
		}

		/// <summary>
		/// Loads all the properties found on the DataMiner system.
		/// </summary>
		internal void LoadDmsProperties()
		{
			isLoaded = true;

			GetInfoMessage message = new GetInfoMessage
			{
				Type = InfoType.PropertyConfiguration
			};

			GetPropertyConfigurationResponse response = (GetPropertyConfigurationResponse)communication.SendSingleResponseMessage(message);

			foreach (PropertyConfig property in response.Properties)
			{
				switch (property.Type)
				{
					case "Element":
						elementProperties[property.Name] = new DmsElementPropertyDefinition(this, property);
						break;
					case "View":
						viewProperties[property.Name] = new DmsViewPropertyDefinition(this, property);
						break;
					case "Service":
						serviceProperties[property.Name] = new DmsServicePropertyDefinition(this, property);
						break;
					default:
						continue;
				}
			}
		}

		/// <summary>
		/// Determines whether the specified trend template exists.
		/// </summary>
		/// <param name="templateName">The name of the template.</param>
		/// <param name="protocol">Instance of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if the trend template exists; otherwise, <c>false</c>.</returns>
		internal bool TrendTemplateExists(string templateName, IDmsProtocol protocol)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			GetTrendingTemplateInfoMessage message = new GetTrendingTemplateInfoMessage
			{
				Protocol = protocol.Name,
				Template = templateName,
				Version = protocol.Version
			};

			cachedTrendTemplateMessage = (GetTrendingTemplateInfoResponseMessage)communication.SendSingleResponseMessage(message);
			return cachedTrendTemplateMessage != null;
		}

		/// <summary>
		/// Retrieves an alarm template or an alarm template group with the specified name.
		/// </summary>
		/// <param name="templateName">The name of the alarm template.</param>
		/// <param name="protocol">The corresponding protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns>The alarm template message.</returns>
		private AlarmTemplateEventMessage GetAlarmTemplate(string templateName, IDmsProtocol protocol)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (protocol == null)
			{
				throw new ArgumentNullException("protocol");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
			{
				AsOneObject = true,
				Protocol = protocol.Name,
				Template = templateName,
				Version = protocol.Version
			};

			cachedAlarmTemplateMessage = (AlarmTemplateEventMessage)communication.SendSingleResponseMessage(message);

			return cachedAlarmTemplateMessage;
		}
	}
}