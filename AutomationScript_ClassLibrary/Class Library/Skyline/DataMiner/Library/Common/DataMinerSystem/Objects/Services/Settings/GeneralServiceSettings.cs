namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents general service information.
	/// </summary>
	internal class GeneralServiceSettings : ServiceSettings
	{
		/// <summary>
		/// Service description.
		/// </summary>
		private string description = String.Empty;

		/// <summary>
		/// The hosting DataMiner agent.
		/// </summary>
		private Dma host;

		/// <summary>
		/// The name of the service.
		/// </summary>
		private string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="GeneralServiceSettings"/> class.
		/// </summary>
		/// <param name="dmsService">The reference to the DmsService where this object will be used in.</param>
		internal GeneralServiceSettings(DmsService dmsService)
		: base(dmsService)
		{
		}

		/// <summary>
		/// Gets or sets the service description.
		/// </summary>
		/// <exception cref="NotSupportedException">A set operation is not supported on a generated service.</exception>
		internal string Description
		{
			get
			{
				DmsService.LoadOnDemand();
				return description;
			}

			set
			{
				DmsService.LoadOnDemand();
				
				if (DmsService.AdvancedSettings.ParentTemplate != null)
				{
					throw new NotSupportedException("Setting the name of a generated service (from service template) is not supported.");
				}

				string newValue = value == null ? String.Empty : value;

				if (!description.Equals(newValue, StringComparison.Ordinal))
				{
					ChangedPropertyList.Add("Description");
					description = newValue;
				}
			}
		}

		/// <summary>
		/// Gets or sets the system-wide service ID.
		/// </summary>
		internal DmsServiceId DmsServiceId
		{
			get;

			set;
		}

		/// <summary>
		/// Gets the DataMiner agent that hosts the service.
		/// </summary>
		internal Dma Host
		{
			get
			{
				DmsService.LoadOnDemand();
				return host;
			}
		}

		/// <summary>
		/// Gets or sets the name of the service.
		/// </summary>
		/// <exception cref="NotSupportedException">A set operation is not supported on a generated service.</exception>
		internal string Name
		{
			get
			{
				DmsService.LoadOnDemand();
				return name;
			}

			set
			{
				DmsService.LoadOnDemand();

				if (DmsService.AdvancedSettings.ParentTemplate != null)
				{
					throw new NotSupportedException("Setting the name of a generated service (from service template) is not supported.");
				}

				if (!name.Equals(value, StringComparison.Ordinal))
				{
					ChangedPropertyList.Add("Name");
					name = value.Trim();
				}
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
			sb.AppendFormat(CultureInfo.InvariantCulture, "Name: {0}{1}", DmsService.Name, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Description: {0}{1}", Description, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "DMA ID: {0}{1}", DmsServiceId.AgentId, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Service ID: {0}{1}", DmsServiceId.ServiceId, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Hosting DMA ID: {0}{1}", Host.Id, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Sets the updated fields when an update is performed.
		/// </summary>
		/// <param name="message">The message to be updated.</param>
		internal override void FillUpdate(AddServiceMessage message)
		{
			foreach (string property in ChangedPropertyList)
			{
				switch (property)
				{
					case "Description":
						message.Service.Description = description;
						break;
					case "Name":
						message.Service.Name = name;
						break;
					default:
						throw new InvalidOperationException("Unexpected value: " + property);
				}
			}
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="serviceInfo">The service information.</param>
		internal override void Load(ServiceInfoEventMessage serviceInfo)
		{
			DmsServiceId = new DmsServiceId(serviceInfo.DataMinerID, serviceInfo.ElementID);
			description = serviceInfo.Description ?? String.Empty;

			name = serviceInfo.Name ?? String.Empty;
			host = new Dma(DmsService.Dms, serviceInfo.HostingAgentID);
		}
	}
}