using System;
using System.Collections.Generic;
using CT.Common.Tools.Collections;
using CTC.GUI;
using CTC.SystemCore;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


namespace CTC.Tests.Control
{
	public class View_KeyBind : ViewBaseWithContext
	{
		private const string LAYOUT_GROUP = "Layout Group";

		[SerializeField, TitleGroup(LAYOUT_GROUP)]
		private GameObject Layout_Gamepad;
		[SerializeField, TitleGroup(LAYOUT_GROUP)]
		private GameObject Layout_Keyboard;
		
		
		
		private const string BUTTON_GROUP = "Button Group";
		
		[SerializeField, TitleGroup(BUTTON_GROUP)]
		private GameObject Button_Gamepad;
		[SerializeField, TitleGroup(BUTTON_GROUP)]
		private GameObject Button_Keyboard;
		[SerializeField, TitleGroup(BUTTON_GROUP)]
		private GameObject Img_ButtonGamepad;
		[SerializeField, TitleGroup(BUTTON_GROUP)]
		private GameObject Img_ButtonKeyboard;

		
		
		private const string BINDUI_GROUP = "Bind UI";

		[SerializeField, TitleGroup(BINDUI_GROUP)]
		private GameObject WaitForBind;

		[SerializeField, TitleGroup(BINDUI_GROUP)]
		private TextMeshProUGUI WaitForBindTxt;

		

		private const string TEMP_ASSIGN = "Temp Assign";

		[SerializeField, TitleGroup(TEMP_ASSIGN)]
		private Test_KeyBindManager KeyBindManager;

		

		private const string KEYTYPES = "Key Types";

		[SerializeField, TitleGroup(KEYTYPES)] 
		private TextMeshProUGUI Action_Txt;

		[SerializeField, TitleGroup(KEYTYPES)] 
		private TextMeshProUGUI Interaction_Txt;


		public Navigation_KeyBind Navigation { get; private set; }
		public Context_KeyBind BindedContext;
		
		
		private ControlScheme _curScheme = ControlScheme.Gamepad;


		protected override void Awake()
		{
			base.Awake();
			this.BindedContext = this.CurrentContext as Context_KeyBind;
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_curScheme = ControlScheme.Gamepad;
			updateSelectedDevice();
			
			KeyBindManager.LoadAllBindings();
			updateKeyBindTxt();
		}
		
		public void Initialize()
		{
			
		}
		
		protected override void onBeginShow()
		{
			base.onBeginShow();
			Navigation = (Navigation_KeyBind)ParentNavigation;
		}
		

		public void OnClick_Gamepad()
		{
			_curScheme = ControlScheme.Gamepad;
			updateSelectedDevice();
			updateKeyBindTxt();
		}

		public void OnClick_Keyboard()
		{
			_curScheme = ControlScheme.Keyboard;
			updateSelectedDevice();
			updateKeyBindTxt();
		}

		private void updateSelectedDevice()
		{
			switch (_curScheme)
			{
				case ControlScheme.Keyboard:
					Img_ButtonGamepad.SetActive(false);
					Layout_Gamepad.SetActive(false);
					Img_ButtonKeyboard.SetActive(true);
					Layout_Keyboard.SetActive(true);
					break;
				
				case ControlScheme.Gamepad:
					Img_ButtonGamepad.SetActive(true);
					Layout_Gamepad.SetActive(true);
					Img_ButtonKeyboard.SetActive(false);
					Layout_Keyboard.SetActive(false);
					break;
			}
		}

		public void OnClick_RebindAction()
		{
			if (WaitForBind.gameObject.activeSelf)
				return;
			
			WaitForBind.SetActive(true);
			WaitForBindTxt.text = getWaitForBindTxt(KeyType.Action.ToString());
			
			KeyBindManager.StartRebinding(_curScheme, KeyType.Action, actionRebindComplete);
		}

		public void OnClick_RebindInteraction()
		{
			if (WaitForBind.gameObject.activeSelf)
				return;
			
			WaitForBind.SetActive(true);
			WaitForBindTxt.text = getWaitForBindTxt(KeyType.Interaction.ToString());
			
			KeyBindManager.StartRebinding(_curScheme, KeyType.Interaction, actionRebindComplete);
		}

		private void actionRebindComplete()
		{
			updateKeyBindTxt();
			WaitForBind.SetActive(false);
		}
		

		public void OnClick_Close()
		{
			ProcessHandler.Instance.StopProcess();
		}

		public void OnClick_Option()
		{
			// ? 왜 굳이 네비게이션 가서 함수를 실행
			Navigation.OpenOption();
		}

		private void updateKeyBindTxt()
		{
			Action_Txt.text = KeyBindManager.GetKeyBindData(_curScheme, KeyType.Action)._boundInputString;
			Interaction_Txt.text = KeyBindManager.GetKeyBindData(_curScheme, KeyType.Interaction)._boundInputString;
		}

		private string getWaitForBindTxt(string _keyName)
		{
			return $"Now waiting for ({_keyName})";
		}
	}
}