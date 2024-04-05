#if UNITY_EDITOR

using System;
using CTC.Utils;
using KaNet.Physics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Physics
{
	[RequireComponent(typeof(KaCollider), typeof(MeshRenderer), typeof(MeshFilter))]
	public class KaColliderGizmo : MonoBehaviour
	{
		public bool CheckValidation = false;

		[SerializeField]
		private KaCollider _collider;

		public KaPhysicsShapeType PhysicsShapeType => _collider.PhysicsShapeType;
		public float Width => _collider.Width;
		public float Height => _collider.Height;
		public float Radius => _collider.Radius;
		public float Rotation => _collider.Rotation;
		public bool IsStatic => _collider.IsStatic;

		public Transform Transform => _collider.transform;

		public void Reset()
		{
			_collider = GetComponent<KaCollider>();
			MeshRenderer = GetComponent<MeshRenderer>();
			MeshFilter = GetComponent<MeshFilter>();
		}

		// Gizmo Color Set
		private const float MESH_ALPHA = 0.4f;
		private const float LINE_ALPHA = 0.7f;

		private static readonly Color DEFAULT_MESH_COLOR = new Color(1, 1, 1, MESH_ALPHA);
		private static readonly Color DEFAULT_LINE_COLOR = new Color(1, 1, 1, LINE_ALPHA);
		private static readonly Color AABB_MESH_COLOR = new Color(0, 1.0f, 0, MESH_ALPHA);
		private static readonly Color AABB_LINE_COLOR = new Color(0, 1.0f, 0, LINE_ALPHA);
		private static readonly Color OBB_MESH_COLOR = new Color(0, 1.0f, 0.75f, MESH_ALPHA);
		private static readonly Color OBB_LINE_COLOR = new Color(0, 1.0f, 0.75f, LINE_ALPHA);
		private static readonly Color CIRCLE_MESH_COLOR = new Color(0, 0.4f, 0, MESH_ALPHA);
		private static readonly Color CIRCLE_LINE_COLOR = new Color(0, 0.4f, 0, LINE_ALPHA);

		private const float STATIC_MESH_COLOR = 0.4f;
		private const float STATIC_LINE_ALPHA = 0.7f;
		private const float DEFAULT_STATIC_COLOR = 0.2f;
		private const float AABB_STATIC_COLOR = 0.6f;
		private const float OBB_STATIC_COLOR = 0.3f;
		private const float CIRCLE_STATIC_COLOR = 0.1f;

		private static readonly Color STATIC_DEFAULT_LINE_COLOR = new Color(1, 1, 1, STATIC_LINE_ALPHA);
		private static readonly Color STATIC_AABB_LINE_COLOR = STATIC_DEFAULT_LINE_COLOR;
		private static readonly Color STATIC_OBB_LINE_COLOR = STATIC_DEFAULT_LINE_COLOR;
		private static readonly Color STATIC_CIRCLE_LINE_COLOR = STATIC_DEFAULT_LINE_COLOR;

		private static readonly Color STATIC_DEFAULT_MESH_COLOR
			= new Color(DEFAULT_STATIC_COLOR, DEFAULT_STATIC_COLOR, DEFAULT_STATIC_COLOR, STATIC_MESH_COLOR);
		private static readonly Color STATIC_AABB_MESH_COLOR
			= new Color(AABB_STATIC_COLOR, AABB_STATIC_COLOR, AABB_STATIC_COLOR, STATIC_MESH_COLOR);
		private static readonly Color STATIC_OBB_MESH_COLOR
			= new Color(OBB_STATIC_COLOR, OBB_STATIC_COLOR, OBB_STATIC_COLOR, STATIC_MESH_COLOR);
		private static readonly Color STATIC_CIRCLE_MESH_COLOR
			= new Color(CIRCLE_STATIC_COLOR, CIRCLE_STATIC_COLOR, CIRCLE_STATIC_COLOR, STATIC_MESH_COLOR);

		// Gizmo Properties
		[TitleGroup("Gizmo")] public MeshRenderer MeshRenderer = null;
		[TitleGroup("Gizmo")] public MeshFilter MeshFilter = null;

		[TitleGroup("Gizmo")] public Color MeshGizmoColor = new Color(1, 1, 1, 0.5f);
		[TitleGroup("Gizmo")] public Color WireGizmoColor = new Color(1, 1, 1, 0.7f);

		[TitleGroup("Gizmo")] private Mesh _gizmoMesh = null;

		// Gizmo Event Variables
		private KaPhysicsShapeType _gizmoShapeType = KaPhysicsShapeType.None;
		private bool _isGizmoInitialized = false;
		private bool _isGizmoStatic = false;
		private float _preRadius = 0;
		private float _preRotation = 0;
		private float _preWidth;
		private float _preHeight;

		private void OnDrawGizmos()
		{
			if (CheckValidation)
			{
				checkValidation();
			}

			// Check it's initialized
			if (_isGizmoInitialized == false)
			{
				_isGizmoInitialized = true;
				MeshRenderer.material = Resources.Load<Material>("mats/DefaultMat");
			}

			// On physics shape changed
			if (_gizmoShapeType != PhysicsShapeType)
			{
				_gizmoShapeType = PhysicsShapeType;

				if (PhysicsShapeType == KaPhysicsShapeType.Box_AABB ||
					PhysicsShapeType == KaPhysicsShapeType.Box_OBB)
				{
					_gizmoMesh = MeshCreator.ColliderBox;
					MeshFilter.mesh = MeshCreator.CreateBoxMesh();
				}
				else if (PhysicsShapeType == KaPhysicsShapeType.Circle)
				{
					_preRadius = Radius;
					int precision = getPrecision(_preRadius);
					_gizmoMesh = MeshCreator.GetCylinderMesh(precision);
					MeshFilter.mesh = MeshCreator.GetCylinderMesh(4);
				}

				setColor(_gizmoShapeType, _isGizmoStatic);
			}

			// On IsStatic changed
			if (_isGizmoStatic != IsStatic)
			{
				_isGizmoStatic = IsStatic;
				setColor(_gizmoShapeType, _isGizmoStatic);
			}

			// Draw gizmo
			if (PhysicsShapeType == KaPhysicsShapeType.Box_AABB ||
				PhysicsShapeType == KaPhysicsShapeType.Box_OBB)
			{
				GizmosHelper.DrawCube(Transform, MeshGizmoColor, WireGizmoColor);
			}
			else if (PhysicsShapeType == KaPhysicsShapeType.Circle)
			{
				if (_preRadius != Radius)
				{
					_preRadius = Radius;
					int precision = getPrecision(_preRadius);
					_gizmoMesh = MeshCreator.GetCylinderMesh(precision);
				}

				GizmosHelper.DrawMesh(Transform, _gizmoMesh, MeshGizmoColor);
				GizmosHelper.DrawWireCylinder(Transform, _gizmoMesh, WireGizmoColor);
			}

			int getPrecision(float radius) => (int)(_preRadius * 4.0f) * 4 + 4;

			void setColor(KaPhysicsShapeType shapeType, bool isStatic)
			{
				if (isStatic)
				{
					switch (shapeType)
					{
						case KaPhysicsShapeType.Box_AABB:
							MeshGizmoColor = STATIC_AABB_MESH_COLOR;
							WireGizmoColor = STATIC_AABB_LINE_COLOR;
							break;

						case KaPhysicsShapeType.Box_OBB:
							MeshGizmoColor = STATIC_OBB_MESH_COLOR;
							WireGizmoColor = STATIC_OBB_LINE_COLOR;
							break;

						case KaPhysicsShapeType.Circle:
							MeshGizmoColor = STATIC_CIRCLE_MESH_COLOR;
							WireGizmoColor = STATIC_CIRCLE_LINE_COLOR;
							break;

						default:
							MeshGizmoColor = STATIC_DEFAULT_MESH_COLOR;
							WireGizmoColor = STATIC_DEFAULT_LINE_COLOR;
							break;
					}
				}
				else
				{
					switch (shapeType)
					{
						case KaPhysicsShapeType.Box_AABB:
							MeshGizmoColor = AABB_MESH_COLOR;
							WireGizmoColor = AABB_LINE_COLOR;
							break;

						case KaPhysicsShapeType.Box_OBB:
							MeshGizmoColor = OBB_MESH_COLOR;
							WireGizmoColor = OBB_LINE_COLOR;
							break;

						case KaPhysicsShapeType.Circle:
							MeshGizmoColor = CIRCLE_MESH_COLOR;
							WireGizmoColor = CIRCLE_LINE_COLOR;
							break;

						default:
							MeshGizmoColor = DEFAULT_MESH_COLOR;
							WireGizmoColor = DEFAULT_LINE_COLOR;
							break;
					}
				}
			}
		}

		private void checkValidation()
		{
			// Check size
			if (_preWidth != Width)
			{
				if (Width < KaPhysics.MIN_COLLIDER_SIZE)
				{
					Vector3 size = Transform.localScale;
					Transform.localScale = new Vector3(KaPhysics.MIN_COLLIDER_SIZE, size.y, size.z);
				}

				_preWidth = Width;
				if (PhysicsShapeType == KaPhysicsShapeType.Circle)
				{
					Vector3 size = Transform.localScale;
					Transform.localScale = new Vector3(size.x, size.y, size.x);
				}
			}
			else if (_preHeight != Height)
			{
				if (Height < KaPhysics.MIN_COLLIDER_SIZE)
				{
					Vector3 size = Transform.localScale;
					Transform.localScale = new Vector3(size.x, size.y, KaPhysics.MIN_COLLIDER_SIZE);
				}

				_preHeight = Height;
				if (PhysicsShapeType == KaPhysicsShapeType.Circle)
				{
					Vector3 size = Transform.localScale;
					Transform.localScale = new Vector3(size.z, size.y, size.z);
				}
			}

			// Check rotation
			float rotation = Transform.localRotation.eulerAngles.y;

			if (_preRotation != rotation)
			{
				_gizmoShapeType = KaPhysicsShapeType.None;
				_preRotation = rotation;
				if (PhysicsShapeType == KaPhysicsShapeType.Circle)
				{
					Transform.localRotation = Quaternion.identity;
				}
				else
				{
					if (KaPhysics.NearlyEqual(MathF.Abs(rotation), 90) ||
						KaPhysics.NearlyEqual(MathF.Abs(rotation), 270))
					{
						Vector3 size = Transform.localScale;
						float temp = size.x;
						size.x = size.z;
						size.z = temp;
						Transform.localScale = size;
						Transform.localRotation = Quaternion.identity;
						_collider.OverrideProperties(KaPhysicsShapeType.Box_AABB);
					}
					else if (KaPhysics.NearlyEqual(MathF.Abs(rotation), 0) ||
							 KaPhysics.NearlyEqual(MathF.Abs(rotation), 180) ||
							 KaPhysics.NearlyEqual(MathF.Abs(rotation), 360))
					{
						Transform.localRotation = Quaternion.identity;
						_collider.OverrideProperties(KaPhysicsShapeType.Box_AABB);
					}
					else
					{
						_collider.OverrideProperties(KaPhysicsShapeType.Box_OBB);
					}
				}
			}
		}
	}
}

#endif