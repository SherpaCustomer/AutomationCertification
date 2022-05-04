using System.ComponentModel;
using System.Reflection;

namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;

	using System.Globalization;
	using System.Linq;

	using Net.Messages;
	using Net.Messages.Advanced;
	using Properties;

	/// <summary>
	/// Class containing helper methods.
	/// </summary>
	internal static class HelperClass
	{
		/// <summary>
		/// Checks the element state and throws an exception if no operation is possible due to the current element state.
		/// </summary>
		/// <param name="element">The element on which to check the state.</param>
		/// <exception cref="ElementNotFoundException">The element was not found in the DataMiner system.</exception>
		/// <exception cref="ElementStoppedException">The element is stopped.</exception>
		internal static void CheckElementState(IDmsElement element)
		{
			if (element.State == ElementState.Deleted)
			{
				throw new ElementNotFoundException(String.Format(CultureInfo.InvariantCulture, "Failed to perform an operation on the element: {0} because it has been deleted.", element.Name));
			}

			if (element.State == ElementState.Stopped)
			{
				throw new ElementStoppedException(String.Format(CultureInfo.InvariantCulture, "Failed to perform an operation on the element: {0} because it has been stopped.", element.Name));
			}
		}

		/// <summary>
		/// Converts provided enumerable to an array string.
		/// </summary>
		/// <param name="primaryKeys">The primary keys.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <exception cref="ArgumentNullException"><paramref name="primaryKeys"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The provided primary keys contains null references.</exception>
		/// <returns>The array string.</returns>
		internal static string[] ToStringArray(IEnumerable<string> primaryKeys, string parameterName)
		{
			string[] keys = null;

			if (primaryKeys == null)
			{
				throw new ArgumentNullException(parameterName);
			}

			ICollection<string> collection = primaryKeys as ICollection<string>;

			if (collection != null)
			{
				if (collection.Contains(null))
				{
					throw new ArgumentException("The provided values must not contain null references.", parameterName);
				}

				keys = collection.ToArray();
			}
			else
			{
				List<string> list = new List<string>();

				foreach (string primaryKey in primaryKeys)
				{
					if (primaryKey == null)
					{
						throw new ArgumentException("The provided values must not contain null references.", parameterName);
					}
					else
					{
						list.Add(primaryKey);
					}
				}

				keys = list.ToArray();
			}

			return keys;
		}

		/// <summary>
		/// Creates an SLNet AddElementMessage.
		/// </summary>
		/// <param name="elementConfiguration">The element configuration.</param>
		/// <returns>The <see cref="AddElementMessage"/> message instance.</returns>
		internal static AddElementMessage CreateAddElementMessage(ElementConfiguration elementConfiguration,bool isCompatibilityIssueDetected)
		{
			AddElementMessage addMessage = new AddElementMessage
				                               {
					                               AlarmTemplate = elementConfiguration.AlarmTemplate != null ? elementConfiguration.AlarmTemplate.Name : String.Empty,
					                               CreateDVEs = elementConfiguration.DveSettings.IsDveCreationEnabled,
					                               Description = elementConfiguration.Description,
					                               ElementID = -1,
					                               ElementName = elementConfiguration.Name,
					                               //ForceAgent = elementConfiguration.FailoverSettings.ForceAgent,
					                               IsHidden = elementConfiguration.AdvancedSettings.IsHidden,
					                               IsReadOnly = elementConfiguration.AdvancedSettings.IsReadOnly,
					                               //KeepOnline = elementConfiguration.FailoverSettings.KeepOnline,
					                               ProtocolName = elementConfiguration.Protocol.Name,
					                               ProtocolVersion = elementConfiguration.Protocol.Version,
					                               State = GetSLNetEnumValue(elementConfiguration.State),
					                               TimeoutTime = (int)elementConfiguration.AdvancedSettings.Timeout.TotalMilliseconds,
					                               TrendTemplate = elementConfiguration.TrendTemplate != null ? elementConfiguration.TrendTemplate.Name : String.Empty,
					                               Type = elementConfiguration.Type,
				                               };

			//todo: should be in model
			addMessage.SlowPollBase = "NO";
			addMessage.PingInterval = 30000;
			addMessage.KeepOnline = false;

			// Build array with view IDs.
			int viewCount = elementConfiguration.Views.Count;
			if (elementConfiguration.Views != null && viewCount > 0)
			{
				int[] viewIds = new int[viewCount];
				for (int i = 0; i < viewCount; i++)
				{
					viewIds[i] = elementConfiguration.Views.ElementAt(i).Id;
				}

				addMessage.ViewIDs = viewIds;
			}

			ICollection<PropertyInfo> propertyInfo = new List<PropertyInfo>();
			if (elementConfiguration.UpdatedProperties.Count() != 0)
			{
				foreach (string updatedProperty in elementConfiguration.UpdatedProperties)
				{
					PropertyConfiguration propertyConfiguration = elementConfiguration.Properties[updatedProperty];
					propertyInfo.Add(new PropertyInfo
						                 {
							                 DataType = "Element",
							                 Name = propertyConfiguration.Definition.Name,
							                 Value = propertyConfiguration.Value
						                 });
				}

				addMessage.Properties = propertyInfo.ToArray();
			}

			IEnumerable<ElementPortInfo> portInfoMessages = elementConfiguration.Connections.CreatePortInfo(isCompatibilityIssueDetected);
			addMessage.Ports = portInfoMessages.ToArray();

			return addMessage;
		}

		/// <summary>
		/// Helper method to extract the ElementPortInfo information from an ElementInfoEventMessage object.
		/// </summary>
		/// <param name="message">The ElementInfoEventMessage</param>
		/// <returns>List of <see cref="ElementPortInfo"/></returns>
		internal static ElementPortInfo[] ObtainElementPortInfos(ElementInfoEventMessage message)
		{
			int count = 1;
			if (message.ExtraPorts != null)
			{
				count += message.ExtraPorts.Length;
			}

			ElementPortInfo[] allPortInfo = new ElementPortInfo[count];

			var mainPortInfo = new ElementPortInfo();

			if(message.MainPort != null)
			{
				mainPortInfo.IsSslTlsEnabled = message.MainPort.IsSslTlsEnabled;
			}

			allPortInfo[0] = mainPortInfo;

			if (message.ExtraPorts != null)
			{
				for (int i = 0; i < message.ExtraPorts.Length; i++)
				{
					var extraPort = new ElementPortInfo();

					if(message.ExtraPorts[i] != null)
					{
						extraPort.IsSslTlsEnabled = message.ExtraPorts[i].IsSslTlsEnabled;
					}

					allPortInfo[i + 1] = extraPort;
				}
			}

			return allPortInfo;
		}

		/// <summary>
		/// Creates a <see cref="SetDataMinerInfoMessage"/> message.
		/// </summary>
		/// <param name="viewConfiguration">The view configuration.</param>
		/// <returns>The <see cref="SetDataMinerInfoMessage"/> message.</returns>
		internal static SetDataMinerInfoMessage CreateAddOrUpdateMessage(ViewConfiguration viewConfiguration)
		{
			SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
				                                  {
					                                  Sa1 = new SA { Sa = new string[2] { viewConfiguration.Name, viewConfiguration.Parent.Id + String.Empty } },
					                                  What = 2
				                                  };

			return message;
		}

		/// <summary>
		/// Creates a <see cref="SetDataMinerInfoMessage"/> SLNet message.
		/// </summary>
		/// <param name="view">The view to add or create.</param>
		/// <returns>The <see cref="SetDataMinerInfoMessage"/> SLNet message.</returns>
		internal static SetDataMinerInfoMessage CreateAddOrUpdateMessage(IDmsView view)
		{
			SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
				                                  {
					                                  Sa1 = new SA { Sa = new string[2] { view.Name, view.Parent.Id + String.Empty } },
					                                  What = 2
				                                  };

			return message;
		}

		/// <summary>
		/// Creates an <see cref="AddElementMessage"/> message.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="element">The element.</param>
		/// <param name="isCompatibilityIssueDetected"></param>
		/// <returns>The <see cref="AddElementMessage"/> message.</returns>
		internal static AddElementMessage CreateAddElementMessage(IDms dms, IDmsElement element,bool isCompatibilityIssueDetected)
		{
			AddElementMessage addMessage = new AddElementMessage
				                               {
					                               AlarmTemplate = element.AlarmTemplate == null ? String.Empty : element.AlarmTemplate.Name,
					                               CreateDVEs = element.DveSettings.IsDveCreationEnabled,
					                               DataMinerID = element.AgentId,
					                               Description = element.Description,
					                               ElementName = element.Name,
					                               ElementID = element.Id,
					                               //ForceAgent = element.FailoverSettings.ForceAgent,
					                               IsHidden = element.AdvancedSettings.IsHidden,
					                               IsReadOnly = element.AdvancedSettings.IsReadOnly,
					                               //KeepOnline = element.FailoverSettings.KeepOnline,
					                               ProtocolName = element.Protocol.Name,
					                               ProtocolVersion = element.Protocol.Version,
					                               State = (Net.Messages.ElementState)element.State,
					                               TimeoutTime = (int)element.AdvancedSettings.Timeout.TotalMilliseconds,
					                               TrendTemplate = element.TrendTemplate == null ? String.Empty : element.TrendTemplate.Name,
					                               Type = element.Type,
				

					                               ReplicationDomain = element.ReplicationSettings.Domain,
					                               ReplicationHost = element.ReplicationSettings.IPAddressSourceAgent,
					                               //ReplicationOptions = element.ReplicationSettings.Options,
					                               ReplicationPassword = element.ReplicationSettings.Password,
					                               ReplicationRemoteElement = element.ReplicationSettings.SourceDmsElementId.AgentId + "/" + element.ReplicationSettings.SourceDmsElementId.ElementId,
					                               ReplicationUser = element.ReplicationSettings.UserName
				                               };

			int i = 0;

			// Build array with view IDs.
			int viewCount = element.Views.Count;
			if (element.Views != null && viewCount > 0)
			{
				addMessage.ViewIDs = element.Views.Select(v => v.Id).ToArray();
			}

			i = 0;
			addMessage.Properties = new PropertyInfo[dms.ElementPropertyDefinitions.Count];

			foreach (IDmsElementPropertyDefinition definition in dms.ElementPropertyDefinitions)
			{
				var elementProperty = element.Properties[definition.Name];

				var property = new PropertyInfo
				{
					DataType = "Element",
					Name = definition.Name,
					Value = elementProperty.Value
				};

				if (elementProperty is IWritableProperty)
				{
					property.AccessType = PropertyAccessType.ReadWrite;
				}
				else
				{
					property.AccessType = PropertyAccessType.ReadOnly;
				}

				addMessage.Properties[i] = property;

				i++;
			}

			// Build port info objects.
			ElementConnectionCollection connections = element.Connections as ElementConnectionCollection;
			//ElementConnectionCollection connectionCollection;
			IEnumerable<ElementPortInfo> portInfo = connections.CreatePortInfo(isCompatibilityIssueDetected);
			addMessage.Ports = portInfo.ToArray();

			return addMessage;
		}

		/// <summary>
		/// Returns the string representation (defined by software) based on the State enumeration.
		/// </summary>
		/// <param name="state">The element state.</param>
		/// <returns>The string representation of the element state.</returns>
		internal static string GetElementStateRepresentation(this ElementState state)
		{
			string returnValue;

			switch (state)
			{
				case ElementState.Active:
					returnValue = "active";
					break;
				case ElementState.Paused:
					returnValue = "inactive";
					break;
				case ElementState.Stopped:
					returnValue = "stop";
					break;
				default:
					returnValue = String.Empty;
					break;
			}

			return returnValue;
		}

		/// <summary>
		/// Helper method to convert a string value to an enumeration value.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to convert to.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="ignoreCase">If true, cases will be ignored when comparing the value.</param>
		/// <param name="defaultValue">The default value of the enum when the parsing or matching fails.</param>
		/// <returns>The corresponding enumeration value.</returns>
		internal static TEnum ParseEnum<TEnum>(this string value, bool ignoreCase, TEnum defaultValue) where TEnum : struct
		{
			TEnum result;

			if (!Enum.TryParse(value, ignoreCase, out result))
			{
				result = defaultValue;
			}

			return result;
		}

		/// <summary>
		/// Gets the corresponding SLNet element state enumeration value.
		/// </summary>
		/// <param name="state">The element state.</param>
		/// <returns>The corresponding SLNet element state enumeration value.</returns>
		private static Net.Messages.ElementState GetSLNetEnumValue(ConfigurationElementState state)
		{
			switch (state)
			{
				case ConfigurationElementState.Active:
					return Net.Messages.ElementState.Active;

				case ConfigurationElementState.Paused:
					return Net.Messages.ElementState.Paused;

				case ConfigurationElementState.Stopped:
					return Net.Messages.ElementState.Stopped;

				default:
					return Net.Messages.ElementState.Undefined;
			}
		}

		/// <summary>
		/// Determines if a connection is using a dedicated connection or not (e.g serial single, smart serial single).
		/// </summary>
		/// <param name="info">ElementPortInfo</param>
		/// <returns>Whether a connection is marked as single or not.</returns>
		internal static bool IsDedicatedConnection(ElementPortInfo info)
		{
			bool isDedicatedConnection = false;

			switch (info.ProtocolType)
			{
				case Net.Messages.ProtocolType.SerialSingle:
				case Net.Messages.ProtocolType.SmartSerialRawSingle:
				case Net.Messages.ProtocolType.SmartSerialSingle:
					isDedicatedConnection = true;
					break;
				default:
					isDedicatedConnection = false;
					break;
			}

			return isDedicatedConnection;
		}
	}
}