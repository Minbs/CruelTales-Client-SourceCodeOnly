using System;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.Gameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CTC.SystemCore
{
	public class GameplaySceneController : SceneController
	{
		[field: SerializeField]
		public GameplayManager GameplayManager { get; private set; }

		public override async UniTask Startup(Action callback)
		{
			this.LogInfo($"Startup {AsyncSceneManager.GetCurrentScene()}");

			// Initialize
			GameplayManager.Initialize();

			// Invoke callback
			callback?.Invoke();
			await UniTask.Yield();
		}

		public override async UniTask Release(Action callback)
		{
			this.LogInfo($"Release {AsyncSceneManager.GetCurrentScene()}");

			// Unload resources
			UnloadGameMap();

			// Invoke callback
			callback?.Invoke();
			await UniTask.Yield();
		}

		public void LoadGameMap(GameSceneIdentity gameID, Action<GameObject> callback)
		{
			GlobalService.ResourcesManager.MapResources
				.SystemHandle_LoadMiniGameAsync(gameID, callback);
		}

		public void UnloadGameMap()
		{
			GlobalService.ResourcesManager.MapResources.UnloadGameMap();
		}
	}
}