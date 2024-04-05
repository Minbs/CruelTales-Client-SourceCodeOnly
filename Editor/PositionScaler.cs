#if UNITY_EDITOR

using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC
{
	public class PositionScaler : MonoBehaviour
    {
		[Button]
		public void Multiply(float scale = 2.0f)
		{
			var allTransform = this.GetComponentList<Transform>();
			foreach (var t in  allTransform)
			{
				var localPos = t.localPosition;
				t.localPosition = localPos * scale;
			}
		}
    }
}

#endif