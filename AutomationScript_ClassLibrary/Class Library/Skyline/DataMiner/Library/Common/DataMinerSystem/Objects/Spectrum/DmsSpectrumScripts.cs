namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Library.Common.Subscription.Monitors;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	using System;
	using System.Collections.Generic;
	using System.Threading;

	/// <summary>
	/// Represents spectrum analyzer scripts.
	/// </summary>
	internal class DmsSpectrumAnalyzerScripts : IDmsSpectrumAnalyzerScripts
	{
		private readonly IDmsElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsSpectrumAnalyzerScripts"/> class.
		/// </summary>
		/// <param name="element">The element this spectrum analyzer component is part of.</param>
		public DmsSpectrumAnalyzerScripts(IDmsElement element)
		{
			this.element = element;
		}

		/// <summary>
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_SCRIPT (9), scriptMetaInfo, scriptStatements, out result);
		/// </summary>
		/// <param name="scriptId">The ID of the script to delete.</param>
		public void DeleteScript(int scriptId)
		{
			string[] scriptMetaInfo = new string[]
			{
				Convert.ToString(scriptId), // script id. SL_NO_ID = 2100000000 = new script
				"delete", // add/delete (also use "add" for updates to existing scripts)
				"",
				"",
				""
			};

			string[] scriptStatements = new string[0];

			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Script,
				Sa1 = new SA(scriptMetaInfo),
				Sa2 = new SA(scriptStatements)
			};

			element.Host.Dms.Communication.SendSingleResponseMessage(message);
		}

		/// <summary>
		/// Executes the specified script.
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_GETINFO (4), SPAI_EXECUTE_SCRIPT (48), scriptInfo, null, out result);
		/// WARNING: Due to SLNet limitations the Return value only supports returning the content & settings of the last GetTrace call.
		/// </summary>
		/// <param name="scriptInfo">All info needed to execute the script.</param>
		/// <returns>An object holding all the results of the script execution.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="scriptInfo"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException"><paramref name="scriptInfo"/>Script failed to execute.</exception>
		/// <exception cref="TimeoutException">Script result timed out.</exception>
		/// <remarks>If no script results are received after one minute, an <see cref="InvalidOperationException"/> is thrown.</remarks>
		public object ExecuteScript(string[] scriptInfo)
		{
			if(scriptInfo == null)
			{
				throw new ArgumentNullException("scriptInfo");
			}

			string[] saCompleteScriptInfo = new string[5];

			Array.Copy(scriptInfo, saCompleteScriptInfo, scriptInfo.Length);

			if (String.IsNullOrWhiteSpace(saCompleteScriptInfo[1])) saCompleteScriptInfo[1] = "-1";

			// 0x01: (0000 0001)return monitored variables only
			// 0x02: (0000 0010)return RAW value (no units)
			// 0x04: (0000 0100)manual script execution called by Normalize function
			// 0x08: (0000 1000)prefix returned variable names with measpt id: id>varname
			// 0x10: (0001 0000)don't send traces in client result

			uint options = 0x08 | 0x04;

			saCompleteScriptInfo[2] = Convert.ToString(options);
			saCompleteScriptInfo[3] = String.Empty;
			saCompleteScriptInfo[4] = String.Empty;

			//-SA1
			//o[0] = ScriptID
			//o[1] = Measurement Points(; separated. - 1 when none)
			//o[2] = Options bit mask
			//o[3] = Returned variable filter(; separated list of variable names.Empty when no filter)
			//o[4] = Preset names(| separated)

			string subsessionId = "subses_" + System.Guid.NewGuid();
			string uniqueSessionName = "sesName_" + System.Guid.NewGuid();

			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.EnterScriptMode,
				Sa1 = new SA(saCompleteScriptInfo),
				SubSessionID = subsessionId,
				UniqueSessionName = uniqueSessionName
			};

			OpenSpectrumAnalyzerMessage startClientMsg = new OpenSpectrumAnalyzerMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				SubSessionID = subsessionId,
				UniqueSessionName = uniqueSessionName
			};

			CloseSpectrumAnalyzerMessage stopClientMsg = new CloseSpectrumAnalyzerMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				SubSessionID = subsessionId,
				UniqueSessionName = uniqueSessionName
			};

			ExecuteScriptResultHandler handlerWrapper = new ExecuteScriptResultHandler(element);
			var handler = handlerWrapper.CreateExecuteScriptHandler(element, subsessionId, element.AgentId, element.Id);
			bool handlerAdded = false;

			try
			{
				element.Host.Dms.Communication.AddSubscriptionHandler(handler);
				handlerAdded = true;

				element.Host.Dms.Communication.SendSingleRawResponseMessage(startClientMsg);
				Thread.Sleep(3000);
				element.Host.Dms.Communication.SendSingleRawResponseMessage(message);

				if (!handlerWrapper.WaitFlag.WaitOne(60000)) throw new TimeoutException("Script result timed out.");
			}
			finally
			{
				if (handlerAdded)
				{
					if (!SubscriptionManager.EmptyHandlerExists)
					{
						// Add empty handler if it doesn't exist, to avoid SLNet Connection crashes.
						element.Host.Dms.Communication.AddSubscriptionHandler((sender, e) => { });
						SubscriptionManager.EmptyHandlerExists = true;
					}

					element.Host.Dms.Communication.ClearSubscriptionHandler(handler);
				}

				element.Host.Dms.Communication.SendSingleRawResponseMessage(stopClientMsg);
			}

			// Need an object that is:
			// object[object[string[]]]
			// Example of rsp: SUCCESS=trace=TRACE=startF=125000000.000000 Hz=stopF=175000000.000000 Hz

			string[] variablesFromSlnet = handlerWrapper.ExecuteScriptResults.Split('=');

			if (variablesFromSlnet[0] != "SUCCESS") throw new InvalidOperationException("Script failed to execute with result: " + handlerWrapper.ExecuteScriptResults);

			List<object> resultVariables = new List<object>();
			string currentVariableName = String.Empty;

			for (int i = 1; i < variablesFromSlnet.Length; i++)
			{
				if (i % 2 != 0)
				{
					currentVariableName = variablesFromSlnet[i];
				}
				else
				{
					if (variablesFromSlnet[i] == "TRACE") continue;

					List<object> variableContent = new List<object>();

					string[] generalInfo = new string[2];
					generalInfo[0] = currentVariableName;
					generalInfo[1] = variablesFromSlnet[i];

					variableContent.Add(generalInfo);

					resultVariables.Add(variableContent.ToArray());
				}
			}

			if (!String.IsNullOrWhiteSpace(handlerWrapper.TraceName))
			{
				object[] traceContent = new object[3];
				traceContent[0] = new[] { handlerWrapper.TraceName, String.Empty };
				traceContent[1] = handlerWrapper.TraceResults;
				traceContent[2] = handlerWrapper.CreateArrayAsInteropWanted();

				resultVariables.Add(traceContent);
			}

			object resultNoTrace = resultVariables.ToArray();

			return resultNoTrace;
		}

		/// <summary>
		/// Retrieves all scripts.
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_GETINFO (4), SPAI_SCRIPTS_ALL (10), null, null, out result);
		/// </summary>
		/// <returns>A list of all the scripts defined on the element.</returns>
		public object GetAllScripts()
		{
			GetSpectrumManagerInfoMessage message = new GetSpectrumManagerInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.AllScripts
			};

			GetSpectrumManagerInfoResponseMessage result = (GetSpectrumManagerInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return PSA.ToInteropArray(result.Psa);
		}

		/// <summary>
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_SCRIPT (9), scriptMetaInfo, scriptStatements, out result);
		/// </summary>
		/// <param name="scriptId">The ID of the script.</param>
		/// <param name="scriptName">The name of the script.</param>
		/// <param name="scriptDescription">A description of the script.</param>
		/// <param name="scriptStatements">Details of the script.</param>
		/// <returns>The ID of the updated script.</returns>
		public int UpdateScript(int scriptId, string scriptName, string scriptDescription, string[] scriptStatements)
		{
			string[] scriptMetaInfo = new string[]
			 {
				Convert.ToString(scriptId), // script id. SL_NO_ID = 2100000000 = new script
				"add", // add/delete (also use "add" for updates to existing scripts)
				element.Protocol.Name, //Anritsu MS2724C
				scriptName,
				scriptDescription
			 };

			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Script,
				Sa1 = new SA(scriptMetaInfo),
				Sa2 = new SA(scriptStatements)
			};

			SetSpectrumInfoResponseMessage result = (SetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return result.NewID;
		}

		/// <summary>
		/// Adds the specified script.
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_SCRIPT (9), scriptMetaInfo, scriptStatements, out result);
		/// Where scriptMetaInfo has scriptId set to 2100000000
		/// </summary>
		/// <param name="scriptName">The name of the script.</param>
		/// <param name="scriptDescription">A description of the script.</param>
		/// <param name="scriptStatements">Details of the script.</param>
		/// <returns>The ID of the added script.</returns>
		public int AddScript(string scriptName, string scriptDescription, string[] scriptStatements)
		{
			return UpdateScript(2100000000, scriptName, scriptDescription, scriptStatements);
		}
	}
}