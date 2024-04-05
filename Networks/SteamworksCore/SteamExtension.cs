namespace CTC.Networks.SteamworksCore
{
	public static class SteamExtension
	{
		public static UnityEngine.Color ToUnityColor(this Steamworks.Data.Color color)
		{
			return new UnityEngine.Color(color.r / 255.0f, color.g / 255.0f, color.b / 255.0f, color.a / 255.0f);
		}

		//public static bool TrySetData(this Lobby lobby, LobbyMetadataKey key, string value, int valueLimits = 255)
		//{
		//	return TrySetData(lobby, key.ToString(), value, valueLimits);
		//}

		//public static bool TrySetData(this Lobby lobby, string key, string value, int valueLimits)
		//{
		//	if (key.Length > Global.Net.Lobby.MAX_METADATA_KEY_LENGTH ||
		//		value.Length > Global.Net.Lobby.MAX_METADATA_VALUE_LENGTH ||
		//		value.Length > valueLimits)
		//	{
		//		return false;
		//	}

		//	return lobby.SetData(key, value);
		//}

		//public static bool TryGetData(this Lobby lobby, LobbyMetadataKey key, out string value)
		//{
		//	return lobby.TryGetData(key.ToString(), out value);
		//}

		//public static bool TryGetData(this Lobby lobby, string key, out string value)
		//{
		//	value = lobby.GetData(key);
		//	if (string.IsNullOrWhiteSpace(value))
		//	{
		//		value = string.Empty;
		//		return false;
		//	}
		//	return true;
		//}

		//public static bool IsValidGameLobby(this Lobby lobby)
		//{
		//	var currentID = ProcessHandler.Instance.ID;

		//	bool isValid = true;
		//	isValid &= lobby.TryGetData(LobbyMetadataKey.AppID, out var appID);
		//	isValid &= currentID.AppID.ToString() == appID;

		//	isValid &= lobby.TryGetData(LobbyMetadataKey.GameName, out var gameName);
		//	isValid &= currentID.GameName == gameName;

		//	return isValid;
		//}

		//#region Custom Getter Setter

		//public static bool TrySetHostEndPoint(this Lobby lobby, EndPointInfo hostEndPoint)
		//{
		//	return lobby.TrySetData(LobbyMetadataKey.HostEndPoint, hostEndPoint.ToString());
		//}

		//public static bool TryGetHostEndPoint(this Lobby lobby, out EndPointInfo hostEndPoint)
		//{
		//	if (lobby.TryGetData(LobbyMetadataKey.HostEndPoint, out var value) &&
		//		EndPointInfo.TryParse(value, out hostEndPoint))
		//	{
		//		return true;
		//	}

		//	hostEndPoint = new();
		//	return false;
		//}

		//#endregion

		//#region Context Getter

		//public static string GetContext_Title(this Lobby lobby) =>
		//	lobby.TryGetData(LobbyMetadataKey.Title, out var value)
		//	? value : Localizer.GetText(TextKey.Lobby_ThereIsNoTitle);

		//public static string GetContext_Description(this Lobby lobby) =>
		//	lobby.TryGetData(LobbyMetadataKey.Description, out var value)
		//	? value : Localizer.GetText(TextKey.Lobby_ThereIsNoDescription);

		//public static string GetContext_GameVersion(this Lobby lobby) =>
		//	lobby.TryGetData(LobbyMetadataKey.GameVersion, out var gameVersion)
		//	? gameVersion : Global.Text.WRONG_VERSION;

		//public static bool GetContext_HasPassword(this Lobby lobby) =>
		//	lobby.TryGetData(LobbyMetadataKey.HasPassword, out var hasPassword) &&
		//	bool.TryParse(hasPassword, out var result) ? result : false;

		//public static string GetContext_MemberCounter(this Lobby lobby)
		//{
		//	bool isValid = true;
		//	isValid &= lobby.TryGetData(LobbyMetadataKey.CurrentMemberCount, out var currentCount);
		//	isValid &= lobby.TryGetData(LobbyMetadataKey.MaxMemberCount, out var maxCount);
		//	return isValid ? $"{currentCount}/{maxCount}" : "?/?";
		//}

		//#endregion
	}
}
