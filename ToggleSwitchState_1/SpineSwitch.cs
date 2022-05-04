using System;
using System.Collections.Generic;
using Skyline.DataMiner.Automation;

namespace ToggleSwitchState_1
{
	public class SpineSwitch : Switch
	{
		public SpineSwitch(IActionableElement element) : base(element)
		{
			
		}

		public IEnumerable<string> EnabledInterfacePrimaryKeys
		{
			get
			{
				var interfacePrimaryKeys = Element.GetTablePrimaryKeys(ParameterConfiguration.InterfaceTableParameterId);
				foreach (var interfacePrimaryKey in interfacePrimaryKeys)
				{
					var state = Convert.ToInt32(Element.GetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnReadParameterId, interfacePrimaryKey));
					if (state == 1) yield return interfacePrimaryKey;
				}
			}
		}

		public override void ToggleState(IEngine engine)
		{
			if (!IsActive)
			{
				var otherSpineSwitch = GetActiveSpineFromSameLocation(engine);

				// copy configuration from active spine
				CopyInterfaceConfiguration(otherSpineSwitch);

				// toggle active spine
				otherSpineSwitch.ToggleState(engine);

				Element.Unmask();
			}
			else
			{
				Element.Mask("Deactivated");

				// set admin status to "Down" for all interfaces
				DeactivateAllInterfaces();
			}

			IsActive = !IsActive;
			Element.SetPropertyValue("IsActive", IsActive.ToString());
		}

		private SpineSwitch GetActiveSpineFromSameLocation(IEngine engine)
		{
			var locationView = Element.FindView();
			var switchElements = engine.FindElements(new ElementFilter { View = locationView  });

			foreach (var switchElement in switchElements)
			{
				var typePropertyValue = switchElement.GetPropertyValue("Type");
				if (typePropertyValue != "Spine") continue;

				var isActive = switchElement.GetPropertyValue("IsActive") == "True";
				if (isActive) return new SpineSwitch(switchElement);
			}

			return null;
		}

		private void CopyInterfaceConfiguration(SpineSwitch otherSpineSwitch)
		{
			foreach (var interfacePrimaryKey in otherSpineSwitch.EnabledInterfacePrimaryKeys)
			{
				Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, interfacePrimaryKey, 1);
			}
		}

		private void DeactivateAllInterfaces()
		{
			var interfacePrimaryKeys = Element.GetTablePrimaryKeys(ParameterConfiguration.InterfaceTableParameterId);
			foreach (var interfacePrimaryKey in interfacePrimaryKeys)
			{
				var state = Convert.ToInt32(Element.GetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnReadParameterId, interfacePrimaryKey));
				if (state != 2)
				{
					Element.SetParameterByPrimaryKey(ParameterConfiguration.InterfaceTableAdminStatusColumnWriteParameterId, interfacePrimaryKey, 2);
				}
			}
		}
	}
}