namespace Skyline.DataMiner.Library.Common.Attributes
{

	/// <summary>
	/// This attribute indicates a DLL is required.
	/// </summary>
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public sealed class DllImportAttribute : System.Attribute
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DllImportAttribute"/> class.
		/// </summary>
		/// <param name="dllImport">The name of the DLL to be imported.</param>
		public DllImportAttribute (string dllImport)
		{
			DllImport = dllImport;
		}

		/// <summary>
		/// Gets the name of the DLL to be imported.
		/// </summary>
		public string DllImport { get; private set; }
	}
}