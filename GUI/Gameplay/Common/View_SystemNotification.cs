using CTC.DataBind.Contexts;
using TMPro;
using UnityEngine;

namespace CTC.GUI.Gameplay.Common
{
	public class Context_SystemNotification : ContextWithView<View_SystemNotification>
	{ 
	
	}

	public class View_SystemNotification : ViewBaseWithContext
	{
		[SerializeField]
		private TextMeshProUGUI _notificationMessageText;

		protected override void onBeginShow()
		{
			_notificationMessageText.alpha = 0.0f;
		}

		public void Update()
		{
			if (_notificationMessageText.alpha > 0)
			{
				_notificationMessageText.alpha -= Time.deltaTime * 0.3f;
			}
		}

		public void OnSystemNotification(string notifyMessage)
		{
			_notificationMessageText.text = notifyMessage;
			_notificationMessageText.alpha = 1.0f;
		}
	}
}
