using CT.Logger;
using KaNet.Physics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Physics
{
	public class KaCollider : MonoBehaviour
	{
		private readonly static ILog _log = LogManager.GetLogger(typeof(KaCollider));

		[field: SerializeField]
		[field: ValidateInput(nameof(Validation_ColliderTransform),
							  "You cannot set rotation x and z",
							  InfoMessageType.Error)]
		public KaPhysicsShapeType PhysicsShapeType { get; private set; }

		[field: SerializeField]
		public PhysicsLayerMask LayerMask { get; private set; }

		[ShowInInspector]
		public float Width => transform.localScale.x;

		[ShowInInspector]
		public float Height => transform.localScale.z;

		[ShowInInspector]
		public float Radius => Width * 0.5f;

		[ShowInInspector]
		public float Rotation => Mathf.Deg2Rad * transform.localRotation.eulerAngles.y;

		[field: SerializeField]
		public bool IsStatic { get; private set; }

		public bool Validation_ColliderTransform()
		{
			return transform.localRotation.eulerAngles.x == 0 &&
				   transform.localRotation.eulerAngles.z == 0;
		}

		public void OverrideProperties(KaPhysicsShapeType physicsShapeType)
		{
			PhysicsShapeType = physicsShapeType;
		}

		public void OverrideProperties(KaPhysicsShapeType physicsShapeType,
									   PhysicsLayerMask layerMask)
		{
			PhysicsShapeType = physicsShapeType;
			LayerMask = layerMask;
		}

		public void OverrideProperties(KaPhysicsShapeType physicsShapeType,
									   PhysicsLayerMask layerMask,
									   bool isStatic)
		{
			PhysicsShapeType = physicsShapeType;
			LayerMask = layerMask;
			IsStatic = isStatic;
		}

		public void OverrideTransform(float width, float height, float rotation = 0)
		{
			Vector3 size = transform.localScale;
			size.x = width;
			size.z = height;
			transform.localScale = size;
			transform.localRotation = Quaternion.Euler(0, rotation, 0);
		}

		public ColliderInfo CreateColliderInfo()
		{
			return new ColliderInfo(PhysicsShapeType, LayerMask,
									transform.position.ToNativeVector2(), 
									Rotation, Radius, Width, Height, IsStatic);
		}
	}
}
