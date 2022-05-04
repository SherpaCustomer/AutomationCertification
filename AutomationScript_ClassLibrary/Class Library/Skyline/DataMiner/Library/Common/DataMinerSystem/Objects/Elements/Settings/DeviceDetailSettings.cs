namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Text;
	using Net.Messages;

	/// <summary>
	///  Represents a class containing the device details of an element.
	/// </summary>
	internal class DeviceSettings : ElementSettings
	{
		/// <summary>
		/// The type of the element.
		/// </summary>
		private string type = String.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceSettings"/> class.
		/// </summary>
		/// <param name="dmsElement">The reference to the DmsElement where this object will be used in.</param>
		internal DeviceSettings(DmsElement dmsElement)
		: base(dmsElement)
		{
		}

		/// <summary>
		/// Gets the element type.
		/// </summary>
		internal string Type
		{
			get
			{
				DmsElement.LoadOnDemand();
				return type;
			}
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("DEVICE SETTINGS:");
			sb.AppendLine("==========================");
			sb.AppendLine("Type: " + type);

			return sb.ToString();
		}

		/// <summary>
		/// Fills in the needed properties in the AddElement message.
		/// </summary>
		/// <param name="message">The AddElement message that will be sent to SLNet.</param>
		internal override void FillUpdate(AddElementMessage message)
		{
		}

		/// <summary>
		/// Loads the information to the component.
		/// </summary>
		/// <param name="elementInfo">The element information.</param>
		internal override void Load(ElementInfoEventMessage elementInfo)
		{
			type = elementInfo.Type ?? String.Empty;
		}
	}
}