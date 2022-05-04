namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;

	/// <summary>
	/// Represents an element configuration.
	/// </summary>
	internal class FailoverElementConfiguration
    {
        /// <summary>
        /// Gets or sets whether the agent is forced.
        /// </summary>
        public string ForceAgent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element must be kept online.
        /// </summary>
        public bool KeepOnline
        {
            get;
            set;
        }

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat(CultureInfo.InvariantCulture, "ForceAgent: {0}{1}", ForceAgent, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "KeepOnline: {0}{1}", KeepOnline, Environment.NewLine);

			return sb.ToString();
		}
	}
}