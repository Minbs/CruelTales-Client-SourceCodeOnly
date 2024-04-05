#nullable enable
#pragma warning disable CS0649

using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using CTC.Gameplay.Proxies;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class DueoksiniTable : Interactor
	{
		// Proxies
		[SerializeField]
		private DueoksiniTableProxy _proxy;

		protected override void Awake()
		{
			base.Awake();
			OnTeamChanged += onTeamChanged;
			OnItemCountByTypeChanged += onItemCountByTypeChanged;
		}

		public override void OnCreated()
		{
			base.OnCreated();
			_proxy.Initialized();
			onTeamChanged(Team);
		}

		private void onTeamChanged(Faction team)
		{
			_proxy.SetFaction(team);
		}

		private void onItemCountByTypeChanged(SyncDictionary<NetInt32, NetByte> itemCountTable)
		{
			_proxy.OnItemChanged(itemCountTable);
		}
	}
}

#pragma warning restore CS0649