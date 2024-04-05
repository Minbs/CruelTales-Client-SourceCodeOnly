using System.Diagnostics;
using UnityEngine;

namespace CTC.Utils
{
	public static class DebugHelper
	{
		[Conditional("UNITY_EDITOR")]
		public static void DrawLine(Vector3 start, Vector3 end) => UnityEngine.Debug.DrawLine(start, end);

		[Conditional("UNITY_EDITOR")]
		public static void DrawLine(Vector3 start, Vector3 end, Color color) => UnityEngine.Debug.DrawLine(start, end, color);

		[Conditional("UNITY_EDITOR")]
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) => UnityEngine.Debug.DrawLine(start, end, color, duration);

		[Conditional("UNITY_EDITOR")]
		public static void DrawRay(Vector3 start, Vector3 dir) => UnityEngine.Debug.DrawRay(start, dir);

		[Conditional("UNITY_EDITOR")]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color) => UnityEngine.Debug.DrawRay(start, dir, color);

		[Conditional("UNITY_EDITOR")]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) => UnityEngine.Debug.DrawRay(start, dir, color, duration);

		[Conditional("UNITY_EDITOR")]
		public static void DrawArrow(Vector3 start, Vector3 end, Color color, float arrowSize, float duration)
		{
			UnityEngine.Debug.DrawLine(end, start, color, duration);
			Vector3 dirVec = (end - start);
			Vector3 dirVecNormal = dirVec.normalized;
			dirVec -= dirVecNormal * arrowSize;
			Vector3 perpendicular = new Vector3(-dirVecNormal.z, 0, dirVecNormal.x) * arrowSize * 0.3f;
			Vector3 arrow1 = start + perpendicular + dirVec;
			Vector3 arrow2 = start - perpendicular + dirVec;
			UnityEngine.Debug.DrawLine(end, arrow1, color, duration);
			UnityEngine.Debug.DrawLine(end, arrow2, color, duration);
			UnityEngine.Debug.DrawLine(arrow1, arrow2, color, duration);
		}
	}
}
