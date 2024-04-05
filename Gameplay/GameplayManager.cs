using CT.Logger;
using CTC.GUI.Gameplay;
using CTC.Networks;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.SystemCore;
using CTC.Tests;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CTC.Gameplay
{
	public class GameplayManager : MonoBehaviour
	{
		// Log
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameplayManager));

		// Reference
		public GameplaySceneController GameplaySceneController { get; private set; }
		public RemoteWorldManager WorldManager { get; private set; }
		public GameplayController GameplayController { get; private set; }

		[field :SerializeField]
		public CameraManager WorldCamera { get; private set; }

		[field :SerializeField]
		public EffectManager EffectManager { get; private set; }

		[field: SerializeField]
		public GameplayResourcesManager GameplayResourcesManager { get; private set; }

		// Map load
		private AsyncOperationHandle _mapLoadHandle;
		public float Progress => _mapLoadHandle.PercentComplete;

		public void Initialize()
		{
			GlobalService.OnGameplayManagerCreated(this);

			// Setup reference
			WorldManager = GetComponentInChildren<RemoteWorldManager>();
			if (WorldManager == null)
			{
				_log.Fatal($"There is no {nameof(WorldManager)} in the scene!");
			}

			GameplaySceneController = GlobalService.GameplayScene;
			if (GameplaySceneController == null)
			{
				_log.Fatal($"There is no {nameof(GameplaySceneController)} in the scene!");
			}

			GameplayResourcesManager.Initialize();

			GlobalService.StaticGUI.NavAsyncNetOperation
				.OnAsyncOperationCompleted(AsyncOperationType.LoadingScene);
		}

		public void OnGameplayControllerCreated(GameplayController gameplayController)
		{
			GameplayController = gameplayController;
			GlobalService.OnGameplayControllerCreated(gameplayController);
		}
	}
}
