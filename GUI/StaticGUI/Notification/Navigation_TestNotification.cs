namespace CTC.GUI.StaticGUI.Notification
{
	public class Navigation_TestNotification : ViewNavigation
	{
		public void SendNotification(NotificationType notificationType)
		{
			if (TryGetTopBy<View_GlobalNotificationLog>(out var notiView))
			{
				notiView.GenerateNotification("message", notificationType);
			}
		}
	}
}
