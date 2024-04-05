#nullable enable
#pragma warning disable CS0649

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType.Input;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CT.Logger;
using CTC.Gameplay;
using CTC.Gameplay.Proxies;
using CTC.GUI.Gameplay.Common.PlayerNameTag;
using CTC.Networks.Synchronizations;
using CTC.SystemCore;
using CTC.Utils;
using CTS.Instance.ClientShared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public enum ActionType : byte
	{
		Push = 0,
		Pushed,
	}

	public static class InteractionOrder
	{
		// 숫자가 높을 수록 우선순위가 높습니다.
		private readonly static Dictionary<Type, int> _orderByType = new()
		{
			{ typeof(FieldItem), 100 },
			{ typeof(Teleporter), 90 },
		};

		public static int GetOerder(Interactor interactor)
		{
			Type interactorType = interactor.GetType();
			if (_orderByType.TryGetValue(interactorType, out int order))
			{
				return order;
			}

			return 0;
		}
	}

	public partial class PlayerCharacter : RemoteNetworkObject, IInputSender
	{
		// Proxies
		[AllowNull] private Item_UserNameTag _usernameNameTag;
		[AllowNull] private FieldItemProxy _fieldItemProxy;

		// Reference
		public PlayerState? PlayerState { get; private set; }

		// Publics
		public DokzaSkinHandler DokzaSkinHandler;

		[field: SerializeField, Title("Assign")]
		[AllowNull]
		public DokzaProxy PlayerProxy { get; private set; }

		// Managers
		private NetworkManager? _networkManager;
		private GameplayManager? _gameplayManager;

		// Other Variables
		private ILog _log = LogManager.GetLogger(typeof(PlayerCharacter));

		private Interactor? _targetInteractor;

		public bool IsLocal
		{
			get
			{
				if (_networkManager == null)
					return false;

				return _networkManager.UserId == _userId;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			// Bind proxies
			_fieldItemProxy = GetComponentInChildren<FieldItemProxy>();
			_usernameNameTag = GetComponentInChildren<Item_UserNameTag>();

			// Bind sync events
			OnFieldItemChanged += onFieldItemChanged;
		}

		protected void Start()
		{
			//PlayerProxy.UpdateAnimation(DokzaAnimationState.Idle, ProxyDirection.RightDown, true);
		}

		public override void OnCreated()
		{
			_networkManager = GlobalService.NetworkManager;
			_gameplayManager = GlobalService.GameplayScene.GameplayManager;
			if (IsLocal)
			{
				GlobalService.InputManager.SetSender(this, _gameplayManager.WorldCamera.MainCamera);
			}

			// Sync Events
			onFieldItemChanged(FieldItem);

			// Set animations
			Server_OnAnimationChanged(_animationState, _proxyDirection);
			_serverPreviousPosition = this.RigidBody.Position.ToUnityVector3();

			if (!DokzaSkinHandler.IsSkinInitiated)
			{
				RequestTest(1);
			}

			if (GameplayController
					.RoomSessionManager
					.PlayerStateTable
					.TryGetValue(UserId, out var state))
			{
				PlayerState = state;
				PlayerState.OnCurrentCostumeChanged += onCurrentCostumeChanged;
				_usernameNameTag.Initialize(state.Username, state.Faction);
				onCurrentCostumeChanged(PlayerState.CurrentCostume);
			}
			else
			{
				// TODO : Set skin to default
			}

			GameplayController.SceneController?.OnPlayerCharacterCreated(this);
		}

		public override void OnDestroyed()
		{
			if (IsLocal)
			{
				GlobalService.InputManager.ReleaseSender();
			}

			if (PlayerState != null)
			{
				PlayerState.OnCurrentCostumeChanged -= onCurrentCostumeChanged;
			}

			GameplayController.SceneController?.OnPlayerCharacterDestroyed(this);
			Destroy(gameObject);
		}

		public override void OnUpdate(float deltaTime)
		{
			//updateAnimationState();
			//updateAnimation();
			//_curState.OnUpdate();

			if (!IsLocal)
				return;

			// Set target interactor
			_targetInteractor?.OnTarget(false);
			_targetInteractor = null;
			if (GameplayController.TryGetCollidedInteractorList(this, out var interactors))
			{
				foreach (var i in interactors)
				{
					if (_targetInteractor == null)
					{
						_targetInteractor = i;
						break;
					}
					
					int curOrder = InteractionOrder.GetOerder(i);
					int targetOrder = InteractionOrder.GetOerder(_targetInteractor);

					if (curOrder >= targetOrder)
					{
						_targetInteractor = i;
					}
				}
			}
			_targetInteractor?.OnTarget(true);

			if (Input.GetKeyDown(KeyCode.O))
			{
				Client_RequestTest(0);
				//Test_RequestSkinChange(true);
			}
			else if (Input.GetKeyDown(KeyCode.P))
			{
				Client_RequestTest(1);
				//Test_RequestSkinChange(false);
			}
		}

		private void onCurrentCostumeChanged(CostumeSet costumeSet)
		{
			//Debug.Log($"OnSkinChanged : {skinSet.Back},{skinSet.Cheek},{skinSet.Dress},{skinSet.Eyes},{skinSet.Eyebrow},{skinSet.FaceAcc},{skinSet.Hair},{skinSet.HairAcc}");
			//DokzaSkinHandler.ChangeSkinFromSQLDB(_serverSkinList.ToArray());
			DokzaSkinHandler.ApplySkin(costumeSet.GetSkinSet());
		}

		public Vector3 GetPosition() => transform.position;

		public void ProcessInput(InputData inputData)
		{
			// TESTING
			Client_RequestInput(inputData);
		}
		
		public void ProcessInteraction()
		{
			if (_targetInteractor != null)
			{
				_targetInteractor.Client_TryInteract();
				return;
			}

			if (FieldItem != FieldItemType.None)
			{
				Client_TryDropItem();
				return;
			}
		}

		public override void OnPhysicsEventChanged()
		{
#if UNITY_EDITOR
			Vector3 curVelocity = RigidBody.LinearVelocity.ToUnityVector3();
			Vector3 physicsPosition = RigidBody.Position.ToUnityVector3();
			DebugHelper.DrawArrow(physicsPosition, physicsPosition + curVelocity * 0.5f, Color.blue, 0.3f, 3.0f);
#endif
		}

		private Vector3 _clientPreviousPosition;
		public override void OnFixedUpdate(float stepTime)
		{
#if UNITY_EDITOR
			Vector3 curPos = RigidBody.Position.ToUnityVector3();
			Debug.DrawLine(curPos, curPos + Vector3.up * 0.3f, Color.magenta, 3.0f);
			DebugHelper.DrawArrow(_clientPreviousPosition, curPos, Color.magenta, 0.2f, 3.0f);
			_clientPreviousPosition = curPos;
#endif
		}

		#region Sync Events

		private void onFieldItemChanged(FieldItemType fieldItemType)
		{
			_fieldItemProxy.Initialize(fieldItemType);
		}

		#endregion

		#region Sync

		public virtual partial void Server_OnAnimationChanged(DokzaAnimationState state)
		{
			_animationState = state;

			if (!DokzaAnimationDB.TryGetAnimationInfo(state, out var info))
			{
				_log.Fatal($"There is no such state. State : {state}");
				return;
			}

			PlayerProxy.UpdateAnimation(_animationState, _proxyDirection, info.IsLoop);
		}

		public virtual partial void Server_OnAnimationChanged(DokzaAnimationState state, ProxyDirection direction)
		{
			_animationState = state;
			//Debug.Log(state +", "+ direction);
			_proxyDirection = direction;


			if (!DokzaAnimationDB.TryGetAnimationInfo(state, out var info))
			{
				_log.Fatal($"There is no such state. State : {state}");
				return;
			}

			PlayerProxy.UpdateAnimation(_animationState, _proxyDirection, info.IsLoop);
		}

		public virtual partial void Server_OnProxyDirectionChanged(ProxyDirection direction)
		{
			PlayerProxy.UpdateProxyDirection(direction);
		}

		public virtual partial void Server_OrderTest(int fromServer)
		{

		}

		private bool _isServerSendingData = false;
		private List<int> _serverSkinList = new();
		public virtual partial void Server_BroadcastOrderTest(int userId, int fromSever)
		{

		}

		public void Test_RequestSkinChange(bool _isNormal)
		{

		}

		private Vector3 _serverPreviousPosition;
		public virtual partial void Server_TestPositionTickByTick(System.Numerics.Vector2 curPosition)
		{
			Vector3 curPos = curPosition.ToUnityVector3();
			Debug.DrawLine(curPos, curPos + Vector3.up * 0.3f, Color.yellow, 3.0f);
			DebugHelper.DrawArrow(_serverPreviousPosition, curPos, Color.yellow, 0.2f, 3.0f);
			_serverPreviousPosition = curPos;
		}

		[Button]
		public void RequestTest(int fromClient)
		{

		}

		#endregion

		/*
		 Legacy Codes
				 
				 
		/*
		/// <summary>
		/// 밀치기 동작을 3D 방향으로 실시합니다.
		/// </summary>
		/// <param name="pushDirection"></param>
		public void StartPushCoroutine(Vector3 pushDirection)
		{
			if (!ReferenceEquals(_actionCoroutine, null))
			{
				StopCoroutine(_actionCoroutine);
				_actionCoroutine = null;
			}

			_actionCoroutine = StartCoroutine(pushEnumerator(_dokzaRigid,
				pushDirection, _dokzaPlayData.PushPower, _dokzaPlayData.PushDecreasePower));
		}
		
				 private IEnumerator pushEnumerator(Rigidbody _rigid, Vector3 _direction,
			float _power, float _decreasePower)
		{
			_pushCoroutineExitTrigger = false;

			while (true)
			{
				if (_pushCoroutineExitTrigger)
					break;

				if (_power >= 0f)
					_power -= Time.deltaTime * _decreasePower;
				else
					_power = 0f;

				_rigid.velocity = _direction * _power;

				yield return null;
			}

			_actionCoroutine = null;
			yield break;
		}

		public void ChangeFSM(DokzaAnimationState _inputState)
		{
			if (_fsmTable.TryGetValue(_inputState, out PlayerCharacterState state))
			{
				_curState.OnExit();
				_curState = state;
				_curState.OnStart();
			}
			else
			{
				_log.Fatal("There is no such" + _inputState);
			}
		}
				 
				 /// <summary>
		/// 현재 PlayerCharacter가 사용하는 DokzaPlayData의 획득을 시도합니다.
		/// </summary>
		/// <param name="playData"></param>
		/// <returns></returns>
		public bool TryGetDokzaPlayData(out DokzaPlayData? playData)
		{
			playData = _dokzaPlayData;
			return !ReferenceEquals(_dokzaPlayData, null);
		}
		 
		public bool TryActionMove(ActionType actionType, Vector3 direction)
		{
			if (_isNowAction)
				return false;

			switch (actionType)
			{
				case ActionType.Push:
					_animCont.OnModelChanged(DokzaAnimationState.Push, direction, 1);
					startActionCoroutine(pushEnumerator(_dokzaRigid, direction,
						_dokzaPlayData.PushPower, _dokzaPlayData.PushDecreasePower));
					break;

				case ActionType.Pushed:
					break;

				default:
					_log.Fatal($"There is no such case of {actionType}");
					return false;
					break;
			}
			return true;
		}
		
		private void startActionCoroutine(IEnumerator routine)
		{
			if (!ReferenceEquals(_actionCoroutine, null))
			{
				StopCoroutine(_actionCoroutine);
				_actionCoroutine = null;
			}

			_actionCoroutine = StartCoroutine(routine);
		}
		
		/// <summary>
		/// Push Coroutine을 제어하는 bool값의 변경을 시도합니다.
		/// </summary>
		/// <returns></returns>
		public bool TryExitPushCoroutine()
		{
			if (_pushCoroutineExitTrigger)
				return false;
			else
			{
				_pushCoroutineExitTrigger = true;
				return true;
			}
		}
		
		public void Server_UpdateInputDirection2D(Vector2 direction, bool isWalk)
		{
			_curMoveDirection = new Vector3(direction.x, 0f, direction.y);
			//_curState.OnInputEvent(InputEvent.MovementAxis, direction);
			//server_updateMoveDirection(new Vector3(direction.x, 0f, direction.y));
			//_isWalk = isWalk;
			//updateWalk(isWalk);
		}
		
		public Vector3 _curMoveDirection { get; private set; }= Vector3.zero;
		private bool _isRightBuffer = true;
		private bool _isFrontBuffer = false;
		private bool _isFront = true;
		private bool _isMoveChanged = false;
		// TODO : 임계 영역으로 애니메이션 트렌지션 만들기
		private void server_updateMoveDirection(Vector3 _direction)
		{
			_curMoveDirection = _direction;
			
			if (_dokzaDirection != GetDokzaDirection(_dokzaDirection, _curMoveDirection))
			{
				_dokzaDirection = GetDokzaDirection(_dokzaDirection, _curMoveDirection);
				_isMoveChanged = true;
			}
			else
				_isMoveChanged = false;

			updateRigid();
			//updateAnimationState();
			
			_moveDirectionBuffer = _moveDirection;
			_moveDirection = _direction;
			
			// 캐릭터 방향 우선 판정
			_isFrontBuffer = _isFront;
			_isFront = _isFront switch
			{
				true when _moveDirection.z > 0 => false,
				false when _moveDirection.z < 0 => true,
				_ => _isFront
			};
			
			_isRightBuffer = _isRight;
			_isRight = _isRight switch
			{
				true when _moveDirection.x < 0 => false,
				false when _moveDirection.x > 0 => true,
				_ => _isRight
			};
			
			// 입력값 변경 시에만 작동 - 캐릭터 4방향 변경 여부 판정
			if (_moveDirection != _moveDirectionBuffer)
			{
				// 움직임 반영
				updateMovement();
				
				_isMoveChanged = true;

				// 예외 처리
				// 우선 움직이는 도중이어야 함
				if (_moveDirectionBuffer == Vector3.zero || _moveDirection == Vector3.zero)
					return;
				
				switch (_isFrontBuffer)
				{
					case true when _isFront && _isRightBuffer && _isRight:
					case false when !_isFront && _isRightBuffer && _isRight:
					case true when _isFront && !_isRightBuffer && !_isRight:
					case false when !_isFront && !_isRightBuffer && !_isRight:
						_isMoveChanged = false;
						break;
				}
			}
			
		}
		
		/// <summary>
		/// 움직임을 계산해서 RigidBody에 반영합니다.
		/// </summary>
		/// <param name="force"></param>
		private void updateRigid(bool force = false)
		{
			switch (force)
			{
				case true:
				case false when !_isNowAction:
					_dokzaRigid.velocity =
						_curMoveDirection * (_isWalk ? _dokzaPlayData.WalkSpeed : _dokzaPlayData.MoveSpeed);
					break;
			}
		}
	
		/// <summary>
		/// 현재 상태에 맞게 애니메이션 상태를 최신화합니다.
		/// </summary>
		private void updateAnimationState(Vector3 _moveDirection)
		{
			var _tempState = _animationState;

			switch (_moveDirection.sqrMagnitude)
			{
				case > 0f:
					_tempState = _isWalk ? DokzaAnimationState.Walk : DokzaAnimationState.Run;

					updateAnimation();
					break;

				case 0f:
					_tempState = DokzaAnimationState.Idle;
					updateAnimation();
					break;

				default:
					_log.Fatal("_moveDirection의 sqrMagnitude가 음수입니다.");
					break;
			}

			_animationState = _tempState;
		}
		
		private float _spineTimeForSmoothChange = 0f;
		/// <summary>
		/// 애니메이션을 최신화합니다.
		/// </summary>
		private void updateAnimation()
		{
			if (!_isMoveChanged || _isNowAction)
				return;

			_isMoveChanged = false;
			_spineTimeForSmoothChange = _dokzaSpineAnimState.GetCurrent(0).AnimationTime;
			_animCont.OnModelChanged(_animationState, _curMoveDirection);
		}
	
		/// <summary>
		/// 강제로 애니메이션을 업데이트합니다.
		/// </summary>
		private void forceUpdateAnimation()
		{
			_animCont.OnModelChanged(_animationState, _curMoveDirection);
		}
			
		// Variables
		/// <summary>
		/// 현재 Dokza의 Action(강제 동작) 실시 여부를 확인합니다.
		/// </summary>
		private bool _isNowAction = false;
			
		/// <summary>
		/// 패턴 매칭 기반 Spine 이벤트 종료 판단
		/// </summary>
		/// <param name="_trackEntry"></param>
		private void spineEndEvent(Spine.TrackEntry _trackEntry)
		{
			_animationBucket = _dokzaSkeleton.state.GetCurrent(0);

			if (_animationBucket.AnimationTime < _animationBucket.AnimationEnd)
				return;
		
			if (!ReferenceEquals(_spineEndAction, null))
			{
				_spineEndAction();
				_spineEndAction = null;
			}
			
			switch (_dokzaSkeleton.AnimationName)
			{
				case { } actionFront when actionFront == DokzaAnimationData.Action.Front:
					if (_isNowAction)
					{
						_interactCol.EndActionMove();
						_isNowAction = false;
					}
					break;
			
				case { } actionBack when actionBack == DokzaAnimationData.Pushed.Front:
					if (_isNowAction)
					{
						_interactCol.EndActionMove();
						_isNowAction = false;
					}
					break;
			}
		}
		
		private bool _isWalkBuffer = false;
		private bool _isWalk = false;

		private void updateWalk(bool isWalk)
		{
			_isWalkBuffer = _isWalk;
			_isWalk = isWalk;
		}
		
	*/
	}
}
#pragma warning restore CS0649
