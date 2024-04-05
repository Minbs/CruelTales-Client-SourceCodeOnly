#if UNITY_EDITOR

using UnityEngine;

namespace CTC.DebugTools
{
	[ExecuteInEditMode]
	public class SphereGizmo : MonoBehaviour
	{
		public SphereCollider SphereCollider;

		public Vector3 Offset = Vector3.zero;
		public Color GizmoFillColor = new Color(1, 1, 1, 0.3f);
		public Color GizmoLineColor = new Color(1, 1, 1, 0.6f);
		public float Radius = 0.5f;

		public void Reset()
		{
			SphereCollider = GetComponent<SphereCollider>();
			if (SphereCollider != null )
			{
				Radius = SphereCollider.radius;
			}
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = GizmoFillColor;
			Vector3 pos = transform.position + Offset;
			float radius = SphereCollider == null ? Radius : SphereCollider.radius;

			Gizmos.DrawSphere(pos, radius);
			Gizmos.color = GizmoLineColor;
			Gizmos.DrawWireSphere(pos, radius);

			Gizmos.DrawLine(pos, pos + transform.forward * radius * 3);
		}
	}
}

#endif