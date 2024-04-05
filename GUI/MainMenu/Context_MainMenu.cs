using CTC.DataBind.Contexts;
using CTC.SystemCore;
using CTC.Utils.Networks;
using Cysharp.Threading.Tasks;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.MainMenu
{
	public class Context_MainMenu : ContextWithView<View_MainMenu>
	{
		private readonly Property<string> announcementContextProperty = new();
		public string AnnouncementContext
		{
			get => this.announcementContextProperty.Value;
			set => this.announcementContextProperty.Value = value;
		}

		private readonly Property<string> usernameProperty = new();
		public string Username
		{
			get => GlobalService.BackendManager.Username;
			set => GlobalService.BackendManager.Username = value;
		}

		private readonly Property<string> ipAddressProperty = new();
		public string IpAddress
		{
			get => GlobalService.NetworkManager.GameServerEndPoint.IPEndPoint;
			set => GlobalService.NetworkManager.GameServerEndPoint.IPEndPoint = value;
		}

		private readonly Property<string> portProperty = new();
		public string Port
		{
			get => GlobalService.NetworkManager.GameServerEndPoint.Port.ToString();
			set
			{
				int endPort = int.TryParse(value, out var port) ? port : 60128;
				GlobalService.NetworkManager.GameServerEndPoint.Port = endPort;
			}
		}

		private readonly Property<string> roomNumberProperty = new();
		public string RoomNumber
		{
			get => GlobalService.NetworkManager.GameServerEndPoint.GameInstanceGuid.ToString();
			set
			{
				ulong guidNumber = ulong.TryParse(value, out var number) ? number : 60128;
				GlobalService.NetworkManager.GameServerEndPoint.GameInstanceGuid = new(guidNumber);
			}
		}

		private readonly Property<string> roomPasswordProperty = new();
		public string RoomPassword
		{
			get => GlobalService.NetworkManager.GameServerEndPoint.Password.ToString();
			set
			{
				int password = int.TryParse(value, out var number) ? number : -1;
				GlobalService.NetworkManager.GameServerEndPoint.Password = password;
			}
		}

		public Context_MainMenu()
		{
			this.AnnouncementContext = "Announcement Context";
			var t = loadWebContext();
		}

		private async UniTask loadWebContext()
		{
			string announcementUrl = GlobalService.ConfigManager.AnnouncementUrl;
			string ctx = await WebUtility.GetDataOnEditorAsync(announcementUrl);
			ctx = ctx.Replace("\t", "<br>");
			this.AnnouncementContext = ctx;
		}

		public void GUI_OnClick_MatchingGame()
		{
			this.CurrentView.OnClick_MatchingGame();
		}

		public void GUI_OnClick_TabInventory()
		{
			CurrentView.OnClick_Closet();
		}

		public void GUI_OnClick_TabBattlePass()
		{
			this.AnnouncementContext += $"<br>{nameof(GUI_OnClick_TabBattlePass)}";
		}

		public void GUI_OnClick_TabAchievements()
		{
			this.AnnouncementContext += $"<br>{nameof(GUI_OnClick_TabAchievements)}";
		}

		public void GUI_OnClick_TabShop()
		{
			CurrentView.OnClick_Closet();
		}

		public void GUI_OnClick_Close()
		{
			CurrentView.OnClick_Close();
		}

		public void GUI_OnClick_Setting()
		{
			CurrentView.OnClick_Setting();
		}

		public void GUI_OnClick_TestTryConnect()
		{
			this.CurrentView.OnClick_TestTryConnect();
			this.AnnouncementContext += $"<br>{nameof(GUI_OnClick_TestTryConnect)}";
		}

		public void GUI_OnClick_GotoTestScene()
		{
			this.CurrentView.OnClick_GotoTestScene();
		}
	}
}
