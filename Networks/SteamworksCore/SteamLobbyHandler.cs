namespace CTC.Networks.SteamworksCore
{
	//public enum LobbyType
	//{
	//	None = 0,
	//	/// <summary>스팀 서버를 통해 공개로 로비를 설정합니다.</summary>
	//	Public,
	//	/// <summary>스팀 서버를 통해 비공개로 로비를 설정합니다.</summary>
	//	Private,
	//	/// <summary>스팀 서버에서 찾을 수 없습니다. 다른 친구만 방에 입장할 수 있습니다.</summary>
	//	FriendsOnly,
	//	/// <summary>스팀 서버를 통해 공개적으로 접속할 수 있습니다. 하지만 친구에게는 보이지 않습니다.</summary>
	//	Invisible,
	//}

	public class SteamLobbyHandler
	{
		//private Lobby? mLobby;

		//public bool TryGetLobby(out Lobby lobby)
		//{
		//	if (HasLobby())
		//	{
		//		lobby = mLobby.Value;
		//		return true;
		//	}

		//	lobby = new Lobby();
		//	return false;
		//}

		//public bool HasLobby()
		//{
		//	return mLobby.HasValue;
		//}

		//public bool IsOwner()
		//{
		//	if (!HasLobby())
		//	{
		//		return false;
		//	}

		//	if (!GlobalService.TryGet<SteamManager>(out var steamService))
		//	{
		//		return false;
		//	}

		//	if (!steamService.IsValid)
		//	{
		//		return false;
		//	}

		//	return mLobby.Value.IsOwnedBy(steamService.ClientSteamID);
		//}

		//public void Leave()
		//{
		//	if (HasLobby())
		//	{
		//		mLobby.Value.Leave();
		//	}

		//	mLobby = null;
		//}

		//public void BindLobby(Lobby lobby)
		//{
		//	mLobby = lobby;
		//}

		//#region Operation

		//public bool TryGetData(LobbyMetadataKey key, out string data)
		//{
		//	if (!HasLobby())
		//	{
		//		data = string.Empty;
		//		return false;
		//	}

		//	data = "";
		//	return mLobby.Value.TryGetData(key, out data);
		//}

		//public bool TrySetData(LobbyMetadataKey key, string value)
		//{
		//	if (!IsOwner())
		//	{
		//		logWarningAuthority($"SetData({key} : {value})");
		//		return false;
		//	}

		//	return mLobby.Value.TrySetData(key, value, Global.Net.Lobby.GetMetadataLimitation(key));
		//}

		//public bool TrySetMaxMembers(int maxMember)
		//{
		//	if (!IsOwner())
		//	{
		//		logWarningAuthority("MaxMembers");
		//		return false;
		//	}

		//	var lobby = mLobby.Value;
		//	lobby.MaxMembers = maxMember;
		//	return true;
		//}

		//public bool TrySetLobbyType(LobbyType lobbyType)
		//{
		//	if (!IsOwner())
		//	{
		//		logWarningAuthority($"LobbyType : {lobbyType}");
		//		return false;
		//	}

		//	TrySetData(LobbyMetadataKey.LobbyType, lobbyType.ToString());

		//	switch (lobbyType)
		//	{
		//		case LobbyType.Private:
		//			mLobby.Value.SetPrivate();
		//			break;

		//		case LobbyType.FriendsOnly:
		//			mLobby.Value.SetFriendsOnly();
		//			break;

		//		case LobbyType.Public:
		//			mLobby.Value.SetPublic();
		//			break;

		//		case LobbyType.Invisible:
		//			mLobby.Value.SetInvisible();
		//			break;

		//		default:
		//			return false;
		//	}

		//	return true;
		//}

		//public bool TrySetJoinable(bool value)
		//{
		//	if (!IsOwner())
		//	{
		//		logWarningAuthority("Joinable");
		//		return false;
		//	}

		//	TrySetData(LobbyMetadataKey.IsJoinable, value.ToString());

		//	mLobby.Value.SetJoinable(value);
		//	return true;
		//}

		//public bool TrySetup(in LobbySettingInfo setting)
		//{
		//	if (!IsOwner())
		//	{
		//		logWarningAuthority("LobbySettingInfo");
		//		return false;
		//	}

		//	// Setup Lobby Data
		//	bool isValidResult = true;

		//	isValidResult &= TrySetData(LobbyMetadataKey.GameName, setting.ProgramID.GameName);
		//	isValidResult &= TrySetData(LobbyMetadataKey.GameVersion, setting.ProgramID.Version);
		//	isValidResult &= TrySetData(LobbyMetadataKey.AppID, setting.ProgramID.AppID.ToString());

		//	isValidResult &= TrySetData(LobbyMetadataKey.Title, setting.LobbyTitle);
		//	isValidResult &= TrySetData(LobbyMetadataKey.Description, setting.LobbyDescription);
		//	isValidResult &= TrySetData(LobbyMetadataKey.HasPassword, setting.HasPassword.ToString());

		//	isValidResult &= TrySetMaxMembers(setting.MaxMembers);
		//	isValidResult &= TrySetJoinable(setting.IsJoinable);
		//	isValidResult &= TrySetLobbyType(setting.LobbyType);

		//	return isValidResult;
		//}

		//public bool TrySetHostAddress(in EndPointInfo hostAddress)
		//{
		//	if (!IsOwner())
		//	{
		//		logWarningAuthority("HostAddress");
		//		return false;
		//	}

		//	return this.mLobby.Value.TrySetHostEndPoint(hostAddress);
		//}

		//public bool TryGetLobbySettingInfo(out LobbySettingInfo lobbySettingInfo)
		//{
		//	lobbySettingInfo = new LobbySettingInfo();

		//	if (!HasLobby())
		//	{
		//		return false;
		//	}

		//	// Set program ID
		//	TryGetData(LobbyMetadataKey.GameVersion, out var gameVersion);
		//	TryGetData(LobbyMetadataKey.GameName, out var gameName);
		//	TryGetData(LobbyMetadataKey.AppID, out var appIdData);
		//	if (!uint.TryParse(appIdData, out var appID))
		//	{
		//		appID = 0;
		//	}
		//	lobbySettingInfo.ProgramID = new(gameName, gameVersion, appID);

		//	// Set lobby title
		//	TryGetData(LobbyMetadataKey.Title, out var lobbyTitle);
		//	lobbySettingInfo.LobbyTitle = lobbyTitle;

		//	// Set lobby description
		//	TryGetData(LobbyMetadataKey.Description, out var lobbyDescription);
		//	lobbySettingInfo.LobbyDescription = lobbyDescription;

		//	// Set lobby HasPassword
		//	TryGetData(LobbyMetadataKey.HasPassword, out var hasPasswordData);
		//	if (!bool.TryParse(hasPasswordData, out var hasPassword))
		//	{
		//		hasPassword = false;
		//	}
		//	lobbySettingInfo.HasPassword = hasPassword;

		//	// Set member counts
		//	lobbySettingInfo.MaxMembers = mLobby.Value.MaxMembers;

		//	// Set lobby joinable
		//	TryGetData(LobbyMetadataKey.IsJoinable, out var isJoinableData);
		//	if (!bool.TryParse(isJoinableData, out var isJoinable))
		//	{
		//		isJoinable = Global.Net.Lobby.DEFAULT_IS_JOINABLE;
		//	}
		//	lobbySettingInfo.IsJoinable = isJoinable;

		//	// Set lobby type
		//	TryGetData(LobbyMetadataKey.LobbyType, out var lobbyTypeData);
		//	bool hasLobbyType = Enum.TryParse(typeof(LobbyType), lobbyTypeData, out object lobbyTypeResult);
		//	lobbySettingInfo.LobbyType = hasLobbyType ? (LobbyType)lobbyTypeResult : LobbyType.None;


		//	return true;
		//}

		//public bool TryInviteFriend(SteamId friendInfo)
		//{
		//	if (!HasLobby())
		//	{
		//		return false;
		//	}
		//	return mLobby.Value.InviteFriend(friendInfo);
		//}

		//#endregion

		//private void logWarningAuthority(string operation)
		//{
		//	Logging.LogWarning($"You cannot set lobby \"{operation}\" when you are not owner of the lobby!");
		//}
	}
}
