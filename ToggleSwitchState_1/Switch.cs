using Skyline.DataMiner.Automation;

namespace ToggleSwitchState_1
{
	public abstract class Switch
	{
		protected readonly IActionableElement Element;
		protected readonly ISwitchParameterConfiguration ParameterConfiguration;

		protected Switch(IActionableElement element)
		{
			Element = element;
			ParameterConfiguration = SwitchParameterConfiguration.Configuration[Element.ProtocolName];

			Location = Element.GetPropertyValue("Location");
			IsActive = Element.GetPropertyValue("IsActive") == "True";
		}

		public string Location { get; protected set; }

		public bool IsActive { get; protected set; }

		public abstract void ToggleState(IEngine engine);
	}
}