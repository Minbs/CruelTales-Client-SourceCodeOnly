#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Logger;
using CTC.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CTC.SystemCore
{
	/// <summary>입력 스키마입니다.</summary>
	public enum ControlScheme
	{
		Keyboard = 0,
		Gamepad
	}

	public interface IInputSender
	{
		public void ProcessInput(InputData inputData);
		public void ProcessInteraction();
		public Vector3 GetPosition();
	}

	public class InputManager : MonoBehaviour, IManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(InputManager));

		// Constant
		public const float INPUT_DETECT_THRESHOLD = 0.1f;
		public const float INPUT_RUN_THRESHOLD = 0.5f;
		public const float INPUT_ACTION_THRESHOLD = 0.5f;
		public const float INPUT_INTERACTION_THRESHOLD = 0.5f;
		public readonly string SchemeKeyboard = ControlScheme.Keyboard.ToString();
		public readonly string SchemeGamepad = ControlScheme.Gamepad.ToString();

		// Reference
		[SerializeField, AllowNull] private PlayerInput _playerInput;
		private Camera? _mainCamera;

		// Sender
		private IInputSender? _inputSender;
		public Vector3 SenderPosition =>
			_inputSender == null ? Vector2.zero : _inputSender.GetPosition();

		// Control scheme
		public ControlScheme CurControlScheme { get; private set; } = ControlScheme.Keyboard;

		// Input Events
		public event Action? OnEscape;
		//public event Action OnInputDeviceChangeAction = null;

		// Input Values
		private Vector2 _curActionDirection = Vector2.zero;
		private Vector2 _curMoveDirection = Vector2.zero;

		// For UI Raycasts
		private PointerEventData _eventData;
		private List<RaycastResult> _raycastResults;
		private EventSystem _curEventSystem;
		
		public void Initialize()
		{
			_eventData = new PointerEventData(EventSystem.current);
			_raycastResults = new List<RaycastResult>();
			_curEventSystem = EventSystem.current;
		}

		public void Release()
		{

		}

		#region Binding

		public void SetSender(IInputSender sender, Camera camera)
		{
			_inputSender = sender;
			_mainCamera = camera;
		}

		public void ReleaseSender()
		{
			_inputSender = null;
			_mainCamera = null;
		}

		#endregion

		#region Process Input

		public void Update()
		{
			raycastUI();
		}

		/// <summary>
		/// 현재 마우스가 UI 위에 있는지 여부를 알려줍니다.
		/// </summary>
		private bool _isHitUI = false;
		/// <summary>
		/// 마우스가 UI 위에 있는지 검출합니다.
		/// </summary>
		private void raycastUI()
		{
			_eventData.position = Input.mousePosition;
			_curEventSystem.RaycastAll(_eventData, _raycastResults);

			_isHitUI = _raycastResults.Count > 0 ? true : false;
		}
		
		private bool _isWalkBtnPressed = false;
		public void Input_WalkBtn(InputAction.CallbackContext context)
		{
			float inputPower = context.ReadValue<float>();

			_isWalkBtnPressed = inputPower != 0f;
			
			if(_curMoveDirection != Vector2.zero)
			{
				sendInputData(_isWalkBtnPressed ?
					MovementType.Walk : MovementType.Run, _curMoveDirection);
			}
		}

		private void sendInputData(MovementType movementType, Vector2 moveDirection)
		{
			if (_inputSender is null)
				return;
			
			InputData inputData = new();
			MovementInputData movementInputData = new();
			movementInputData.MoveInputType = movementType;
			movementInputData.MoveDirectionVector = moveDirection.ToNativeVector2();
			inputData.MovementInput = movementInputData;
			
			_inputSender?.ProcessInput(inputData);
		}
		
		public void Input_MovementAxis(InputAction.CallbackContext context)
		{
			Vector2 moveDirection = context.ReadValue<Vector2>();
			float movePower = moveDirection.sqrMagnitude;
			_curMoveDirection = moveDirection.normalized;

			if (CurControlScheme == ControlScheme.Gamepad)
			{
				if (movePower <= INPUT_DETECT_THRESHOLD)
				{
					sendInputData(MovementType.Stop, Vector2.zero);
				}
				else if (movePower <= INPUT_RUN_THRESHOLD)
				{
					sendInputData(MovementType.Walk, _curMoveDirection);
				}
				else
				{
					sendInputData(MovementType.Run, _curMoveDirection);
				}
			}
			else if (CurControlScheme == ControlScheme.Keyboard)
			{
				if (movePower <= INPUT_DETECT_THRESHOLD)
					sendInputData(MovementType.Stop, Vector2.zero);
				else
					sendInputData(_isWalkBtnPressed ? 
							MovementType.Walk : MovementType.Run, _curMoveDirection);
			}
		}

		/// <summary>조준 방향을 캐싱합니다.</summary>
		/// <param name="context"></param>
		public void InputActionAxis(InputAction.CallbackContext context)
		{
			_curActionDirection = context.ReadValue<Vector2>().normalized;
		}

		public void Input_ActionBtn(InputAction.CallbackContext context)
		{
			if (_inputSender is null)
				return;

			float actionPower = context.ReadValue<float>();
			Vector2 actionDirection;

			if (CurControlScheme == ControlScheme.Keyboard)
			{
				if (_mainCamera is null)
				{
					_log.Warn($"There is no camera");
					return;
				}

				// 키보드&마우스 사용 중일 때, UI에 마우스 히트 중이면 입력 무시
				if (_isHitUI)
					return;
				
				actionDirection = _mainCamera.ToMouseDirection(SenderPosition);
			}
			else if (CurControlScheme == ControlScheme.Gamepad)
			{
				if (actionPower < INPUT_ACTION_THRESHOLD)
					return;

				if (_curActionDirection == Vector2.zero)
				{
					if (_curMoveDirection == Vector2.zero)
					{
						return;
					}
					else
					{
						actionDirection = _curMoveDirection;
					}
				}
				else
				{
					actionDirection = _curActionDirection;
				}
			}
			else
			{
				return;
			}

			InputData inputData = new InputData();
			inputData.Type = InputType.Action;
			inputData.ActionDirection = new ByteDirection(actionDirection.ToNativeVector2());
			_inputSender?.ProcessInput(inputData);
		}

		public void Input_InteractionBtn(InputAction.CallbackContext _context)
		{
			if (_inputSender is null)
				return;

			float inputPower = _context.ReadValue<float>();
			if (inputPower < INPUT_INTERACTION_THRESHOLD)
				return;

			_inputSender.ProcessInteraction();
		}

		public void Input_DeviceChangeEvent()
		{
			switch (_playerInput.currentControlScheme)
			{
				case { } _scheme when String.Equals(_scheme, SchemeKeyboard):
					// Event when Keyboard.
					CurControlScheme = ControlScheme.Keyboard;
					break;

				case { } _scheme when String.Equals(_scheme, SchemeGamepad):
					// Event when GamePad.
					CurControlScheme = ControlScheme.Gamepad;
					break;
			}

			//OnInputDeviceChangeAction?.Invoke();
		}

		public void Input_EscapeBtn(InputAction.CallbackContext context)
		{
			//Debug.Log("Escaped");
			OnEscape?.Invoke();
		}

		#endregion
	}
}

#nullable disable