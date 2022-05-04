namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents a base class for all of the components in a DmsService object.
	/// </summary>
	internal class ServiceParamsSettings : ServiceSettings, IServiceParamsSettings
	{
		private ServiceParamSettings[] includedParams;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceParamsSettings"/> class.
		/// </summary>
		/// <param name="dmsService">The reference to the <see cref="DmsService"/> instance this object is part of.</param>
		internal ServiceParamsSettings(DmsService dmsService)
		: base(dmsService)
		{
		}

		/// <summary>
		/// Gets the included elements and parameters.
		/// </summary>
		public ServiceParamSettings[] IncludedParameters
		{
			get
			{
				return includedParams;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("PARAM SETTINGS:");
			sb.AppendLine("==========================");
			foreach (ServiceParamSettings includedElement in includedParams)
			{
				sb.AppendFormat(CultureInfo.InvariantCulture, "Included Element: {0}{1}", includedElement.Alias, Environment.NewLine);
			}

			return sb.ToString();
		}

		internal override void FillUpdate(AddServiceMessage message)
		{
			List<ServiceInfoParams> lParams = new List<ServiceInfoParams>();
			foreach (var param in includedParams)
			{
				lParams.Add(param.IncludedElement);
			}

			message.Service.ServiceParams = lParams.ToArray();
		}

		internal override void Load(ServiceInfoEventMessage serviceInfo)
		{
			includedParams = ServiceParamSettings.GetServiceParameters(serviceInfo);
		}
	}
}