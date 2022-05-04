using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Net.Messages.Advanced;

namespace Skyline.DataMiner.Library.Common
{
	/// <summary>
	/// Represents the spectrum analyzer component of an element.
	/// </summary>
	internal class DmsSpectrumAnalyzer : IDmsSpectrumAnalyzer
	{
		private readonly IDmsElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsSpectrumAnalyzer"/> class.
		/// </summary>
		/// <param name="element">The element this spectrum analyzer is part of.</param>
		public DmsSpectrumAnalyzer(IDmsElement element)
		{
			this.element = element;

			Monitors = new DmsSpectrumAnalyzerMonitors(element);
			Presets = new DmsSpectrumAnalyzerPresets(element);
			Scripts = new DmsSpectrumAnalyzerScripts(element);
		}

		/// <summary>
		/// Manipulate the spectrum monitors.
		/// </summary>
		public IDmsSpectrumAnalyzerMonitors Monitors { get; private set; }

		/// <summary>
		/// Manipulate the spectrum presets.
		/// </summary>
		public IDmsSpectrumAnalyzerPresets Presets { get; private set; }

		/// <summary>
		/// Manipulate the spectrum scripts.
		/// </summary>
		public IDmsSpectrumAnalyzerScripts Scripts { get; private set; }

		/// <summary>
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_GETINFO (4), SPAI_MEASUREMENT_POINTS (29), null, null, out result);
		/// </summary>
		/// <returns>An object similar to the return value of the old notify.</returns>
		public object GetMeasurementPoints()
		{
			GetSpectrumInfoMessage message = new GetSpectrumInfoMessage()
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.MeasurementPoints
			};

			GetSpectrumInfoResponseMessage result = (GetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return PSA.ToInteropArray(result.Psa);
		}

		/// <summary>
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MEASUREMENT_POINTS (29), createServices, measptdata, out result);
		/// </summary>
		/// <param name="createServices">When <c>true</c>, services will be created for the measurement points.</param>
		/// <param name="measurementPointData">An image of all measurement points.</param>
		/// <returns>All measurement ids.</returns>
		public int[] SetMeasurementPoints(bool createServices, object[] measurementPointData)
		{
			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.MeasurementPoints,
				BInfo = createServices,
				Psa = new PSA(measurementPointData)
			};

			SetSpectrumMeasurementPointsResponseMessage result = (SetSpectrumMeasurementPointsResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return result.MeasurementPointIDs;
		}
	}
}