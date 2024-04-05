#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CTC.GUI.Gameplay.Common;
using CTC.GUI.Gameplay.Lobby;
using CTC.Networks.SyncObjects.SyncObjects;

namespace CTC.GUI.MiniGames
{
	public class Lobby_Navigation : Scene_Navigation
	{
		// Reference
		[AllowNull] private CustomLobby_SceneController _lobbyController;

		// Views
		public View_LobbyWaiting? LobbyWaiting { get; private set; }
		public View_RoomSetting? RoomSetting { get; private set; }
		public View_InGameHub? InGameHub { get; private set; }
		public View_SystemNotification? SystemNotification { get; private set; }

		// Events
		[AllowNull] private Action _onRoomClose;

		public override void Initialize(SceneControllerBase sceneController)
		{
			base.Initialize(sceneController);

			_lobbyController = sceneController as CustomLobby_SceneController;

			// View lobby waiting
			LobbyWaiting = Push<View_LobbyWaiting>();
			LobbyWaiting.Initialized(_lobbyController, RoomSessionManager, ClientPlayerState);

			// Push HUD
			InGameHub = Push<View_InGameHub>();
			SystemNotification = Push<View_SystemNotification>(sortingOrder: 100);

			// Events
			_onRoomClose = CloseRoomSetting;
			RoomSessionManager.OnRoomSettingCallback += onRoomSettingCallback;
		}

		public void Dispose()
		{
			RoomSessionManager.OnRoomSettingCallback -= onRoomSettingCallback;

			Clear();
		}

		public void OnTryStartGameCallback(StartGameResultType result)
		{
			LobbyWaiting?.OnTryStartGameCallback(result);
		}

		public void OnGameStartCountdown(float second)
		{
			LobbyWaiting?.OnGameStartCountdown(second);
		}

		public void OnCancelGameStartCountdown()
		{
			LobbyWaiting?.OnCancelGameStartCountdown();
		}

		private void onRoomSettingCallback(RoomSettingResult callback)
		{
			SystemNotification?.OnSystemNotification(callback.GetText());
		}

		public void OpenRoomSetting()
		{
			if (!ReferenceEquals(RoomSetting, null))
				return;

			RoomSetting = Push<View_RoomSetting>();
			RoomSetting.Initilize(RoomSessionManager);
			RoomSetting.BindCloseAction(_onRoomClose);
		}

		public void CloseRoomSetting()
		{
			if (ReferenceEquals(RoomSetting, null))
				return;

			Pop<View_RoomSetting>();
			RoomSetting = null;
		}
	}
}

#nullable disable