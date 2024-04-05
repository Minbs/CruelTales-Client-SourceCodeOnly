using System;
using CT.Common.DataType;
using CTC.Globalizations;
using CTC.GUI.Gameplay.Overlay;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.SystemCore;
using UnityEngine;

namespace CTC.GUI.Gameplay.Lobby
{
	[Obsolete]
	public class View_LobbyMain : ViewBaseWithContext
	{
		[field: SerializeField]
		public ViewNavigation InContentNavigation { get; private set; }

		[field: SerializeField]
		public ViewNavigation OutContentNavigation { get; private set; }

		public Context_LobbyMain BindedContext { get; private set; }

		// Reference
		public Navigation_SystemControl ParentNav_SystemControl { get; private set; }

		private PlayerState _playerState;

		private RoomSessionManager _roomSessionManager;

		protected override void onBeginShow()
		{
			base.onBeginShow();
			this.BindedContext = this.CurrentContext as Context_LobbyMain;
			ParentNav_SystemControl = (Navigation_SystemControl)ParentNavigation;

			BindPlayerState(GlobalService.GameplayController.ClientPlayerState);

			_roomSessionManager = GlobalService.GameplayController.RoomSessionManager;
			_roomSessionManager.OnRoomNameChanged += onRoomNameChanged;
			onRoomNameChanged(_roomSessionManager.RoomName);
		}

		protected override void onAfterHide()
		{
			base.onAfterHide();

			if (_playerState != null)
			{
				_playerState.OnIsHostChanged -= onIsHostChanged;
			}

			_roomSessionManager.OnRoomNameChanged -= onRoomNameChanged;
			_roomSessionManager = null;
		}

		public void BindPlayerState(PlayerState playerState)
		{
			if (playerState == null)
				return;

			if (_playerState == null)
			{
				_playerState = playerState;
				BindedContext.IsHost = _playerState.IsHost;
				_playerState.OnIsHostChanged += onIsHostChanged;
			}
		}

		private void onIsHostChanged(bool isHost)
		{
			BindedContext.IsHost = isHost;
		}

		public void EscapePressed()
		{
			OpenLobbyWatting();
		}

		public View_LobbyWaiting OpenLobbyWatting()
		{
			var view = InContentNavigation.Switch<View_LobbyWaiting>();
			OutContentNavigation.Clear();
			return view;
		}

		public void OpenRoomSetting()
		{
			if (InContentNavigation.HasView<View_RoomSetting>())
			{
				InContentNavigation.Pop<View_RoomSetting>();
				return;
			}
			var view = InContentNavigation.Switch<View_RoomSetting>();
			view.BindCloseAction(EscapePressed);
			OutContentNavigation.Clear();
		}

		public void OpenPlayerInfoList()
		{
			InContentNavigation.Clear();
			var playerInfoListView = OutContentNavigation.Switch<View_PlayerInfoList>();
			var playerStateTable = GlobalService.GameplayController.RoomSessionManager.PlayerStateTable;
			playerInfoListView.Initialize(playerStateTable);
		}

		public void OpenMenu()
		{
			OpenLobbyWatting();
		}

		public void OnClick_RoomSetting()
		{
			OpenRoomSetting();
		}

		public void OnClick_PlayerInfoList()
		{
			OpenPlayerInfoList();
		}

		public void OnClick_Menu()
		{
			OpenMenu();
		}

		public void OnClick_Leave()
		{
			string title = TextKey.Dialog_WantLeaveGame_Title.GetText();
			string content = TextKey.Dialog_WantLeaveGame_Content.GetText();
			GlobalService.StaticGUI
				.OpenSystemDialogPopup(isTemporary: true, title, content, responseCallback: (response) =>
				{
					if (response == DialogResult.Yes)
					{
						GlobalService.NetworkManager.Disconnect();
					}
				},
				DialogResult.Yes, DialogResult.No);
		}

		private void onRoomNameChanged(NetStringShort roomName)
			=> BindedContext.RoomName = roomName.Value;
	}
}
