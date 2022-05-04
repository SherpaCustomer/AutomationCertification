namespace Skyline.DataMiner.Library.Common.Templates
{
	using System;
	using Net.Exceptions;
	using Net.Messages;

	/// <summary>
	/// Base class for standalone alarm templates and alarm template groups.
	/// </summary>
	internal abstract class DmsAlarmTemplate : DmsTemplate, IDmsAlarmTemplate
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DmsAlarmTemplate"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="name">The name of the alarm template.</param>
		/// <param name="protocol">Instance of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		protected DmsAlarmTemplate(IDms dms, string name, IDmsProtocol protocol)
			: base(dms, name, protocol)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsAlarmTemplate"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="name">The name of the alarm template.</param>
		/// <param name="protocolName">The name of the protocol.</param>
		/// <param name="protocolVersion">The version of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocolName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocolVersion"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="protocolName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="protocolVersion"/> is the empty string ("") or white space.</exception>
		protected DmsAlarmTemplate(IDms dms, string name, string protocolName, string protocolVersion)
			: base(dms, name, protocolName, protocolVersion)
		{
		}

		/// <summary>
		/// Loads all the data and properties found related to the alarm template.
		/// </summary>
		/// <exception cref="TemplateNotFoundException">The template does not exist in the DataMiner system.</exception>
		internal override void Load()
		{
			GetAlarmTemplateMessage message = new GetAlarmTemplateMessage
			{
				AsOneObject = true,
				Protocol = Protocol.Name,
				Version = Protocol.Version,
				Template = Name
			};

			AlarmTemplateEventMessage response = (AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);

			if (response != null)
			{
				Parse(response);
			}
			else
			{
				throw new TemplateNotFoundException(Name, Protocol.Name, Protocol.Version);
			}
		}

		/// <summary>
		/// Parses the alarm template event message.
		/// </summary>
		/// <param name="message">The message received from SLNet.</param>
		internal abstract void Parse(AlarmTemplateEventMessage message);
	}
}