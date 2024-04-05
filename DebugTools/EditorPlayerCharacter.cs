using CTC.Physics;
using CTC.SystemCore;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using UnityEngine;
using CTC.Utils;

namespace CTC.DebugTools
{
	public class EditorPlayerCharacter : MonoBehaviour
	{
		public KaNetEditorPhysicsRunner EditorPhysicsRunner;
		public KaRigidBodyBehaviour RigidBodyBehaviour;
		public KaRigidBody RigidBody => RigidBodyBehaviour.RigidBody;
		public float Speed = 5;
		public float WalkSpeedRatio = 0.5f;
		public float PushPower = 12f;
		public float PushFriction = 2.0f;


		public DebugCamera Camera { get; private set; }

		// Physics Interpolation
		public Vector3 PreviousPosition;
		public Vector3 CurrentPosition;

		public void OnEnable()
		{
			Camera = FindObjectOfType<DebugCamera>();
			Camera.Target = transform;

			EditorPhysicsRunner = FindObjectOfType<KaNetEditorPhysicsRunner>();
			EditorPhysicsRunner.OnPhysicsCalculated += onPhysicsCalculated;
		}

		public void OnDisable()
		{
			if (Camera.Target == transform)
			{
				Camera.Target = null;
			}

			EditorPhysicsRunner.OnPhysicsCalculated -= onPhysicsCalculated;
		}

		void Update()
		{
			processAction();
			processMovement();
			transform.position = Vector3.Lerp(PreviousPosition,
											  CurrentPosition,
											  EditorPhysicsRunner.RenderDelta);
		}

		private void processAction()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 aimDirection = Camera.MainCamera.ToMouseDirection(transform.position);
				RigidBody.ForceVelocity = aimDirection.ToNativeVector2() * PushPower;
				RigidBody.ForceFriction = PushFriction;
			}
		}

		private void processMovement()
		{
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");
			float walkFactor = Input.GetKey(KeyCode.LeftShift) ? WalkSpeedRatio : 1;

			Vector2 moveDirection = new Vector2(x, y).normalized;
			if (KaPhysics.NearlyEqual(moveDirection.magnitude, 0))
			{
				RigidBody.LinearVelocity = new System.Numerics.Vector2(0, 0);
			}
			else
			{
				RigidBody.LinearVelocity = moveDirection.ToNativeVector2() * Speed * walkFactor;
			}
		}

		private void onPhysicsCalculated()
		{
			PreviousPosition = CurrentPosition;
			CurrentPosition = RigidBody.Position.ToUnityVector3();
		}
	}
}
