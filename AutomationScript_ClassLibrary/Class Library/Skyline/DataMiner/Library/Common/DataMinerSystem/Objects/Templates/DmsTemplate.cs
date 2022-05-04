namespace Skyline.DataMiner.Library.Common.Templates
{
    using System;

    /// <summary>
    /// Represents an alarm template.
    /// </summary>
    internal abstract class DmsTemplate : DmsObject
    {
        /// <summary>
        /// Alarm template name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The protocol this alarm template corresponds with.
        /// </summary>
        private readonly IDmsProtocol protocol;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsTemplate"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="name">The name of the alarm template.</param>
		/// <param name="protocol">Instance of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocol"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		protected DmsTemplate(IDms dms, string name, IDmsProtocol protocol)
            : base(dms)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (protocol == null)
            {
                throw new ArgumentNullException("protocol");
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name of the template is the empty string (\"\") or white space.");
            }

            this.name = name;
            this.protocol = protocol;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsTemplate"/> class.
		/// </summary>
		/// <param name="dms">The DataMiner System reference.</param>
		/// <param name="name">The template name.</param>
		/// <param name="protocolName">The name of the protocol.</param>
		/// <param name="protocolVersion">The version of the protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocolName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="protocolVersion"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="protocolName"/> is the empty string ("") or white space.</exception>
		/// <exception cref="ArgumentException"><paramref name="protocolVersion"/> is the empty string ("") or white space.</exception>
		protected DmsTemplate(IDms dms, string name, string protocolName, string protocolVersion)
            : base(dms)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (protocolName == null)
            {
                throw new ArgumentNullException("protocolName");
            }

            if (protocolVersion == null)
            {
                throw new ArgumentNullException("protocolVersion");
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name of the template is the empty string(\"\") or white space.", "name");
            }

            if (String.IsNullOrWhiteSpace(protocolName))
            {
                throw new ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "protocolName");
            }

            if (String.IsNullOrWhiteSpace(protocolVersion))
            {
                throw new ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "protocolVersion");
            }

            this.name = name;
            protocol = new DmsProtocol(dms, protocolName, protocolVersion);
        }

        /// <summary>
        /// Gets the template name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the protocol this template corresponds with.
        /// </summary>
        public IDmsProtocol Protocol
        {
            get
            {
                return protocol;
            }
        }
    }
}