/*
****************************************************************************
*  Copyright (c) 2022,  Skyline Communications NV  All Rights Reserved.    *
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

dd/mm/2022	1.0.0.1		XXX, Skyline	Initial version
****************************************************************************
*/

using System;
using Skyline.DataMiner.Automation;
using SwitchToBackupSpine_1;

/// <summary>
/// DataMiner Script Class.
/// </summary>
public class Script
{
	/// <summary>
	/// The Script entry point.
	/// </summary>
	/// <param name="engine">Link with SLAutomation process.</param>
	public static void Run(IEngine engine)
	{
		var location = engine.GetScriptParam("Location").Value;

		var backupSpineSwitch = RetrieveBackupSpineSwitchElement(engine, location);
		if (backupSpineSwitch == null)
		{
			engine.Log($"No backup spine switch found in location '{location}'");
			return;
		}

		var requireUserConfirmation = engine.UserCookie.Contains("BRAIN");
		if (requireUserConfirmation)
		{
			bool isConfirmed = InteractiveHelper.GetUserConfirmation(engine, $"Please confirm active spine for location {location} can be toggled?");
			if (!isConfirmed) return;
		}

		ExecuteToggleSwitchState(engine, backupSpineSwitch);
	}

	private static IActionableElement RetrieveBackupSpineSwitchElement(IEngine engine, string location)
	{
		try
		{
			var elements = engine.FindElementsInView(location);
			foreach (var element in elements)
			{
				var typePropertyValue = element.GetPropertyValue("Type");
				if (typePropertyValue != "Spine") continue;

				var isActivePropertyValue = element.GetPropertyValue("IsActive");
				if (isActivePropertyValue != "True") return element;
			}
		}
		catch (Exception e)
		{
			engine.Log("Exception retrieving backup spine switch: " + e);
		}

		return null;
	}

	private static void ExecuteToggleSwitchState(IEngine engine, IActionableElement element)
	{
		var scriptOptions = engine.PrepareSubScript("ToggleSwitchState");
		scriptOptions.Synchronous = false;
		scriptOptions.PerformChecks = false;
		scriptOptions.SelectScriptParam("SwitchElementId", String.Join("/", element.DmaId, element.ElementId));

		scriptOptions.StartScript();
	}
}