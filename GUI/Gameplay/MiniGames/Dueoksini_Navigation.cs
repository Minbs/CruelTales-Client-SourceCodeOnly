#nullable enable

using System.Diagnostics.CodeAnalysis;
using CT.Common.Gameplay;
using CTC.GUI.Gameplay.Common;
using CTC.Networks.SyncObjects.SyncObjects;

namespace CTC.GUI.MiniGames
{
	public class Dueoksini_Navigation : MiniGame_Navigation
	{
		private static readonly string _title = @"두억시니의 책";
		private static readonly string _hint = @"맵 곳곳에 숨겨진 음식을 찾아 밥상을 차리자!";

		// Reference
		[AllowNull] private Dueoksini_MiniGameController _minigameController;

		private View_TeamScore? _teamScore;
		private View_MissionHint? _missionHintView;

		public override void Initialize(SceneControllerBase sceneController)
		{
			base.Initialize(sceneController);
			_minigameController = sceneController as Dueoksini_MiniGameController;
		}

		public override void ShowMiniGameViews()
		{
			base.ShowMiniGameViews();
			_teamScore = Push<View_TeamScore>();
			_teamScore.Initialize(_minigameController);
			_missionHintView = Push<View_MissionHint>();
			_missionHintView.SetHintContent(_title, _hint);
		}

		public override void HideMiniGameViews()
		{
			if (_teamScore != null) PopByObject(_teamScore.gameObject);
			base.HideMiniGameViews();
		}

		public void SetTeamScore(Faction faction, int score)
		{
			_teamScore?.SetTeamScore(faction, score);
		}
	}
}

#nullable disable