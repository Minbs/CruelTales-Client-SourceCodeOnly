#if UNITY_EDITOR

using System;
using CTC.Data;
using CTC.Globalizations;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class OptionManager : OdinEditorWindow
{
	[MenuItem ("Editor/Option Manager")]
	private static void initialize()
	{
		var window = GetWindow<OptionManager>(true, "Option Manager");
		window.minSize = new Vector2(360, 600);
		window.Show();
	}

	public const int HEADER_SPACE_HEIGHT = 15;
	public const int AFTER_HEADER_SPACE_HEIGHT = 15;

	[Title("언어")]
	public LanguageType Language;

	[Title("화면")]
	public FullScreenMode ScreenMode;
	public int ScreenWidth;
	public int ScreenHeight;
	public int ScreenRefreshRate;

	[Title("소리")]
	[Range(0, 1)] public float VolumeMaster;
	[Range(0, 1)] public float VolumeEffect;
	[Range(0, 1)] public float VolumeBackground;

	protected override void OnEnable()
	{
		base.OnEnable();
		loadOption();
	}

	private void loadOption()
	{
		var option = OptionData.LoadFromFile();

		Language					 = option.Language;

		ScreenMode					 = option.ScreenMode;
		ScreenWidth					 = option.ScreenWidth;
		ScreenHeight				 = option.ScreenHeight;
		ScreenRefreshRate			 = option.ScreenRefreshRate;

		VolumeMaster				 = option.VolumeMaster;
		VolumeEffect				 = option.VolumeEffect;
		VolumeBackground			 = option.VolumeBackground;
	}

	[HorizontalGroup(order: 1), Button("기본 옵션으로 설정", ButtonHeight = 40)]
	public void SetOptionToDefault()
	{
		var defaultOption = new OptionData(true);
		OptionData.SaveToFile(defaultOption);
		loadOption();
	}

	[HorizontalGroup(order: 1), Button("옵션 저장", ButtonHeight = 40)]
	public void SaveOption()
	{
		OptionData option = new OptionData();

		option.Language				= Language;

		option.ScreenMode			= ScreenMode;
		option.ScreenWidth			= ScreenWidth;
		option.ScreenHeight			= ScreenHeight;
		option.ScreenRefreshRate	= ScreenRefreshRate;

		option.VolumeMaster			= VolumeMaster;
		option.VolumeEffect			= VolumeEffect;
		option.VolumeBackground		= VolumeBackground;

		OptionData.SaveToFile(option);
	}
}

#endif
