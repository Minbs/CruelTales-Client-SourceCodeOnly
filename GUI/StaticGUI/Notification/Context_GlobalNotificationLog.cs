using Slash.Unity.DataBind.Core.Data;
using System;
using UnityEngine;

namespace CTC.GUI.StaticGUI.Notification
{
	public class Context_GlobalNotificationLog : Context
	{
		private readonly Property<string> textProperty = new Property<string>();
		private readonly Property<NotificationType> notificationType = new Property<NotificationType>();
		private readonly Property<Color> textColor = new Property<Color>();
		public event Action<Context_GlobalNotificationLog> FadedOut;
		public string NotificationText
		{
			get
			{
				return this.textProperty.Value;
			}
			set
			{
				this.textProperty.Value = value;
			}
		}

		public NotificationType NotificationType
		{
			get
			{
				return this.notificationType.Value;
			}
			set
			{
				this.notificationType.Value = value;
			}
		}

		public Color TextColor
		{
			get
			{
				return this.textColor.Value;
			}
			set
			{
				this.textColor.Value = value;
			}
		}

		public void OnFadedOut()
		{
			var handler = this.FadedOut;
			if (handler != null)
			{
				handler(this);
			}
		}
	}
}