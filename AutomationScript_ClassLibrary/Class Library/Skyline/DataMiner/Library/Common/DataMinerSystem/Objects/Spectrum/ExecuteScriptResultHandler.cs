namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Globalization;
	using System.Threading;

	internal class ExecuteScriptResultHandler
	{
		public ExecuteScriptResultHandler(IDmsElement myElement)
		{
			WaitFlag = new AutoResetEvent(false);
			TraceResults = new string[0];
			ExecuteScriptResults = String.Empty;
			MyElement = myElement;
		}

		public IDmsElement MyElement { get; set; }

		public string AmountDivisionsHorizontal { get; private set; }

		public string AmountDivisionsVertical { get; private set; }

		public int AmountOfPoints { get; private set; }

		public string AmpUnit { get; private set; }

		public string DetectionMode { get; private set; }

		public string ExecuteScriptResults { get; private set; }

		public string FirstMixerInput { get; private set; }

		public string FrequencyCenter { get; private set; }

		public string FrequencySpan { get; private set; }

		public string FrequencyStart { get; private set; }

		public string FrequencyStop { get; private set; }

		public string InputAttenuation { get; private set; }

		public string InputAttenuationActual { get; set; }

		public string MeasurementPointName { get; private set; }

		public string MeasurementTime { get; private set; }

		public string NotCalibrated { get; private set; }

		public string PreAmplifier { get; private set; }

		public string Rbw { get; private set; }

		public string RbwActual { get; set; }

		public string RefLevel { get; private set; }

		public string RefScale { get; private set; }

		public string ScalarType { get; private set; }

		public string SweepTime { get; private set; }

		public string SweepTimeActual { get; set; }

		public string TraceName { get; set; }

		public string[] TraceResults { get; private set; }

		public string Vbw { get; private set; }

		public string VbwActual { get; set; }

		public AutoResetEvent WaitFlag { get; private set; }

		public object[] CreateArrayAsInteropWanted()
		{
			return new string[]
			{
				FrequencyCenter,
				FrequencySpan,
				Convert.ToString(AmountOfPoints),
				FrequencyStart,
				FrequencyStop,
				Rbw!="-1"?Rbw:RbwActual, // If Auto, fill in the Actual
				Vbw!="-1"?Vbw:VbwActual, // If Auto, fill in the Actual
				RefScale,
				RefLevel,
				SweepTime!="-1"?SweepTime:SweepTimeActual,// If Auto, fill in the Actual
				InputAttenuation!="-1"?InputAttenuation:InputAttenuationActual,// If Auto, fill in the Actual
				MeasurementTime,
				MeasurementPointName,
				PreAmplifier,
				FirstMixerInput,
				DetectionMode,
				ScalarType,
				NotCalibrated,
				AmpUnit,
				AmountDivisionsHorizontal,
				AmountDivisionsVertical
			};
		}

		public NewMessageEventHandler CreateExecuteScriptHandler(IDmsElement element, string subsessionId, int agentId, int elementId)
		{
			return (sender, e) =>
			{
				try
				{
					if (!e.FromSet(subsessionId)) return;
					if (!(e.Message is ParameterChangeEventMessage)) return;

					var paramChangeMessage = e.Message as ParameterChangeEventMessage;

					if (!String.IsNullOrWhiteSpace(paramChangeMessage.TableIndexPK))
					{
						System.Diagnostics.Debug.WriteLine("Received event from table cell.");
						return;
					}

					if (paramChangeMessage.DataMinerID == agentId && paramChangeMessage.ElementID == elementId)
					{
						switch (paramChangeMessage.ParameterID)
						{
							case 64210:
								TraceName = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64209:
								ExecuteScriptResults = paramChangeMessage.NewValue.StringValue;
								WaitFlag.Set();
								break;

							case 64001:
								TraceResults = ParseSLNetTraceArray(paramChangeMessage.NewValue.ArrayValue);
								AmountOfPoints = TraceResults.Length;
								break;

							case 64003:
								FrequencyCenter = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64004:
								FrequencySpan = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64005:
								FrequencyStart = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64006:
								FrequencyStop = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64014:
								Rbw = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64020:
								RbwActual = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64012:
								Vbw = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64019:
								VbwActual = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64011:
								RefScale = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64010:
								RefLevel = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64015:
								SweepTime = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64021:
								SweepTimeActual = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64009:
								InputAttenuation = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64022:
								InputAttenuationActual = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64206:
								MeasurementTime = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64208:
								string measurementPointId = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);

								DmsSpectrumAnalyzerMeasurementPoints msp = new DmsSpectrumAnalyzerMeasurementPoints(element);
								object[] allMeasurementPoints = (object[])msp.GetMeasurementPoints();
								string measurementPointName = String.Empty;

								for (int i = 1; i < allMeasurementPoints.Length; i++)
								{
									string[] mp = (string[])allMeasurementPoints[i];
									if (mp[0] == measurementPointId)
									{
										measurementPointName = mp[4];
										break;
									}
								}

								MeasurementPointName = measurementPointName;
								break;

							case 64029:
								PreAmplifier = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64030:
								FirstMixerInput = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64031:
								DetectionMode = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64032:
								ScalarType = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64023:
								NotCalibrated = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64008:
								AmpUnit = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64038:
								AmountDivisionsHorizontal = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							case 64039:
								AmountDivisionsVertical = Convert.ToString(paramChangeMessage.NewValue.InteropValue, CultureInfo.InvariantCulture);
								break;

							default:
								// Do nothing
								break;
						}
					}
				}
				catch (Exception ex)
				{
					var message = "Spectrum event error: Exception occurred in event handler of ParamChange event (Class Library side): " + subsessionId + " -- " + e + " with exception: " + ex;
					System.Diagnostics.Debug.WriteLine(message);
					Logger.Log(message);
				}
			};
		}

		private static string[] ParseSLNetTraceArray(ParameterValue[] rawTrace)
		{
			int decimals = rawTrace[0].Int32Value;
			if (decimals == -1) decimals = 1;

			string[] result = new string[rawTrace.Length - 1];

			for (int i = 1; i < rawTrace.Length; i++)
			{
				double decimalTrace = (double)rawTrace[i].Int32Value / decimals;
				result[i - 1] = Convert.ToString(decimalTrace, CultureInfo.InvariantCulture);
			}

			return result;
		}
	}
}