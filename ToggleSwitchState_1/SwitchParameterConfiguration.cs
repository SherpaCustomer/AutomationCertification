using System.Collections.Generic;

namespace ToggleSwitchState_1
{
	public static class SwitchParameterConfiguration
	{
		private static readonly Dictionary<string, ISwitchParameterConfiguration> configuration = new Dictionary<string, ISwitchParameterConfiguration>
		{
			{ "Cisco Switch", new CiscoSwitchParameterConfiguration() },
			{ "Arista Switch", new AristaSwitchParameterConfiguration() }
		};

		public static Dictionary<string, ISwitchParameterConfiguration> Configuration => configuration;
	}

	public interface ISwitchParameterConfiguration
	{
		int InterfaceTableParameterId { get; }

		int InterfaceTableAdminStatusColumnReadParameterId { get; }

		int InterfaceTableAdminStatusColumnWriteParameterId { get; }
	}

	public class CiscoSwitchParameterConfiguration : ISwitchParameterConfiguration
	{
		public int InterfaceTableParameterId => 1000;

		public int InterfaceTableAdminStatusColumnReadParameterId => 1007;

		public int InterfaceTableAdminStatusColumnWriteParameterId => 1107;
	}

	public class AristaSwitchParameterConfiguration : ISwitchParameterConfiguration
	{
		public int InterfaceTableParameterId => 11000;

		public int InterfaceTableAdminStatusColumnReadParameterId => 11007;

		public int InterfaceTableAdminStatusColumnWriteParameterId => 11107;
	}
}