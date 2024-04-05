#nullable enable
#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.Gameplay;
using CTC.Networks.Synchronizations;
using CTC.SystemCore;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class GameplayController : RemoteNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameplayController));
		private NetworkManager? _networkManager;

		public RoomSessionManager RoomSessionManager => _roomSessionManager;
		public PlayerState? ClientPlayerState => RoomSessionManager.ClientPlayerState;
		public SceneControllerBase? SceneController { get; private set; }
		public MiniGameControllerBase? MiniGameController { get; private set; }

		public bool IsHost => (ClientPlayerState == null) ? false : ClientPlayerState.IsHost;
		public bool IsReady => (ClientPlayerState == null) ? false : ClientPlayerState.IsReady;

		private readonly HashSet<Interactor> _interactorSet = new();

		protected override void Awake()
		{
			base.Awake();
			var gameplayManager = FindObjectOfType<GameplayManager>();
			gameplayManager.OnGameplayControllerCreated(this);
		}

		public override void OnCreated()
		{
			OnGameSynchronized();
			EffectController.Initialize(this);
		}

		public override void OnDestroyed()
		{
			OnGameClosed();
		}

		public void OnGameSynchronized()
		{
			_networkManager = GlobalService.NetworkManager;
			_networkManager.OnSynchronized(ServerRuntimeOption);
			SkinSet skinSet = GlobalService.UserDataManager.UserData.SkinSet.SelectedSkinSet;
			JoinRequestToken requestToken = new JoinRequestToken()
			{
				ClientSkinSet = skinSet,
			};

			this.Client_ReadyToSync(requestToken);
		}

		public void OnGameClosed()
		{
			GameplaySceneController?.UnloadGameMap();
		}

		public void OnSceneControllerLoaded(SceneControllerBase sceneController)
		{
			SceneController = sceneController;
			MiniGameController = SceneController as MiniGameControllerBase;
		}

		public void OnInteractorCreated(Interactor interactor)
		{
			_interactorSet.Add(interactor);
		}

		public void OnInteractorDestroyed(Interactor interactor)
		{
			_interactorSet.Remove(interactor);
		}

		private HashSet<Interactor> _interactorSetResult = new();
		public bool TryGetCollidedInteractorList(PlayerCharacter player,
												 out HashSet<Interactor> interactors)
		{
			_interactorSetResult.Clear();
			foreach (Interactor i in _interactorSet)
			{
				if (i.IsCollideWith(player))
				{
					_interactorSetResult.Add(i);
				}
			}

			interactors = _interactorSetResult;
			return _interactorSetResult.Count > 0;
		}
	}
}

#pragma warning restore CS0649