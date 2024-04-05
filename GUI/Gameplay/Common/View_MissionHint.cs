using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CTC.GUI.Gameplay.Common
{
	public class View_MissionHint : ViewBase
	{
		public TextMeshProUGUI TitleText;
		public TextMeshProUGUI ContentText;
		public GameObject HintContentPrefab;
		public CanvasGroup HintCanvasGroup;

		/// <summary>
		/// 힌트 오브젝트 추가
		/// </summary>
		public void SetHintContent(string title, string content)
		{
			TitleText.text = title;
			ContentText.text = content;
		}

		public void FadeCanvasGroup(float duration)
		{
			HintCanvasGroup.DOFade(0, duration);
		}
	}
}
