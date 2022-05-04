using System;
using Skyline.DataMiner.Automation;

namespace ToggleSwitchState_1
{
	public class LeafSwitch : Switch
	{
		public LeafSwitch(IActionableElement element) : base(element)
		{

		}

		public override void ToggleState(IEngine engine)
		{
			IsActive = !IsActive;
			Element.SetPropertyValue("IsActive", IsActive.ToString());

			// set admin status to "Up" for default interfaces
			UpdateInterfaceStates();

			// set alarm template
			var alarmTemplate = IsActive ? "Leaf_Default" : String.Empty;
			Element.SetAlarmTemplate(alarmTemplate);
		}

		private void UpdateInterfaceStates()
		{
			var newValue = IsActive ? 1 : 2;

			Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, "1", newValue);
			Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, "2", newValue);
			Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, "3", newValue);
			Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, "49", newValue);
			Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, "50", newValue);
		}
	}
}