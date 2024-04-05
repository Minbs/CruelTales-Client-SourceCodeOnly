

using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CTC.GUI.Gameplay.Timer
{
	public class View_Timer : ViewBaseWithContext
	{
		private Context_Timer BindedContext;
		public float currentTimerCount;

		private void Start()
		{
			BindedContext = GetComponent<ContextHolder>().Context as Context_Timer;
			BindedContext.MaxTimerCount = currentTimerCount;
			BindedContext.CurrentTimerCount = BindedContext.MaxTimerCount;
		}

		// TODO: Update 대신 Coroutine, View 대신 컴포넌트로 만들기
		private void Update()
		{
			currentTimerCount -= Time.deltaTime;
			BindedContext.CurrentTimerCount = currentTimerCount;
		}
	}
}
