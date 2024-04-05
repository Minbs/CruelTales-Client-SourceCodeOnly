using CTC.DataBind.Contexts;
using CTC.SystemCore;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.Gameplay.Lobby
{
	public class Context_LobbyMain : ContextWithView<View_LobbyMain>
	{
		private readonly Property<string> roomNumberProperty = new();
		public string RoomNumber
		{
			get
			{
				var id = GlobalService
					.NetworkManager
					.GameServerEndPoint
					.GameInstanceGuid.Guid;

				return id.ToString("D6");
			}
		}

		private readonly Property<string> roomNameProperty = new();
		public string RoomName
		{
			get => roomNameProperty.Value;
			set => roomNameProperty.Value = value;
		}

		private readonly Property<bool> isHostProperty = new();
		public bool IsHost
		{
			get => isHostProperty.Value;
			set => isHostProperty.Value = value;
		}

		public void GUI_OnClick_RoomSetting()
		{
			this.CurrentView.OnClick_RoomSetting();
		}

		public void GUI_OnClick_Menu()
		{
			this.CurrentView.OnClick_Menu();
		}

		public void GUI_OnClick_PlayerInfoList()
		{
			this.CurrentView.OnClick_PlayerInfoList();
		}

		public void GUI_OnClick_Leave()
		{
			this.CurrentView.OnClick_Leave();
		}
	}
}
