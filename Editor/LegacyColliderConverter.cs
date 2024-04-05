#if UNITY_EDITOR

using System.Collections.Generic;
using CTC.DebugTools;
using CTC.Physics;
using KaNet.Physics;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CTC
{
	public struct LegacyColliderInfo
	{
		public Vector3 Position;
		public float Width;
		public float Height;
		public float Rotation;
	}

	public class LegacyColliderConverter : MonoBehaviour
	{
		public GameObject ColliderPrefab;
		public Transform TargetObjectTransform;

		[Button]
		public void Convert()
		{
			List<LegacyColliderInfo> colliderInfoList = new();
			List<ColliderInfo> newColliderInfoList = new();
			var gizmos = GetComponentsInChildren<BoxColliderGizmo>();

			foreach (var gizmo in gizmos)
			{
				LegacyColliderInfo info = new();
				Vector3 size = gizmo.transform.localScale;
				Vector3 position = gizmo.transform.position;
				info.Position = position._x0z();
				info.Width = size.x;
				info.Height = size.z;
				info.Rotation = gizmo.transform.localRotation.eulerAngles.y;
				colliderInfoList.Add(info);
			}

			foreach (var info in colliderInfoList)
			{
				var go = PrefabUtility.InstantiatePrefab(ColliderPrefab, TargetObjectTransform) as GameObject;
				go.transform.position = info.Position;
				go.transform.rotation = Quaternion.Euler(0, info.Rotation, 0);
				go.transform.localScale = new Vector3(info.Width, 1, info.Height);
				var KaCollider = go.GetComponent<KaCollider>();
				KaCollider.OverrideProperties(KaPhysicsShapeType.Box_AABB,
											  PhysicsLayerMask.None, true);

				newColliderInfoList.Add(KaCollider.CreateColliderInfo());
			}
		}
	}
}

#endif