namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Net.Exceptions;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	using System;

	/// <summary>
	/// Represents the spectrum analyzer monitors.
	/// </summary>
	internal class DmsSpectrumAnalyzerMonitors : IDmsSpectrumAnalyzerMonitors
	{
		private readonly IDmsElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsSpectrumAnalyzerMonitors"/> class.
		/// </summary>
		/// <param name="element">The element to which this spectrum analyzer component is part of.</param>
		public DmsSpectrumAnalyzerMonitors(IDmsElement element)
		{
			this.element = element;
		}

		/// <summary>
		/// Deletes the monitor with the specified ID.
		/// Replace: sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor to be deleted.</param>
		/// <returns></returns>
		public int DeleteMonitor(int monitorId)
		{
			string[] monitorMetaInfo = new string[]
			{
				Convert.ToString(monitorId),	// monitor id. SL_NO_ID = 2100000000 = new monitor
				"delete"						// add/delete (also use "add" for updates to existing scripts)
			};

			string[] monitorDetails = new string[] { };

			monitorMetaInfo[1] = "delete"; // Forcing metaInfo to delete
			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Monitor,
				Sa1 = new SA(monitorMetaInfo),
				Sa2 = new SA(monitorDetails)
			};

			SetSpectrumInfoResponseMessage result = (SetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return result.NewID;
		}

		/// <summary>
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_GETINFO (4), SPAI_MONITORS_ALL (7), null, null, out result);
		/// </summary>
		/// <returns>An object representing all monitors.</returns>
		public object GetMonitors()
		{
			GetSpectrumManagerInfoMessage message = new GetSpectrumManagerInfoMessage()
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.AllMonitors
			};

			GetSpectrumManagerInfoResponseMessage result = (GetSpectrumManagerInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return PSA.ToInteropArray(result.Psa);
		}

		/// <summary>
		/// Retrieve a single monitor.
		/// Replaces sa.NotifyElementEx(userId,elementInfo.DmaId,elementInfo.ElementId,SPA_NE_GETINFO (4),SPAI_MONITOR (8),monitorId,null,out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor to be retrieved.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Monitor not found.</exception>
		public object GetMonitor(int monitorId)
		{
			GetSpectrumInfoMessage message = new GetSpectrumInfoMessage()
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Monitor,
				Info = monitorId
			};

			/*
SA for the monitor:
0 = script id 
1 = dma id
2 = eid
3 = interval (seconds)
4 = name
5 = description
6 = true/false (enabled/disabled)
7 = pre:post (amount of traces to store for alarm recordings) 
8 = measpointid,measpointid,...
9 = true/false (auto group params)
10 = amount of templates (n) (RN1043)
11 -> 10+n = alarm template info strings (see GetInfo(SPAI_MONITOR)) 
10+n+1 = amount of CSV exports (m)
10+n+2 -> 10+n+1+m: CSV export info
x = generate service? (true/false)
x+1 = generated service ID
x+2 = presetname|presetname|...


		Alarm template format:

		 *				6:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:templateType:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:measPtId:#:DOUBLE:#:bUsesCustomRange:#:rangelow:#:rangehigh:#:decimals:#:units
		 *					pid -> SL_NO_ID for new ids
		 *				7:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:templateType:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:measPtId:#:bCustomPos:#:row:#:col:#:DOUBLE:#:bUsesCustomRange:#:rangelow:#:rangehigh:#:decimals:#:units
		 *					pid -> SL_NO_ID for new ids
		 *				6.5.4+ support for TRACE
		 *				7:#:pid:#:variable name:#:Description:#::#::#::#::#::#::#::#::#::#::#::#::#::#:measPtId:#::#::#::#:TRACE:#:
			*/

			try
			{
				GetSpectrumInfoResponseMessage result = (GetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

				return SA.ToInteropArray(result.Sa);
			}
			catch (DataMinerCOMException)
			{
				throw new InvalidOperationException("Monitor with ID: " + monitorId + " was not found.");
			}
		}

		/// <summary>
		/// Replaces:sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor.</param>
		/// <param name="monitorDetails">Details describing the monitor.</param>
		/// <returns>ID of the updated monitor.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="monitorDetails"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="monitorDetails"/> must be an array of at least size 6.</exception>
		public int UpdateMonitor(int monitorId, string[] monitorDetails)
		{
			if (monitorDetails == null)
			{
				throw new ArgumentNullException("monitorDetails");
			}

			if (monitorDetails.Length < 6)
			{
				throw new ArgumentException("monitorDetails must be an array of at least size 6.");
			}

			string monitorName = monitorDetails[4];
			string monitorDescription = monitorDetails[5];

			string[] scriptMetaInfo = new[]
			{
				Convert.ToString(monitorId),	// script id. SL_NO_ID = 2100000000 = new script
				"add", // add/delete
				element.Protocol.Name,
				monitorName,
				monitorDescription
			};

			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Monitor,
				Sa1 = new SA(scriptMetaInfo),
				Sa2 = new SA(monitorDetails)
			};

			/*
			 * 		 * @param varWhat
		 *		SA metadata
		 *			0 = monitor id
		 *			1 = action (add/delete) (note: for updates, also use 'add')
		 *			n (last) = user cookie with subsession info
		 * @param varData
		 *		SA data (VT_EMPTY on deleted)
		 *			0 = script id (must exist!)
		 *			1 = dma id
		 *			2 = eid
		 *			3 = interval (seconds)
		 *			4 = name
		 *			5 = description
		 *			6 = true/false (enabled/disabled)
		 *			7 = pre:post
		 *			8 = meas points (comma-separated)
		 *			9 = auto group params? (true/false)
		 *			10 = amount alarm templates (n) (RN1043)
		 *			11 -> 10+n = Alarm Parameter Info
		 *				3:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:BOOL:#:TrueDisplay:#:FalseDisplay
		 *				3:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:DOUBLE
		 *					pid -> SL_NO_ID for new ids
		 *				4:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:DOUBLE:#:bUsesCustomRange:#:rangelow:#:rangehigh:#:decimals:#:units
		 *					pid -> SL_NO_ID for new ids
		 *				5:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:measPtId:#:DOUBLE:#:bUsesCustomRange:#:rangelow:#:rangehigh:#:decimals:#:units
		 *					pid -> SL_NO_ID for new ids
		 *				RN761:
		 *				6:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:templateType:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:measPtId:#:DOUBLE:#:bUsesCustomRange:#:rangelow:#:rangehigh:#:decimals:#:units
		 *					pid -> SL_NO_ID for new ids
		 *				7:#:pid:#:variable name:#:Description:#:bDisplayed:#:bMonitored:#:bTrended:#:templateType:#:valCH:#:valMaH:#:valMiH:#:valWaH:#:valNormal:#:valWaL:#:valMiL:#:valMaL:#:valCL:#:measPtId:#:bCustomPos:#:row:#:col:#:DOUBLE:#:bUsesCustomRange:#:rangelow:#:rangehigh:#:decimals:#:units
		 *					pid -> SL_NO_ID for new ids
		 *				6.5.4+ support for TRACE
		 *				7:#:pid:#:variable name:#:Description:#::#::#::#::#::#::#::#::#::#::#::#::#::#:measPtId:#::#::#::#:TRACE:#:
		 *			(@see CConfig::UpdateMonitor)
		 *			10+n+1 = amount CSV exports (m)
		 *			10+n+2 -> 10+n+1+m: CSV export info 
		 *			x = generate service (true/false) [ note: there's no field to update the service ID! ]
		 *			x+1 = preset names (separated by '|', no '(public)' part in names)
			 */

			SetSpectrumInfoResponseMessage result = (SetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);
			return result.NewID;
		}

		/// <summary>
		/// Replaces:sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// Where monitorId is set to 2100000000 for creation
		/// </summary>
		/// <param name="monitorDetails">Details of the monitor.</param>µ
		/// <returns>ID of the added monitor.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="monitorDetails"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="monitorDetails"/> must be an array of at least size 6.</exception>
		public int AddMonitor(string[] monitorDetails)
		{
			return UpdateMonitor(2100000000, monitorDetails);
		}
	}
}