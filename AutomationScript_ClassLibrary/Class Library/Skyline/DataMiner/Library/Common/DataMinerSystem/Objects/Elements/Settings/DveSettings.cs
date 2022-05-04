namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Globalization;
	using System.Text;
	using Net.Messages;

	/// <summary>
	/// Represents DVE information of an element.
	/// </summary>
	internal class DveSettings : ElementSettings, IDveSettings
	{
		/// <summary>
		/// Value indicating whether DVE creation is enabled.
		/// </summary>
		private bool isDveCreationEnabled = true;

		/// <summary>
		/// Value indicating whether this element is a parent DVE.
		/// </summary>
		private bool isParent;

		/// <summary>
		/// The parent element.
		/// </summary>
		private IDmsElement parent;

		/// <summary>
		/// Initializes a new instance of the <see cref="DveSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the DmsElement where this object will be used in.</param>
		internal DveSettings(DmsElement dmsElement)
			: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets a value indicating whether this element is a DVE child.
		/// </summary>
		/// <value><c>true</c> if this element is a DVE child element; otherwise, <c>false</c>.</value>
		public bool IsChild
		{
			get
			{
				return parent != null;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether DVE creation is enabled for this element.
		/// </summary>
		/// <value><c>true</c> if the element DVE generation is enabled; otherwise, <c>false</c>.</value>
		/// <exception cref="NotSupportedException">The set operation is not supported: The element is not a DVE parent element.</exception>
		public bool IsDveCreationEnabled
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isDveCreationEnabled;
			}

			set
			{
				DmsElement.LoadOnDemand();

				if (!DmsElement.DveSettings.IsParent)
				{
					throw new NotSupportedException("This operation is only supported on DVE parent elements.");
				}

				if (isDveCreationEnabled != value)
				{
					ChangedPropertyList.Add("IsDveCreationEnabled");
					isDveCreationEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this element is a DVE parent.
		/// </summary>
		/// <value><c>true</c> if the element is a DVE parent element; otherwise, <c>false</c>.</value>
		public bool IsParent
		{
			get
			{
				DmsElement.LoadOnDemand();
				return isParent;
			}
		}

		/// <summary>
		/// Gets the parent element.
		/// </summary>
		/// <value>The parent element.</value>
		public IDmsElement Parent
		{
			get
			{
				DmsElement.LoadOnDemand();
				return parent;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("DVE SETTINGS:");
			sb.AppendLine("==========================");

			sb.AppendFormat(CultureInfo.InvariantCulture, "DVE creation enabled: {0}{1}", IsDveCreationEnabled, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Is parent DVE: {0}{1}", IsParent, Environment.NewLine);
			sb.AppendFormat(CultureInfo.InvariantCulture, "Is child DVE: {0}{1}", IsChild, Environment.NewLine);

			if (IsChild)
			{
				sb.AppendFormat(CultureInfo.InvariantCulture, "Parent DataMiner agent ID/element ID: {0}{1}", parent.DmsElementId.Value, Environment.NewLine);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			if (elementInfo.IsDynamicElement && elementInfo.DveParentDmaId != 0 && elementInfo.DveParentElementId != 0)
			{
				parent = new DmsElement(DmsElement.Dms, new DmsElementId(elementInfo.DveParentDmaId, elementInfo.DveParentElementId));
			}

			isParent = elementInfo.IsDveMainElement;
			isDveCreationEnabled = elementInfo.CreateDVEs;
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message that will be sent to SLNet.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
			foreach (string property in ChangedPropertyList)
			{
				if (property.Equals("IsDveCreationEnabled", StringComparison.Ordinal))
				{
					message.CreateDVEs = isDveCreationEnabled;
				}
			}
		}
	}
}