namespace Skyline.DataMiner.Library.Common
{
	using System;
	using Skyline.DataMiner.Net.Messages;

	/// <summary>
	/// Represents spectrum analyzer presets.
	/// </summary>
	internal class DmsSpectrumAnalyzerPresets : IDmsSpectrumAnalyzerPresets
	{
		private readonly IDmsElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsSpectrumAnalyzerPresets"/> class.
		/// </summary>
		/// <param name="element">The element to which this spectrum analyzer component belongs.</param>
		public DmsSpectrumAnalyzerPresets(IDmsElement element)
		{
			this.element = element;
		}

		/// <summary>
		/// Deletes the preset with the specified name.
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_PRESET_DELETE (2), 0, presetGlobalName, null, out result);
		/// </summary>
		/// <param name="presetName">The name of the preset to delete.</param>
		/// <param name="isGlobalPreset">Allows to define if the preset should be shared to all users or private for scripting.</param>
		public void DeletePreset(string presetName, bool isGlobalPreset = true)
		{
			presetName = CheckNameAgainstSharedType(presetName, isGlobalPreset);

			DeleteSpectrumPresetMessage message = new DeleteSpectrumPresetMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				Spectrum = presetName,
			};

			element.Host.Dms.Communication.SendSingleResponseMessage(message);
		}

		/// <summary>
		/// Retrieves the preset with the specified name.
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_PRESET_LOAD (0), 0, presetName, presetLoadOptions, out result);
		/// </summary>
		/// <param name="presetName">The name of the preset to get.</param>
		/// <param name="isGlobalPreset">Allows to define if the preset should be shared to all users or private for scripting.</param>
		/// <returns>The preset with the specified name.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="presetName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="presetName"/> is empty.</exception>
		public object GetPreset(string presetName, bool isGlobalPreset = true)
		{
			if (presetName == null)
			{
				throw new ArgumentNullException("presetName");
			}

			if (String.IsNullOrEmpty(presetName))
			{
				throw new ArgumentException("Preset name is empty.", "presetName");
			}

			presetName = CheckNameAgainstSharedType(presetName, isGlobalPreset);

			int presetLoadOptions = 0x02; // = SPA_PRESET_DONTAPPLY
			LoadSpectrumPresetMessage message = new LoadSpectrumPresetMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				Spectrum = presetName,
				Info = presetLoadOptions
			};

			LoadSpectrumPresetResponseMessage result = (LoadSpectrumPresetResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return PSA.ToInteropArray(result.Psa);
		}

		/// <summary>
		/// Saves the preset.
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_PRESET_SAVE (1), 0, presetName, presetData, out result);
		/// </summary>
		/// <param name="presetName">The name of the preset to update or create.</param>
		/// <param name="presetData">Object array as required by old SpectrumAnalyzerClass for SPA_NE_PRESET_SAVE = 1.</param>
		/// <param name="isGlobalPreset">Allows to define if the preset should be shared to all users or private for scripting.</param>
		/// <exception cref="ArgumentNullException"><paramref name="presetName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="presetName"/> is empty.</exception>
		public void SavePreset(string presetName, object[] presetData, bool isGlobalPreset = true)
		{
			if(presetName == null)
			{
				throw new ArgumentNullException("presetName");
			}

			if(String.IsNullOrEmpty(presetName))
			{
				throw new ArgumentException("Preset name is empty.", "presetName");
			}

			presetName = CheckNameAgainstSharedType(presetName, isGlobalPreset);

			if (presetData != null)
			{
				PSA psa = new PSA(presetData);

				UpdateSpectrumPresetMessage message = new UpdateSpectrumPresetMessage
				{
					DataMinerID = element.AgentId,
					ElId = element.Id,
					Spectrum = presetName,
					Psa = psa
				};

				element.Host.Dms.Communication.SendSingleResponseMessage(message);
			}
		}

		private static string CheckNameAgainstSharedType(string presetName, bool isGlobalPreset)
		{
			if (isGlobalPreset)
			{
				if (!presetName.StartsWith("GLOBAL:",StringComparison.Ordinal))
				{
					presetName = "GLOBAL:" + presetName;
				}
			}
			else
			{
				if (presetName.StartsWith("GLOBAL:", StringComparison.Ordinal))
				{
					presetName = presetName.Substring(7);
				}
			}

			return presetName;
		}
	}
}