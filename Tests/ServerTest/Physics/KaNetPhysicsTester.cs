using System;
using CTC.Physics;
using FMOD.Studio;
using KaNet.Physics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Assets.Scripts.Tests.Physics
{
	public class KaNetPhysicsTester : MonoBehaviour
	{
		public GameObject ColliderPrefab;

		[ShowInInspector]
		public System.Numerics.Vector2 RightTop = new System.Numerics.Vector2(1, 1);
		[ShowInInspector]
		public System.Numerics.Vector2 LeftBottom = new System.Numerics.Vector2(-1, -1);
		public float MinSize = 2;
		public float MaxSize = 3;

		[Button]
		public void CreateAABBs(int count = 1, bool isStatic = false)
		{
			for (int i = 0; i < count; i++)
			{
				var randPos = RandomHelper.NextVector2(RightTop, LeftBottom);
				float width = RandomHelper.NextSingle(MinSize, MaxSize);
				float height = RandomHelper.NextSingle(MinSize, MaxSize);

				CreateTestEntity(KaPhysicsShapeType.Box_AABB, PhysicsLayerMask.Environment,
								 randPos, width, height, 0, isStatic);
			}
		}

		[Button]
		public void CreateOBBs(int count = 1, bool isStatic = false)
		{
			for (int i = 0; i < count; i++)
			{
				var randPos = RandomHelper.NextVector2(RightTop, LeftBottom);
				float width = RandomHelper.NextSingle(MinSize, MaxSize);
				float height = RandomHelper.NextSingle(MinSize, MaxSize);
				float rotation = RandomHelper.NextSingle(0, 360);

				CreateTestEntity(KaPhysicsShapeType.Box_OBB, PhysicsLayerMask.Item,
								 randPos, width, height, rotation, isStatic);
			}
		}

		[Button]
		public void CreateCircles(int count = 1, bool isStatic = false)
		{
			for (int i = 0; i < count; i++)
			{
				var randPos = RandomHelper.NextVector2(RightTop, LeftBottom);
				float radius = RandomHelper.NextSingle(MinSize, MaxSize);

				CreateTestEntity(KaPhysicsShapeType.Circle, PhysicsLayerMask.Player,
								 randPos, radius, radius, 0, isStatic);
			}
		}

		public void CreateTestEntity(KaPhysicsShapeType shapeType,
									 PhysicsLayerMask layerMask,
									 System.Numerics.Vector2 randPos,
									 float width, float height, float rotation,
									 bool isStatic)
		{
			var unityVector3 = randPos.ToUnityVector3();
			var go = Instantiate(ColliderPrefab, unityVector3, Quaternion.identity);
			go.SetActive(true);
			var rigidBody = go.GetComponent<KaRigidBodyBehaviour>();
			var collider = rigidBody.Collider;
			collider.OverrideProperties(shapeType, layerMask, isStatic);
			collider.OverrideTransform(width, height, rotation);
		}
	}
}
