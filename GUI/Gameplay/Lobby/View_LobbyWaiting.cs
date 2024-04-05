using CT.Common.DataType;
using CT.Logger;
using CTC.GUI.MiniGames;
using CTC.Networks.SyncObjects.SyncObjects;
using Slash.Unity.DataBind.Core.Presentation;
using TMPro;
using UnityEngine;

namespace CTC.GUI.Gameplay.Lobby
{
	public class View_LobbyWaiting : ViewBaseWithContext
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(View_LobbyWaiting));

		public Context_LobbyWaiting BindedContext { get; private set; }

		[field: SerializeField]
		public ContextHolder RoomInfoContextHolder { get; private set; }
		public Context_RoomInfo RoomInfoContext 
			=> RoomInfoContextHolder.Context as Context_RoomInfo;

		private CustomLobby_SceneController _lobbyController;
		private RoomSessionManager _roomSessionManager;
		private PlayerState _clientPlayerState;

		// Callback
		[SerializeField]
		private TextMeshProUGUI _tryGameStartCallbackText;

		// Components
		[SerializeField] private Item_GameStartTimer _gameStartTimer;

		public void Initialized(CustomLobby_SceneController lobbyController,
								RoomSessionManager roomSessionManager,
								PlayerState playerState)
		{
			_lobbyController = lobbyController;

			// Room Session Manager
			_roomSessionManager = roomSessionManager;

			_roomSessionManager.PlayerStateTable.OnAdded += playerStateTable_OnAdded;
			_roomSessionManager.PlayerStateTable.OnRemoved += playerStateTable_OnRemoved;
			_roomSessionManager.PlayerStateTable.OnChanged += onPlayerStateChanged;
			onPlayerCountChanged();

			_roomSessionManager.OnRoomNameChanged += onRoomNameChanged;
			onRoomNameChanged(_roomSessionManager.RoomName);
			_roomSessionManager.OnRoomDiscriptionChanged += onRoomDiscriptionChanged;
			onRoomDiscriptionChanged(_roomSessionManager.RoomDiscription);

			// Player state
			_clientPlayerState = playerState;

			_clientPlayerState.OnIsHostChanged += onIsHostChanged;
			onIsHostChanged(_clientPlayerState.IsHost);
			_clientPlayerState.OnIsReadyChanged += onIsReadyChanged;
			onIsReadyChanged(_clientPlayerState.IsReady);

			// Callback
			_tryGameStartCallbackText.alpha = 0.0f;

			// Components
			_gameStartTimer.gameObject.SetActive(false);
		}

		public void Dispose()
		{
			if (_roomSessionManager != null)
			{
				_roomSessionManager.PlayerStateTable.OnAdded -= playerStateTable_OnAdded;
				_roomSessionManager.PlayerStateTable.OnRemoved -= playerStateTable_OnRemoved;
				_roomSessionManager.PlayerStateTable.OnChanged -= onPlayerStateChanged;

				_roomSessionManager.OnRoomNameChanged -= onRoomNameChanged;
				_roomSessionManager.OnRoomDiscriptionChanged -= onRoomDiscriptionChanged;
				_roomSessionManager = null;
			}

			if (_clientPlayerState != null)
			{
				_clientPlayerState.OnIsHostChanged -= onIsHostChanged;
				_clientPlayerState.OnIsReadyChanged -= onIsReadyChanged;
				_clientPlayerState = null;
			}
		}

		public void Update()
		{
			if (_tryGameStartCallbackText.alpha > 0)
			{
				_tryGameStartCallbackText.alpha -= Time.deltaTime * 0.4f;
			}
		}

		protected override void onBeginShow()
		{
			this.BindedContext = this.CurrentContext as Context_LobbyWaiting;
		}

		protected override void onAfterHide()
		{
			Dispose();
		}

		public void OnTryStartGameCallback(StartGameResultType result)
		{
			_tryGameStartCallbackText.text = result.GetText();
			_tryGameStartCallbackText.alpha = 1.0f;
		}

		public void OnGameStartCountdown(float second)
		{
			_gameStartTimer.gameObject.SetActive(true);
			_gameStartTimer.StartTimer(second);
		}

		public void OnCancelGameStartCountdown()
		{
			_gameStartTimer.StopTimer();
			_gameStartTimer.gameObject.SetActive(false);
		}

		private void onIsHostChanged(bool isHost)
		{
			BindedContext.IsHost = isHost;
		}

		private void onIsReadyChanged(bool isReady)
		{
			BindedContext.IsReady = isReady;
		}

		private void onRoomDiscriptionChanged(NetStringShort roomDiscription)
		{
			RoomInfoContext.RoomDiscription = roomDiscription;
		}

		private void onRoomNameChanged(NetStringShort roomName)
		{
			RoomInfoContext.RoomName = roomName;
		}

		private void playerStateTable_OnAdded(UserId arg1, PlayerState arg2)
			=> onPlayerCountChanged();

		private void playerStateTable_OnRemoved(UserId obj)
			=> onPlayerCountChanged();

		private void onPlayerStateChanged(UserId userId, PlayerState state)
		{
			onPlayerCountChanged();
			foreach (var p in _roomSessionManager.PlayerStateTable.Values)
			{
				if (p.IsHost)
					continue;

				if (!p.IsReady)
				{
					BindedContext.CanStartGame = false;
				}
			}

			BindedContext.CanStartGame = true;
		}

		private void onPlayerCountChanged()
		{
			RoomInfoContext.PlayerCount = $"Player Count : {_roomSessionManager.PlayerCount}";
		}

		public void OnClick_GameStart()
		{
			if (_roomSessionManager == null)
				return;

			// TODO : 클라이언트에서 시작 가능 여부를 판단하는 로직이 유효한지 확인이 필요
			//var result = _roomSessionManager.CheckStartGameState();
			//if (result != StartGameResultType.Success)
			//{
			//	OnTryStartGameCallback(result);
			//	return;
			//}

			if (_lobbyController == null)
				return;

			_lobbyController.Client_ReadyGame(true);
		}

		public void OnClick_GameReady()
		{
			if (_clientPlayerState != null)
			{
				_lobbyController.Client_ReadyGame(!_clientPlayerState.IsReady);
			}
		}

		public void OnClick_OpenRoomSetting()
		{
			if (ParentNavigation is not Lobby_Navigation nav)
				return;

			nav.OpenRoomSetting();
		}
	}
}
