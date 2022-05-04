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
using ToggleSwitchState_1;

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
		bool succeeded = true;
		try
		{
			var switchElementId = engine.GetScriptParam("SwitchElementId").Value;
			var switchObject = GetSwitch(engine, switchElementId);
			if (switchObject == null)
			{
				succeeded = false;
				return;
			}

			switchObject.ToggleState(engine);
		}
		catch (Exception e)
		{
			engine.Log($"Exception toggling state: {e}");
			succeeded = false;
		}
		finally
		{
			engine.AddOrUpdateScriptOutput("succeeded", succeeded.ToString());
		}
	}

	private static Switch GetSwitch(IEngine engine, string id)
	{
		IActionableElement element = null;
		try
		{
			element = engine.FindElementByKey(id);
		}
		catch (Exception e)
		{
			engine.Log($"Exception retrieving element {id}: {e}");
			return null;
		}

		var typePropertyValue = element.GetPropertyValue("Type");
		switch (typePropertyValue)
		{
			case "Leaf":
				return new LeafSwitch(element);
			case "Spine":
				return new SpineSwitch(element);
			default:
				engine.Log($"Unsupported type: {typePropertyValue}");
				return null;
		}
	}
}