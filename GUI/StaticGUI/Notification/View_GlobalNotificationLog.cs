using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace CTC.GUI.StaticGUI.Notification
{
	public enum NotificationType
	{
		None = 0,
		Warning,
		Critical
	}

	public class View_GlobalNotificationLog : ViewBaseWithContext
	{
		private Context_GlobalNotificationLogCollection _notificationsContext;
		public GameObject NotificationPrefab;
		public RectTransform NotificationTransform; // 오브젝트 생성할 부모 오브젝트
		private Dictionary<Context_GlobalNotificationLog, GameObject> _notificationInstanceTable = new();
		protected override void onBeginShow()
		{
		}


		private void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			this._notificationsContext = new Context_GlobalNotificationLogCollection();
			this.ContextHolder.Context = this._notificationsContext;
		}

		public void GenerateNotification(string msg = "", NotificationType notificationType = NotificationType.None)
		{
			GameObject NotificationInstance = Instantiate(NotificationPrefab, NotificationTransform);
			var contextHolder = NotificationInstance.GetComponent<ContextHolder>();


			var notification = new Context_GlobalNotificationLog
			{
				NotificationText = msg,
				NotificationType = notificationType,
			};


			switch (notification.NotificationType)
			{
				case NotificationType.Critical:
					notification.TextColor = Color.red;
					break;
				case NotificationType.Warning:
					notification.TextColor = Color.yellow;
					break;
				case NotificationType.None:
					notification.TextColor = Color.white;
					break;
			}

			contextHolder.Context = notification;
			_notificationsContext.Notifications.Add(notification);
			//_notificationInstanceTable.TryAdd(notification, NotificationInstance);
		}

		public void GenerateNoti()
		{
			GenerateNotification();
		}
		/*
		[Button]
		public void DisableCriticalNotifications()
		{
			var criticalNotifications = _notificationInstanceTable.Where(s => s.Key.NotificationType.Equals(NotificationType.Critical)).Select(w => w.Value).ToList();
			foreach (var g in criticalNotifications)
			{

				g.SetActive(false);
			}
		}
		 */ 
	}
}