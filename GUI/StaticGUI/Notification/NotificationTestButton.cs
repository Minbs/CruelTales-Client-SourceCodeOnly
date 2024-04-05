using CTC.SystemCore;
using UnityEngine;

namespace CTC.GUI.StaticGUI.Notification
{
	public class NotificationTestButton : MonoBehaviour
	{
		public void SendCriticalNotification() => GlobalService.StaticGUI.SendNotification(NotificationType.Critical);
		public void SendWarningNotification() => GlobalService.StaticGUI.SendNotification(NotificationType.Warning);
		public void SendNormalNotification() => GlobalService.StaticGUI.SendNotification(NotificationType.None);
	}
}