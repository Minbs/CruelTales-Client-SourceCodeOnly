#nullable enable

using System;
using CT.Common.Gameplay;
using CTC.Globalizations;
using CTC.GUI.Gameplay.Common;
using CTC.GUI.Gameplay.Common.MissionBoard;
using CTC.GUI.MiniGameSelect;
using CTC.Networks.SyncObjects.SyncObjects;

namespace CTC.GUI.MiniGames
{
	public class MiniGame_Navigation : Scene_Navigation
	{
		private View_MissionBoardHandler? _missionBoardHandler;
		private View_MiniGameTimer? _miniGameTimer;
		private View_MiniGameSelect? _miniGameSelect;

		// Common
		private View_InGameHub? _inGameHub;
		private View_InteractionProgress? _interactionProgress;

		public virtual void ShowMiniGameViews()
		{
			var miniGameController = SceneController as MiniGameControllerBase;

			_inGameHub = Push<View_InGameHub>();
			_interactionProgress = Push<View_InteractionProgress>();
			_interactionProgress.ProgressBarConcealed();
		}

		public virtual void HideMiniGameViews()
		{
			if (_inGameHub != null)
			{
				PopMatch((viewObj) => viewObj.View == _inGameHub);
				PopMatch((viewObj) => viewObj.View == _interactionProgress);
			}
		}

		public void GameStartCountdown(GameModeType gameMode,
									   float missionShowTime,
									   float countdown)
		{
			Clear();
			_missionBoardHandler = Push<View_MissionBoardHandler>();
			_missionBoardHandler.OnGameStartCountdown(gameMode, missionShowTime, countdown);
		}

		public virtual void OnPlayerCharacterCreated(PlayerCharacter playerCharacter)
		{
		}

		public virtual void OnPlayerCharacterDestroyed(PlayerCharacter playerCharacter)
		{
		}

		public void GameStart(float timeLeft)
		{
			//Clear();
			_miniGameTimer = Push<View_MiniGameTimer>();
			SetTimer(timeLeft);
			ShowMiniGameViews();
		}

		public void FeverTimeStart(GameModeType gameMode)
		{
			var view = Push<View_FeverTime>();
			view.Initialized(gameMode);
		}

		public void SetTimer(float timeLeft)
		{
			_miniGameTimer?.SetCurrentTime(timeLeft);
			_miniGameTimer?.StartTimer();
		}

		public void GameEnd(float freezeTime)
		{
			Clear();
			Push<View_GameEnd>();
		}

		public void ShowResult(float resultTime)
		{
			Clear();
			Push<View_Result>();
		}

		public void OnExecution()
		{
			Clear();
		}

		public void ShowVoteMap(float mapVoteTime)
		{
			Clear();
			_miniGameSelect = Push<View_MiniGameSelect>();
			_miniGameSelect.SetVoteTimeLeft(mapVoteTime);
		}

		public void ShowVotedNextMap(GameSceneIdentity nextMap, float showTime)
		{
			// TODO : Show next map
			Push<View_MiniGameVoteResult>();
		}

		public void Dispose()
		{
			Clear();
		}

		public void ApplyInteractResult(InteractResultType result, float ProgressTime, TextKey info)
		{
			if (_interactionProgress == null)
				return;
			_interactionProgress.OnInteractResult(result, ProgressTime, info);
		}
	}
}

#nullable disable