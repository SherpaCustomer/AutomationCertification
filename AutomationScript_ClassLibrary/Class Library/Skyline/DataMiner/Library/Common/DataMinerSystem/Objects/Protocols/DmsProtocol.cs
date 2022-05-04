namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Net.Messages;
	using Templates;

	/// <summary>
	/// Represents a DataMiner protocol.
	/// </summary>
	internal class DmsProtocol : DmsObject, IDmsProtocol
	{
		/// <summary>
		/// The constant value 'Production'.
		/// </summary>
		private const string Production = "Production";

		/// <summary>
		/// The protocol name.
		/// </summary>
		private string name;

		/// <summary>
		/// The protocol version.
		/// </summary>
		private string version;

		/// <summary>
		/// The type of the protocol.
		/// </summary>
		private ProtocolType type;

		/// <summary>
		/// The protocol referenced version.
		/// </summary>
		private string referencedVersion;

		/// <summary>
		/// Whether the version is 'Production'.
		/// </summary>
		private bool isProduction;

		/// <summary>
		/// The connection info of the protocol.
		/// </summary>
		private IList<IDmsConnectionInfo> connectionInfo = new List<IDmsConnectionInfo>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsProtocol"/> class.
		/// </summary>
		/// <param name="dms">The DataMiner System.</param>
		/// <param name="name">The protocol name.</param>
		/// <param name="version">The protocol version.</param>
		/// <param name="type">The type of the protocol.</param>
		/// <param name="referencedVersion">The protocol referenced version.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="version"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="version"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="version"/> is not 'Production' and <paramref name="referencedVersion"/> is not the empty string ("") or white space.</exception>
		internal DmsProtocol(IDms dms, string name, string version, ProtocolType type = ProtocolType.Undefined, string referencedVersion = "")
			: base(dms)
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
				throw new ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "version");
			}

			this.name = name;
			this.version = version;
			this.type = type;
			this.isProduction = CheckIsProduction(this.version);
			if (!this.isProduction && !String.IsNullOrWhiteSpace(referencedVersion))
			{
				throw new ArgumentException("The version of the protocol is not referenced version of the protocol is not the empty string (\"\") or white space.", "referencedVersion");
			}

			this.referencedVersion = referencedVersion;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsProtocol"/> class.
		/// </summary>
		/// <param name="dms">The DataMiner system.</param>
		/// <param name="infoMessage">The information message received from SLNet.</param>
		/// <param name="requestedProduction">The version requested to SLNet.</param>
		/// <exception cref="ArgumentNullException"><paramref name="infoMessage"/> is <see langword="null"/>.</exception>
		internal DmsProtocol(IDms dms, GetProtocolInfoResponseMessage infoMessage, bool requestedProduction)
			: base(dms)
		{
			if (infoMessage == null)
			{
				throw new ArgumentNullException("infoMessage");
			}

			this.isProduction = requestedProduction;
			Parse(infoMessage);
		}

		/// <summary>
		/// Gets the connection information.
		/// </summary>
		/// <value>The connection information.</value>
		public IList<IDmsConnectionInfo> ConnectionInfo
		{
			get
			{
				LoadOnDemand();
				return new ReadOnlyCollection<IDmsConnectionInfo>(connectionInfo);
			}
		}

		/// <summary>
		/// Gets the protocol name.
		/// </summary>
		/// <value>The protocol name.</value>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// Gets the protocol version.
		/// </summary>
		/// <value>The protocol version.</value>
		public string Version
		{
			get
			{
				return version;
			}
		}

		public ProtocolType Type
		{
			get
			{
				return type; 
			}
		}

		/// <summary>
		/// Gets the protocol referenced version.
		/// </summary>
		/// <value>The protocol referenced version.</value>
		public string ReferencedVersion
		{
			get
			{
				if (String.IsNullOrEmpty(referencedVersion))
				{
					LoadOnDemand();
				}

				return referencedVersion == String.Empty ? null : referencedVersion;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the version is 'Production'.
		/// </summary>
		/// <value>Whether the version is 'Production'.</value>
		public bool IsProduction
		{
			get
			{
				return isProduction;
			}
		}

		/// <summary>
		/// Determines whether a standalone alarm template or alarm template group with the specified name exists for this protocol.
		/// </summary>
		/// <param name="templateName">Name of the alarm template.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if an alarm template with the specified name exists; otherwise, <c>false</c>.</returns>
		public bool AlarmTemplateExists(string templateName)
		{
			bool exists = false;
			AlarmTemplateEventMessage response = GetAlarmTemplateSLNet(templateName);

			if (response != null)
			{
				exists = true;
			}

			return exists;
		}

		/// <summary>
		/// Determines whether an alarm template group with the specified name exists for this protocol.
		/// </summary>
		/// <param name="templateName">Name of the alarm template.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if an alarm template group with the specified name exists; otherwise, <c>false</c>.</returns>
		public bool AlarmTemplateGroupExists(string templateName)
		{
			bool exists = false;

			AlarmTemplateEventMessage template = GetAlarmTemplateSLNet(templateName);

			if (template != null && template.Type == AlarmTemplateType.Group)
			{
				exists = true;
			}

			return exists;
		}

		/// <summary>
		/// Determines whether this protocol exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if this protocol exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return Dms.ProtocolExists(name, version);
		}

		/// <summary>
		/// Gets the alarm template with the specified name defined for this protocol.
		/// </summary>
		/// <param name="templateName">The name of the alarm template.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="AlarmTemplateNotFoundException">No alarm template with the specified name was found.</exception>
		/// <returns>The alarm template with the specified name defined for this protocol.</returns>
		public IDmsAlarmTemplate GetAlarmTemplate(string templateName)
		{
			GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
			{
				AsOneObject = true,
				Protocol = this.Name,
				Version = this.Version,
				Template = templateName
			};

			AlarmTemplateEventMessage alarmTemplateEventMessage = (AlarmTemplateEventMessage)dms.Communication.SendSingleResponseMessage(message);

			if (alarmTemplateEventMessage == null)
			{
				throw new AlarmTemplateNotFoundException(templateName, this);
			}

			if (alarmTemplateEventMessage.Type == AlarmTemplateType.Template)
			{
				return new DmsStandaloneAlarmTemplate(dms, alarmTemplateEventMessage);
			}
			else if (alarmTemplateEventMessage.Type == AlarmTemplateType.Group)
			{
				return new DmsAlarmTemplateGroup(dms, alarmTemplateEventMessage);
			}
			else
			{
				throw new NotSupportedException("Support for " + alarmTemplateEventMessage.Type + " has not yet been implemented.");
			}
		}

		/// <summary>
		/// Gets the alarm templates (standalone and groups) defined for this protocol.
		/// </summary>
		/// <returns>The alarm templates (standalone and groups) defined for this protocol.</returns>
		public ICollection<IDmsAlarmTemplate> GetAlarmTemplates()
		{
			List<IDmsAlarmTemplate> alarmTemplates = new List<IDmsAlarmTemplate>();

			GetAvailableAlarmTemplatesMessage message = new GetAvailableAlarmTemplatesMessage
			{
				IncludeGroups = true,
				IncludeTemplates = true,
				ProtocolName = name,
				ProtocolVersion = version
			};

			GetAvailableAlarmTemplatesResponse responses = (GetAvailableAlarmTemplatesResponse)Dms.Communication.SendSingleResponseMessage(message);

			foreach (AlarmTemplateMetaInfo response in responses.Templates)
			{
				DmsAlarmTemplate definition = null;
				definition = response.IsGroup ? (DmsAlarmTemplate)new DmsAlarmTemplateGroup(dms, response.Name, this) : new DmsStandaloneAlarmTemplate(dms, response.Name, this);
				alarmTemplates.Add(definition);
			}

			return alarmTemplates;
		}

		/// <summary>
		/// Gets the alarm template group with the specified name.
		/// </summary>
		/// <param name="templateName">The name of the alarm template group.</param>
		/// <returns>The alarm template group with the specified name.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="AlarmTemplateNotFoundException">No alarm template group with the specified name was found for this protocol.</exception>
		public IDmsAlarmTemplateGroup GetAlarmTemplateGroup(string templateName)
		{
			AlarmTemplateEventMessage response = GetAlarmTemplateSLNet(templateName);

			if (response != null && response.Type == AlarmTemplateType.Group)
			{
				return new DmsAlarmTemplateGroup(dms, response);
			}
			else
			{
				throw new AlarmTemplateNotFoundException(templateName, this);
			}
		}

		/// <summary>
		/// Gets the alarm template groups defined for this protocol.
		/// </summary>
		/// <returns>The alarm template groups defined for this protocol.</returns>
		public ICollection<IDmsAlarmTemplateGroup> GetAlarmTemplateGroups()
		{
			List<IDmsAlarmTemplateGroup> alarmGroups = new List<IDmsAlarmTemplateGroup>();

			GetAvailableAlarmTemplatesMessage message = new GetAvailableAlarmTemplatesMessage
			{
				IncludeGroups = true,
				IncludeTemplates = false,
				ProtocolName = name,
				ProtocolVersion = version
			};

			GetAvailableAlarmTemplatesResponse responses = (GetAvailableAlarmTemplatesResponse)Dms.Communication.SendSingleResponseMessage(message);

			foreach (AlarmTemplateMetaInfo response in responses.Templates)
			{
				DmsAlarmTemplateGroup group = new DmsAlarmTemplateGroup(dms, response.Name, this);
				alarmGroups.Add(group);
			}

			return alarmGroups;
		}

		/// <summary>
		/// Gets the standalone alarm template with the specified name defined for this protocol.
		/// </summary>
		/// <param name="templateName">The alarm template name.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="AlarmTemplateNotFoundException">No alarm template group with the specified name was found for this protocol.</exception>
		/// <returns>The standalone alarm template with the specified name.</returns>
		public IDmsStandaloneAlarmTemplate GetStandaloneAlarmTemplate(string templateName)
		{
			AlarmTemplateEventMessage template = GetAlarmTemplateSLNet(templateName);

			if (template != null && template.Type == AlarmTemplateType.Template)
			{
				return new DmsStandaloneAlarmTemplate(dms, template);
			}
			else
			{
				throw new AlarmTemplateNotFoundException(templateName, this);
			}
		}

		/// <summary>
		/// Gets the standalone alarm templates defined for this protocol.
		/// </summary>
		/// <returns>The standalone alarm templates defined for this protocol.</returns>
		public ICollection<IDmsStandaloneAlarmTemplate> GetStandaloneAlarmTemplates()
		{
			List<IDmsStandaloneAlarmTemplate> alarmTemplates = new List<IDmsStandaloneAlarmTemplate>();

			GetAvailableAlarmTemplatesMessage message = new GetAvailableAlarmTemplatesMessage
			{
				IncludeGroups = false,
				IncludeTemplates = true,
				ProtocolName = name,
				ProtocolVersion = version
			};

			GetAvailableAlarmTemplatesResponse responses = (GetAvailableAlarmTemplatesResponse)Dms.Communication.SendSingleResponseMessage(message);

			foreach (AlarmTemplateMetaInfo response in responses.Templates)
			{
				DmsStandaloneAlarmTemplate template = new DmsStandaloneAlarmTemplate(dms, response.Name, this);
				alarmTemplates.Add(template);
			}

			return alarmTemplates;
		}

		/// <summary>
		/// Gets the trend template with the specified name for this protocol.
		/// </summary>
		/// <param name="templateName">The name of the trend template.</param>
		/// <returns>The trend template with the specified name for this protocol.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="TrendTemplateNotFoundException">No trend template with the specified name was found.</exception>
		public IDmsTrendTemplate GetTrendTemplate(string templateName)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			GetTrendingTemplateInfoMessage message = new GetTrendingTemplateInfoMessage
			{
				Protocol = this.Name,
				Version = this.Version,
				Template = templateName
			};

			GetTrendingTemplateInfoResponseMessage getTrendingTemplateInfoResponseMessage = (GetTrendingTemplateInfoResponseMessage)Dms.Communication.SendSingleResponseMessage(message);

			if (getTrendingTemplateInfoResponseMessage == null)
			{
				throw new TrendTemplateNotFoundException(templateName, this);
			}

			return new DmsTrendTemplate(dms, getTrendingTemplateInfoResponseMessage);
		}

		/// <summary>
		/// Gets the trend templates defined for this protocol.
		/// </summary>
		/// <returns>The trend templates defined for this protocol.</returns>
		public ICollection<IDmsTrendTemplate> GetTrendTemplates()
		{
			List<IDmsTrendTemplate> trendTemplates = new List<IDmsTrendTemplate>();

			GetAvailableTrendTemplatesMessage message = new GetAvailableTrendTemplatesMessage
			{
				ProtocolName = name,
				ProtocolVersion = version
			};

			GetAvailableTrendTemplatesResponse responses = (GetAvailableTrendTemplatesResponse)Dms.Communication.SendSingleResponseMessage(message);

			foreach (TrendTemplateMetaInfo response in responses.Templates)
			{
				DmsTrendTemplate template = new DmsTrendTemplate(dms, response.Name, this);
				trendTemplates.Add(template);
			}

			return trendTemplates;
		}

		/// <summary>
		/// Determines whether a standalone alarm template with the specified name exists for this protocol.
		/// </summary>
		/// <param name="templateName">Name of the alarm template.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if a standalone alarm template with the specified name exists; otherwise, <c>false</c>.</returns>
		public bool StandaloneAlarmTemplateExists(string templateName)
		{
			bool exists = false;
			AlarmTemplateEventMessage template = GetAlarmTemplateSLNet(templateName);

			if (template != null && template.Type == AlarmTemplateType.Template)
			{
				exists = true;
			}

			return exists;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Protocol name: {0}, version: {1}", Name, Version);
		}

		/// <summary>
		/// Determines whether a trend template with the specified name has been defined for this protocol.
		/// </summary>
		/// <param name="templateName">The name of the trend template.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns><c>true</c> if a trend template with the specified name exists for this protocol; otherwise, <c>false</c>.</returns>
		public bool TrendTemplateExists(string templateName)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("The name of the template is the empty string (\"\") or white space.", "templateName");
			}

			GetTrendingTemplateInfoMessage message = new GetTrendingTemplateInfoMessage
			{
				Protocol = this.Name,
				Version = this.Version,
				Template = templateName
			};

			GetTrendingTemplateInfoResponseMessage getTrendingTemplateInfoResponseMessage = (GetTrendingTemplateInfoResponseMessage)Dms.Communication.SendSingleResponseMessage(message);

			if (getTrendingTemplateInfoResponseMessage == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Validate if <paramref name="version"/> is 'Production'.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <returns>Whether <paramref name="version"/> is 'Production'.</returns>
		internal static bool CheckIsProduction(string version)
		{
			return String.Equals(version, Production, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Loads the object.
		/// </summary>
		/// <exception cref="ProtocolNotFoundException">No protocol with the specified name and version exists in the DataMiner system.</exception>
		internal override void Load()
		{
			isProduction = CheckIsProduction(version);
			GetProtocolMessage getProtocolMessage = new GetProtocolMessage
			{
				Protocol = name,
				Version = version
			};

			GetProtocolInfoResponseMessage protocolInfo = (GetProtocolInfoResponseMessage)Communication.SendSingleResponseMessage(getProtocolMessage);

			if (protocolInfo != null)
			{
				Parse(protocolInfo);
			}
			else
			{
				throw new ProtocolNotFoundException(name, version);
			}
		}

		/// <summary>
		/// Parses the <see cref="GetProtocolInfoResponseMessage"/> message.
		/// </summary>
		/// <param name="protocolInfo">The protocol information.</param>
		private void Parse(GetProtocolInfoResponseMessage protocolInfo)
		{
			IsLoaded = true;

			name = protocolInfo.Name;
			type = (ProtocolType)protocolInfo.ProtocolType;
			if (isProduction)
			{
				version = Production;
				referencedVersion = protocolInfo.Version;
			}
			else
			{
				version = protocolInfo.Version;
				referencedVersion = String.Empty;
			}

			ParseConnectionInfo(protocolInfo);
		}

		/// <summary>
		/// Parses the <see cref="GetProtocolInfoResponseMessage"/> message.
		/// </summary>
		/// <param name="protocolInfo">The protocol information.</param>
		private void ParseConnectionInfo(GetProtocolInfoResponseMessage protocolInfo)
		{
			List<DmsConnectionInfo> info = new List<DmsConnectionInfo>();
			info.Add(new DmsConnectionInfo(String.Empty, EnumMapper.ConvertStringToConnectionType(protocolInfo.Type)));
			if (protocolInfo.AdvancedTypes != null && protocolInfo.AdvancedTypes.Length > 0 && !String.IsNullOrWhiteSpace(protocolInfo.AdvancedTypes))
			{
				string[] split = protocolInfo.AdvancedTypes.Split(';');
				foreach (string part in split)
				{
					if (part.Contains(":"))
					{
						string[] connectionSplit = part.Split(':');

						ConnectionType connectionType = EnumMapper.ConvertStringToConnectionType(connectionSplit[0]);
						string connectionName = connectionSplit[1];
						info.Add(new DmsConnectionInfo(connectionName, connectionType));
					}
					else
					{
						ConnectionType connectionType = EnumMapper.ConvertStringToConnectionType(part);
						string connectionName = String.Empty;
						info.Add(new DmsConnectionInfo(connectionName, connectionType));
					}
				}
			}

			connectionInfo = info.ToArray();
		}

		/// <summary>
		/// Gets the alarm template via SLNet.
		/// </summary>
		/// <param name="templateName">The name of the alarm template.</param>
		/// <exception cref="ArgumentNullException"><paramref name="templateName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="templateName"/> is the empty string ("") or white space.</exception>
		/// <returns>The AlarmTemplateEventMessage object.</returns>
		private AlarmTemplateEventMessage GetAlarmTemplateSLNet(string templateName)
		{
			if (templateName == null)
			{
				throw new ArgumentNullException("templateName");
			}

			if (String.IsNullOrWhiteSpace(templateName))
			{
				throw new ArgumentException("Provided template name must not be the empty string (\"\") or white space", "templateName");
			}

			GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
			{
				AsOneObject = true,
				Protocol = this.Name,
				Template = templateName,
				Version = this.Version
			};

			return (AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);
		}
	}
}