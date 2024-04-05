using UnityEngine;
using UnityEngine.Events;

namespace CTC.GUI.StaticGUI.Notification
{
	// TODO: 범용적으로 쓸 수 있는 컴포넌트로 만들기
	public class NotificationAnimationEvents : MonoBehaviour
	{
		public UnityEvent FadedOut;

		public void OnFadedOut()
		{
			this.FadedOut.Invoke();
			Destroy(gameObject);
			Debug.Log("dd");
		}
	}
}
