#nullable enable
#pragma warning disable CS0649

using System.Diagnostics.CodeAnalysis;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.Gameplay;
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class SceneControllerBase : RemoteNetworkObject
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(SceneControllerBase));

		[AllowNull] public GameSceneMapController GameSceneMapController { get; private set; }
		[AllowNull] public GameSceneMapData MapData { get; private set; }

		public PlayerCharacter? LocalPlayer;

		public virtual partial void Server_TryLoadSceneAll(GameSceneIdentity gameScene)
		{
			Server_TryLoadScene(gameScene);
		}

		public virtual partial void Server_TryLoadScene(GameSceneIdentity gameScene)
		{
			_log.Info($"Server load game : {gameScene}");
			GameplaySceneController.LoadGameMap(gameScene, callback: (gameMapObject) =>
			{
				GameSceneMapController = gameMapObject.GetComponent<GameSceneMapController>();
				MapData = GameSceneMapController.GetGameSceneMapData();
				MapData.Initialize();
				WorldManager.SetGameMapData(MapData);

				OnSceneLoaded();
				Client_OnSceneLoaded();
			});
		}

		public virtual void OnSceneLoaded()
		{

		}

		public virtual void OnPlayerCharacterCreated(PlayerCharacter playerCharacter)
		{
			LocalPlayer = playerCharacter;
		}

		public virtual void OnPlayerCharacterDestroyed(PlayerCharacter playerCharecter)
		{
			if (LocalPlayer == playerCharecter)
			{
				LocalPlayer = null;
			}
		}
	}
}

#pragma warning restore CS0649
