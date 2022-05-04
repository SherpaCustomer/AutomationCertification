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

16/02/2022	1.0.0.1		JVT, Skyline	Initial version
****************************************************************************
*/

using System;
using AddNewSwitch_1;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;
using Skyline.DataMiner.Library.Automation;
using Skyline.DataMiner.Library.Common;

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
		var controller = new InteractiveController(engine);

		var model = new Model(engine.GetDms());
		var addSwitchView = new AddSwitchView(engine);
		var addSwitchViewPresenter = new AddSwitchViewPresenter(addSwitchView, model);

		addSwitchViewPresenter.Create += (sender, args) =>
		{
			CreateLeafSwitchElement(engine, model);
		};

		addSwitchView.Show(false);
		addSwitchViewPresenter.LoadFromModel();

		controller.Run(addSwitchView);
	}

	private static void CreateLeafSwitchElement(IEngine engine, Model model)
	{
		string message = null;
		try
		{
			var element = model.CreateSwitchElement(engine);
			if (element == null)
			{
				message = $"Switch element {model.Name} was not successfully created (in time).";
				return;
			}

			if (model.ActivateImmediately)
			{
				var succeeded = ExecuteToggleSwitchState(engine, element);
				message = $"Switch element {model.Name} was {(succeeded ? "" : "not")} successfully created and activated.";
			}
			else
			{
				message = $"Switch element {model.Name} was successfully created.";
			}
		}
		catch (Exception e)
		{
			message = $"Exception creating switch element {model.Name}: ${e}.";
		}
		finally
		{
			ShowResult(engine, message);
		}
	}

	private static bool ExecuteToggleSwitchState(IEngine engine, IDmsElement element)
	{
		try
		{
			var script = engine.PrepareSubScript("ToggleSwitchState");
			script.Synchronous = true;

			script.SelectScriptParam("SwitchElementId", element.DmsElementId.Value);

			script.StartScript();

			var results = script.GetScriptResult();
			return results.TryGetValue("succeeded", out var succeeded) && Convert.ToBoolean(succeeded);
		}
		catch (Exception e)
		{
			engine.Log("Exception executing ToggleSwitchState script: " + e);
		}

		return false;
	}

	private static void ShowResult(IEngine engine, string message)
	{
		var resultsView = new MessageDialog(engine, message);
		resultsView.Title = "Add New Leaf";
		resultsView.Show();

		engine.ExitSuccess("finished");
	}
}