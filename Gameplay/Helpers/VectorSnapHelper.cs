using System.Collections.Generic;
using UnityEngine;

namespace CTC.Gameplay.Helpers
{
	public static class VectorSnapHelper
	{
		public static readonly Vector2 Right = new Vector2(1, 0);
		public static readonly Vector2 RightUp = new Vector2(1, 1).normalized;
		public static readonly Vector2 Up = new Vector2(0, 1);
		public static readonly Vector2 LeftUp = new Vector2(-1, 1).normalized;
		public static readonly Vector2 Left = new Vector2(-1, 0);
		public static readonly Vector2 LeftDown = new Vector2(-1, -1).normalized;
		public static readonly Vector2 Down = new Vector2(0, -1).normalized;
		public static readonly Vector2 RightDown = new Vector2(1, -1).normalized;

		public static readonly Vector2 Dir_0 = Right;
		public static readonly Vector2 Dir_45 = RightUp;
		public static readonly Vector2 Dir_90 = Up;
		public static readonly Vector2 Dir_135 = LeftUp;
		public static readonly Vector2 Dir_180 = Left;
		public static readonly Vector2 Dir_225 = LeftDown;
		public static readonly Vector2 Dir_270 = Down;
		public static readonly Vector2 Dir_315 = RightDown;
		public static readonly Vector2 Dir_360 = Right;

		// Member Variables
		private static readonly Dictionary<float, Vector2> snappedVectorDic = new Dictionary<float, Vector2>
		{
			{ 0f, new Vector2(0f, 1f) },
			{ 45f, new Vector2(-1f, 1f).normalized },
			{ 90f, new Vector2(-1f, 0f) },
			{ 135f, new Vector2(-1f, -1f).normalized },
			{ 180f, new Vector2(0f, -1f) },
			{ 225f, new Vector2(1f, -1f).normalized },
			{ 270f, new Vector2(1f, 0f) },
			{ 315f, new Vector2(1f, 1f).normalized },
			{ 360f, new Vector2(0f, 1f) },
		};

		private static readonly float[] _snappedAngle = new[]
		{
			0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f, 360f
		};

		public static Vector2 GetSnappedVector(Vector2 inputDirection)
		{
			float angle = Vector2.SignedAngle(Vector2.up, inputDirection);

			if (angle < 0f)
				angle += (360f - angle) + angle;

			float nearAngle = 0f;
			float nearValue = 360f;
			float curValue = 360f;
			for (int i = 0; i < _snappedAngle.Length; i++)
			{
				curValue = Mathf.Abs(_snappedAngle[i] - angle);
				if (curValue <= nearValue)
				{
					nearValue = curValue;
					nearAngle = _snappedAngle[i];
				}
			}

			return snappedVectorDic[nearAngle];
		}

		public static Vector3 GetRotateVectorOnXZ(Vector3 originVec, float angle)
		{
			Vector3 _returnVec = originVec;
			angle *= Mathf.Deg2Rad;

			_returnVec.x = originVec.x * Mathf.Cos(angle) - originVec.z * Mathf.Sin(angle);
			_returnVec.z = originVec.x * Mathf.Sin(angle) + originVec.z * Mathf.Cos(angle);

			return _returnVec;
		}
	}
}
