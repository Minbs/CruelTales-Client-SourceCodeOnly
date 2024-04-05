using System;
using CT.Logger;
using CTC.Networks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CTC.SystemCore
{
	/// <summary>비동기로 Scene을 전환하는 서비스입니다.</summary>
	public class AsyncSceneManager : MonoBehaviour, IManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(AsyncSceneManager));

		public bool IsInitialized => true;

		private SceneType _defaultLoaderSceneType = SceneType.scn_game_loader;
		private string _loaderSceneName => _defaultLoaderSceneType.GetName();

		public SystemEventHandle SystemHandle_LoadSceneAsync
			(SceneType sceneType,
			LoadSceneMode loadSceneMode = LoadSceneMode.Single,
			Action onSceneLoaded = null)
		{
			var handle = GlobalService.SystemEventManager.CreateHandle();

			// Callback
			onSceneLoaded += () =>
			{
				handle.OnCompleted();
				GlobalService.StaticGUI.NavAsyncNetOperation
					.OnAsyncOperationCompleted(AsyncOperationType.LoadingScene);
			};

			// System event operation
			handle.Push(() =>
			{
				GlobalService.StaticGUI.NavAsyncNetOperation
					.OnAsyncOperationStarted(AsyncOperationType.LoadingScene);
				this.loadSceneAsync(sceneType, loadSceneMode, onSceneLoaded);
			});

			return handle;
		}

		private void loadSceneAsync(SceneType sceneType,
									LoadSceneMode loadSceneMode = LoadSceneMode.Single,
									Action onSceneLoaded = null)
		{
			this.LogInfo($"Start load scene : {sceneType.GetName()}");

			var currentScene = GlobalService.CurrentScene;
			if (currentScene != null)
			{
				_ = currentScene.Release(loadSceneOperation);
			}
			else
			{
				loadSceneOperation();
			}

			void loadSceneOperation()
			{
				GlobalService.StaticGUI.CloseAllTemporaryViews();

				SceneManager.LoadSceneAsync(_loaderSceneName, loadSceneMode).completed += (a) =>
				{
					SceneManager.LoadSceneAsync(sceneType.GetName(), loadSceneMode).completed += (a) =>
					{
						this.LogInfo($"Scene load completed : {sceneType.GetName()}");

						var sceneController = FindObjectOfType<SceneController>();
						if (sceneController == null)
						{
							_log.Fatal($"There is no scene on {GetCurrentScene()}");
							return;
						}

						this.LogInfo($"Try to startup scene controller : {sceneController.GetType().Name}");
						GlobalService.SetCurrentScene(sceneController);
						sceneController.Startup(onSceneLoaded);
					};
				};
			}
		}

		public static SceneType GetCurrentScene()
		{
			var curSceneName = SceneManager.GetActiveScene().name;
			return Enum.TryParse(typeof(SceneType), curSceneName, out var curScene)
				? (SceneType)curScene : SceneType.None;
		}

		public void Initialize() { }

		public void Release() { }
	}

	public static class SceneTypeExtension
	{
		/// <summary>Scene 이름을 반환받습니다.</summary>
		/// <param name="sceneType">Scene 타입</param>
		/// <returns>Scene 문자열</returns>
		public static string GetName(this SceneType sceneType)
		{
			// Enum 타입에 대해서 ToString을 GetSceneName으로 캡슐화 한 이유는
			// 추후 특수한 이유로 Type에 대해서 별도의 Scene 이름 관리가 필요한 경우를 대비하기 위함입니다.
			return sceneType.ToString();
		}

		public static bool IsLoadableScene(this SceneType sceneType)
		{
			return !(sceneType == SceneType.None || sceneType == SceneType.scn_game_loader);
		}
	}
}