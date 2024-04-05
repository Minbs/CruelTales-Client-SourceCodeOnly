using UnityEngine;

namespace CTC.DebugTools
{
	[ExecuteInEditMode]
	public class BoxColliderGizmo : MonoBehaviour
	{
#if UNITY_EDITOR
		public BoxCollider BoxCollider;

		public Color GizmoFillColor = new Color(1, 1, 1, 0.3f);
		public Color GizmoLineColor = new Color(1, 1, 1, 0.6f);

		public void Reset()
		{
			BoxCollider = GetComponent<BoxCollider>();
		}

		public void OnDrawGizmos()
		{
			if (BoxCollider == null)
				return;

			Vector3 currentPosition = BoxCollider.center - transform.position;
			Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(transform.position),
										  transform.rotation, transform.lossyScale);
			Vector3 boxSize = BoxCollider.size;

			Gizmos.color = GizmoFillColor;
			Gizmos.DrawCube(currentPosition, boxSize);
			Gizmos.color = GizmoLineColor;
			Gizmos.DrawWireCube(currentPosition, boxSize);
		}
#endif
	}
}
