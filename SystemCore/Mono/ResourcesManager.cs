using System;
using System.Collections.Generic;
using CT.Logger;
using CTC.GUI;
using CTC.Networks.Synchronizations;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CTC.SystemCore
{
	[Serializable]
	public struct NetworkObjectKeyValue
	{
		public NetworkObjectType Type;
		public GameObject NetworkObject;

		public NetworkObjectKeyValue(NetworkObjectType type, GameObject networkObject)
		{
			Type = type;
			NetworkObject = networkObject;
		}
	}

	public class ResourcesManager : MonoBehaviour, IManager, IInitializable
	{
		[field: SerializeField] public AssetLabelReference GuiLabel { get; private set; }
		[field: SerializeField] public AssetLabelReference MapLabel { get; private set; }
		[field: SerializeField] public AssetLabelReference MiniGameRuleVideoLabel { get; private set; }
		[field: SerializeField] public AssetLabelReference ExecutionSceneLabel { get; private set; }

		public MapResources MapResources { get; private set; }
		public VideoResources VideoResources { get; private set; }
		// View
		private Dictionary<Type, GameObject> _viewPrefabTable = new();

		// Network Object
		[SerializeField]
		private List<NetworkObjectKeyValue> _networkObjectList = new();
		private Dictionary<NetworkObjectType, GameObject> _networkObjectByType = new();

		// Operations
		private List<AsyncOperationHandle> _operationHandle = new();
		private object _operationLock = new();

		public void Awake()
		{
			foreach (var kv in _networkObjectList)
			{
				_networkObjectByType.Add(kv.Type, kv.NetworkObject);
			}

			MapResources = new MapResources(this);
			VideoResources = new VideoResources(this);
		}

#if UNITY_EDITOR
		[Button]
		public void RefreshResourcesBinding()
		{
			_networkObjectList.Clear();
			string prefabPath = "Prefabs/Gameplay/NetworkObjects";
			var netObjects = AssetLoader.GetAssetsFromPath<GameObject>(prefabPath);
			foreach (var obj in netObjects)
			{
				var netObj = obj.GetComponent<RemoteNetworkObject>();
				if (netObj == null)
					continue;
				_networkObjectList.Add(new NetworkObjectKeyValue()
				{
					Type = netObj.Type,
					NetworkObject = obj
				});
			}
		}
#endif

		public bool IsInitialized()
		{
			lock (_operationLock)
			{
				bool preResult = _operationHandle.Count == 0;
				for (int i = _operationHandle.Count - 1; i >= 0; i--)
				{
					if (_operationHandle[i].IsDone)
					{
						_operationHandle.RemoveAt(i);
					}
				}
				return preResult;
			}
		}

		public void Initialize()
		{
			loadGuiPrefabs();
			MapResources.Initialize();
			VideoResources.Initialize();
		}

		public void Release()
		{
			MapResources.Release();
			VideoResources.Release();
		}

		private void loadGuiPrefabs()
		{
			var locationLoadOperation = Addressables.LoadResourceLocationsAsync(GuiLabel);
			RegisterOperationHandle(locationLoadOperation);

			locationLoadOperation.Completed += (locations) =>
			{
				foreach (var location in locations.Result)
				{
					this.LogInfo($"Try load view in {location.InternalId}");
					var operation = Addressables.LoadAssetAsync<GameObject>(location);
					RegisterOperationHandle(operation);

					operation.Completed += (view) =>
					{
						var guiGo = view.Result;
						var guiView = guiGo.GetComponent<ViewBase>();

						if (guiView == null)
						{
							this.LogError($"There is no View script component in \"{guiGo.name}\"!");
							return;
						}

						if (!_viewPrefabTable.TryAdd(guiView.GetType(), guiGo))
						{
							this.LogError($"There is duplicated View perfab! Name : {guiGo.name}");
							return;
						}

						this.LogInfo($"{guiGo.name} loaded!");
					};
				}
			};
		}

		public bool TryGetViewInstance<T>(Transform targetTransform, out GameObject guiInstance)
			where T : ViewBase
		{
			if (!_viewPrefabTable.TryGetValue(typeof(T), out var guiPrefab))
			{
				this.LogError($"There is no such GUI prefab in the table! GUI type : {typeof(T).Name}");
				guiInstance = null;
				return false;
			}

			guiInstance = GameObject.Instantiate(guiPrefab, targetTransform);
			var view = guiInstance.GetComponent<ViewBaseWithContext>();
			if (view != null)
			{
				view.BindContext();
			}

			return true;
		}

		public bool TryGetNetworkObjectPrefab(NetworkObjectType type, out GameObject networkObject)
		{
			return _networkObjectByType.TryGetValue(type, out networkObject);
		}

		/// <summary>비동기 작업이 끝났는지 확인하기 위해 비동기 Operation handle을 등록합니다.</summary>
		/// <param name="handle">등록할 비동기 Operation handle</param>
		public void RegisterOperationHandle(AsyncOperationHandle handle)
		{
			lock (_operationLock)
			{
				_operationHandle.Add(handle);
			}
		}
	}
}
