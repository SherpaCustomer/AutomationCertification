namespace Skyline.DataMiner.Library.Common.Templates
{
    using System;
    using System.Globalization;
    using Net.Messages;

    /// <summary>
    /// Represents a trend template.
    /// </summary>
    internal class DmsTrendTemplate : DmsTemplate, IDmsTrendTemplate
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DmsTrendTemplate"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="name">The name of the alarm template.</param>
		/// <param name="protocol">The instance of the protocol.</param>
		/// <exception cref="ArgumentNullException">Dms is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">Name is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">Protocol is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		internal DmsTrendTemplate(IDms dms, string name, IDmsProtocol protocol)
            : base(dms, name, protocol)
        {
            IsLoaded = true;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsTrendTemplate"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="templateInfo">The template info received by SLNet.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">name is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">protocolName is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException">protocolVersion is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">name is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException">ProtocolName is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException">ProtocolVersion is the empty string ("") or white space.</exception>
		internal DmsTrendTemplate(IDms dms, GetTrendingTemplateInfoResponseMessage templateInfo)
            : base(dms, templateInfo.Name, templateInfo.Protocol, templateInfo.Version)
        {
            IsLoaded = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsTrendTemplate"/> class.
        /// </summary>
        /// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
        /// <param name="templateInfo">The template info received by SLNet.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Name is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">ProtocolName is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">ProtocolVersion is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Name is the empty string ("") or white space.</exception>
        /// <exception cref="ArgumentException">ProtocolName is the empty string ("") or white space.</exception>
        /// <exception cref="ArgumentException">ProtocolVersion is the empty string ("") or white space.</exception>
        internal DmsTrendTemplate(IDms dms, TrendTemplateMetaInfo templateInfo)
            : base(dms, templateInfo.Name, templateInfo.ProtocolName, templateInfo.ProtocolVersion)
        {
            IsLoaded = true;
        }

        /// <summary>
        /// Determines whether this trend template exists in the DataMiner System.
        /// </summary>
        /// <returns><c>true</c> if the trend template exists in the DataMiner System; otherwise, <c>false</c>.</returns>
        public override bool Exists()
        {
            GetTrendingTemplateInfoMessage message = new GetTrendingTemplateInfoMessage
            {
                Protocol = Protocol.Name,
                Version = Protocol.Version,
                Template = Name
            };

            GetTrendingTemplateInfoResponseMessage response = (GetTrendingTemplateInfoResponseMessage)Dms.Communication.SendSingleResponseMessage(message);
            return response != null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Trend Template Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
        }

		/// <summary>
		/// Loads this object.
		/// </summary>
        internal override void Load()
        {
        }
    }
}