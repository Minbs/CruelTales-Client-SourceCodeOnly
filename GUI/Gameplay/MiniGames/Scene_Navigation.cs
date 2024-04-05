#nullable enable

using CTC.GUI.Gameplay.Common;
using CTC.Networks.SyncObjects.SyncObjects;
using System.Diagnostics.CodeAnalysis;

namespace CTC.GUI.MiniGames
{
	public class Scene_Navigation : ViewNavigation
	{
		// Reference
		[AllowNull] public SceneControllerBase SceneController { get; private set; }
		[AllowNull] public GameplayController GameplayController { get; private set; }
		[AllowNull] public RoomSessionManager RoomSessionManager { get; private set; }
		[AllowNull] public PlayerState ClientPlayerState { get; private set; }

		public virtual void Initialize(SceneControllerBase sceneController)
		{
			SceneController = sceneController;

			// Reference
			GameplayController = SceneController.GameplayController;
			RoomSessionManager = GameplayController.RoomSessionManager;
			ClientPlayerState = RoomSessionManager.ClientPlayerState;
		}
	}
}

#nullable disable