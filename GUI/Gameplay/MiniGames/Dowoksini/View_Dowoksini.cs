using CTC.GUI;
using CTC.GUI.Components.Timer;
using CTC.GUI.Gameplay.MiniGames.Dowoksini;
using Slash.Unity.DataBind.Core.Presentation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CTC.Gameplay.MiniGames.Dowoksini
{
	public class View_Dowoksini : ViewBaseWithContext
	{
		public SliderTimer MissionGaugeSlider;
		public CanvasGroup GoalGuideCanvasGroup;
		private Context_Dowoksini _context;
		public float timer;
		public Transform RedTeamTransform;
		public Transform BlueTeamTransform;
		//private List<>

		public void Start()
		{
			StartCoroutine(FadeGoalGuide(3));
			_context = GetComponent<ContextHolder>().Context as Context_Dowoksini;
			timer = 65;
			_context.RemainTime = timer;

			MissionGaugeSlider.Initialize(5, 0);
			MissionGaugeSlider.StartTimer();
		}

		private void Update()
		{
			timer -= Time.deltaTime;
			if(_context != null)
			_context.RemainTime = timer;
		}

		private IEnumerator FadeGoalGuide(float fadeDuration)
		{
			float fadeTimer = 0;

			while (fadeTimer < fadeDuration) 
			{
				fadeTimer += Time.deltaTime;
				float proceed = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);
				GoalGuideCanvasGroup.alpha = proceed;
				yield return null;
			}
		}
	}
}
