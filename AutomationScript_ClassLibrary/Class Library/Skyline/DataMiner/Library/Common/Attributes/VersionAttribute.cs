namespace Skyline.DataMiner.Library.Common.Attributes
{
	/// <summary>
	/// This attribute indicates the version.
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
	public sealed class VersionAttribute : System.Attribute
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="VersionAttribute"/> class.
		/// </summary>
		/// <param name="version">The version of this member.</param>
		public VersionAttribute (int version)
		{
			Version = version;
		}

		/// <summary>
		/// Gets the version number.
		/// </summary>
		public int Version { get; private set; }
	}

}