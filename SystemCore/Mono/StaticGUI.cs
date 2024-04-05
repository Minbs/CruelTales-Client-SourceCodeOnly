using System;
using CTC.GUI;
using CTC.GUI.StaticGUI.AsyncNetOperation;
using CTC.GUI.StaticGUI.DialogPopup;
using CTC.GUI.StaticGUI.Notification;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CTC.SystemCore
{
	public class StaticGUI : MonoBehaviour, IManager
	{
		[TitleGroup("Navigations"), SerializeField]
		private ViewNavigation _navStatic;

		[field: TitleGroup("Navigations"), SerializeField]
		public Navigation_AsyncNetOperation NavAsyncNetOperation { get; private set; }

		[field: TitleGroup("Navigations"), SerializeField]
		public Navigation_TestNotification NavNotification { get; private set; }

		public void Reset()
		{
			_navStatic = GetComponentInChildren<ViewNavigation>();
		}

		public void Initialize()
		{
			//GlobalService.InputManager.Action_Escape.canceled += onEscapePressed;
		}

		public void Release()
		{

		}

		public void CloseAllTemporaryViews()
		{
			// Close all system dialog popups
			_navStatic.PopMatch((v) =>
			{
				if (v.View is View_SystemDialogPopup dialogView)
				{
					return dialogView.IsTemporary;
				}

				return false;
			});
		}

		/// <summary>시스템 Dialog를 엽니다.</summary>
		/// <param name="isTemporary">
		/// 임시 Dialog인지 여부입니다. true인 경우 다른 이벤트가 발생하면 꺼집니다.
		/// false인 경우 다른 이벤트가 발생해도 그대로 떠있습니다.
		/// </param>
		/// <param name="title">제목 문자열입니다.</param>
		/// <param name="content">내용 문자열입니다.</param>
		/// <param name="responseCallback">사용자가 선택한 Dialog의 결과 콜백입니다.</param>
		/// <param name="dialogResults">사용자에게 보여줄 Dialog 버튼들입니다.</param>
		public void OpenSystemDialogPopup(bool isTemporary, string title, string content,
										  Action<DialogResult> responseCallback,
										  params DialogResult[] dialogResults)
		{
			var popup = _navStatic.Push<View_SystemDialogPopup>();
			popup.Initialize(isTemporary, title, content, responseCallback, dialogResults);
		}

		/// <summary>
		/// Escape 키가 눌렸을 때 호출됩니다.
		/// 활성화되어있는 자식 View가 없다면 최상단 View를 제거합니다.
		/// </summary>
		private void onEscapePressed(InputAction.CallbackContext _)
		{
			if (NavNotification.HasAnyView)
			{
				NavNotification.Clear();
			}
			else
			{
				NavNotification.Push<View_GlobalNotificationLog>();
			}

			return;

			//if (mNavigation.TryFind<View_EscapeMenu>(out var view))
			//{
			//	if (view.HasChildView)
			//	{
			//		return;
			//	}
			//	mNavigation.Pop<View_EscapeMenu>();
			//}
			//else
			//{
			//	mNavigation.Push<View_EscapeMenu>();
			//}
		}

		public void SendNotification(NotificationType notificationType)
		{
			NavNotification.SendNotification(notificationType);
		}
	}
}
