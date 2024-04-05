//using Cysharp.Threading.Tasks;
//using System;
//using CruelTales.Data;
//using CruelTales.DataBind.Contexts;
//using CruelTales.GUI.View;
//using CruelTales.Net;
//using Legacy.CruelTales.Net;
//using Utils;
//using CruelTales;
//using Legacy.CruelTales.GUI.View;

//namespace Legacy.CruelTales.GUI.Contexts.ServerBrowser
//{
//	[Obsolete("Legacy")]
//	public class Context_CreateLobby : ContextWithView<View_CreateLobby>
//	{
//		private UserDataService mUserDataService;
//		private LobbyCreationInputFieldCache InputFieldCache
//			=> mUserDataService.UserDataSet.InputFieldCache;

//		public Context_CreateLobby()
//		{
//			mUserDataService = GlobalService.GetOrNull<UserDataService>();
//		}

//		public string LobbyTitle
//		{
//			get => InputFieldCache.LobbyTitle;
//			set => InputFieldCache.LobbyTitle = value;
//		}
//		public string LobbyDescription
//		{
//			get => InputFieldCache.LobbyDescription;
//			set => InputFieldCache.LobbyDescription = value;
//		}
//		public string LobbyPassword
//		{
//			get => InputFieldCache.LobbyPassword;
//			set => InputFieldCache.LobbyPassword = value;
//		}
//		public bool CanSeeRawPassword
//		{
//			get => InputFieldCache.CanSeePassword;
//			set => InputFieldCache.CanSeePassword = value;
//		}

//		public string LobbyMaxMember
//		{
//			get => InputFieldCache.LobbyMaxMembers.ToString();
//			set
//			{
//				int memberCount = int.TryParse(value, out int count)
//					? count : Global.Net.Lobby.DEFAULT_MAX_PLAYER;

//				InputFieldCache.LobbyMaxMembers = Global.Net.GetValidMaxPlayerCount(memberCount);
//			}
//		}

//		public bool IsJoinable
//		{
//			get => InputFieldCache.IsLobbyJoinable;
//			set => InputFieldCache.IsLobbyJoinable = value;
//		}
//		public LobbyType IsInvisible
//		{
//			get => InputFieldCache.LobbyType;
//			set => InputFieldCache.LobbyType = value;
//		}

//		public void GUI_OnClick_CloseLobbyCreatePopup()
//		{
//			this.CurrentView.HideItself();
//		}

//		public void GUI_OnClick_MakeLobby()
//		{
//			if (!GlobalService.TryGet<SteamManager>(out var steamService) || !steamService.IsValid)
//			{
//				Logging.LogError($"Steam is invalid!");
//				return;
//			}

//			var lobbySetting = this.InputFieldCache.GetLobbySetting();
//			lobbySetting.ProgramID = ProcessHandler.Instance.ID;

//			Logging.Log(LogChanel.Network, $"Start create lobby!");

//			var clientID = GlobalService.GetOrNull<SteamManager>().ClientSteamID;
//			GlobalService.GetOrNull<GameManagerService>().StartListenServer(new EndPointInfo(clientID), lobbySetting);
//		}
//	}
//}