using System;
using CTC.GUI;
using CTC.Tests.Control;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


namespace CTC.Tests
{
	public class View_InGameUI : ViewBaseWithContext
	{
		//public Test_InputManager InputManager;
		public Test_KeyBindManager KeyBindManager;
		
		private const string LAYOUT_GROUP = "Layout Group";

		[SerializeField, TitleGroup(LAYOUT_GROUP)]
		private GameObject Layout_ControlInfo;


		private const string CONTROL_INFO = "Control Info";

		[SerializeField, TitleGroup(CONTROL_INFO)]
		private TextMeshProUGUI Text_ActionInfo;
		
		
		// ViewBaseWithContext
		public Context_InGameUI InGameUIContext;

		protected override void Awake()
		{
			this.InGameUIContext = this.CurrentContext as Context_InGameUI;
		}
		
		protected override void Start()
		{
			base.Awake();
		}

		protected override void OnEnable()
		{
			//InputManager.OnInputDeviceChangeAction += OnInputSchemeChange;
		}

		protected void OnDisable()
		{
			//InputManager.OnInputDeviceChangeAction -= OnInputSchemeChange;
		}

		public void OnInputSchemeChange()
		{
			//Text_ActionInfo.text = "Press " + KeyBindManager.GetKeyBindData(InputManager.CurControlScheme, KeyType.Action)._boundInputString + " to Action";
		}
	}
}