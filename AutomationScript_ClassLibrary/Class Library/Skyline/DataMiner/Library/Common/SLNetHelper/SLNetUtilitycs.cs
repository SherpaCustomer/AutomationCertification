namespace Skyline.DataMiner.Library.Common.SLNetHelper
{
	using Skyline.DataMiner.Library.Common.Selectors.Data;
	using Skyline.DataMiner.Net.Messages;

	using System;
	using System.Globalization;

	internal static class SLNetUtility
	{
#pragma warning disable S4018 // Generic methods should provide type parameters
#pragma warning disable S3242 // Method parameters should be declared with base types

		internal static ParamAlarmLevel ParseAlarmStateParameterChangeEvent(ParameterChangeEventMessage paramChangeMessage)
		{
			Skyline.DataMiner.Library.Common.AlarmLevel alarmLevel = (Skyline.DataMiner.Library.Common.AlarmLevel)paramChangeMessage.AlarmLevel;

			return new ParamAlarmLevel(paramChangeMessage.DataMinerID, paramChangeMessage.ElementID, paramChangeMessage.ParameterID, alarmLevel);
		}

		internal static CellValue ParseCellParameterChangeEventMessage<T>(ParameterChangeEventMessage paramChangeMessage, int tableId)
		{
			int dmaId = paramChangeMessage.DataMinerID;
			int elementId = paramChangeMessage.ElementID;
			int parameterId = paramChangeMessage.ParameterID;
			string primaryKey = paramChangeMessage.TableIndexPK;

			T value = ParseValue<T>(paramChangeMessage);

			return new CellValue(dmaId, elementId, tableId, parameterId, primaryKey, value);
		}

		internal static CellAlarmLevel ParseCellParameterChangeEventMessageAlarmLevel(ParameterChangeEventMessage paramChangeMessage, int tableId)
		{
			int dmaId = paramChangeMessage.DataMinerID;
			int elementId = paramChangeMessage.ElementID;
			int parameterId = paramChangeMessage.ParameterID;
			string primaryKey = paramChangeMessage.TableIndexPK;

			Skyline.DataMiner.Library.Common.AlarmLevel alarmLevel = (Skyline.DataMiner.Library.Common.AlarmLevel)paramChangeMessage.AlarmLevel;

			return new CellAlarmLevel(dmaId, elementId, tableId, parameterId, primaryKey, alarmLevel);
		}

		internal static ParamValue ParseStandaloneParameterChangeEventMessageString<T>(ParameterChangeEventMessage paramChangeMessage)
		{
			int dmaId = paramChangeMessage.DataMinerID;
			int elementId = paramChangeMessage.ElementID;
			int parameterId = paramChangeMessage.ParameterID;

			T value = ParseValue<T>(paramChangeMessage);

			return new ParamValue(dmaId, elementId, parameterId, value);
		}

		internal static T ProcessResponseNonNullable<T>(object interopValue, Type type)
		{
			T value;

			if (type == typeof(DateTime))
			{
				if (interopValue == null)
				{
					value = default(T);
				}
				else
				{
					double oleAutomationDate = Convert.ToDouble(interopValue, CultureInfo.CurrentCulture);
					value = (T)Convert.ChangeType(DateTime.FromOADate(oleAutomationDate), type, CultureInfo.CurrentCulture);
				}
			}
			else
			{
				value = interopValue == null ? default(T) : (T)Convert.ChangeType(interopValue, type, CultureInfo.CurrentCulture);
			}

			return value;
		}

		internal static T ProcessResponseNullable<T>(object interopValue, Type underlyingType)
		{
			T value;

			// Nullable type.
			if (underlyingType == typeof(DateTime))
			{
				if (interopValue == null)
				{
					value = default(T);
				}
				else
				{
					double oleAutomationDate = Convert.ToDouble(interopValue, CultureInfo.CurrentCulture);
					DateTime dateTime = DateTime.FromOADate(oleAutomationDate);
					value = (T)Convert.ChangeType(dateTime, underlyingType, CultureInfo.CurrentCulture);
				}
			}
			else
			{
				value = interopValue == null ? default(T) : (T)Convert.ChangeType(interopValue, underlyingType, CultureInfo.CurrentCulture);
			}

			return value;
		}

		private static T ParseValue<T>(ParameterChangeEventMessage paramChangeMessage)
		{
			var type = typeof(T);
			var underlyingType = Nullable.GetUnderlyingType(type);
			var interopValue = paramChangeMessage.NewValue.InteropValue;
			T value = underlyingType == null ? ProcessResponseNonNullable<T>(interopValue, type) : ProcessResponseNullable<T>(interopValue, underlyingType);

			return value;
		}

#pragma warning restore S4018 // Generic methods should provide type parameters
#pragma warning restore S3242 // Method parameters should be declared with base types
	}
}