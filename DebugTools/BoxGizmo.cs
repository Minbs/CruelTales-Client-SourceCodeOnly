using KaNet.Physics;
using UnityEngine;

namespace CTC.DebugTools
{
#if UNITY_EDITOR
	[ExecuteInEditMode]
#endif
	public class BoxGizmo : MonoBehaviour
	{
		public bool IsBottomPivot = false;
		public bool IsOriginPivot = false;
		public Color GizmoFillColor = new Color(1, 1, 1, 0.3f);
		public Color GizmoLineColor = new Color(1, 1, 1, 0.6f);
		private Vector3 BoxSize = Vector3.one;

#if UNITY_EDITOR
		public void OnDrawGizmos()
		{
			Vector3 currentPosition = transform.position;
			// Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(currentPosition),
			// 							  _transform.rotation, _transform.lossyScale);

			Vector3 localScale = transform.localScale;
			Vector3 boxSize = new Vector3()
			{
				x = BoxSize.x * localScale.x,
				y = BoxSize.y * localScale.y,
				z = BoxSize.z * localScale.z
			};

			Vector3 halfSize = boxSize * 0.5f;

			if (IsBottomPivot)
			{
				currentPosition.y += halfSize.y;
			}

			if (IsOriginPivot)
			{
				currentPosition.x += halfSize.x;
				currentPosition.z += halfSize.z;
			}

			Gizmos.color = GizmoFillColor;
			Gizmos.DrawCube(currentPosition, boxSize);
			Gizmos.color = GizmoLineColor;
			Gizmos.DrawWireCube(currentPosition, boxSize);
		}
#endif

		public BoundingBox GetBoundingBox()
		{
			Vector3 currentPosition = transform.position;
			Vector3 localScale = transform.localScale;
			Vector3 boxSize = new Vector3()
			{
				x = BoxSize.x * localScale.x,
				y = BoxSize.y * localScale.y,
				z = BoxSize.z * localScale.z
			};

			Vector3 halfSize = boxSize * 0.5f;

			if (IsBottomPivot)
			{
				currentPosition.y += halfSize.y;
			}

			if (IsOriginPivot)
			{
				currentPosition.x += halfSize.x;
				currentPosition.z += halfSize.z;
			}

			return new BoundingBox(currentPosition.ToNativeVector2(),
								   boxSize.x, boxSize.z);
		}
	}
}
