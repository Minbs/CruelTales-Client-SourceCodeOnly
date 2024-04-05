using System;
using System.Collections.Generic;
using CTC.Physics;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using UnityEngine;

namespace CTC.SystemCore
{
	public class KaNetEditorPhysicsRunner : MonoBehaviour, IManager
	{
		// Physics
		private KaPhysicsWorld _physicsWorld = new();

		// Options
		private const float DELTA_EPSILON = 0.0001f;
		public int TickPerSecond = 60;
		public float StepTime
		{
			get
			{
				float value = 1.0f / TickPerSecond;
				return value < DELTA_EPSILON ? DELTA_EPSILON : value;
			}
		}

		// Values
		public float DeltaTime { get; private set; }
		public float DeltaAccumulator { get; private set; }
		public bool IsConstInterpolate { get; private set; }
		public float RenderDelta { get; private set; }

		/// <summary>물리가 계산되었습니다.</summary>
		public event Action OnPhysicsCalculated;

		public void Initialize()
		{
		}

		public void Release()
		{
		}

		public void OnEnable() => onInitialized();

		public void onInitialized()
		{
			LayerMaskHelper.Initialize();
			GlobalService.BindPhysicsWorld(_physicsWorld);

			// Set static colliders
			List<KaRigidBody> staticRigidBodies = new();
			var colliders = FindObjectsOfType<KaCollider>();
			foreach (var c in colliders)
			{
				ColliderInfo info = c.CreateColliderInfo();
				if (!c.IsStatic)
					continue;
				staticRigidBodies.Add(info.CreateRigidBody());
			}

			_physicsWorld.SetStaticRigidBodies(staticRigidBodies);
		}

		public void OnDisable()
		{
			_physicsWorld.ReleaseStaticRigidBodies();
			GlobalService.ReleasePhysicsWorld(_physicsWorld);
		}

		public void Update()
		{
			// Bind delta time
			DeltaTime = Time.deltaTime;

			// 물리 Tick이 Rendering Frame 만큼, 혹은 그 이상 주기가 짧다면 상수로 보간한다.
			// 물리 Tick이 Rendering Frame 보다 주기가 길다면 render delta를 구해서 보간한다.
			IsConstInterpolate =
				(1.0f / StepTime) > (Application.targetFrameRate * 0.8f);

			DeltaAccumulator += DeltaTime;
			if (DeltaAccumulator > StepTime * 5)
				DeltaAccumulator = StepTime * 5;

			bool hasPhysicsCalculated = false;
			while (DeltaAccumulator >= StepTime)
			{
				hasPhysicsCalculated = true;
				DeltaAccumulator -= StepTime;
				_physicsWorld.Step(StepTime);
			}

			RenderDelta = DeltaAccumulator / StepTime;

			if (hasPhysicsCalculated)
				OnPhysicsCalculated?.Invoke();
		}

		public void AddRigidBody(KaRigidBody rigidBody)
		{
			_physicsWorld.AddRigidBody(rigidBody);
		}

		public void AddRigidBody(KaRigidBodyBehaviour rigidBodyBehaviour)
		{
			_physicsWorld.AddRigidBody(rigidBodyBehaviour.RigidBody);
		}

		public void RemoveRigidBody(KaRigidBody rigidBody)
		{
			_physicsWorld.RemoveRigidBody(rigidBody);
		}

		public void RemoveRigidBody(KaRigidBodyBehaviour rigidBodyBehaviour)
		{
			_physicsWorld.RemoveRigidBody(rigidBodyBehaviour.RigidBody);
		}
	}
}
