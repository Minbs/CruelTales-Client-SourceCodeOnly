using System.Collections.Generic;
using CTC.GUI.Components.Timer;
using UnityEngine;

namespace CTC.GUI.Gameplay.Lobby
{
	public class Item_GameStartTimer : MonoBehaviour
	{
		[SerializeField]
		private List<BaseTimer> _timers = new();

		public void StartTimer(float countdown)
		{
			foreach (var t in _timers)
			{
				t.Initialize(countdown);
				t.StartTimer();
			}
		}

		public void StopTimer()
		{
			foreach (var t in _timers)
			{
				t.StopTimer();
			}
		}
	}
}
