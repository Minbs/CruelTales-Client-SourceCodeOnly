#nullable enable
#pragma warning disable CS0649

using CTC.GUI.MiniGames;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class RedHood_MiniGameController : MiniGameControllerBase
	{
		// Reference
		[field: SerializeField]
		public RedHood_Navigation RedHood_Navigation { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			Initialize(RedHood_Navigation);
		}

		public override void OnCreated()
		{
			base.OnCreated();
			RedHood_Navigation.Initialize(this);
		}
	}
}

#pragma warning restore CS0649
