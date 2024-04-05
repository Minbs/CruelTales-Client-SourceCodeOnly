using UnityEngine;

namespace CTC.Utils
{
	public static class TransformExtension
	{
		/// <summary>Target에서 마우스 까지의 방향벡터를 반환합니다.</summary>
		/// <param name="camera"></param>
		/// <param name="from"></param>
		/// <returns></returns>
		public static Vector2 ToMouseDirection(this Camera camera, Vector3 from)
		{
			Vector2 mousePosition = Input.mousePosition;
			Vector2 screenPosition = camera.WorldToScreenPoint(from);
			
			return (mousePosition - screenPosition).normalized;
		}
	}
}
