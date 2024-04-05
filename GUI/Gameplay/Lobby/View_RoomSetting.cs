using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CT.Logger;
using CT.Networks;
using CTC.GUI.Components;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.SystemCore;

namespace CTC.GUI.Gameplay.Lobby
{
	public class View_RoomSetting : ViewBaseWithContext
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(View_RoomSetting));

		// Reference
		private RoomSessionManager _roomSessionManager;

		private Action _onClose;

		public Context_RoomSetting BindedContext;
		public Pagination MaxPlayerCountPagination;
		public Pagination MinPlayerCountPagination;

		private List<object> _playerCountOptions;

		private string _roomName;
		private string _roomDiscription;
		private int _password;

		protected override void Awake()
		{
			base.Awake();

			_playerCountOptions = new();
			int minUser = GlobalNetwork.SYSTEM_MIN_USER;
			int maxUser = GlobalNetwork.SYSTEM_MAX_USER;
			for (int i = minUser; i <= maxUser; i++)
			{
				_playerCountOptions.Add(i);
			}
		}

		protected override void onBeginShow()
		{
			this.BindedContext = this.CurrentContext as Context_RoomSetting;
		}

		protected override void onAfterHide()
		{
			Dispose();
		}

		public void Initilize(RoomSessionManager roomSessionManager)
		{
			_roomSessionManager = roomSessionManager;

			// Bind escape key map
			GlobalService.InputManager.OnEscape += onEscape;


			_roomName = _roomSessionManager.RoomName;
			_roomDiscription = _roomSessionManager.RoomDiscription;
			_password = _roomSessionManager.Password;

			BindedContext.RoomName = _roomName;
			BindedContext.RoomDiscription = _roomDiscription;
			BindedContext.Password = _password;
			BindedContext.CallBackMessage = string.Empty;

			// Bind Setting option
			MaxPlayerCountPagination.Initialize(_playerCountOptions);
			MaxPlayerCountPagination.SetIndex(_roomSessionManager.MaxPlayerCount - GlobalNetwork.SYSTEM_MIN_USER);
			MinPlayerCountPagination.Initialize(_playerCountOptions);
			MinPlayerCountPagination.SetIndex(_roomSessionManager.MinPlayerCount - GlobalNetwork.SYSTEM_MIN_USER);
		}

		public void Dispose()
		{
			// Release escape key map
			GlobalService.InputManager.OnEscape -= onEscape;

			_roomSessionManager = null;
		}

		public void BindCloseAction(Action onClose)
		{
			_onClose = onClose;
		}

		public void OnClick_Apply(Context_RoomSetting context)
		{
			if (!GlobalService.GameplayController.IsHost)
			{
				BindedContext.CallBackMessage = RoomSettingResult.YouAreNotHost.GetText();
				_log.Error("You are not a host!");
				return;
			}

			if (_roomSessionManager == null)
			{
				_log.Error($"There is no {nameof(_roomSessionManager)}!");
				return;
			}

			if (!string.Equals(_roomName, context.RoomName))
			{
				_roomName = context.RoomName;
				_roomSessionManager.ClientRoomSetReq_SetRoomName(_roomName);
			}

			if (!string.Equals(_roomDiscription, context.RoomDiscription))
			{
				_roomDiscription = context.RoomDiscription;
				_roomSessionManager.ClientRoomSetReq_SetRoomDiscription(_roomDiscription);
			}

			if (_password != context.Password)
			{
				_password = context.Password;
				_roomSessionManager.ClientRoomSetReq_SetPassword(_password);
			}

			int selectMaxPlayerCount = MaxPlayerCountPagination.GetCurrentOption<int>();
			if (_roomSessionManager.MaxPlayerCount != selectMaxPlayerCount)
			{
				_roomSessionManager.ClientRoomSetReq_SetRoomMaxUser(selectMaxPlayerCount);
			}

			int selectMinPlayerCount = MinPlayerCountPagination.GetCurrentOption<int>();
			if (_roomSessionManager.MinPlayerCount != selectMinPlayerCount)
			{
				_roomSessionManager.ClientRoomSetReq_SetRoomMinUser(selectMinPlayerCount);
			}
		}

		public void OnClick_Cancel()
		{
			_onClose?.Invoke();
		}

		private void onEscape() => _onClose?.Invoke();
	}
}
