using System.Collections;
using CTC.MainMenu;
using CTC.Networks;
using CTC.Utils.Coroutines;
using TMPro;
using UnityEngine;

namespace CTC.GUI.StaticGUI.AsyncNetOperation
{
	public class View_AsyncNetOperation : ViewBase
	{
		[field: SerializeField]
		public TextMeshProUGUI LoadingText { get; private set; }

		[field: SerializeField]
		public TextMeshProUGUI LoadingDots { get; private set; }

		public AsyncOperationType NetworkOperationType { get; private set; }
		public string Message { get; private set; }

		private CoroutineRunner _loadingTextAnimation;

		public void Awake()
		{
			_loadingTextAnimation = new CoroutineRunner(this);
		}

		public void Initialize(AsyncOperationType operationType)
		{
			NetworkOperationType = operationType;
			Message = operationType.GetText();
			LoadingText.text = Message;
		}

		protected override void onBeginShow()
		{
			_loadingTextAnimation.Start(animationloadingText());
		}

		protected override void onBeginHide()
		{
			_loadingTextAnimation.Stop();
		}

		private IEnumerator animationloadingText()
		{
			LoadingDots.text = string.Empty;
			while (true)
			{
				for (int i = 0; i < 3; i++)
				{
					yield return CoroutineCache.GetWaitForSeconds(0.03f);
					LoadingDots.text += ".";
				}

				LoadingDots.text = string.Empty;
			}
		}
	}
}
