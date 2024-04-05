using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.Gameplay.Lobby
{
	public class Context_LobbyWaiting : ContextWithView<View_LobbyWaiting>
	{
		private readonly Property<bool> isHostProperty = new();
		public bool IsHost
		{
			get => isHostProperty.Value;
			set => isHostProperty.Value = value;
		}

		private readonly Property<bool> canStartProperty = new();
		public bool CanStartGame
		{
			get => canStartProperty.Value;
			set => canStartProperty.Value = value;
		}

		private readonly Property<bool> isReadyProperty = new();
		public bool IsReady
		{
			get => isReadyProperty.Value;
			set => isReadyProperty.Value = value;
		}

		// Room context

		public void GUI_OnClick_GameStart()
		{
			this.CurrentView.OnClick_GameStart();
		}

		public void GUI_OnClick_GameReady()
		{
			this.CurrentView.OnClick_GameReady();
		}

		public void GUI_OnClick_OpenRoomSetting()
		{
			this.CurrentView.OnClick_OpenRoomSetting();
		}
	}
}