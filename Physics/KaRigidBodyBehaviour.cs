using System.Collections;
using CT.Logger;
using CTC.SystemCore;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Physics
{
	/// <summary>
	/// TODO : Remote World Manager로 동기화 시켜야함
	/// </summary>
	public class KaRigidBodyBehaviour : MonoBehaviour
	{
		private readonly static ILog _log = LogManager.GetLogger(typeof(KaRigidBodyBehaviour));
#if UNITY_EDITOR
		[field: ValidateInput(nameof(Validation_Collider), "There is no KaCollider!", InfoMessageType.Error)]
#endif
		[field: SerializeField]
		public KaCollider Collider { get; private set; }
		[ShowInInspector]
		public KaRigidBody RigidBody { get; private set; }

		private KaPhysicsWorld _physicsWorld;
		private bool _isInitialized;
		public bool AutoApplyTransform;

#if UNITY_EDITOR
		public void Reset()
		{
			Collider = GetComponentInChildren<KaCollider>();
		}

		public bool Validation_Collider()
		{
			Collider = this.GetComponentInChildren<KaCollider>();
			return Collider != null;
		}
#endif

		public void OnEnable()
		{
			OnInitialized();
		}

		public void OnDisable()
		{
			OnReleased();
		}

		public void OnInitialized()
		{
			StartCoroutine(initialize());
		}

		private IEnumerator initialize()
		{
			if (_isInitialized)
				yield break;
			_isInitialized = true;

			if (RigidBody == null)
			{
				RigidBody = Collider
					.CreateColliderInfo()
					.CreateRigidBody();
			}

			for (int i = 0; i < 100; i++)
			{
				yield return new WaitUntil(isWorldLoaded);
			}

			_physicsWorld = GlobalService.PhysicsWorld;
			_physicsWorld.AddRigidBody(RigidBody);
		}

		private static bool isWorldLoaded()
		{
			return GlobalService.PhysicsWorld != null;
		}

		public void OnReleased()
		{
			if (!_isInitialized)
				return;
			_isInitialized = false;

			_physicsWorld.RemoveRigidBody(RigidBody);
		}

		public void Update()
		{
			if (!_isInitialized)
			{
				return;
			}

			if (AutoApplyTransform)
			{
				transform.position = RigidBody.Position.ToUnityVector3();
				transform.rotation = Quaternion.Euler(0, RigidBody.Rotation * Mathf.Rad2Deg, 0);
			}
		}
	}
}
