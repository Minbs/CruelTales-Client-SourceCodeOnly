#nullable enable
#pragma warning disable CS0649


using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using CTC.Gameplay.Proxies;
using CTC.GUI.MiniGames;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class Dueoksini_MiniGameController : MiniGameControllerBase
	{
		// Reference
		[field: SerializeField]
		public Dueoksini_Navigation Duoksini_Navigation { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			Initialize(Duoksini_Navigation);

			OnTeamScoreByFactionChanged += onTeamScoreByFactionChanged;
		}

		private void onTeamScoreByFactionChanged(SyncDictionary<NetByte, NetInt16> scoreTable)
		{
			foreach (var key in scoreTable.Keys)
			{
				Faction team = (Faction)key.Value;
				int score = (int)scoreTable[key];
				Duoksini_Navigation.SetTeamScore(team, score);
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
			Duoksini_Navigation.Initialize(this);
		}
	}
}

#pragma warning restore CS0649
