using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTC.Physics;
using UnityEngine;

namespace CTC.Assets.Scripts.Tests.Physics
{
	public class TestPhysicsController : MonoBehaviour
	{
		public float Speed = 1.0f;

		public KaRigidBodyBehaviour RigidObject;

		public void Start()
		{
			RigidObject.OnInitialized();
		}

		public void Update()
		{
			if (RigidObject?.RigidBody == null)
				return;

			float horizental = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

			Vector3 moveDir = new Vector3(horizental, 0, vertical).normalized;

			RigidObject.RigidBody.LinearVelocity = moveDir.ToNativeVector2() * Speed;

			float rotationCW = Input.GetKey(KeyCode.E) ? 1 : 0;
			float rotationCCW = Input.GetKey(KeyCode.Q) ? -1 : 0;
			float rotationDir = (rotationCW + rotationCCW) * Time.deltaTime;

			RigidObject.RigidBody.Rotate(rotationDir);
		}
	}
}
