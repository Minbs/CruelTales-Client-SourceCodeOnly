using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CTC.SystemCore
{
	//[Serializable]
	//public struct MapReferenceByGameMode
	//{
	//	public GameMapType GameMode;
	//	public AssetReferenceGameObject MapPrefab;
	//}

	[Serializable]
	public class MapResources : IManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(MapResources));

		// Reference
		ResourcesManager _resourcesManager;

		[SerializeField]
		//private List<MapReferenceByGameMode> _gameMapRefList = new();
		private Dictionary<GameMapType, AssetReferenceGameObject> _miniGameRefByMapType = new();

		private GameObject _miniGameObject;
		public GameSceneIdentity? CurrentMiniGameID { get; private set; } = null;

		public MapResources(ResourcesManager resourcesManager)
		{
			_resourcesManager = resourcesManager;
		}

		public void Initialize()
		{
			loadMiniGameReferences();
		}

		public void Release() {}

		private void loadMiniGameReferences()
		{
			var loadOperation = Addressables.LoadResourceLocationsAsync(_resourcesManager.MapLabel);
			_resourcesManager.RegisterOperationHandle(loadOperation);

			loadOperation.Completed += (operation) =>
			{
				foreach (IResourceLocation miniGameRef in operation.Result)
				{
					string miniGamePath = miniGameRef.PrimaryKey;
					string miniGameName = System.IO.Path.GetFileNameWithoutExtension(miniGamePath).Replace("MiniGame_", string.Empty);

					if (!Enum.TryParse<GameMapType>(miniGameName, true, out var mapType))
					{
						_log.Fatal($"There is no such map {miniGamePath}");
						throw new Exception($"There is no such map {miniGamePath}");
					}

					AssetReferenceGameObject assetRef = new(miniGamePath);
					if (!_miniGameRefByMapType.TryAdd(mapType, assetRef))
					{
						//_log.Fatal($"There are same map exist! {mapName}");
						//throw new Exception($"There is no such map {mapName}");
					}
				}
			};
		}

		public void SystemHandle_LoadMiniGameAsync(GameSceneIdentity gameID, Action<GameObject> onLoaded)
		{
			if (!_miniGameRefByMapType.TryGetValue(gameID.Map, out var miniGameRef))
			{
				_log.Fatal($"There is no {gameID}");
				return;
			}

			var handle = GlobalService.SystemEventManager.CreateHandle();
			handle.Push(() =>
			{
				UnloadGameMap();

				Addressables.InstantiateAsync(miniGameRef, Vector3.zero, Quaternion.identity)
					.Completed += (asyncHandle) =>
				{
					_miniGameObject = asyncHandle.Result;
					CurrentMiniGameID = gameID;
					onLoaded?.Invoke(_miniGameObject);
					handle.OnCompleted();
				};
			});
		}

		public void UnloadGameMap()
		{
			if (_miniGameObject != null)
			{
				Addressables.ReleaseInstance(_miniGameObject);
				_miniGameObject = null;
				CurrentMiniGameID = null;
			}
		}
	}
}
