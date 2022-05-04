namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Net.Messages;
	using Templates;

	/// <summary>
	/// Represents the advanced element information.
	/// </summary>
	internal class AdvancedServiceSettings : ServiceSettings, IAdvancedServiceSettings
	{
		/// <summary>
		/// Value indicating whether the service is a service template.
		/// </summary>
		private bool isTemplate;

		/// <summary>
		/// The service template from which the service is generated in case the service is generated through a service template.
		/// </summary>
		private DmsServiceId? parentTemplate;

		/// <summary>
		/// The element that is linked to this service in case of an enhanced service.
		/// </summary>
		private DmsElementId? serviceElement;

		/// <summary>
		/// The alarm template assigned to the service element.
		/// </summary>
		private IDmsAlarmTemplate serviceElementAlarmTemplate;

		/// <summary>
		/// Instance of the protocol for the service element executes.
		/// </summary>
		private IDmsProtocol serviceElementProtocol;

		/// <summary>
		/// The trend template assigned to the service element.
		/// </summary>
		private IDmsTrendTemplate serviceElementTrendTemplate;

		/// <summary>
		/// Indicates if the service will ignore timeouts.
		/// </summary>
		private bool ignoreTimeouts;

		/// <summary>
		/// Initializes a new instance of the <see cref="AdvancedServiceSettings"/> class.
		/// </summary>
		/// <param name="dmsService">The reference to the <see cref="DmsService"/> instance this object is part of.</param>
		internal AdvancedServiceSettings(DmsService dmsService)
		: base(dmsService)
		{
		}

		/// <summary>
		/// Gets a value indicating whether the service is a service template.
		/// </summary>
		/// <value><c>true</c> if the service is a service template; otherwise, <c>false</c>.</value>
		public bool IsTemplate
		{
			get
			{
				DmsService.LoadOnDemand();
				return isTemplate;
			}
		}

		/// <summary>
		/// Gets the service template from which the service is generated in case the service is generated through a service template.
		/// </summary>
		/// <returns>The service template from which this service is generated.</returns>
		public DmsServiceId? ParentTemplate
		{
			get
			{
				DmsService.LoadOnDemand();
				return parentTemplate;
			}
		}

		/// <summary>
		/// Gets the element that is linked to this service in case of an enhanced service.
		/// </summary>
		/// <returns>The service element that is linked with the service.</returns>
		public DmsElementId? ServiceElement
		{
			get
			{
				DmsService.LoadOnDemand();
				return serviceElement;
			}
		}

		/// <summary>
		/// Gets or sets the alarm template of the service element (enhanced service).
		/// </summary>
		public IDmsAlarmTemplate ServiceElementAlarmTemplate
		{
			get
			{
				DmsService.LoadOnDemand();
				return serviceElementAlarmTemplate;
			}

			set
			{
				if (!InputValidator.IsCompatibleTemplate(value, this.serviceElementProtocol))
				{
					throw new ArgumentException(
						"The specified alarm template is not compatible with the protocol that the service element executes.",
						"value");
				}

				this.serviceElementAlarmTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the trend template of the service element (enhanced service).
		/// </summary>
		public IDmsTrendTemplate ServiceElementTrendTemplate
		{
			get
			{
				DmsService.LoadOnDemand();
				return serviceElementTrendTemplate;
			}

			set
			{
				if (!InputValidator.IsCompatibleTemplate(value, this.serviceElementProtocol))
				{
					throw new ArgumentException(
						"The specified trend template is not compatible with the protocol that the service element executes.",
						"value");
				}

				this.serviceElementTrendTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the protocol applied to the service element (enhanced service).
		/// </summary>
		public IDmsProtocol ServiceElementProtocol
		{
			get
			{
				DmsService.LoadOnDemand();
				return serviceElementProtocol;
			}

			set
			{
				if (serviceElementAlarmTemplate != null && !InputValidator.IsCompatibleTemplate(serviceElementAlarmTemplate, value))
				{
					serviceElementAlarmTemplate = null;
				}

				if (serviceElementTrendTemplate != null && !InputValidator.IsCompatibleTemplate(serviceElementTrendTemplate, value))
				{
					serviceElementTrendTemplate = null;
				}

				if (value.Type != ProtocolType.Service)
				{
					throw new ArgumentException("The specified protocol is not compatible with services.", "value");
				}

				this.serviceElementProtocol = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the service will ignore timeouts.
		/// </summary>
		public bool IgnoreTimeouts
		{
			get
			{
				DmsService.LoadOnDemand();
				return ignoreTimeouts;
			}

			set
			{
				ignoreTimeouts = value;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ADVANCED SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendFormat(CultureInfo.InvariantCulture, "Service Template: {0}{1}", IsTemplate, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Parent (template): {0}{1}", ParentTemplate, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Enhanced: {0}{1}", ServiceElement, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="serviceInfo">The service information.</param>
		internal override void Load(ServiceInfoEventMessage serviceInfo)
		{
			isTemplate = serviceInfo.IsTemplate;
			string parent = serviceInfo.ParentTemplateID;
			string servEl = null;
			if (serviceInfo.ServiceElement != null && serviceInfo.ServiceElementProtocolName != null && serviceInfo.ServiceElementProtocolVersion != null)
			{
				// Order is important!
				servEl = serviceInfo.ServiceElement.Element;
				ignoreTimeouts = serviceInfo.IgnoreTimeouts;
				serviceElementProtocol = new DmsProtocol(DmsService.Dms, serviceInfo.ServiceElementProtocolName, serviceInfo.ServiceElementProtocolVersion, ProtocolType.Service);
				serviceElementTrendTemplate = String.IsNullOrWhiteSpace(serviceInfo.ServiceElementTrendTemplate) ? null : new DmsTrendTemplate(DmsService.Dms, serviceInfo.ServiceElementTrendTemplate, serviceElementProtocol);
				LoadServieElementAlarmTemplateDefinition(serviceInfo.ServiceElementAlarmTemplate);
			}

			if (!string.IsNullOrWhiteSpace(parent))
			{
				parentTemplate = new DmsServiceId(parent);
			}

			if (!string.IsNullOrWhiteSpace(servEl))
			{
				serviceElement = new DmsElementId(servEl);
			}
		}

		/// <summary>
		/// Fills in the needed properties in the AddService message.
		/// </summary>
		/// <param name="message">The AddService message which will be sent to SLNet.</param>
		internal override void FillUpdate(AddServiceMessage message)
		{
			message.Service.IsTemplate = IsTemplate;
			message.Service.ParentTemplateID = parentTemplate.HasValue ? parentTemplate.Value.Value : null;
			message.Service.IgnoreTimeouts = ignoreTimeouts;
			message.Service.ServiceElement = serviceElement.HasValue ? new ServiceInfoParams(serviceElement.Value.AgentId, serviceElement.Value.ElementId, false) : null;
			message.Service.ServiceElementProtocolName = serviceElementProtocol == null ? null : serviceElementProtocol.Name;
			message.Service.ServiceElementProtocolVersion = serviceElementProtocol == null ? null : serviceElementProtocol.Version;
			message.Service.ServiceElementAlarmTemplate = serviceElementAlarmTemplate == null ? null : serviceElementAlarmTemplate.Name;
			message.Service.ServiceElementTrendTemplate = serviceElementTrendTemplate == null ? null : serviceElementTrendTemplate.Name;
		}

		/// <summary>
		/// Loads the alarm template definition for the service element.
		/// This method checks whether there is a group or a template assigned to the service element.
		/// </summary>
		/// <param name="alarmTemplateName">The name of the alarm template.</param>
		private void LoadServieElementAlarmTemplateDefinition(string alarmTemplateName)
		{
			if (serviceElementAlarmTemplate == null && !String.IsNullOrWhiteSpace(alarmTemplateName))
			{
				GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
				{
					AsOneObject = true,
					Protocol = serviceElementProtocol.Name,
					Version = serviceElementProtocol.Version,
					Template = alarmTemplateName
				};

				AlarmTemplateEventMessage response = (AlarmTemplateEventMessage)DmsService.Dms.Communication.SendSingleResponseMessage(message);

				if (response != null)
				{
					switch (response.Type)
					{
						case AlarmTemplateType.Template:
							serviceElementAlarmTemplate = new DmsStandaloneAlarmTemplate(DmsService.Dms, response);
							break;
						case AlarmTemplateType.Group:
							serviceElementAlarmTemplate = new DmsAlarmTemplateGroup(DmsService.Dms, response);
							break;
						default:
							throw new InvalidOperationException("Unexpected value: " + response.Type);
					}
				}
			}
		}
	}
}