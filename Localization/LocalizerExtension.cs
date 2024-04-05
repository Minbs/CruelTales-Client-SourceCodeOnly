using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Gameplay.RedHood;
using CTC.Globalizations;
using CTC.Networks;
using CTC.SystemCore;

public static class LocalizerExtension
{
	public static string GetText(this TextKey value)
	{
		return Localizer.GetText(value);
	}
}

public static class EnumLocalizerExtension
{
	public static string GetText(this AsyncOperationType value)
		=> Localizer.GetText($"AsyncOperationType_{value}");

	public static string GetText(this DialogResult value)
		=> Localizer.GetText($"DialogResult_{value}");

	public static string GetText(this UserSessionState value)
		=> Localizer.GetText($"UserSessionState_{value}");

	public static string GetText(this RoomSettingResult value)
		=> Localizer.GetText($"RoomSettingResult_{value}");

	public static string GetText(this StartGameResultType value)
		=> Localizer.GetText($"StartGameResultType_{value}");

	public static string GetText(this GameModeType value)
		=> Localizer.GetText($"GameModeType_{value}");

	public static string GetText(this CompetitionType value)
		=> Localizer.GetText($"CompetitionType_{value}");

	public static string GetText(this InteractResultType value)
		=> Localizer.GetText($"InteractResultType_{value}");

	public static string GetText(this RedHoodMission value)
		=> Localizer.GetText($"MG_RedHood_Mission_{value}");
}
