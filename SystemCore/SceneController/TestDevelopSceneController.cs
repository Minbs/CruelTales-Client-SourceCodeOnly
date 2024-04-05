using System;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.Gameplay;
using CTC.GUI.Gameplay;
using CTC.Networks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CTC.SystemCore
{
	public class TestDevelopSceneController : SceneController
	{
		// Log
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameplayManager));

		[field: SerializeField]
		public Navigation_TestDevelop Navigation { get; private set; }

		// Map load
		private AsyncOperationHandle _mapLoadHandle;
		public float Progress => _mapLoadHandle.PercentComplete;

		public override async UniTask Startup(Action callback)
		{
			await UniTask.Yield();

			this.LogInfo($"Startup {AsyncSceneManager.GetCurrentScene()}");

			Initialize(GameMapType.RedHood_0);

			callback?.Invoke();
		}

		public override async UniTask Release(Action callback)
		{
			await UniTask.Yield();

			this.LogInfo($"Release {AsyncSceneManager.GetCurrentScene()}");

			callback?.Invoke();
		}

		public void Initialize(GameMapType startMap)
		{
			GameSceneIdentity gameID = new GameSceneIdentity()
			{
				Map = GameMapType.RedHood_0,
				Mode = GameModeType.RedHood,
				Theme = GameMapThemeType.EastAsia_Korea,
			};

			// Load initial map
			ChangeMap(gameID, (go) =>
			{
				GlobalService.StaticGUI.NavAsyncNetOperation
					.OnAsyncOperationCompleted(AsyncOperationType.LoadingScene);

				_log.Info($"Map load completed");
			});
		}

		public void ChangeMap(GameSceneIdentity gameID, Action<GameObject> callback)
		{
			GlobalService.ResourcesManager.MapResources
				.SystemHandle_LoadMiniGameAsync(gameID, callback);
		}
	}
}