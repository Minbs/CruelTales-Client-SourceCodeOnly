#if UNITY_EDITOR

using UnityEngine;

namespace CTC.Utils
{
	public static class GizmosHelper
	{
		public static void DrawMesh(Transform baseTransform, Mesh mesh, Color meshColor)
		{
			if (mesh == null)
				return;

			Gizmos.matrix = baseTransform.localToWorldMatrix;
			Gizmos.color = meshColor;
			Gizmos.DrawMesh(mesh);
		}

		public static void DrawCube(Transform baseTransform, Color meshColor, Color wireColor)
		{
			Vector3 center = new Vector3(0, 0.5f, 0);
			Vector3 size = Vector3.one;
			Gizmos.matrix = baseTransform.localToWorldMatrix;
			Gizmos.color = meshColor;
			Gizmos.DrawCube(center, size);
			Gizmos.color = wireColor;
			Gizmos.DrawWireCube(center, size);
		}

		public static void DrawMesh(Transform baseTransform, Mesh mesh, Color meshColor, Color wireColor)
		{
			if (mesh == null)
				return;

			Gizmos.matrix = baseTransform.localToWorldMatrix;
			Gizmos.color = meshColor;
			Gizmos.DrawMesh(mesh);
			Gizmos.color = wireColor;
			Gizmos.DrawWireMesh(mesh);
		}

		public static void DrawWireCylinder(Transform baseTransform, Mesh cylinderMesh, Color wireColor)
		{
			if (cylinderMesh == null)
				return;

			Gizmos.color = wireColor;
			Gizmos.matrix = baseTransform.localToWorldMatrix;

			Vector3[] vertices = cylinderMesh.vertices;
			int count = vertices.Length;

			for (int i = 0; i < count / 2 - 1; i++)
				Gizmos.DrawLine(vertices[i], vertices[i + 1]);

			Gizmos.DrawLine(vertices[count / 2 - 1], vertices[0]);

			for (int i = count / 2; i < count - 1; i++)
				Gizmos.DrawLine(vertices[i], vertices[i + 1]);

			Gizmos.DrawLine(vertices[^1], vertices[count / 2]);

			for (int i = 0; i < count / 2; i++)
				Gizmos.DrawLine(vertices[i], vertices[i + count / 2]);
		}
	}
}

#endif