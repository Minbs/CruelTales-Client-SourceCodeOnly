////using Cysharp.Threading.Tasks;
//using CruelTales.DataBind.Contexts;
//using CruelTales.GUI.View;
//using CruelTales.Net;
//using Slash.Unity.DataBind.Core.Data;
//using Utils;
//using System.Threading.Tasks;

//namespace CruelTales.GUI.Contexts.ServerBrowser
//{
//	public class Context_ServerBrowser : ContextWithView<View_ServerBrowser>
//	{
//		// TODO : Make lobby context
//		private readonly Property<Collection<Context_LobbyLabelItem>> mLobbyLabelItems
//			= new(new Collection<Context_LobbyLabelItem>());

//		public Collection<Context_LobbyLabelItem> LobbyLabelItems
//		{
//			get => mLobbyLabelItems.Value;
//			set => mLobbyLabelItems.Value = value;
//		}

//		public void GUI_OnClick_CloseServerBrowser()
//		{
//			this.CurrentView.HideItself();
//		}

//		public void GUI_OnClick_RefreshServerList()
//		{
//			var task = refreshServerListAsync();
//			async Task refreshServerListAsync()
//			{
//				// Clear lobby list
//				mLobbyLabelItems.Value.Clear();

//				try
//				{
//					//var queryLobbys = await GlobalService.GetOrNull<NetworkManagerService>().RequestLobbies();

//					//foreach (var lobby in queryLobbys)
//					//{
//					//	if (!lobby.IsValidGameLobby())
//					//	{
//					//		continue;
//					//	}

//					//	Context_LobbyLabelItem item = new(lobby);
//					//	mLobbyLabelItems.Value.Add(item);
//					//}
//				}
//				catch
//				{
//					Logging.LogWarning(this, $"There was no lobbies.");
//				}
//			}
//		}

//		public void GUI_OnClick_ShowLobbyCreatePopup()
//		{
//			this.CurrentView.OnClick_ShowLobbyCreatePopup();
//		}
//	}
//}