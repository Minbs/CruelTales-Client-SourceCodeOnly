using System;
using CTC.Utils;
using UnityEngine;

namespace CTC
{
	/// <summary>8방향 입력 enum flag입니다.</summary>
	public enum TestInputDirection : byte
	{
		Right = 0,
		RightUp = 1,
		Up = 2,
		LeftUp = 3,
		Left = 4,
		LeftDown = 5,
		Down = 6,
		RightDown = 7,
		None = 8,
	}

	public class InputSnapTest : MonoBehaviour
	{
		public Transform BaseTransform;
		public Transform TargetTransform;

		public const float INV_SNAP_RAD = 1.0f / (MathF.PI / 8);

		public TestInputDirection GetInputDirectionBy(Vector2 vec)
		{
			if (vec.y >= 0)
			{
				return (TestInputDirection)((MathF.Acos(vec.x) * INV_SNAP_RAD + 1) * 0.5f);
			}
			else
			{
				return (TestInputDirection)((int)((MathF.Acos(-vec.x) * INV_SNAP_RAD + 9) * 0.5f) % 8);
			}
		}

		private static readonly Vector2[] directionTable = new Vector2[]
		{
			new Vector2(1, 0),
			new Vector2(1, 1).normalized,
			new Vector2(0, 1),
			new Vector2(-1, 1).normalized,
			new Vector2(-1, 0),
			new Vector2(-1, -1).normalized,
			new Vector2(0, -1),
			new Vector2(1, -1).normalized,
		};

		public Vector2 GetDirectionVectorBy(TestInputDirection inputDirection)
		{
			return directionTable[(int)inputDirection];
		}

		public void Update()
		{
			Vector3 targetPosition = TargetTransform.position;
			Vector3 basePosition = BaseTransform.position;

			Vector2 direction = (targetPosition - basePosition)._xz().normalized;
			var inputDirection = GetInputDirectionBy(direction);
			//Debug.Log(inputDirection);
			Debug.DrawLine(basePosition, targetPosition, Color.yellow);
			DebugHelper.DrawArrow(basePosition, basePosition + GetDirectionVectorBy(inputDirection)._x0y() * 2, Color.blue, 0.3f, 0);

			int index = 8;
			float rad = MathF.PI / index;
			for (int i = 0; i < index; i++)
			{
				float angle = rad * i;
				float x = MathF.Cos(angle);
				float y = MathF.Sin(angle);
				Debug.DrawLine(basePosition, basePosition + new Vector3(x, 0, y));
			}
			for (int i = 0; i < index; i++)
			{
				float angle = rad * i;
				float x = MathF.Cos(angle);
				float y = -MathF.Sin(angle);
				Debug.DrawLine(basePosition, basePosition + new Vector3(x, 0, y));
			}
		}
	}
}
