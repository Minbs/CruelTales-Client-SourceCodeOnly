#nullable enable
#pragma warning disable CS0649

using System;
using CT.Common.DataType;
using CT.Logger;
using CTC.SystemCore;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class RoomSessionManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(RoomSessionManager));

		public event Action<RoomSettingResult> OnRoomSettingCallback;

		public int PlayerCount => PlayerStateTable.Count;

		public PlayerState? ClientPlayerState
		{
			get
			{
				UserId id = GlobalService.BackendManager.UserId;
				return PlayerStateTable.TryGetValue(id, out var playerState)
						? playerState : null;
			}
		}

		public partial void ServerRoomSetAck_Callback(RoomSettingResult callback)
		{
			OnRoomSettingCallback?.Invoke(callback);
		}

		public StartGameResultType CheckStartGameState()
		{
			if (PlayerCount < MinPlayerCount)
				return StartGameResultType.NoEnoughPlayer;

			if (PlayerCount > MaxPlayerCount)
				return StartGameResultType.TooManyPlayer;

			var clientState = ClientPlayerState;
			if (clientState == null)
				return StartGameResultType.FatalError;

			if (!clientState.IsHost)
				return StartGameResultType.YouAreNotHost;

			return StartGameResultType.Success;
		}
	}
}
#pragma warning restore CS0649
