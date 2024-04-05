using UnityEngine;

public static class VectorExtension
{
	#region Vector3
	public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
	public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
	public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);
	#endregion

	#region Vector2
	public static Vector2 WithX(this Vector2 v, float x) => new Vector2(x, v.y);
	public static Vector2 WithY(this Vector2 v, float y) => new Vector2(v.x, y);
	/// <summary>시계방향으로 90도 회전합니다.</summary>
	public static Vector2 RotateRight90(this Vector2 v) => new Vector2(v.y, -v.x);
	/// <summary>반시계방향으로 90도 회전합니다.</summary>
	public static Vector2 RotateLeft90(this Vector2 v) => new Vector2(-v.y, v.x);
	#endregion

	#region Quaternion
	public static Quaternion WithX(this Quaternion q, float x) => new Quaternion(x, q.y, q.z, q.w);
	public static Quaternion WithY(this Quaternion q, float y) => new Quaternion(q.x, y, q.z, q.w);
	public static Quaternion WithZ(this Quaternion q, float z) => new Quaternion(q.x, q.y, z, q.w);
	public static Quaternion WithW(this Quaternion q, float w) => new Quaternion(q.x, q.y, q.z, w);
	#endregion

	public static Quaternion ToQuaternion(this Vector3 vector)
	{
		Quaternion q = Quaternion.Euler(vector);
		return q;
	}

	/// <summary>벡터의 x를 min으로 y를 max로 하는 범위에서 렌덤한 값을 반환합니다.</summary>
	/// <returns>렌덤한 값</returns>
	public static float GetRandomFromMinMax(this Vector2 vector)
	{
		return Random.Range(vector.x, vector.y);
	}

	public static System.Numerics.Vector2 ToNativeVector2(this UnityEngine.Vector2 v)
	{
		return new System.Numerics.Vector2(v.x, v.y);
	}

	public static System.Numerics.Vector2 ToNativeVector2(this UnityEngine.Vector3 v)
	{
		return new System.Numerics.Vector2(v.x, v.z);
	}

	public static UnityEngine.Vector3 ToUnityVector3(this System.Numerics.Vector2 v)
	{
		return new UnityEngine.Vector3(v.X, 0, v.Y);
	}

	public static UnityEngine.Vector2 ToUnityVector2(this System.Numerics.Vector2 v)
	{
		return new UnityEngine.Vector3(v.X, v.Y);
	}

	public static UnityEngine.Vector3 ToUnityVector3(this System.Numerics.Vector3 v)
	{
		return new UnityEngine.Vector3(v.X, v.Y, v.Z);
	}
}
