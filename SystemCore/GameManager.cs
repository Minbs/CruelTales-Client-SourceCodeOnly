using System;
using System.Collections.Generic;

namespace CTC.SystemCore
{
	/// <summary>게임 전역을 관리하는 서비스입니다.</summary>
	public class GameManager_TEMP : IManager
	{
		public void Initialize()
		{
			//GlobalService.Bind(NetworkManagerService);

			//NetworkManagerService.OnServerStarted += (networkMode) =>
			//{

			//};
		}

		public void Release()
		{
		}

		#region Gameplay

		//public void StartListenServer(EndPointInfo endPointInfo, LobbySettingInfo lobbySettingInfo)
		//{
		//	var task = startServer();

		//	async UniTask startServer()
		//	{
		//		var callback = await NetworkManagerService.TryStartServer
		//		(
		//			true,
		//			endPointInfo,
		//			lobbySettingInfo
		//		);
		//	}
		//}

		//public void LeaveGame(string stopReason)
		//{
		//	var networkManager = GlobalService.GetOrNull<NetworkManagerService>();

		//	if (networkManager.IsActivated)
		//	{
		//		networkManager.Stop();
		//	}
		//}

		//public void StartConnectToGame(EndPointInfo endPointInfo, Lobby targetLobby)
		//{
		//	var task = startConnect(endPointInfo);

		//	async UniTask startConnect(EndPointInfo endPointInfo)
		//	{
		//		// TODO : Show start popup
		//		this.Log($"Start : StartConnect");

		//		var callback = await NetworkManagerService.TryStartAsClient(endPointInfo, targetLobby);

		//		this.Log($"End : StartConnect {callback.Result}");
		//	}
		//}

		#endregion

		#region State

		// 세마포어 형태로 State를 관리합니다.
		public event Action<StateFlag> OnStateChanged;
		private readonly Dictionary<StateFlag, int> mStateSemaphoreTable = new();

		public void EnableStateFlag(StateFlag state)
		{
			lock (mStateSemaphoreTable)
			{
				mStateSemaphoreTable[state]++;
			}

			OnStateChanged?.Invoke(GetCurrentState());
		}

		public void DisableStateFlag(StateFlag state)
		{
			lock (mStateSemaphoreTable)
			{
				if (mStateSemaphoreTable.ContainsKey(state))
				{
					mStateSemaphoreTable[state]--;
					if (mStateSemaphoreTable[state] <= 0)
					{
						mStateSemaphoreTable.Remove(state);
					}
				}
			}

			OnStateChanged?.Invoke(GetCurrentState());
		}

		public StateFlag GetCurrentState()
		{
			lock (mStateSemaphoreTable)
			{
				StateFlag currentState = StateFlag.None;

				foreach (var e in StateFlagExtension.GetArray())
				{
					if (mStateSemaphoreTable.ContainsKey(e))
					{
						currentState |= e;
					}
				}

				return currentState;
			}
		}

		public bool HasState(StateFlag state)
		{
			return mStateSemaphoreTable.ContainsKey(state);
		}

		#endregion
	}
}