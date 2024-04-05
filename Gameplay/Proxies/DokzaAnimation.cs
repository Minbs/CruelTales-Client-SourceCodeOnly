using System;
using System.Collections.Generic;
using CT.Common.Gameplay.Players;

namespace CTC.Gameplay.Proxies
{
	public enum DokzaType
	{
		None = 0,
		Normal,
		RedHood,
		Wolf
	}

	public static class DokzaAnimationSet
	{
		/// <summary>
		/// Tuple의 bool은 isFront를 뜻합니다.
		/// </summary>
		private static Dictionary<Tuple<DokzaAnimationState, bool>, string> _dokzaAnimationSetDic = new()
		{
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Idle, true), "Idle_Front" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Idle, false), "Idle_Back" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Walk, true), "Walk_Front" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Walk, false), "Walk_Back" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Run, true), "Run_Front" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Run, false), "Run_Back" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_Hammer, true), "Action_Push" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_Hammer, false), "Action_Push" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_WolfCatch, true), "Action_wolf_attack_side" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_WolfCatch, false), "Action_wolf_attack_up" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Knockback, true), "Action_Pushed" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Knockback, false), "Action_Pushed" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Event_RedHood_Bird, true), "Redhood_mission/Bird" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Event_RedHood_Bird, false), "Redhood_mission/Bird" },
		};
		
		public static bool TryGetDokzaAnimPath(DokzaAnimationState state, ProxyDirection direction, out string path)
		{
			return _dokzaAnimationSetDic.TryGetValue(Tuple.Create(state, direction.IsDown()), out path);
		}
	}
}