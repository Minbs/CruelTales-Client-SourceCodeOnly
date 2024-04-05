using System;
using CT.Common.Gameplay.Infos;
using CT.Networks;
using CTC.DebugTools;
using UnityEngine;

namespace CTC.Locators
{
#if UNITY_EDITOR
	[RequireComponent(typeof(SphereGizmo))]
#endif
	public class PivotGizmo : MonoBehaviour
	{
		public int Index = GlobalNetwork.SYSTEM_PIVOT_INDEX_LIMIT;

		public PivotInfo CreateInfo()
		{
			return new PivotInfo()
			{
				Index = Index,
				Position = transform.position.ToNativeVector2(),
			};
		}
	}
}