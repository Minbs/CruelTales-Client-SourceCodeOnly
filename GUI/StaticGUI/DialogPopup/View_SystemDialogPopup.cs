using System;
using CT.Common.Tools.Collections;
using CTC.SystemCore;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.GUI.StaticGUI.DialogPopup
{
	public class View_SystemDialogPopup : ViewBaseWithContext
	{
		private const string TITLE_GROUP_BUTTON = "Button Group";

		[SerializeField, TitleGroup(TITLE_GROUP_BUTTON)]
		private GameObject Button_OK;
		[SerializeField, TitleGroup(TITLE_GROUP_BUTTON)]
		private GameObject Button_Yes;
		[SerializeField, TitleGroup(TITLE_GROUP_BUTTON)]
		private GameObject Button_No;
		[SerializeField, TitleGroup(TITLE_GROUP_BUTTON)]
		private GameObject Button_Apply;
		[SerializeField, TitleGroup(TITLE_GROUP_BUTTON)]
		private GameObject Button_Cancel;

		private BidirectionalMap<DialogResult, GameObject> _buttonByDialogResult;
		private Action<DialogResult> _callback;
		public bool IsTemporary { get; private set; }
		public Context_SystemDialogPopup BindedContext;

		protected override void Awake()
		{
			base.Awake();

			_buttonByDialogResult = new()
			{
				{ DialogResult.OK, Button_OK },
				{ DialogResult.Yes, Button_Yes },
				{ DialogResult.No, Button_No },
				{ DialogResult.Apply, Button_Apply },
				{ DialogResult.Cancel, Button_Cancel },
			};
		}

		public void Initialize(bool isTemporary, string title, string content,
							   Action<DialogResult> responseCallback,
							   params DialogResult[] dialogResults)
		{
			IsTemporary = isTemporary;
			BindedContext = CurrentContext as Context_SystemDialogPopup;
			BindedContext.Title = title;
			BindedContext.Content =	content;
			_callback = responseCallback;

			foreach (var go in _buttonByDialogResult.ForwardValues)
			{
				go.SetActive(false);
			}

			foreach (var d in dialogResults)
			{
				if (_buttonByDialogResult.TryGetValue(d, out var go))
				{
					go.SetActive(true);
				}
			}
		}

		public void OnClick(DialogResult dialogResult)
		{
			_callback?.Invoke(dialogResult);
			_callback = null;
			this.ParentNavigation.PopByObject(gameObject);
		}
	}
}
