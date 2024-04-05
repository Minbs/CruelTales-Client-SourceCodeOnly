using System.Collections.Generic;

/// <summary>Material type for sound system(FMOD)</summary>
public enum SoundMatType
{
	Normal,
	Dirt,
	Stone,
	Metal
}

/// <summary>Toolbox for Sound(FMOD)</summary>
public static class SoundUtils
{
	private static readonly Dictionary<string, float> _soundMatTypeDic = new()
	{
		{ "Dirt", 0f },
		{ "Stone", 1f },
		{ "Metal", 2f },
		{ "Wood", 3f }
	};

	private static readonly Dictionary<string, string> _eventPathDic = new()
	{
		{ "OnStepWalk", "event:/SFX/PC/Move/Run" },
		{ "OnStepRun", "event:/SFX/PC/Move/Run" }
	};

	/// <summary>Returns float value from Tag.</summary>
	/// <returns>float value for FMOD parameter</returns>
	public static float GetParamFromTag(string objectTag)
	{
		return _soundMatTypeDic.ContainsKey(objectTag) ?
			_soundMatTypeDic[objectTag] :
			_soundMatTypeDic["Dirt"];
	}

	/// <summary>Returns path from Spine event.</summary>
	/// <returns>path(string)</returns>
	public static string GetPathFromSpineEvent(string spineEvent)
	{
		return _eventPathDic.ContainsKey(spineEvent) ?
			_eventPathDic[spineEvent] :
			_eventPathDic["OnStepWalk"];
	}
}
