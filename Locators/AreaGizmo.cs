using CT.Common.Gameplay.Infos;
using CTC.DebugTools;
using KaNet.Physics;
using UnityEngine;

namespace CTC.Locators
{
	[RequireComponent(typeof(BoxGizmo))]
	public class AreaGizmo : MonoBehaviour
	{
		public int Index;

		public BoundingBox GetBoundingBox()
		{
			var gizmo = GetComponent<BoxGizmo>();
			return gizmo.GetBoundingBox();
		}

		public AreaInfo CreateInfo()
		{
			var bound = GetBoundingBox();
			return new AreaInfo()
			{
				Index = Index,
				Position = bound.Center,
				Size = bound.Size,
			};
		}
	}
}