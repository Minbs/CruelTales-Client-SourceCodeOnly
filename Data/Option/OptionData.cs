using CT.Logger;
using CT.Common.Tools.Data;
using CTC.Globalizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace CTC.Data
{
	public class OptionData
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(OptionData));

		#region Options

		// Localization
		[JsonConverter(typeof(StringEnumConverter))]
		public LanguageType Language;

		// Screen
		[JsonConverter(typeof(StringEnumConverter))]
		public FullScreenMode ScreenMode;
		public int ScreenWidth;
		public int ScreenHeight;
		public int ScreenRefreshRate;

		// Sound
		public float VolumeMaster;
		public float VolumeEffect;
		public float VolumeBackground;

		#endregion

		public static string OptionPath =
			Global.Path.UserDataPath + "/" + 
			Global.File.OptionFileName + 
			Global.File.JsonExtension;

		public OptionData() {}

		public OptionData(bool isDefault = false)
		{
			if (!isDefault)
				return;

			// Setup default game option

			// Localization
			Language = Localizer.GetCurrentLanguage();

			// Screen
			ScreenMode = FullScreenMode.Windowed;
			ScreenWidth = 1366;
			ScreenHeight = 768;
			ScreenRefreshRate = 60;

			// Sound
			VolumeMaster = 0.5f;
			VolumeEffect = 1.0f;
			VolumeBackground = 1.0f;
		}

		/// <summary>옵션을 지정된 위치에서 불러옵니다. 존재하지 않는다면 기본 옵션을 불러옵니다.</summary>
		/// <returns>옵션 데이터 입니다.</returns>
		public static OptionData LoadFromFile()
		{
			var readResult = JsonHandler.TryReadObject<OptionData>(OptionPath);
			OptionData optionData = readResult.Value;

			if (readResult.ResultType == JobResultType.Success)
			{
				_log.Info($"Json option data loaded!");

				if (optionData.ScreenWidth < Global.Environment.SCREEN_MINIMUM_WIDTH ||
					optionData.ScreenHeight < Global.Environment.SCREEN_MINIMUM_HEIGHT)
				{
					optionData.ScreenWidth = Global.Environment.SCREEN_MINIMUM_WIDTH;
					optionData.ScreenHeight = Global.Environment.SCREEN_MINIMUM_HEIGHT;
				}

				if (optionData.ScreenRefreshRate < Global.Environment.MINIMUM_FRAMERATE)
				{
					optionData.ScreenRefreshRate = Global.Environment.MINIMUM_FRAMERATE;
				}

				return optionData;
			}

			_log.Warn($"There is no json option data. Create default option!");
			return new OptionData(true);
		}

		/// <summary>옵션을 지정된 위치에 파일로 저장합니다.</summary>
		/// <param name="instance">저장할 옵션 인스턴스입니다.</param>
		public static void SaveToFile(OptionData instance)
		{
			if (instance == null)
			{
				instance = new OptionData(true);
				_log.Warn($"There is no option data to save. Create default option and save it.");
			}

			if (JsonHandler.TryWriteObject(OptionPath, instance, makeDirectory: true).ResultType == JobResultType.Success)
			{
				_log.Info($"Save option data to json format!");
			}
			else
			{
				_log.Error($"Fail to save option!");
			}
		}
	}
}
