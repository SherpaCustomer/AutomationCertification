namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;

	/// <summary>
	/// Represents information about a connection.
	/// </summary>
	internal class DmsConnectionInfo : IDmsConnectionInfo
    {
        /// <summary>
        /// The name of the connection.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The connection type.
        /// </summary>
        private readonly ConnectionType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsConnectionInfo"/> class.
        /// </summary>
        /// <param name="name">The connection name.</param>
        /// <param name="type">The connection type.</param>
        internal DmsConnectionInfo(string name, ConnectionType type)
        {
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// Gets the connection name.
        /// </summary>
		/// <value>The connection name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the connection type.
        /// </summary>
		/// <value>The connection type.</value>
        public ConnectionType Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Connection with Name:{0} and Type:{1}.", name, type);
        }
    }
}