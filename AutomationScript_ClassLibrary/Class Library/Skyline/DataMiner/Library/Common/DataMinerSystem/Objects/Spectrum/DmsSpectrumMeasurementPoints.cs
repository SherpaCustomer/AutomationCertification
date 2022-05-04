namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	/// <summary>
	/// Manipulation of measurement points.
	/// </summary>
	internal class DmsSpectrumAnalyzerMeasurementPoints : IDmsSpectrumAnalyzerMeasurementPoints
	{
		private readonly IDmsElement element;

		/// <summary>
		/// Create an instance of DmsSpectrumMeasurementPoints.
		/// </summary>
		/// <param name="element">The spectrum element.</param>
		public DmsSpectrumAnalyzerMeasurementPoints(IDmsElement element)
		{
			this.element = element;
		}

		/// <summary>
		/// Get all spectrum measurement points.
		/// </summary>
		/// <returns>An object representing all measurement points similar to how the interop call returned it.</returns>
		public object GetMeasurementPoints()
		{
			GetSpectrumInfoMessage getMeasurementPointMsg = new GetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.MeasurementPoints,
			};

			GetSpectrumInfoResponseMessage rsp = (GetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(getMeasurementPointMsg);
			var result = PSA.ToInteropArray(rsp.Psa);
			return result;
		}
	}
}