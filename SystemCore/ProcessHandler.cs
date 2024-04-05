using System;
using System.Collections.Generic;
using CT.Logger;
using CT.Logger.Processor;
using CTC.Data;
using CTC.Utils;
using Cysharp.Threading.Tasks;
using KaNet.Physics;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CTC.SystemCore
{
	public class ProcessHandler : PreloadedSingleton<ProcessHandler>
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessHandler));

		[SerializeField, LabelText("에디터 시작 옵션")]
		private ProcessStartOptionData _startOption;

		[SerializeField]
		private TMP_Text _logText;

		// Process options
		public bool IsDebugMode { get; set; } = false;
		public SceneType InitialSceneType { get; private set; }

		// Data
		public ProgramID ID { get; private set; }
		public NetworkPlatform RunningPlatform { get; private set; } = NetworkPlatform.Standalone;

		// Events
		public event Action OnStopProcess;

		// Members
		private static bool _isQuitted = false;

		public override void OnAwake()
		{
			// Set initial logger
			EventLogProcessor logProcessor = new((log) =>
			{
				_logText.text += log; 
				_logText.text += "<br>";
			});
			LogManager.BindLogger(logProcessor);

			// Initialize
			LayerMaskHelper.Initialize();
			Application.targetFrameRate = 60;

			_log.Info($"ProcessDataPath : {Global.Path.ProcessDataPath}");
			_log.Info($"UserDataPath : {Global.Path.UserDataPath}");

			IsDebugMode = _startOption.IsDebugMode;
			InitialSceneType = _startOption.InitialSceneType;

			_log.Info("Start initialize process!");

			// Setup process identification
			this.ID = new ProgramID
			(
				Application.productName,
				Application.version,
				_startOption.AppID
			);

			// Start process
			string[] processCommandArguments = null;

			try
			{
#if UNITY_EDITOR
				processCommandArguments = _startOption.EditorProcessArguments.ToArray();
#else
				processCommandArguments = System.Environment.GetCommandLineArgs();
#endif
			}
			catch (Exception e)
			{
				_log.Error("Environmnet command getting error!");
				_log.Error($"{e.Message}\n");
				return;
			}

			_log.Info("Command line initialized.");
			_log.Info("Logging initialized...");

			try
			{
				Addressables.InitializeAsync().Completed += (e) =>
				{
					var t = startProcess(processCommandArguments);
				};
			}
			catch (Exception e)
			{
				_log.Info(e.Message);
			}
		}

		/// <summary>프로세스를 시작합니다.</summary>
		/// <param name="args">프로세스 실행 인자</param>
		private async UniTask startProcess(string[] args)
		{
			_log.Info("Start process...");

			onArguments(args);
			onStartProcess();

			// Wait for initialized
			await UniTask.WaitUntil(() => this.isInitialized());

			// Change nextscene
			if (AsyncSceneManager.GetCurrentScene() == SceneType.scn_game_initial)
			{
				_log.Info($"Start initial scene success. Change scene to \"{InitialSceneType}\"");

				LogManager.BindLogger(new UnityDebugLogProcessor());

				var handle = GlobalService.SceneManager
					.SystemHandle_LoadSceneAsync(InitialSceneType);
			}
			else
			{
				_log.Warn($"You're not start from the \"{SceneType.scn_game_initial}\"." +
						  $"If you want to start the game properly, you must start from the \"{SceneType.scn_game_initial}\" scene.");
			}
		}

		public void StopProcess()
		{
			var t = stopProcess();
		}

		/// <summary>프로세스를 중단합니다.</summary>
		public async UniTask stopProcess()
		{
			if (_isQuitted)
				return;
			_isQuitted = true;

			try
			{
				onStopProcess();
				await UniTask.WaitUntil(() => this.isFinalized());
			}
			catch (Exception e)
			{
				_log.Error($"[EXTRIME SERIOUS ERROR OCCUR] Process stop operation fail!\n{e}");
			}
			finally
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
			}
		}

		private void OnApplicationQuit()
		{
			if (_isQuitted)
				return;

			_isQuitted = true;

			try
			{
				onStopProcess();
			}
			catch (Exception e)
			{
				_log.Error($"[EXTRIME SERIOUS ERROR OCCUR] Process stop operation fail!\n{e}");
			}
		}

		/// <summary>프로세스의 초기화 단계에서 호출됩니다.</summary>
		private void onStartProcess()
		{
			GlobalService.Initialize(this);
		}

		/// <summary>프로세스가 해제될 때 호출됩니다.</summary>
		private void onStopProcess()
		{
			_log.Info("Try to stop process...");

			OnStopProcess?.Invoke();
			GlobalService.Release();

			_log.Info("Process stop successful!");
		}

		private bool isInitialized()
		{
			return GlobalService.IsInitialized();
		}

		private bool isFinalized()
		{
			return GlobalService.IsFinalized();
		}

		private void onArguments(string[] args)
		{
			List<string> argumentsList = new List<string>(args);
			if (argumentsList.Contains("-SomeKindOfOption")) { } // Do something
			_log.Info("Command arguments check finished...");
		}
	}
}
