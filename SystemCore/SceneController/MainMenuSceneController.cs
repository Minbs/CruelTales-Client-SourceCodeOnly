using System;
using CT.Logger;
using CTC.MainMenu;
using CTC.Networks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CTC.SystemCore
{
	public class MainMenuSceneController : SceneController
	{
		[field: SerializeField]
		public Navigation_MainMenu Navigation { get; private set; }

		public override async UniTask Startup(Action callback)
		{
			await UniTask.Yield();
			this.LogInfo($"Startup {AsyncSceneManager.GetCurrentScene()}");

			if (GlobalService.BackendManager.IsValid == false)
			{
				GlobalService.ErrorManager.OnErrorOccur(ClientError.PlatformDRM_Steam);
				GlobalService.SceneManager.SystemHandle_LoadSceneAsync(SceneType.scn_error);
				return;
			}

			Navigation.OnSceneLoaded();
			callback?.Invoke();
		}

		public override async UniTask Release(Action callback)
		{
			await UniTask.Yield();
			this.LogInfo($"Release {AsyncSceneManager.GetCurrentScene()}");

			callback?.Invoke();
		}

		public void OnMatchmakingStart()
		{
			GlobalService.StaticGUI
				.NavAsyncNetOperation
				.OnAsyncOperationStarted(AsyncOperationType.TryMatchMaking);
		}

		public void OnMatchmakingEnd()
		{
			GlobalService.StaticGUI
				.NavAsyncNetOperation
				.OnAsyncOperationCompleted(AsyncOperationType.TryMatchMaking);
		}
	}
}