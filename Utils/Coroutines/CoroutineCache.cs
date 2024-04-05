using System.Collections.Generic;
using UnityEngine;

namespace CTC.Utils.Coroutines
{
	public static class CoroutineCache
	{
		private const int INITIAL_CACHE_SIZE = 64;
		private static Dictionary<float, WaitForSeconds> _waitForSecondsTable = new(INITIAL_CACHE_SIZE);
		private static Dictionary<float, WaitForSecondsRealtime> _waitForSecondsRealTimeTable = new(INITIAL_CACHE_SIZE);

		public static WaitForEndOfFrame WaitForEndOfFrame { get; private set; } = new();
		public static WaitForFixedUpdate WaitForFixedUpdate { get; private set; } = new();

		public static WaitForSeconds GetWaitForSeconds(float seconds)
		{
			if (_waitForSecondsTable.TryGetValue(seconds, out WaitForSeconds result))
			{
				return result;
			}

			var instruction = new WaitForSeconds(seconds);
			_waitForSecondsTable.Add(seconds, instruction);
			return instruction;
		}

		public static WaitForSecondsRealtime GetWaitForSecondsRealTime(float seconds)
		{
			if (_waitForSecondsRealTimeTable.TryGetValue(seconds, out WaitForSecondsRealtime result))
			{
				return result;
			}

			var instruction = new WaitForSecondsRealtime(seconds);
			_waitForSecondsRealTimeTable.Add(seconds, instruction);
			return instruction;
		}
	}
}
