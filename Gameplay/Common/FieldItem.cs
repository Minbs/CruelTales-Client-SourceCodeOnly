#nullable enable
#pragma warning disable CS0649

using CTC.Gameplay.Proxies;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class FieldItem : Interactor
	{
		[SerializeField]
		private FieldItemProxy _proxy;

#if UNITY_EDITOR
		public override void Reset()
		{
			base.Reset();
			_proxy = GetComponentInChildren<FieldItemProxy>();
		}
#endif

		public override void OnSpawn()
		{
			base.OnSpawn();
			_proxy.OnSpawn();
		}

		public override void OnCreated()
		{
			base.OnCreated();
			_proxy.Initialize(ItemType);
		}

		public override void OnTarget(bool isTarget)
		{
			_proxy.OnOutline(isOn: isTarget);
		}
	}
}
