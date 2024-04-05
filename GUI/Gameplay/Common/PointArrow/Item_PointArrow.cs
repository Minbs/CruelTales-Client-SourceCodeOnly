using UnityEngine;

namespace CTC.GUI.Gameplay.Common.PointArrow
{
	public class Item_PointArrow : MonoBehaviour
	{
		public void SetDirection(Vector2 direction)
		{
			SetDirection(direction._x0y());
		}

		public void SetDirection(Vector3 direction)
		{
			direction = direction.normalized;
			float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
			Vector3 rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward).eulerAngles;
			rotation.x = 90;
			transform.eulerAngles = rotation;
		}

		/*
		public Transform PlayerTransform;
		public Transform TargetTransform;

		public void Bind(Transform playerTransform ,Transform targetTransform)
		{
			PlayerTransform = playerTransform;
			TargetTransform = targetTransform;
		}

		private void Update()
		{
			SetDirection(TargetTransform.position - PlayerTransform.position);
		}
		*/
	}
}
