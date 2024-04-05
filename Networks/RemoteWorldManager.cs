using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Logger;
using CT.Networks;
using CT.Packets;
using CTC.Gameplay;
using CTC.Networks.Synchronizations;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.SystemCore;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using UnityEngine;

namespace CTC.Networks
{
	public enum SyncResult
	{
		None = 0,
		Success,
		Ignore,
		Error,
	}

	[Serializable]
	public struct TestNetworkObjectKeyValue
	{
		public NetworkObjectType Type;
		public GameObject NetworkObject;
	}

	public class RemoteWorldManager : MonoBehaviour
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(RemoteWorldManager));

		// Formats
		private static readonly string SPAWN_ERROR_FORMAT = "OnMasterSpawn error! Deserialize error on : {0}";
		private static readonly string SPAWN_IGNORED_FORMAT = "Object spawn ignored";
		private static readonly string ENTER_ERROR_FORMAT = "OnMasterEnter error! Deserialize error on : {0}";
		private static readonly string ENTER_IGNORED_FORMAT = "Object enter ignored";

		private static readonly string DESPAWN_ERROR_FORMAT = "OnMasterDespawn error! there is no such id {0}";
		private static readonly string LEAVE_ERROR_FORMAT = "OnMasterLeave error! there is no such id {0}";

		// References
		[field: SerializeField]
		public GameplayManager GameplayManager { get; private set; }

		// Manager
		private NetworkManager _networkManager;
		private ResourcesManager _resourcesManager;

		// Physics
		private KaPhysicsWorld _physicsWorld;
		private float _deltaAccumulator = 0;

		// Network
		private float _serializeTimer = 0;

		// Object management
		private Dictionary<NetworkIdentity, RemoteNetworkObject> _networkObjectById = new();
		private Dictionary<NetworkObjectType, HashSet<RemoteNetworkObject>> _netObjSetByType = new();
		private Queue<(RemoteNetworkObject NetObj, bool IsSpawn)> _createdObjectQueue = new();

		// State
		private bool _isRunning = true;

		// Visibility
		/// <summary>
		/// 보간 가능한 거리 한계입니다.
		/// 한계를 넘어가는 경우 텔레포트합니다.
		/// </summary>
		public const float INTERPOLATE_DISTANCE_LIMIT = 3.0f * 3.0f;

		public void Awake()
		{
			_networkManager = GlobalService.NetworkManager;
			_resourcesManager = GlobalService.ResourcesManager;
			_physicsWorld = new KaPhysicsWorld();
		}

		public void Update()
		{
			float deltaTime = Time.deltaTime;

			updateRemoteNetworkObjects(deltaTime);
			fixedUpdate(deltaTime);
			sendSynchronization();
			updateObjectLifeCycle();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void updateRemoteNetworkObjects(float deltaTime)
			{
				foreach (var netObj in _networkObjectById.Values)
				{
					if (!netObj.IsAlive)
						continue;

					netObj.UpdateByManager(deltaTime);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void fixedUpdate(float deltaTime)
			{
				float stepTime = GlobalService
					.NetworkManager
					.ServerRuntimeOption
					.PhysicsStepTime;

				// 물리 Tick이 Rendering Frame 만큼, 혹은 그 이상 주기가 짧다면 상수로 보간한다.
				// 물리 Tick이 Rendering Frame 보다 주기가 길다면 render delta를 구해서 보간한다.
				bool isConstInterpolate = 
					(1.0f / stepTime) > (Application.targetFrameRate * 0.8f);

				var netObjs = _networkObjectById.Values;

				_deltaAccumulator += deltaTime;
				if (_deltaAccumulator > stepTime * 5)
					_deltaAccumulator = stepTime * 5;

				bool hasPhysicsCalculated = false;
				while (_deltaAccumulator >= stepTime)
				{
					hasPhysicsCalculated = true;
					_deltaAccumulator -= stepTime;
					_physicsWorld.Step(stepTime);

					foreach (var netObj in netObjs)
					{
						if (!netObj.IsAlive)
							continue;

						netObj.OnFixedUpdate(stepTime);
					}
				}

				foreach (var netObj in netObjs)
				{
					if (!netObj.IsAlive)
						continue;

					if (hasPhysicsCalculated)
					{
						netObj.PreviousPosition = netObj.CurrentPosition;
						netObj.CurrentPosition = netObj.RigidBody.Position.ToUnityVector3();
					}

					Vector3 pos = netObj.CurrentPosition;
					float gap = (netObj.CurrentPosition - netObj.PreviousPosition).sqrMagnitude;

					if (isConstInterpolate)
					{
						if (gap < INTERPOLATE_DISTANCE_LIMIT)
						{
							pos = Vector3.Lerp(netObj.transform.position,
											   netObj.CurrentPosition,
											   0.5f);
						}

						netObj.transform.position = pos;
					}
					else
					{
						float renderDelta = _deltaAccumulator / stepTime;
						if (gap < INTERPOLATE_DISTANCE_LIMIT)
						{
							pos = Vector3.Lerp(netObj.PreviousPosition,
											   netObj.CurrentPosition,
											   renderDelta);
						}

						netObj.transform.position = pos;
					}
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void sendSynchronization()
			{
				// Send network data
				_serializeTimer += deltaTime;
				if (_serializeTimer > GlobalNetwork.CLIENT_NETWORK_TICK_INTERVAL)
				{
					_serializeTimer -= GlobalNetwork.CLIENT_NETWORK_TICK_INTERVAL;
					if (_serializeTimer > GlobalNetwork.CLIENT_NETWORK_TICK_INTERVAL * 4)
					{
						_serializeTimer = 0;
					}

					// Serialize reliable data
					_reliableBuffer.Reset();
					SerializeReliableData(_reliableBuffer, _networkObjectById, PacketType.CS_Sync_RemoteReliable);

					if (_reliableBuffer.Size > 0)
					{
						_networkManager?.SendReliable(_reliableBuffer);
					}

					// Serialize unreliable data
					_mtuBuffer.Reset();
					SerializeUnreliableData(_mtuBuffer, _networkObjectById, PacketType.CS_Sync_RemoteUnreliable);
					if (_mtuBuffer.Size > 0)
					{
						_networkManager?.SendUnreliable(_mtuBuffer);
					}
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void updateObjectLifeCycle()
			{
				while (_createdObjectQueue.Count > 0)
				{
					var createSet = _createdObjectQueue.Dequeue();
					createSet.NetObj.OnCreated();
					if (createSet.IsSpawn)
					{
						createSet.NetObj.OnSpawn();
					}
					else
					{
						createSet.NetObj.OnEnter();
					}
				}
			}
		}

		public void Clear()
		{
			foreach (var netObj in _networkObjectById.Values)
			{
				netObj.OnLeave();
				netObj.OnDestroyed();
				netObj.OnDestroy?.Invoke(netObj);
			}

			_networkObjectById.Clear();
			_netObjSetByType.Clear();
			_createdObjectQueue.Clear();
			_isRunning = false;
		}

		public bool TryGetNetworkObject(NetworkIdentity id,
										[MaybeNullWhen(false)]
										out RemoteNetworkObject networkObject)
		{
			return _networkObjectById.TryGetValue(id, out networkObject);
		}

		public bool TryGetNetworkObjectSetBy(NetworkObjectType networkObjectType,
											 [MaybeNullWhen(false)]
											 out HashSet<RemoteNetworkObject> remoteNetworks)
		{
			return _netObjSetByType.TryGetValue(networkObjectType, out remoteNetworks);
		}

		public void AddRigidBody(KaRigidBody rigidBody)
		{
			_physicsWorld.AddRigidBody(rigidBody);
		}

		public void RemoveRigidBody(KaRigidBody rigidBody)
		{
			_physicsWorld.RemoveRigidBody(rigidBody);
		}

		public void SetGameMapData(GameSceneMapData mapData)
		{
			_physicsWorld.ReleaseStaticRigidBodies();
			_physicsWorld.SetStaticRigidBodies(mapData.StaticRigidBodies);
		}

		public void OnMasterPhysics(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				if (_networkObjectById.TryGetValue(id, out var syncObj))
				{
					if (!syncObj.RigidBody.TryDeserializeEventSyncData(reader))
						return;

					syncObj.OnPhysicsEventChanged();
				}
				else
				{
					NetRigidBody.IgnoreSyncData(reader);
				}
			}
		}

		public void OnMasterSpawn(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				var result = tryCreateInstanceFrom(reader,
												   SPAWN_ERROR_FORMAT, 
												   SPAWN_IGNORED_FORMAT, 
												   out var netObj);
				_createdObjectQueue.Enqueue((netObj, IsSpawn: true));

				if (result == SyncResult.Error)
					return;

				if (result == SyncResult.Ignore)
					continue;
			}
		}

		public void OnMasterEnter(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				var result = tryCreateInstanceFrom(reader,
												   ENTER_ERROR_FORMAT,
												   ENTER_IGNORED_FORMAT,
												   out var netObj);
				_createdObjectQueue.Enqueue((netObj, IsSpawn: false));

				if (result == SyncResult.Error)
					return;

				if (result == SyncResult.Ignore)
					continue;
			}
		}

		public void OnMasterDespawn(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				var result = tryDestroyInstanceFrom(reader,
													DESPAWN_ERROR_FORMAT,
													out var destroyedObject);

				if (result == SyncResult.Success)
				{
					destroyedObject.OnDespawn();
					destroyedObject.OnDestroyed();
					destroyedObject.Destroyed();
#if DEBUG_LOG
					_log.Debug($"Despawn {destroyedObject.Identity}");
#endif
				}
				else
				{
					return;
				}
			}
		}

		public void OnMasterLeave(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				var result = tryDestroyInstanceFrom(reader,
													LEAVE_ERROR_FORMAT,
													out var destroyedObject);

				if (result == SyncResult.Success)
				{
					destroyedObject.OnLeave();
					destroyedObject.OnDestroyed();
					destroyedObject.Destroyed();
#if DEBUG_LOG
					_log.Debug($"Leave {destroyedObject.Identity}");
#endif
				}
				else
				{
					return;
				}
			}
		}

		private SyncResult tryCreateInstanceFrom(IPacketReader reader,
												 string errorFormat,
												 string ignoredFormat,
												 out RemoteNetworkObject netObj)
		{
			NetworkObjectType type = reader.ReadNetworkObjectType();
			NetworkIdentity id = new(reader);
			if (_resourcesManager.TryGetNetworkObjectPrefab(type, out var prefab))
			{
				var go = Instantiate(prefab);
				netObj = go.GetComponent<RemoteNetworkObject>();
				netObj.InitializeMasterProperties();
				netObj.InitializeRemoteProperties();
				netObj.RigidBody.TryDeserializeInitialSyncData(reader);
				netObj.BindReferences(this, GameplayManager, _physicsWorld);
				netObj.Created(id);
				if (!netObj.TryDeserializeEveryProperty(reader))
				{
					_networkManager.Disconnect(DisconnectReasonType.ClientError_CannotHandlePacket,
											   string.Format(errorFormat, id));
					return SyncResult.Error;
				}
				go.transform.position = netObj.RigidBody.Position.ToUnityVector3();
				if (_networkObjectById.ContainsKey(id))
				{
					Destroy(netObj);
					_log.Fatal(ignoredFormat);
					return SyncResult.Ignore;
				}
				_networkObjectById.Add(id, netObj);
				if (!_netObjSetByType.ContainsKey(type))
				{
					_netObjSetByType.Add(type, new HashSet<RemoteNetworkObject>());
				}
				_netObjSetByType[type].Add(netObj);
				return SyncResult.Success;
			}

			netObj = null;
			_networkManager.Disconnect(DisconnectReasonType.ClientError_WrongClientData,
									   $"There is no matched prefab. Type : {type}");
			return SyncResult.Error;
		}

		private SyncResult tryDestroyInstanceFrom(IPacketReader reader,
												  string errorFormat,
												  out RemoteNetworkObject destroyedObject)
		{
			NetworkIdentity id = new(reader);
			if (!_networkObjectById.ContainsKey(id))
			{
				reader.IgnoreAll();
				string errorMessage = string.Format(errorFormat, id);

				_log.Fatal(errorMessage);
				_networkManager.Disconnect(DisconnectReasonType.ClientError_CannotHandlePacket,
										   errorMessage); // DEBUGLOG

				destroyedObject = null;
				return SyncResult.Error;
			}

			destroyedObject = _networkObjectById[id];
			_networkObjectById.Remove(id);
			_netObjSetByType[destroyedObject.Type].Remove(destroyedObject);

			return SyncResult.Success;
		}

		public void OnMasterReliable(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				//_log.Debug($"OnMasterReliable {id}");
				if (!_networkObjectById.TryGetValue(id, out var netObj))
				{
					reader.IgnoreAll();
					_log.Fatal($"OnMasterReliable error! there is no such id {id}");
					_networkManager.Disconnect(DisconnectReasonType.ClientError_CannotHandlePacket,
											   $"OnMasterReliable error! there is no such id {id}");
					return;
				}
				if (!netObj.TryDeserializeSyncReliable(reader))
				{
					_networkManager.Disconnect(DisconnectReasonType.ClientError_CannotHandlePacket,
											   $"OnMasterReliable error! parse error at : {id}");
					return;
				}
			}
		}

		public void OnMasterUnreliable(IPacketReader reader)
		{
			if (!_isRunning) return;

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				if (!_networkObjectById.TryGetValue(id, out var netObj))
				{
					reader.IgnoreAll();
					return;
				}
				if (!netObj.TryDeserializeSyncUnreliable(reader))
				{
					reader.IgnoreAll();
					return;
				}
			}
		}

		// TODO : Need to pool packet
		private ByteBuffer _reliableBuffer = new ByteBuffer(64 * 1024);
		private ByteBuffer _mtuBuffer = new ByteBuffer(GlobalNetwork.MTU * 16);

		private const int OFFSET_SIZE = sizeof(PacketType) + sizeof(byte);
		public static void SerializeReliableData(IPacketWriter writer,
												 Dictionary<NetworkIdentity, RemoteNetworkObject> netObjs,
												 PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			int syncCount = 0;
			foreach (var syncObj in netObjs.Values)
			{
				if (syncObj.IsDirtyReliable)
				{
					syncCount++;
					writer.Put(syncObj.Identity);
					syncObj.SerializeSyncReliable(writer);
					syncObj.ClearDirtyReliable();
				}
			}

			// Revert size if there is no serialized data
			if (originSize == writer.Size - OFFSET_SIZE)
			{
				writer.SetSize(originSize);
			}
			else
			{
				int serializeSize = writer.Size;
				writer.SetSize(originSize);
				writer.Put(packetType);
				writer.Put((byte)syncCount);
				writer.SetSize(serializeSize);
			}
		}

		public static void SerializeUnreliableData(IPacketWriter writer,
												   Dictionary<NetworkIdentity, RemoteNetworkObject> netObjs,
												   PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			int syncCount = 0;
			foreach (var syncObj in netObjs.Values)
			{
				if (syncObj.IsDirtyUnreliable)
				{
					syncCount++;
					writer.Put(syncObj.Identity);
					syncObj.SerializeSyncUnreliable(writer);
					syncObj.ClearDirtyUnreliable();
				}
			}

			// Revert size if there is no serialized data
			if (originSize == writer.Size - OFFSET_SIZE)
			{
				writer.SetSize(originSize);
			}
			else
			{
				int serializeSize = writer.Size;
				writer.SetSize(originSize);
				writer.Put(packetType);
				writer.Put((byte)syncCount);
				writer.SetSize(serializeSize);
			}
		}
	}
}
