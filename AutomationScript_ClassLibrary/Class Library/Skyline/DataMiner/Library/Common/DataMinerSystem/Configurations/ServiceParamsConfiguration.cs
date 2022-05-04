namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents a base class for all of the components in a DmsService object.
	/// </summary>
	internal class ServiceParamsConfiguration
	{
		private readonly Dictionary<int, ParamConfiguration> includedParams;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceParamsConfiguration"/> class. Parameter settings need to be added to create the service.
		/// </summary>
		internal ServiceParamsConfiguration()
		{
			includedParams = new Dictionary<int, ParamConfiguration>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceParamsConfiguration"/> class.
		/// </summary>
		/// <param name="includedElements">The elements and services that need to be included in the configuration.</param>
		internal ServiceParamsConfiguration(ServiceInfoParams[] includedElements)
		{
			includedParams = new Dictionary<int, ParamConfiguration>();
			foreach (var item in includedElements)
			{
				ParamConfiguration includedElement;
				int index = GetNextIndex(item.Index);
				if (item.IsService)
				{
					includedElement = new ServiceParamConfiguration(this, item, index);
				}
				else
				{
					includedElement = new ElementParamConfiguration(this, item, index);
				}

				includedParams[index] = includedElement;
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
			foreach (ParamConfiguration includedElement in includedParams.Values)
			{
				sb.AppendFormat(CultureInfo.InvariantCulture, "Included Element: ({0}){1}{2}", includedElement.Index, includedElement.Alias, Environment.NewLine);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Gets the included elements and parameters.
		/// If it is a service it can be casted to a <see cref="ServiceParamConfiguration"/>, if not to a <see cref="ElementParamConfiguration"/> instance.
		/// </summary>
		/// <returns>The included elements and/or services.</returns>
		internal ParamConfiguration[] GetIncludedElements()
		{
			return includedParams.Values.ToArray();
		}

		/// <summary>
		/// Add a service to the service.
		/// </summary>
		/// <param name="serviceId">The DataMiner/Service ID of the service you want to include in the service.</param>
		internal void AddService(DmsServiceId serviceId)
		{
			int index = GetNextIndex();
			includedParams[index] = new ServiceParamConfiguration(this, serviceId, index);
		}

		/// <summary>
		/// Add an element to the service.
		/// </summary>
		/// <param name="elementId">The DataMiner/Element ID of the element to include in the service.</param>
		/// <param name="parameters">The parameters that need to be included into the service.</param>
		internal void AddElement(DmsElementId elementId, List<ElementParamFilterConfiguration> parameters)
		{
			int index = GetNextIndex();
			includedParams[index] = new ElementParamConfiguration(this, elementId, parameters, index);
		}

		/// <summary>
		/// Indicates whether the alias is already in use by an included element or service.
		/// </summary>
		/// <param name="alias">The alias to check.</param>
		/// <returns><c>true</c> if the alias is being used for an included element or service; otherwise, <c>false</c>.</returns>
		internal bool ContainsAlias(string alias)
		{
			foreach (var includedParam in includedParams.Values)
			{
				if (includedParam.Alias == alias)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the <see cref="ServiceInfoParams"/> array for SLNet messages that represents this configuration.
		/// </summary>
		/// <returns>The <see cref="ServiceInfoParams"/> array.</returns>
		internal ServiceInfoParams[] GetServiceInfoParams()
		{
			List<ServiceInfoParams> serviceParams = new List<ServiceInfoParams>();
			foreach (var item in includedParams.Values)
			{
				serviceParams.Add(item.GetServiceInfoParams());
			}

			return serviceParams.ToArray();
		}

		/// <summary>
		/// Uses the suggested entry if not in use or gives the next index that can be used.
		/// </summary>
		/// <param name="suggested">The suggested index.</param>
		/// <returns>The index that can be used. If the suggested index is available this one will be used.</returns>
		/// <exception cref="ServiceOverflowException">The service configuration contains too many items.</exception>
		private int GetNextIndex(int suggested)
		{
			if (includedParams.ContainsKey(suggested))
			{
				return GetNextIndex();
			}

			return suggested;
		}

		/// <summary>
		/// Gives the next index that can be used.
		/// </summary>
		/// <returns>The next index that is not in use.</returns>
		/// <exception cref="ServiceOverflowException">The service configuration contains too many items.</exception>
		private int GetNextIndex()
		{
			// A service should never contain more than 1000 entries.
			for (int index = 1; index < 1000; index++)
			{
				if (!includedParams.ContainsKey(index))
				{
					return index;
				}
			}

			throw new ServiceOverflowException();
		}
	}
}