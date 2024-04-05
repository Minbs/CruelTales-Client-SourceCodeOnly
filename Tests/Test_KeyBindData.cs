using System;
using CTC.SystemCore;
using UnityEngine;
using UnityEngine.InputSystem;


public class Test_KeyBindData
{
	public ControlScheme _controlScheme;
	public KeyType _keyType;
	public InputActionReference _actionRef;

	public string _boundInputString { get; private set; }
	public InputBinding _inputBinding { get; private set; }
	public int _bindIdx { get; private set; }
	

	// Constructor
	public Test_KeyBindData(ControlScheme _scheme, KeyType _type, InputActionReference _inputRef)
	{
		_controlScheme = _scheme;
		_keyType = _type;
		_actionRef = _inputRef;

		UpdateInputBindingnIdx();
		UpdateBoundInputString();
	}

	// Functions
	public void UpdateInputBindingnIdx()
	{
		string _schemeName = _controlScheme.ToString();
		
		for (int i = 0; i < _actionRef.action.bindings.Count; i++)
		{
			if (String.Equals(_actionRef.action.bindings[i].groups, _schemeName))
			{
				_inputBinding = _actionRef.action.bindings[i];
				_bindIdx = i;
			}
		}
	}

	public void UpdateBoundInputString()
	{
		_boundInputString = InputControlPath.ToHumanReadableString(
			_inputBinding.effectivePath,
			InputControlPath.HumanReadableStringOptions.OmitDevice
		);
	}
}