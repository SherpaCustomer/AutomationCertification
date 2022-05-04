using Skyline.DataMiner.Automation;

namespace SwitchToBackupSpine_1
{
	public static class InteractiveHelper
	{
		public static bool GetUserConfirmation(IEngine engine, string message)
		{
			if (!AttachUser(engine, "User confirmation required")) return false;

			return Confirm(engine, message);
		}

		private static bool AttachUser(IEngine engine, string message)
		{
			var isUserAttached = engine.FindInteractiveClient(message, 30);
			if (!isUserAttached)
			{
				engine.Log("No user attached (in time)");
				return false;
			}

			return true;
		}

		private static bool Confirm(IEngine engine, string message)
		{
			var uiBuilder = new UIBuilder();

			uiBuilder.Append(message);
			uiBuilder.AppendButton("okBtn", "OK");
			uiBuilder.AppendButton("cancelBtn", "Cancel");

			var uiResults = engine.ShowUI(uiBuilder);
			return uiResults.WasButtonPressed("okBtn");
		}
	}
}