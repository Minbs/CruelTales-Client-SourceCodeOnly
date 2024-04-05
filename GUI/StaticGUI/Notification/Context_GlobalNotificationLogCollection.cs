using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace CTC.GUI.StaticGUI.Notification
{
	public class Context_GlobalNotificationLogCollection : Context
	{
		private readonly Property<Collection<Context_GlobalNotificationLog>> _notificationsProperty =
			new Property<Collection<Context_GlobalNotificationLog>>(new Collection<Context_GlobalNotificationLog>());

		public Context_GlobalNotificationLogCollection()
		{
			this._notificationsProperty.Value.ItemAdded += this.OnNotificationAdded;
			this._notificationsProperty.Value.ItemInserted += this.OnNotificationInserted;
			this._notificationsProperty.Value.ItemRemoved += this.OnNotificationRemoved;
		}

		public Collection<Context_GlobalNotificationLog> Notifications
		{
			get
			{
				return this._notificationsProperty.Value;
			}
			set
			{
				this._notificationsProperty.Value = value;
			}
		}

		private void OnNotificationAdded(object item)
		{
			var notification = (Context_GlobalNotificationLog)item;
			notification.FadedOut += this.OnNotificationFadedOut;
			Debug.Log("추가됨");
		}

		private void OnNotificationInserted(object item, int index)
		{
			var notification = (Context_GlobalNotificationLog)item;
			notification.FadedOut += this.OnNotificationFadedOut;
		}

		private void OnNotificationRemoved(object item)
		{
			var notification = (Context_GlobalNotificationLog)item;
			notification.FadedOut -= this.OnNotificationFadedOut;
		}

		private void OnNotificationFadedOut(Context_GlobalNotificationLog notification)
		{
			this.Notifications.Remove(notification);
		}
	}
}