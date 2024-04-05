#nullable enable

using System.Diagnostics.CodeAnalysis;
using CTC.GUI.Gameplay.Common;
using CTC.Networks.SyncObjects.SyncObjects;

namespace CTC.GUI.MiniGames
{
	public class RedHood_Navigation : MiniGame_Navigation
	{
		private static readonly string _title = @"빨간모자의 책";
		private static readonly string _redHoodHint = @"늑대를 피해 심부름을 완수하자!";
		private static readonly string _wolfHint = @"빨간 모자를 잡아 늑대 역할을 넘겨주자!";

		// Reference
		[AllowNull] private RedHood_MiniGameController _minigameController;

		private View_MissionHint? _missionHintView;

		public override void Initialize(SceneControllerBase sceneController)
		{
			base.Initialize(sceneController);
			_minigameController = sceneController as RedHood_MiniGameController;
		}

		public override void OnPlayerCharacterCreated(PlayerCharacter playerCharacter)
		{
			base.OnPlayerCharacterCreated(playerCharacter);
			setHintBy(playerCharacter);
		}

		private void setHintBy(PlayerCharacter playerCharacter)
		{
			if (playerCharacter.IsLocal)
			{
				if (_missionHintView != null)
				{
					PopByObject(_missionHintView.gameObject);
					_missionHintView = null;
				}

				if (playerCharacter is WolfCharacter)
				{
					_missionHintView = Push<View_MissionHint>();
					_missionHintView.SetHintContent(_title, _wolfHint);
				}
				else if (playerCharacter is RedHoodCharacter)
				{
					_missionHintView = Push<View_MissionHint>();
					_missionHintView.SetHintContent(_title, _redHoodHint);
				}
			}
		}

		public override void ShowMiniGameViews()
		{
			base.ShowMiniGameViews();
			if (_minigameController.LocalPlayer != null)
			{
				setHintBy(_minigameController.LocalPlayer);
			}
		}
	}
}

#nullable disable