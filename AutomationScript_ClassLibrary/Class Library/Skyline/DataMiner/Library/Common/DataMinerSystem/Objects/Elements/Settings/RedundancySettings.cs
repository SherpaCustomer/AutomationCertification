namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents the redundancy settings for a element.
	/// </summary>
	internal class RedundancySettings : ElementSettings, IRedundancySettings
	{
		/// <summary>
		/// Value indicating whether or not this element is derived from another element.
		/// </summary>
		private bool isDerived;

		/// <summary>
		/// Initializes a new instance of the <see cref="RedundancySettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the <see cref="DmsElement"/> instance this object is part of.</param>
		internal RedundancySettings(DmsElement dmsElement)
		: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether the element is derived from another element.
		/// </summary>
		/// <value><c>true</c> if the element is derived from another element; otherwise, <c>false</c>.</value>
		public bool IsDerived
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isDerived;
			}

			internal set
			{
				isDerived = value;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("REDUNDANCY SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendFormat(CultureInfo.InvariantCulture, "Derived: {0}{1}", isDerived, Environment.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			isDerived = elementInfo.IsDerivedElement;
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message which will be sent to SLNet.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
		}
	}
}