/*
****************************************************************************
*  Copyright (c) 2021,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

dd/mm/2021	1.0.0.1		XXX, Skyline	Initial version
****************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;
using Skyline.DataMiner.Library.Automation;
using Skyline.DataMiner.Library.Common;
using Skyline.DataMiner.Library.Common.Properties;
using Skyline.DataMiner.Library.Common.Templates;
using Skyline.DataMiner.Net.Messages;

/// <summary>
/// DataMiner Script Class.
/// </summary>
public class Script
{
	public static void Run(Engine engine)
	{
		try
		{
			RunSafe(engine);
		}
		catch (ScriptAbortException)
		{
			throw;
		}
		catch (Exception e)
		{
			ShowExceptionDialog(engine, e);
		}
	}

	public static void RunSafe(Engine engine)
	{
		string action = engine.GetScriptParam("Action").Value;
		engine.Log("ACTION: " + action);
		switch (action)
		{
			case "Create":
				CreateSetup(engine);
				break;

			case "Delete":
				DeleteSetup(engine);
				break;

			case "Reset":
				DeleteSetup(engine);
				Thread.Sleep(5000);
				CreateSetup(engine);
				break;

			default:
				throw new NotSupportedException($"Unknown action '{action}'.");
		}
	}

	private static void CreateSetup(Engine engine)
	{
		var dms = engine.GetDms();
		var agent = dms.GetAgents().First();

		var rootViewName = engine.GetScriptParam("View Info").Value;
		if (String.IsNullOrEmpty(rootViewName))
		{
			engine.GenerateInformation($"The entered root view cannot be null or empty.");
			return;
		}

		IDmsView rootView = null;
		try
		{
			rootView = dms.GetView(rootViewName);
		}
		catch (ViewNotFoundException)
		{
			engine.GenerateInformation($"Could not find the view '{rootViewName}'");
			return;
		}

		string locationsViewName = "Locations";
		if (!dms.ViewExists(locationsViewName) || dms.GetView(locationsViewName).Parent != rootView)
		{
			locationsViewName = CreateUniqueViewName(dms, locationsViewName);

			var locationsViewconfig = new ViewConfiguration(locationsViewName, rootView);
			int locationsViewId = dms.CreateView(locationsViewconfig);

			if (!(Extensions.Retry(() => dms.ViewExists(locationsViewId), TimeSpan.FromSeconds(30))))
			{
				engine.Log($"Failed to create the view locations underneath the '{rootViewName}'");
				return;
			}
		}

		IDmsView locationsView = dms.GetView(locationsViewName);
		engine.SendSLNetMessage(new AssignVisualToViewRequestMessage(locationsView.Id, "Switch Manager.vsdx"));

		CreateSiteView(engine, dms, agent, CreateUniqueViewName(dms, "SLA"), locationsView, 2);
		CreateSiteView(engine, dms, agent, CreateUniqueViewName(dms, "SLP"), locationsView, 3);
		CreateSiteView(engine, dms, agent, CreateUniqueViewName(dms, "SLC"), locationsView, 4);
		CreateSiteView(engine, dms, agent, CreateUniqueViewName(dms, "SLS"), locationsView, 1, false);
	}

	private static string CreateUniqueViewName(IDms dms, string viewName)
	{
		StringBuilder sbViewName = new StringBuilder(viewName);

		while (dms.ViewExists(sbViewName.ToString()))
		{
			sbViewName.Append(" - Copy");
		}

		return sbViewName.ToString();
	}

	private static void CreateSiteView(Engine engine, IDms dms, IDma agent, string location, IDmsView parentView, int amountOfLeafs, bool hasBackupSpine = true)
	{
		engine.Log("CreateSiteView: " + location);
		var siteViewConfig = new ViewConfiguration(location, parentView);
		var viewId = dms.CreateView(siteViewConfig);

		if (!Extensions.Retry(() => dms.ViewExists(viewId), TimeSpan.FromSeconds(30)))
		{
			engine.Log($"Failed to create the location view '{location}'");
			return;
		}

		var view = dms.GetView(viewId);

		var aristaSwitchProtocol = dms.GetProtocol("Arista Switch", "Production");
		var aristaSwitchDefaultLeafAlarmTemplate = aristaSwitchProtocol.AlarmTemplateExists("Leaf_Default") ? aristaSwitchProtocol.GetAlarmTemplate("Leaf_Default") : null;

		for (int i = 1; i <= amountOfLeafs; i++)
		{
			var leafSwitchElementName = $"{location}-LEAF_{i}";
			var leafSwitchElement = new SwitchElement(leafSwitchElementName, location, SwitchType.Leaf, aristaSwitchProtocol, aristaSwitchDefaultLeafAlarmTemplate);
			leafSwitchElement.CreateElement(dms, agent, view);
			leafSwitchElement.DeployConfiguration(engine);
		}

		var aristaSwitchDefaultSpineAlarmTemplate = aristaSwitchProtocol.AlarmTemplateExists("Spine_Default") ? aristaSwitchProtocol.GetAlarmTemplate("Spine_Default") : null;

		var spineSwitchElementName = $"{location}-SPINE_MAIN";
		var spineSwitchElement = new SwitchElement(spineSwitchElementName, location, SwitchType.Spine, aristaSwitchProtocol, aristaSwitchDefaultSpineAlarmTemplate);
		spineSwitchElement.CreateElement(dms, agent, view);
		spineSwitchElement.DeployConfiguration(engine);

		if (hasBackupSpine)
		{
			var ciscoSwitchProtocol = dms.GetProtocol("Cisco Switch", "Production");
			var ciscoSwitchDefaultLeafAlarmTemplate = ciscoSwitchProtocol.AlarmTemplateExists("Spine_Default") ? ciscoSwitchProtocol.GetAlarmTemplate("Spine_Default") : null;

			var backupSpineSwitchElementName = $"{location}-SPINE_BACKUP";
			var backupSpineSwitchElement = new SwitchElement(backupSpineSwitchElementName, location, SwitchType.Spine, ciscoSwitchProtocol, ciscoSwitchDefaultLeafAlarmTemplate);
			backupSpineSwitchElement.CreateElement(dms, agent, view);
			backupSpineSwitchElement.DeployConfiguration(engine);
		}
	}

	private static void DeleteSetup(Engine engine)
	{
		IDms dms = engine.GetDms();

		IDmsView rootView = GetView(dms, engine.GetScriptParam("View Info").Value);

		HashSet<IDmsElement> elementsToDelete = new HashSet<IDmsElement>();

		FindElementsToDelete(rootView, ref elementsToDelete);

		foreach (IDmsElement element in elementsToDelete)
		{
			element.Delete();
		}

		foreach (IDmsView view in rootView.ChildViews)
		{
			view.Delete();
		}
	}

	private static void FindElementsToDelete(IDmsView root, ref HashSet<IDmsElement> elementsToDelete)
	{
		foreach (IDmsElement element in root.Elements)
		{
			elementsToDelete.Add(element);
		}

		foreach (IDmsView view in root.ChildViews)
		{
			FindElementsToDelete(view, ref elementsToDelete);
		}
	}

	private static IDmsView GetView(IDms dms, string viewInfo)
	{
		if (int.TryParse(viewInfo, out int viewId) && dms.ViewExists(viewId))
		{
			return dms.GetView(viewId);
		}
		else if (dms.ViewExists(viewInfo))
		{
			return dms.GetView(viewInfo);
		}
		else
		{
			throw new ViewNotFoundException($"No view found with info '{viewInfo}'");
		}
	}

	private static void ShowExceptionDialog(Engine engine, Exception e)
	{
		ExceptionDialog exceptionDialog = new ExceptionDialog(engine, e);
		exceptionDialog.OkButton.Pressed += (sender, args) => engine.ExitFail(e.ToString());
		exceptionDialog.Show();
	}
}

public enum SwitchType
{
	Leaf = 1,
	Spine = 2
}

public interface ISwitchParameterConfiguration
{
	int SystemContactParameterId { get; }

	int SystemLocationParameterId { get; }

	int SystemNameParameterId { get; }

	int SystemDescriptionParameterId { get; }

	int ModelConfigurationParameterId { get; }

	int DeployConfigButtonParameterId { get; }

	int InterfacesModeParameterId { get; }

	int InterfacesTableParameterId { get; }

	int InterfacesTableAdminStatusParameterId { get; }

	int InterfacesTablePhysicalConnectionParameterId { get; }

	string LeafModelName { get; }

	string SpineModelName { get; }

	string SystemDescription { get; }
}

public class AristaSwitchParameterConfiguration : ISwitchParameterConfiguration
{
	public int SystemContactParameterId => 10502;

	public int SystemLocationParameterId => 10504;

	public int SystemNameParameterId => 10111;

	public int SystemDescriptionParameterId => 10113;

	public int ModelConfigurationParameterId => 10115;

	public int DeployConfigButtonParameterId => 10120;

	public int InterfacesModeParameterId => 10232;

	public int InterfacesTableParameterId => 11000;

	public int InterfacesTableAdminStatusParameterId => 11107;

	public int InterfacesTablePhysicalConnectionParameterId => 11134;

	public string LeafModelName => "7050X3";

	public string SpineModelName => "7500R3";

	public string SystemDescription => "Arista Networks EOS version 4.15.2F running on an Arista Networks DCS-7010T-48";
}

public class CiscoSwitchParameterConfiguration : ISwitchParameterConfiguration
{
	public int SystemContactParameterId => 502;

	public int SystemLocationParameterId => 504;

	public int SystemNameParameterId => 111;

	public int SystemDescriptionParameterId => 113;

	public int ModelConfigurationParameterId => 115;

	public int DeployConfigButtonParameterId => 120;

	public int InterfacesModeParameterId => 232;

	public int InterfacesTableParameterId => 1000;

	public int InterfacesTableAdminStatusParameterId => 1107;

	public int InterfacesTablePhysicalConnectionParameterId => 1134;

	public string LeafModelName => "Nexus5000";

	public string SpineModelName => "Nexus9000";

	public string SystemDescription => "Cisco NX-OS(tm) nxos.7.0.3.I3.1.bin, Software (nxos), Version 7.0(3)I3(1), RELEASE SOFTWARE Copyright (c) 2002-2013 by Cisco Systems, Inc. Compiled 2/8/2016 19:00:00";
}

public class SwitchElement
{
	private readonly ISwitchParameterConfiguration switchConfiguration;

	public SwitchElement(string name, string location, SwitchType type, IDmsProtocol protocol)
	{
		Name = name;
		Location = location;
		Type = type;

		Protocol = protocol;
		switch (protocol.Name)
		{
			case "Arista Switch":
				switchConfiguration = new AristaSwitchParameterConfiguration();
				break;
			case "Cisco Switch":
				switchConfiguration = new CiscoSwitchParameterConfiguration();
				break;
			default:
				throw new ArgumentException("Unsupported protocol: " + protocol.Name, "protocol");
		}
	}

	public SwitchElement(string name, string location, SwitchType type, IDmsProtocol protocol, IDmsAlarmTemplate alarmTemplate) : this(name, location, type, protocol)
	{
		AlarmTemplate = alarmTemplate;
	}

	public string Name { get; set; }

	public string Location { get; set; }

	public SwitchType Type { get; set; }

	public IDmsProtocol Protocol { get; set; }

	public IDmsAlarmTemplate AlarmTemplate { get; set; }

	public IDmsElement Element { get; private set; }

	public void CreateElement(IDms dms, IDma agent, IDmsView view)
	{
		ElementConfiguration switchElement = new ElementConfiguration(dms, Name, Protocol, new List<IElementConnection> { new SnmpV2Connection(new Udp("127.0.0.1", 161)) });

		switchElement.Views.Add(view);

		DmsElementId elementId = agent.CreateElement(switchElement);
		if (!Extensions.Retry(() => dms.ElementExists(elementId), TimeSpan.FromSeconds(30))) throw new ElementNotFoundException($"New switch element {Name} not created (in time)");

		Element = agent.GetElement(elementId);
	}

	public void DeployConfiguration(Engine engine)
	{
		SetProperties();

		var systemContactParameter = Element.GetStandaloneParameter<string>(switchConfiguration.SystemContactParameterId);
		systemContactParameter.SetValue("it@skyline.be");

		var systemLocationParameter = Element.GetStandaloneParameter<string>(switchConfiguration.SystemLocationParameterId);
		systemLocationParameter.SetValue(Location);

		var systemNameConfigurationParameter = Element.GetStandaloneParameter<string>(switchConfiguration.SystemNameParameterId);
		systemNameConfigurationParameter.SetValue(Name);

		var systemDescriptionConfigurationParameter = Element.GetStandaloneParameter<string>(switchConfiguration.SystemDescriptionParameterId);
		systemDescriptionConfigurationParameter.SetValue(switchConfiguration.SystemDescription);

		var modelConfigurationParameter = Element.GetStandaloneParameter<string>(switchConfiguration.ModelConfigurationParameterId);
		if (Type == SwitchType.Leaf) modelConfigurationParameter.SetValue(switchConfiguration.LeafModelName);
		else modelConfigurationParameter.SetValue(switchConfiguration.SpineModelName);

		var interfacesConfigurationModeParameter = Element.GetStandaloneParameter<double?>(switchConfiguration.InterfacesModeParameterId);
		interfacesConfigurationModeParameter.SetValue((int)Type);

		var deployConfigButtonParameter = Element.GetStandaloneParameter<double?>(switchConfiguration.DeployConfigButtonParameterId);
		deployConfigButtonParameter.SetValue(1);

		// enable interfaces
		var interfaceTable = Element.GetTable(switchConfiguration.InterfacesTableParameterId);
		var adminStatusColumn = interfaceTable.GetColumn<int?>(switchConfiguration.InterfacesTableAdminStatusParameterId);
		var physicalConnectionColumn = interfaceTable.GetColumn<int?>(switchConfiguration.InterfacesTablePhysicalConnectionParameterId);

		var interfacesToEnable = new List<string>(new[] { "1", "2", "3" });
		if (Type == SwitchType.Spine)
		{
			var isMain = Name.Contains("MAIN");
			if (!isMain)
			{
				try
				{
					// mask element
					var engineElement = engine.FindElement(Name);
					engineElement.Mask("Deactivated");
				}
				catch (Exception)
				{
					// ignore
				}
			}

			interfacesToEnable.AddRange(new[] { "129", "130" });
		}
		else
		{
			interfacesToEnable.AddRange(new[] { "49", "50" });
		}

		foreach (var interfaceToEnable in interfacesToEnable)
		{
			adminStatusColumn.SetValue(interfaceToEnable, 1);
			physicalConnectionColumn.SetValue(interfaceToEnable, 1);
		}

		Element.AlarmTemplate = AlarmTemplate;

		Element.Update();
	}

	private void SetProperties()
	{
		var locationProperty = (IWritableProperty)Element.Properties["Location"];
		if (locationProperty != null) locationProperty.Value = Location;

		var typeProperty = (IWritableProperty)Element.Properties["Type"];
		if (typeProperty != null) typeProperty.Value = Type.ToString();

		var isActiveProperty = (IWritableProperty)Element.Properties["IsActive"];
		if (isActiveProperty != null)
		{
			if (Type == SwitchType.Spine)
			{
				isActiveProperty.Value = Name.Contains("MAIN").ToString();
			}
			else
			{
				isActiveProperty.Value = "True";
			}
		}
	}
}

public static class Extensions
{
	/// <summary>
	/// Retry until success or until timeout. 
	/// </summary>
	/// <param name="func">Operation to retry.</param>
	/// <param name="timeout">Max TimeSpan during which the operation specified in <paramref name="func"/> can be retried.</param>
	/// <returns><c>true</c> if one of the retries succeeded within the specified <paramref name="timeout"/>. Otherwise <c>false</c>.</returns>
	public static bool Retry(Func<bool> func, TimeSpan timeout)
	{
		bool success = false;

		Stopwatch sw = new Stopwatch();
		sw.Start();

		do
		{
			success = func();
			if (!success)
			{
				System.Threading.Thread.Sleep(100);
			}
		}
		while (!success && sw.Elapsed <= timeout);

		return success;
	}
}