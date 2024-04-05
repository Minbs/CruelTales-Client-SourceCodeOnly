using System;
using System.Collections.Generic;
using CT.Logger;
using CTC.Gameplay;
using CTC.Networks;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.Tests;
using KaNet.Physics;

namespace CTC.SystemCore
{
	/// <summary>
	/// This is a static class that manages the services and managers used in the game client.
	/// </summary>
	public static class GlobalService
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GlobalService));

		// Scene Controller
		public static SceneController CurrentScene { get; private set; }
		public static GameplaySceneController GameplayScene { get; private set; }

		// Gameplay Reference
		public static GameplayManager GameplayManager { get; private set; }
		public static GameplayController GameplayController { get; private set; }

		// Manager
		public static UserDataManager UserDataManager { get; private set; }
		public static ConfigManager ConfigManager { get; private set; }
		public static ErrorManager ErrorManager { get; private set; }
		public static OptionManager OptionManager { get; private set; }

		// Network
		public static BackendManager BackendManager { get; private set; }

		// Mono Behaviour
		public static NetworkManager NetworkManager { get; private set; }
		public static ResourcesManager ResourcesManager { get; private set; }
		public static SoundManager SoundManager { get; private set; }
		public static StaticGUI StaticGUI { get; private set; }
		public static AsyncSceneManager SceneManager { get; private set; }
		public static InputManager InputManager { get; private set; }
		public static ClientQueueManager ClientQueueManager { get; private set; }
		public static DokzaSpineDataManager DokzaSpineDataManager { get; private set; }
		public static EffectManager EffectManager { get; private set; }
		public static Dokza_BoneFollowerManager DokzaBoneFollowerManager { get; private set; }


		// Dispose list
		private static List<IManager> _managerList = new();
		private static List<IInitializable> _initializeList = new();
		private static List<IFinalizable> _finalizableList = new();

		// Mono Handler
		public static SystemEventManager SystemEventManager { get; private set; }

		/// <summary>Initialize all services and managers.</summary>
		public static void Initialize(ProcessHandler process)
		{
			// Manager
			UserDataManager = new();	_managerList.Add(UserDataManager);
			ConfigManager = new();		_managerList.Add(ConfigManager);
			ErrorManager = new();		_managerList.Add(ErrorManager);
			OptionManager = new();		_managerList.Add(OptionManager);

			// Network
			BackendManager = new BackendManager();	_managerList.Add(BackendManager);

			// Mono Behaviour
			NetworkManager = process.GetComponentInChildren<NetworkManager>();
			_managerList.Add(NetworkManager);
			ResourcesManager = process.GetComponentInChildren<ResourcesManager>();
			_managerList.Add(ResourcesManager);
			SoundManager = process.GetComponentInChildren<SoundManager>();
			_managerList.Add(SoundManager);
			StaticGUI = process.GetComponentInChildren<StaticGUI>();
			_managerList.Add(StaticGUI);
			SceneManager = process.GetComponentInChildren<AsyncSceneManager>();
			_managerList.Add(SceneManager);
			InputManager = process.GetComponentInChildren<InputManager>();
			_managerList.Add(InputManager);
			ClientQueueManager = process.GetComponentInChildren<ClientQueueManager>();
			_managerList.Add(ClientQueueManager);
			DokzaSpineDataManager = process.GetComponentInChildren<DokzaSpineDataManager>();
			_managerList.Add(DokzaSpineDataManager);
			EffectManager = process.GetComponentInChildren<EffectManager>();
			_managerList.Add(EffectManager);
			DokzaBoneFollowerManager = process.GetComponentInChildren<Dokza_BoneFollowerManager>();
			_managerList.Add(DokzaBoneFollowerManager);
			
			// System Event Handler
			SystemEventManager = process.GetComponentInChildren<SystemEventManager>();
			_managerList.Add(SystemEventManager);

			// Set initialize list and finialize list
			foreach (var manager in _managerList)
			{
				manager.Initialize();

				if (manager is IInitializable)
				{
					_initializeList.Add((IInitializable)manager);
				}

				if (manager is IFinalizable)
				{
					_finalizableList.Add((IFinalizable)manager);
				}
			}
		}

		public static void SetCurrentScene(SceneController currentScene)
		{
			CurrentScene = currentScene;
			if (CurrentScene is GameplaySceneController)
			{
				GameplayScene = (GameplaySceneController)CurrentScene;
			}
		}

		/// <summary>Release all services and managers.</summary>
		public static void Release()
		{
			for (int i = _managerList.Count - 1; i >= 0; i--)
			{
				_managerList[i].Release();
			}
		}

		/// <summary>Return whether all services and managers have been initialized.</summary>
		public static bool IsInitialized()
		{
			return _initializeList.Find((i) => i.IsInitialized() == false) == null;
		}

		/// <summary>Return whether all services and managers have been finalized.</summary>
		public static bool IsFinalized()
		{
			return _finalizableList.Find((i) => i.IsFinalized() == false) == null;
		}

		public static void OnGameplayManagerCreated(GameplayManager gameplayManager)
		{
			GameplayManager = gameplayManager;
		}

		public static void OnGameplayControllerCreated(GameplayController gameplayController)
		{
			GameplayController = gameplayController;
		}

		// Physics
		public static KaPhysicsWorld PhysicsWorld { get; private set; }

		public static void BindPhysicsWorld(KaPhysicsWorld physicsWorld)
		{
			if (ReferenceEquals(PhysicsWorld, physicsWorld))
				return;

			PhysicsWorld = physicsWorld;
		}

		public static void ReleasePhysicsWorld(KaPhysicsWorld physicsWorld)
		{
			if (!ReferenceEquals(PhysicsWorld, physicsWorld))
				return;

			PhysicsWorld = null;
		}
	}
}
