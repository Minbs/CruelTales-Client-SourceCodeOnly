using System;
using System.Collections.Generic;
using CT.Common.Tools.Data;
using CT.Logger;
using CTC.SystemCore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyType
{
	Action = 0,
	Interaction
}

public class Test_KeyBindManager : MonoBehaviour
{
	// Log
	private static readonly ILog _log = LogManager.GetLogger(typeof(Test_KeyBindManager));

	// const
	private const string ACTIONMAP_MAIN = "Actions";
	private const string ACTIONMAP_REBIND = "Rebind";
	
	// Tests
	private const string INPUT_GROUP = "Input Group";
	private string _bindText_Action;

	[SerializeField] private InputActionReference _action = null;
	[SerializeField] private InputActionReference _interaction = null;

	[SerializeField] private PlayerInput playerInput = null;

	private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

	public string KeySettingPath => Global.Path.GetUserDataFilePath(Global.File.InputDataFileName +
																	Global.File.JsonExtension);


	public List<Test_KeyBindData> KeyBindList { get; private set; } = new();


	private void Awake()
	{
		// 반복해서 진행할 것
		
		// Gamepad
		KeyBindList.Add(new Test_KeyBindData(ControlScheme.Gamepad, KeyType.Action, _action));
		KeyBindList.Add(new Test_KeyBindData(ControlScheme.Gamepad, KeyType.Interaction, _interaction));
		
		// Keyboard
		KeyBindList.Add(new Test_KeyBindData(ControlScheme.Keyboard, KeyType.Action, _action));
		KeyBindList.Add(new Test_KeyBindData(ControlScheme.Keyboard, KeyType.Interaction, _interaction));
	}

	public Test_KeyBindData GetKeyBindData(ControlScheme _scheme, KeyType _keyType)
	{
		foreach (var VARIABLE in KeyBindList)
		{
			if (VARIABLE._controlScheme == _scheme && VARIABLE._keyType == _keyType)
				return VARIABLE;
		}

		return null;
	}
	
	
	[Button]
	public void SaveAllBindings()
	{
		string _allBindingJSON = playerInput.actions.SaveBindingOverridesAsJson();
		var result = FileHandler.TryWriteText(this.KeySettingPath, _allBindingJSON, makeDirectory: true);

		if (result.ResultType == JobResultType.Success)
		{
			_log.Info($"Key binding saved!");
		}
		else
		{
			_log.Error($"Key binding failed to save! {result.ResultType}");
		}
	}
	
	[Button]
	public void LoadAllBindings()
	{
		JobResult _readJSONResult = FileHandler.TryReadText(this.KeySettingPath);

		if (_readJSONResult.ResultType != JobResultType.Success)
		{
			_log.Error($"Load key bindings failed!");
			return;
		}
	
		playerInput.actions.LoadBindingOverridesFromJson(_readJSONResult.Value);

		foreach (var VARIABLE in KeyBindList)
		{
			VARIABLE.UpdateInputBindingnIdx();
			VARIABLE.UpdateBoundInputString();
		}
	}
	


	[Button]
	public void StartRebinding(ControlScheme _scheme, KeyType _type, Action _callBack = null)
	{
		playerInput.SwitchCurrentActionMap(ACTIONMAP_REBIND);

		var _targetKeyBindData = GetKeyBindData(_scheme, _type);

		rebindingOperation = 
			_targetKeyBindData._actionRef.action.PerformInteractiveRebinding(_targetKeyBindData._bindIdx)
			.OnMatchWaitForAnother(0.1f)
			.OnComplete(operation => rebindComplete(_scheme, _type, _callBack))
			.Start();
	}
	
	private void rebindComplete(ControlScheme _scheme, KeyType _type, Action _callBack = null)
	{
		rebindingOperation.Dispose();
		
		playerInput.SwitchCurrentActionMap(ACTIONMAP_MAIN);

		if (isOverlapBind(_scheme, _type))
		{
			_log.Info("Detected Bind Overlap");
			LoadAllBindings();
		}
		else
		{
			SaveAllBindings();
			LoadAllBindings();
		}
		
		_callBack?.Invoke();
	}

	private bool isOverlapBind(ControlScheme _scheme, KeyType _type)
	{
		foreach (var VARIABLE in KeyBindList)
		{
			VARIABLE.UpdateInputBindingnIdx();
			VARIABLE.UpdateBoundInputString();
		}

		var targetBoundInputString = GetKeyBindData(_scheme, _type)._boundInputString;
		foreach (var VARIABLE in KeyBindList)
		{
			if (VARIABLE != GetKeyBindData(_scheme, _type) &&
			    String.Equals(VARIABLE._boundInputString, targetBoundInputString))
			{
				return true;
			}
		}
		
		return false;
	}
}