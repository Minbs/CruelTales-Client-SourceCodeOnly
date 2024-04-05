using System.Collections.Generic;
using CTC.Gameplay.Helpers;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace CTC.Utils
{
    public class SimpleGizmo : MonoBehaviour
    {
	    public Color GizmoColor_Mesh;
	    public Color GizmoColor_Wire;
	    [ToggleGroup("Use_Box")] public bool Use_Box = false;

	    [ToggleGroup("Use_Cylinder")] public bool Use_Cylinder = false;
	    [ToggleGroup("Use_Cylinder")] public float HalfRadius_Cylinder;
	    [ToggleGroup("Use_Cylinder")] public float Height_Cylinder;
	    [ToggleGroup("Use_Cylinder")] public int Segment_Cylinder = 10;
	    
	    [TitleGroup("Assignment")] public MeshRenderer _meshRenderer = null;
	    [TitleGroup("Assignment")] public MeshFilter _meshFilter = null;

	    private Material _material = null;
	    private Mesh _boxMesh = null;
	    private Mesh _cylinderMesh = null;

	    private static readonly Vector3[] _boxVertices = {
		    new Vector3(-0.5f, 0f, 0.5f),
		    new Vector3(0.5f, 0f, 0.5f),
		    new Vector3(0.5f, 0f, -0.5f),
		    new Vector3(-0.5f, 0f, -0.5f),
		    new Vector3(-0.5f, 1f, 0.5f),
		    new Vector3(0.5f, 1f, 0.5f),
		    new Vector3(0.5f, 1f, -0.5f),
		    new Vector3(-0.5f, 1f, -0.5f),
	    };
	    private static readonly int[] _boxTriangles = {
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
	    
	    private List<Vector3> _cylinderVertices = new List<Vector3>();
	    private List<int> _cylinderTriangles = new List<int>();
	    private float _cylinderRadiusBuffer = 0f;
	    private float _cylinderHeightBuffer = 0f;
	    private int _cyllinderSegmentBuffer = 0;
	    
	    private bool _firstCylinderRender = true;
	    
	    private void OnDrawGizmos()
	    {
		    if(_material == null)
			    _material = Resources.Load<Material>("mats/DefaultMat");
		    
		    _meshRenderer.material = _material;

		    if (Use_Box && !Use_Cylinder)
		    {
			    _firstCylinderRender = true;
			    _meshFilter.mesh = _boxMesh;
			    drawMeshCube();
			    drawWireCube(new Vector3(0f, 0.5f, 0f));
		    }
		    else if (!Use_Box && Use_Cylinder)
		    {
			    if (_firstCylinderRender)
			    {
				    reCalculateCylinderMesh();
				    _firstCylinderRender = false;
			    }
			    
			    _meshFilter.mesh = _cylinderMesh;
			    
			    if(IsCylinderValueChanged())
				    reCalculateCylinderMesh();
			    
			    drawMeshCylinder();
			    drawWireCylinder();
		    }
	    }

	    private void drawMeshCube()
	    {
		    if (_boxMesh == null)
		    {
			    _boxMesh = new Mesh
			    {
				    vertices = _boxVertices,
				    triangles = _boxTriangles
			    };
			    
			    _boxMesh.RecalculateBounds();
			    _boxMesh.RecalculateNormals();
		    }
		    
		    Gizmos.color = GizmoColor_Mesh;
		    Gizmos.matrix = transform.localToWorldMatrix;
		    Gizmos.DrawMesh(_boxMesh);
	    }

	    private void drawWireCube(Vector3 position)
	    {
		    Gizmos.color = GizmoColor_Wire;
		    Gizmos.matrix = transform.localToWorldMatrix;
		    Gizmos.DrawWireCube(position, Vector3.one);
	    }


	    private bool IsCylinderValueChanged()
	    {
		    if (HalfRadius_Cylinder != _cylinderRadiusBuffer || Height_Cylinder != _cylinderHeightBuffer||
		        Segment_Cylinder != _cyllinderSegmentBuffer)
		    {
			    _cylinderRadiusBuffer = HalfRadius_Cylinder;
			    _cylinderHeightBuffer = Height_Cylinder;
			    _cyllinderSegmentBuffer = Segment_Cylinder;
			    return true;
		    }
		    else
		    {
			    _cylinderRadiusBuffer = HalfRadius_Cylinder;
			    _cylinderHeightBuffer = Height_Cylinder; 
			    _cyllinderSegmentBuffer = Segment_Cylinder;
			    return false;
		    }
	    }

	    private void reCalculateCylinderMesh()
	    {
		    _cylinderVertices.Clear();
		    _cylinderTriangles.Clear();

		    float angle = 0f;
		    float times = 360f / Segment_Cylinder;

		    for (int i = 0; i < Segment_Cylinder; i++)
		    {
			    _cylinderVertices.Add(
				    VectorSnapHelper.GetRotateVectorOnXZ(new Vector3(0f, 0f, HalfRadius_Cylinder), angle));
			    angle += times;
		    }

		    angle = 0f;
		    for (int i = 0; i < Segment_Cylinder; i++)
		    {
			    _cylinderVertices.Add(
				    VectorSnapHelper.GetRotateVectorOnXZ(new Vector3(0f, Height_Cylinder, HalfRadius_Cylinder), angle));
			    angle += times;
		    }
		    
		    for (int i = 0; i < Segment_Cylinder - 1; i++)
		    {
			    _cylinderTriangles.Add(i + Segment_Cylinder);
			    _cylinderTriangles.Add(i + Segment_Cylinder + 1);
			    _cylinderTriangles.Add(i + 1);
		    }

		    _cylinderTriangles.Add(Segment_Cylinder * 2 - 1);
		    _cylinderTriangles.Add(Segment_Cylinder);
		    _cylinderTriangles.Add(0);

		    for (int i = 0; i < Segment_Cylinder - 1; i++)
		    {
			    _cylinderTriangles.Add(i + 1);
			    _cylinderTriangles.Add(i);
			    _cylinderTriangles.Add(i + Segment_Cylinder);
		    }

		    _cylinderTriangles.Add(0);
		    _cylinderTriangles.Add(Segment_Cylinder - 1);
		    _cylinderTriangles.Add(Segment_Cylinder * 2 - 1);

		    for (int i = 1; i < Segment_Cylinder - 1; i++)
		    {
			    _cylinderTriangles.Add(Segment_Cylinder);
			    _cylinderTriangles.Add(Segment_Cylinder + i + 1);
			    _cylinderTriangles.Add(Segment_Cylinder + i);
		    }

		    if (_cylinderMesh == null)
			    _cylinderMesh = new Mesh();

		    _cylinderMesh.vertices = _cylinderVertices.ToArray();
		    _cylinderMesh.triangles = _cylinderTriangles.ToArray();
		    _cylinderMesh.RecalculateBounds();
		    _cylinderMesh.RecalculateNormals();
	    }
	    
	    private void drawMeshCylinder()
	    {
		    Gizmos.color = GizmoColor_Mesh;
		    Gizmos.matrix = transform.localToWorldMatrix;
		    Gizmos.DrawMesh(_cylinderMesh);
	    }

	    private void drawWireCylinder()
	    {
		    Gizmos.color = GizmoColor_Wire;
		    Gizmos.matrix = transform.localToWorldMatrix;

		    for (int i = 0; i < _cylinderVertices.Count / 2 - 1; i++)
		    {
			    Gizmos.DrawLine(_cylinderVertices[i], _cylinderVertices[i + 1]);
		    }
		    Gizmos.DrawLine(_cylinderVertices[_cylinderVertices.Count / 2 - 1], _cylinderVertices[0]);

		    for (int i = _cylinderVertices.Count / 2; i < _cylinderVertices.Count - 1; i++)
		    {
			    Gizmos.DrawLine(_cylinderVertices[i], _cylinderVertices[i + 1]);
		    }
		    Gizmos.DrawLine(_cylinderVertices[^1], _cylinderVertices[_cylinderVertices.Count / 2]);
		    
		    for (int i = 0; i < _cylinderVertices.Count / 2; i++)
		    {
			    Gizmos.DrawLine(_cylinderVertices[i], _cylinderVertices[i + _cylinderVertices.Count / 2]);
		    }
	    }

	    [Button]
	    public void SaveCurrentMesh()
	    {
		    string path = "Assets/Resources/meshes/" + "GeneratedMesh" + ".asset";
		    Mesh saveMesh = (Mesh)Instantiate(_cylinderMesh);
		    AssetDatabase.CreateAsset(saveMesh, AssetDatabase.GenerateUniqueAssetPath(path));
		    AssetDatabase.SaveAssets();
	    }
    }
}
#endif
