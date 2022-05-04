namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;
	using Templates;

	/// <summary>
	/// Represents general element information.
	/// </summary>
	internal class GeneralSettings : ElementSettings
	{
		/// <summary>
		/// The name of the alarm template.
		/// </summary>
		private string alarmTemplateName;

		/// <summary>
		/// The SLNet call that will retrieve the alarm template from the system if needed.
		/// </summary>
		private Lazy<IDmsAlarmTemplate> alarmTemplateLoader;

		/// <summary>
		/// The alarm template assigned to this element.
		/// </summary>
		private IDmsAlarmTemplate alarmTemplate;

		/// <summary>
		/// Element description.
		/// </summary>
		private string description = String.Empty;

		/// <summary>
		/// The hosting DataMiner agent.
		/// </summary>
		private Dma host;

		/// <summary>
		/// The element state.
		/// </summary>
		private ElementState state = ElementState.Active;

		/// <summary>
		/// Instance of the protocol this element executes.
		/// </summary>
		private DmsProtocol protocol;

		/// <summary>
		/// The trend template assigned to this element.
		/// </summary>
		private IDmsTrendTemplate trendTemplate;

		/// <summary>
		/// The name of the element.
		/// </summary>
		private string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="GeneralSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the DmsElement where this object will be used in.</param>
		internal GeneralSettings(DmsElement dmsElement)
		: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets or sets the alarm template definition of the element.
		/// This can either be an alarm template or an alarm template group.
		/// </summary>
		internal IDmsAlarmTemplate AlarmTemplate
		{
			get
			{
				DmsElement.LoadOnDemand();
				if (!alarmTemplateLoader.IsValueCreated)
				{
					alarmTemplate = alarmTemplateLoader.Value;
				}

				return alarmTemplate;
			}

			set
			{
				DmsElement.LoadOnDemand();
				string newAlarmTemplateName = value == null ? String.Empty : value.Name;
				bool isCurrentEmpty = String.IsNullOrWhiteSpace(alarmTemplateName);
				bool isNewEmpty = String.IsNullOrWhiteSpace(newAlarmTemplateName);

				bool updateRequired = isCurrentEmpty 
					? !isNewEmpty 
					: !alarmTemplateName.Equals(newAlarmTemplateName, StringComparison.OrdinalIgnoreCase);

				if (updateRequired)
				{
					ChangedPropertyList.Add("AlarmTemplate");

					alarmTemplateName = newAlarmTemplateName;
					alarmTemplateLoader = new Lazy<IDmsAlarmTemplate>(() => value);
					alarmTemplate = alarmTemplateLoader.Value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the element description.
		/// </summary>
		internal string Description
		{
			get
			{
				DmsElement.LoadOnDemand();
				return description;
			}

			set
			{
				DmsElement.LoadOnDemand();

				string newValue = value == null ? String.Empty : value;

				if (!description.Equals(newValue, StringComparison.Ordinal))
				{
					ChangedPropertyList.Add("Description");
					description = newValue;
				}
			}
		}

		/// <summary>
		/// Gets or sets the system-wide element ID.
		/// </summary>
		internal DmsElementId DmsElementId
		{
			get;

			set;
		}

		/// <summary>
		/// Gets the DataMiner agent that hosts the element.
		/// </summary>
		internal Dma Host
		{
			get
			{
				DmsElement.LoadOnDemand();
				return host;
			}
		}

		/// <summary>
		/// Gets or sets the state of the element.
		/// </summary>
		internal Common.ElementState State
		{
			get
			{
				DmsElement.LoadOnDemand();
				return state;
			}

			set
			{
				DmsElement.LoadOnDemand();
				state = value;
			}
		}

		/// <summary>
		/// Gets or sets the trend template assigned to this element.
		/// </summary>
		internal IDmsTrendTemplate TrendTemplate
		{
			get
			{
				DmsElement.LoadOnDemand();
				return trendTemplate;
			}

			set
			{
				DmsElement.LoadOnDemand();

				bool updateRequired = false;
				if (trendTemplate == null)
				{
					if (value != null)
					{
						updateRequired = true;
					}
				}
				else
				{
					if (value == null || !trendTemplate.Equals(value))
					{
						updateRequired = true;
					}
				}

				if (updateRequired)
				{
					ChangedPropertyList.Add("TrendTemplate");
					trendTemplate = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the name of the element.
		/// </summary>
		/// <exception cref="NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
		internal string Name
		{
			get
			{
				DmsElement.LoadOnDemand();
				return name;
			}

			set
			{
				DmsElement.LoadOnDemand();

				if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
				{
					throw new NotSupportedException("Setting the name of a DVE child or a derived element is not supported.");
				}

				if (!name.Equals(value, StringComparison.Ordinal))
				{
					ChangedPropertyList.Add("Name");
					name = value.Trim();
				}
			}
		}

		/// <summary>
		/// Gets or sets the instance of the protocol.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is empty.</exception>
		internal DmsProtocol Protocol
		{
			get
			{
				DmsElement.LoadOnDemand();
				return protocol;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				DmsElement.LoadOnDemand();
				ChangedPropertyList.Add("Protocol");
				protocol = value;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("GENERAL SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendFormat(CultureInfo.InvariantCulture, "Name: {0}{1}", DmsElement.Name, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Description: {0}{1}", Description, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Protocol name: {0}{1}", Protocol.Name, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Protocol version: {0}{1}", Protocol.Version, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "DMA ID: {0}{1}", DmsElementId.AgentId, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Element ID: {0}{1}", DmsElementId.ElementId, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Hosting DMA ID: {0}{1}", Host.Id, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Alarm template: {0}{1}", AlarmTemplate, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Trend template: {0}{1}", TrendTemplate, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "State: {0}{1}", State, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Sets the updated fields when an update is performed.
		/// </summary>
		/// <param name="message">The message to be updated.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
			foreach (string property in ChangedPropertyList)
			{
				switch (property)
				{
					case "AlarmTemplate":
						message.AlarmTemplate = AlarmTemplate == null ? String.Empty : AlarmTemplate.Name;
						break;
					case "Description":
						message.Description = description;
						break;
					case "TrendTemplate":
						message.TrendTemplate = trendTemplate == null ? String.Empty : trendTemplate.Name;
						break;
					case "Name":
						message.ElementName = name;
						break;
					default:
						throw new InvalidOperationException("Unexpected value: " + property);
				}
			}
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			DmsElementId = new DmsElementId(elementInfo.DataMinerID, elementInfo.ElementID);
			description = elementInfo.Description ?? String.Empty;

			protocol = new DmsProtocol(DmsElement.Dms, elementInfo.Protocol, elementInfo.ProtocolVersion);
			alarmTemplateName = elementInfo.ProtocolTemplate;
			trendTemplate = String.IsNullOrWhiteSpace(elementInfo.Trending) ? null : new DmsTrendTemplate(DmsElement.Dms, elementInfo.Trending, protocol);
			state = (ElementState)elementInfo.State;
			name = elementInfo.Name ?? String.Empty;
			host = new Dma(DmsElement.Dms, elementInfo.HostingAgentID);

			alarmTemplateLoader = new Lazy<IDmsAlarmTemplate>(() => LoadAlarmTemplateDefinition());
		}

		/// <summary>
		/// Loads the alarm template definition.
		/// This method checks whether there is a group or a template assigned to the element.
		/// </summary>
		private IDmsAlarmTemplate LoadAlarmTemplateDefinition()
		{
			IDmsAlarmTemplate innerAlarmTemplate = alarmTemplate; // do not use public property here as it will cause cyclic call
			if (innerAlarmTemplate == null && !String.IsNullOrWhiteSpace(alarmTemplateName))
			{
				GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
				{
					AsOneObject = true,
					Protocol = protocol.Name,
					Version = protocol.Version,
					Template = alarmTemplateName
				};

				AlarmTemplateEventMessage response = (AlarmTemplateEventMessage)DmsElement.Dms.Communication.SendSingleResponseMessage(message);

				if (response != null)
				{
					switch (response.Type)
					{
						case AlarmTemplateType.Template:
							innerAlarmTemplate = new DmsStandaloneAlarmTemplate(DmsElement.Dms, response);
							break;
						case AlarmTemplateType.Group:
							innerAlarmTemplate = new DmsAlarmTemplateGroup(DmsElement.Dms, response);
							break;
						default:
							throw new InvalidOperationException("Unexpected value: " + response.Type);
					}
				}
			}

			return innerAlarmTemplate;
		}
	}
}