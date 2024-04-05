using CT.Logger;
using CTC.Data;
using CTC.Globalizations;
using UnityEngine;

namespace CTC.SystemCore
{
	/// <summary>옵션을 관리하는 서비스입니다.</summary>
	public class OptionManager : IManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(OptionManager));

		private OptionData mOptionData;

		#region Option Getter

		public float VolumeMaster => mOptionData.VolumeMaster;
		public float VolumeEffect => mOptionData.VolumeEffect;
		public float VolumeBackground => mOptionData.VolumeBackground;

		#endregion

		public void Initialize()
		{
			mOptionData = OptionData.LoadFromFile();
			OnApplyOption();
		}

		public void Release()
		{
			// Save option
			saveOption();
		}

		private void saveOption()
		{
			OptionData.SaveToFile(mOptionData);
			_log.Info($"Option Saved");
		}

		public void OnApplyOption()
		{
			Localizer.SetLanguage(mOptionData.Language);

			Screen.SetResolution
			(
				mOptionData.ScreenWidth,
				mOptionData.ScreenHeight,
				mOptionData.ScreenMode,
				mOptionData.ScreenRefreshRate
			);

			Application.targetFrameRate = mOptionData.ScreenRefreshRate;

			_log.Info($"Option applied");

			saveOption();
		}
	}
}