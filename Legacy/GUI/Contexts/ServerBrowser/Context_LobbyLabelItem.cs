//using CruelTales.Data;
//using CruelTales.Net;
//using Slash.Unity.DataBind.Core.Data;
//using Steamworks.Data;
//using Utils;

//namespace CruelTales.GUI.Contexts.ServerBrowser
//{
//	public class Context_LobbyLabelItem : Context
//	{
//		private readonly Property<Lobby> mLobbyProperty = new();

//		public Context_LobbyLabelItem(Lobby lobby)
//		{
//			mLobbyProperty.Value = lobby;
//		}

//		public Lobby Lobby => mLobbyProperty.Value;

//		public string MemberCounter
//		{
//			get
//			{
//				bool isValid = true;
//				isValid &= Lobby.TryGetData(LobbyMetadataKey.CurrentMemberCount, out var currentCount);
//				isValid &= Lobby.TryGetData(LobbyMetadataKey.MaxMemberCount, out var maxCount);
//				return isValid ? $"{currentCount}/{maxCount}" : "?/?";
//				return $"-/-";
//			}
//		}

//		public string Title => Lobby.GetContext_Title();
//		public string Description => Lobby.GetContext_Description();
//		public string GameVersion => Lobby.GetContext_GameVersion();
//		public bool HasPassword => Lobby.GetContext_HasPassword();

//		public void GUI_OnClick_LobbyItem()
//		{
//			Lobby.TryGetHostEndPoint(out var hostEndpoint);

//			switch (hostEndpoint.Platform)
//			{
//				case NetworkPlatform.Steam:

//					GlobalService.GetOrNull<GameManagerService>().StartConnectToGame(hostEndpoint, Lobby);
//					break;

//				case NetworkPlatform.Standalone:
//					GlobalService.GetOrNull<GameManagerService>().StartConnectToGame(hostEndpoint, Lobby);
//					break;

//				default:
//					Logging.LogError(LogChanel.Lobby, $"Cannot parse host address! Host address : {hostEndpoint}");
//					break;
//			}

//		}
//	}
//}