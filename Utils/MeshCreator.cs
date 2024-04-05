using System.Collections.Generic;
using CTC.Gameplay.Helpers;
using UnityEngine;

namespace CTC.Utils
{
	public static class MeshCreator
	{
		private static readonly Vector3[] _boxVertices =
		{
			new Vector3(-0.5f, 0f, 0.5f),
			new Vector3(0.5f, 0f, 0.5f),
			new Vector3(0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 0f, -0.5f),
			new Vector3(-0.5f, 1f, 0.5f),
			new Vector3(0.5f, 1f, 0.5f),
			new Vector3(0.5f, 1f, -0.5f),
			new Vector3(-0.5f, 1f, -0.5f),
		};

		private static readonly int[] _boxTriangleIndices =
		{
			7, 6, 2,
			2, 3, 7,
			6, 5, 1,
			1, 2, 6,
			5, 4, 0,
			0, 1, 5,
			4, 7, 3,
			3, 0, 4,
			4, 5, 6,
			6, 7, 4
		};

		public static Mesh CreateBoxMesh()
		{
			Mesh mesh = new Mesh
			{
				vertices = _boxVertices,
				triangles = _boxTriangleIndices
			};

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return mesh;
		}

		private static readonly List<Vector3> _tempVertices = new(128);
		private static readonly List<int> _tempTriangles = new(128);

		public static Mesh ColliderBox = CreateBoxMesh();

		public static Mesh CreateCylinderMesh(int cylinderPrecision)
		{
			_tempVertices.Clear();
			_tempTriangles.Clear();

			float angle = 0f;
			float times = 360f / cylinderPrecision;
			const float RADIUS = 0.5f;
			const float HEIGHT = 1;

			for (int i = 0; i < cylinderPrecision; i++)
			{
				Vector3 v = new Vector3(0f, 0f, RADIUS);
				_tempVertices.Add(VectorSnapHelper.GetRotateVectorOnXZ(v, angle));
				angle += times;
			}

			angle = 0f;
			for (int i = 0; i < cylinderPrecision; i++)
			{
				Vector3 v = new Vector3(0f, HEIGHT, RADIUS);
				_tempVertices.Add(VectorSnapHelper.GetRotateVectorOnXZ(v, angle));
				angle += times;
			}

			for (int i = 0; i < cylinderPrecision - 1; i++)
			{
				_tempTriangles.Add(i + cylinderPrecision);
				_tempTriangles.Add(i + cylinderPrecision + 1);
				_tempTriangles.Add(i + 1);
			}

			_tempTriangles.Add(cylinderPrecision * 2 - 1);
			_tempTriangles.Add(cylinderPrecision);
			_tempTriangles.Add(0);

			for (int i = 0; i < cylinderPrecision - 1; i++)
			{
				_tempTriangles.Add(i + 1);
				_tempTriangles.Add(i);
				_tempTriangles.Add(i + cylinderPrecision);
			}

			_tempTriangles.Add(0);
			_tempTriangles.Add(cylinderPrecision - 1);
			_tempTriangles.Add(cylinderPrecision * 2 - 1);

			for (int i = 1; i < cylinderPrecision - 1; i++)
			{
				_tempTriangles.Add(cylinderPrecision);
				_tempTriangles.Add(cylinderPrecision + i + 1);
				_tempTriangles.Add(cylinderPrecision + i);
			}

			Mesh mesh = new Mesh()
			{
				vertices = _tempVertices.ToArray(),
				triangles = _tempTriangles.ToArray(),
			};

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return mesh;
		}

#if GIZMO_CACHE
		private static Dictionary<int, Mesh> _cylinderMeshByPrecision = new();
#endif

		public static Mesh GetCylinderMesh(int cylinderPrecision)
		{
#if GIZMO_CACHE
			if (_cylinderMeshByPrecision == null)
				_cylinderMeshByPrecision = new();

			if (_cylinderMeshByPrecision.TryGetValue(cylinderPrecision, out Mesh mesh))
				return mesh;
			Mesh tempMesh = CreateCylinderMesh(cylinderPrecision);
			_cylinderMeshByPrecision.Add(cylinderPrecision, tempMesh);
			return tempMesh;
#else
			return CreateCylinderMesh(cylinderPrecision);
#endif
		}
	}
}
