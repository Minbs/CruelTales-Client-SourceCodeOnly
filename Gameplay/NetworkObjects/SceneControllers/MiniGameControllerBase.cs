#nullable enable
#pragma warning disable CS0649

using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Tools.Collections;
using CT.Logger;
using CTC.Globalizations;
using CTC.GUI.MiniGames;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class MiniGameControllerBase : SceneControllerBase
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MiniGameControllerBase));

		private MiniGame_Navigation _miniGameNavigation;
		public readonly BidirectionalMap<UserId, PlayerCharacter> PlayerCharacterTable = new();

		public override void OnCreated()
		{
			GameplayController.OnSceneControllerLoaded(this);
		}

		public override void OnDestroyed()
		{
			base.OnDestroyed();
			_miniGameNavigation.Dispose();
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			_currentTime -= deltaTime;
		}

		public void Initialize(MiniGame_Navigation navigation)
		{
			_miniGameNavigation = navigation;
		}

		public void ApplyInteractResult(InteractResultType result, float ProgressTime, TextKey info)
		{
			_miniGameNavigation.ApplyInteractResult(result, ProgressTime, info);
		}

		public override void OnPlayerCharacterCreated(PlayerCharacter playerCharacter)
		{
			PlayerCharacterTable.TryAdd(playerCharacter.UserId, playerCharacter);
			_miniGameNavigation.OnPlayerCharacterCreated(playerCharacter);
		}

		public override void OnPlayerCharacterDestroyed(PlayerCharacter playerCharacter)
		{
			PlayerCharacterTable.TryRemove(playerCharacter);
			_miniGameNavigation.OnPlayerCharacterDestroyed(playerCharacter);
		}

		#region Flow

		/// <summary>서버의 남은 게임 시간을 동기화합니다.</summary>
		/// <param name="timeLeft">남은 시간입니다.</param>
		public virtual partial void Server_SyncTimer(float timeLeft)
		{
			_currentTime = timeLeft;
			_miniGameNavigation.SetTimer(CurrentTime);
		}

		/// <summary>게임이 시작됩니다.</summary>
		/// <param name="missionShowTime">Mission을 보여주는 시간입니다.</param>
		/// <param name="countdown">게임 시작 카운트다운입니다.</param>
		public virtual partial void Server_GameStartCountdown(float missionShowTime, float countdown)
		{
			_miniGameNavigation.GameStartCountdown(GameSceneIdentity.Mode,
												   missionShowTime,
												   countdown);
		}

		/// <summary>게임이 시작되었습니다.</summary>
		/// <param name="timeLeft">남은 시간입니다.</param>
		public virtual partial void Server_GameStart(float timeLeft)
		{
			_miniGameNavigation.GameStart(timeLeft);
		}

		/// <summary>피버타임이 시작되었습니다.</summary>
		public virtual partial void Server_FeverTimeStart()
		{
			_miniGameNavigation.FeverTimeStart(MapData.GameSceneIdentity.Mode);
		}

		/// <summary>게임이 종료되었습니다. 플레이어들은 그 자리에 서있고 게임 결과를 기다립니다.</summary>
		/// <param name="freezeTime">정지 시간입니다.</param>
		public virtual partial void Server_GameEnd(float freezeTime)
		{
			_miniGameNavigation.GameEnd(freezeTime);
		}

		/// <summary>게임 플레이 결과를 보여줍니다.</summary>
		/// <param name="resultTime">결과를 보여주는 시간입니다.</param>
		public virtual partial void Server_ShowResult(float resultTime)
		{
			_miniGameNavigation.ShowResult(resultTime);
		}

		/// <summary>처형씬을 보여줍니다.</summary>
		/// <param name="cutSceneType">처형씬의 타입입니다.</param>
		/// <param name="playTime">처형씬 플레이타임입니다. 플레이 타임 이후 맵 투표가 시작됩니다.</param>
		public virtual partial void Server_ShowExecution(ExecutionCutSceneType cutSceneType, float playTime)
		{
			_miniGameNavigation.OnExecution();
			// TODO : Show execution
			// EliminatedPlayers
			string eliminatedPlayers = "Eliminated players : ";
			foreach (var p in EliminatedPlayers)
			{
				if (GameplayController.RoomSessionManager.PlayerStateTable.TryGetValue(p, out var state))
				{
					eliminatedPlayers += $"({state.Username})";
				}
			}
			_log.Info(eliminatedPlayers);
		}

		/// <summary>맵 투표가 시작됩니다.</summary>
		/// <param name="mapVoteTime">맵 투표 제한시간입니다.</param>
		public virtual partial void Server_StartVoteMap(float mapVoteTime)
		{
			_miniGameNavigation.ShowVoteMap(mapVoteTime);
		}

		/// <summary>선택된 맵을 보여줍니다.</summary>
		/// <param name="gameId">선택된 맵입니다.</param>
		/// <param name="showTime">선택된 맵을 보여주는 시간입니다.</param>
		public virtual partial void Server_ShowVotedNextMap(GameSceneIdentity nextMap, float showTime)
		{
			_miniGameNavigation.ShowVotedNextMap(nextMap, showTime);
		}

		#endregion
	}
}

#pragma warning restore CS0649
