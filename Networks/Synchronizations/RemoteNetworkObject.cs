#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTC.Gameplay;
using CTC.GUI.Gameplay;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.Physics;
using CTC.SystemCore;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using UnityEngine;

namespace CTC.Networks.Synchronizations
{
	public abstract class RemoteNetworkObject : MonoBehaviour, IRemoteSynchronizable
	{
		// Reference
		[AllowNull] public GameplayManager GameplayManager { get; private set; }

		[AllowNull] public GameplaySceneController GameplaySceneController { get; private set; }

		/// <summary>네트워크 객체가 속해있는 World 입니다.</summary>
		[AllowNull] public RemoteWorldManager WorldManager { get; private set; }

		/// <summary>물리 계산 월드입니다.</summary>
		[AllowNull] public KaPhysicsWorld PhysicsWorld { get; private set; }

		public GameplayController GameplayController => GameplayManager.GameplayController;

		/// <summary>물리 콜라이더입니다.</summary>
		[field: SerializeField]
		public KaCollider? Collider { get; private set; }

		/// <summary>물리 RigidBody입니다.</summary>
		public NetRigidBody RigidBody { get; private set; }
		private KaRigidBody _physicsRigidBody;

		// Physics Interpolation
		public Vector3 PreviousPosition;
		public Vector3 CurrentPosition;

		/// <summary>객체의 위치입니다.</summary>
		public System.Numerics.Vector2 Position => RigidBody.Position;

		/// <summary>네트워크 객체의 식별자입니다.</summary>
		public NetworkIdentity Identity { get; protected set; }

		/// <summary>네트워크 객체의 오브젝트 타입입니다.</summary>
		public abstract NetworkObjectType Type { get; }

		/// <summary>네트워크 객체가 활성화된 상태인지 여부입니다.</summary>
		public bool IsAlive { get; private set; } = false;

		public void BindOwner(IDirtyable owner) => throw new System.NotImplementedException();

		public Action<RemoteNetworkObject>? OnDestroy;

		/// <summary>객체가 가시성 내에서 생성되었을 때 호출됩니다.</summary>
		public virtual void OnSpawn() { }

		/// <summary>객체가 가시성 내에서 삭제되었을 때 호출됩니다.</summary>
		public virtual void OnDespawn() { }

		/// <summary>객체가 가시성에 들어왔을 때 호출됩니다.</summary>
		public virtual void OnEnter() { }

		/// <summary>객체가 가시성에서 나갔을 때 호출됩니다.</summary>
		public virtual void OnLeave() { }

		/// <summary>
		/// 객체가 생성되면 호출됩니다.<br/>
		/// OnSpawn과 OnEnter보다 먼저 호출됩니다.
		/// </summary>
		public virtual void OnCreated() { }

		/// <summary>
		/// 객체가 삭제되면 호출됩니다.
		/// OnDespawn과 OnLeave보다 나중에 호출됩니다.
		/// </summary>
		public virtual void OnDestroyed() => Destroy(gameObject);

		/// <summary>객체가 갱신되었을 때 호출됩니다.</summary>
		public virtual void OnUpdate(float deltaTime) { }

		/// <summary>객체가 고정 업데이트 되었을 때 호출됩니다.</summary>
		public virtual void OnFixedUpdate(float stepTime) { }

		/// <summary>물리 이벤트가 변경되었을 때 호출됩니다.</summary>
		public virtual void OnPhysicsEventChanged() { }

		public void BindReferences(RemoteWorldManager worldManager,
								   GameplayManager gameplayManager,
								   KaPhysicsWorld physicsWorld)
		{
			WorldManager = worldManager;
			GameplayManager = gameplayManager;
			GameplaySceneController = GameplayManager.GameplaySceneController;
			PhysicsWorld = physicsWorld;
		}

		public void Created(NetworkIdentity id)
		{
			IsAlive = true;
			Identity = id;
			_physicsRigidBody.Initialize(id, onCollideWith);
			WorldManager.AddRigidBody(_physicsRigidBody);
			CurrentPosition = _physicsRigidBody.Position.ToUnityVector3();
		}

		private void onCollideWith(int id)
		{
			if (WorldManager.TryGetNetworkObject(new NetworkIdentity(id),
												 out var netObj))
			{
				OnCollideWith(netObj);
			}
		}

		public virtual void OnCollideWith(RemoteNetworkObject other) { }

		public void Destroyed()
		{
 			WorldManager.RemoveRigidBody(_physicsRigidBody);
		}

		/// <summary>객체를 삭제합니다. 다음 프레임에 삭제됩니다.</summary>
		public void Destroy()
		{
			IsAlive = false;
		}

		protected virtual void Awake()
		{
			if (Collider == null)
			{
				_physicsRigidBody = new BoxAABBRigidBody(1, 1, true, PhysicsLayerMask.System);
			}
			else
			{
				_physicsRigidBody = Collider
					.CreateColliderInfo()
					.CreateRigidBody();
			}

			RigidBody = new NetRigidBody(_physicsRigidBody);
		}

#if UNITY_EDITOR
		public virtual void Reset()
		{
			Collider = GetComponentInChildren<KaCollider>();
		}
#endif

#if UNITY_EDITOR
		private Vector3 _lastVelocity;
#endif

		public void UpdateByManager(float deltaTime)
		{
			// Update remote network object
			OnUpdate(deltaTime);
		}

		protected bool _isDirtyReliable;
		protected bool _isDirtyUnreliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		public bool IsDirtyUnreliable => _isDirtyUnreliable;
		public abstract void SerializeSyncReliable(IPacketWriter writer);
		public abstract void SerializeSyncUnreliable(IPacketWriter writer);
		public abstract bool TryDeserializeSyncReliable(IPacketReader reader);
		public abstract bool TryDeserializeEveryProperty(IPacketReader reader);
		public abstract bool TryDeserializeSyncUnreliable(IPacketReader reader);
		public void MarkDirtyReliable() => _isDirtyReliable = true;
		public void MarkDirtyUnreliable() => _isDirtyUnreliable = true;
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void InitializeMasterProperties();
		public abstract void InitializeRemoteProperties();
		public abstract void IgnoreSyncReliable(IPacketReader reader);
		public abstract void IgnoreSyncUnreliable(IPacketReader reader);
	}
}

#nullable disable