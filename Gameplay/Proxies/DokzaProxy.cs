using System;
using System.Collections.Generic;
using CT.Common.DataType.Input;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CT.Logger;
using CTC.Gameplay.Helpers;
using CTC.Networks.SyncObjects.SyncObjects;
using CTC.SystemCore;
using CTC.Tests;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace CTC.Gameplay.Proxies
{
	public class DokzaProxy : MonoBehaviour
	{
		// Publics
		[field: SerializeField]
		public SkeletonAnimation DokzaSkeletonAnimation { get; private set; }

		[ReadOnly, ShowInInspector]
		public DokzaAnimationState CurDokzaAnimState { get; private set; } = DokzaAnimationState.Idle;

		[ReadOnly, ShowInInspector]
		public ProxyDirection CurProxyDirection { get; private set; } = ProxyDirection.RightDown;
		
		[field: SerializeField] public PlayerCharacter _playerCharacter { get; private set; }

		[field: SerializeField] public DokzaSkinHandler _dokzaSkinHandler { get; private set; }

		// Privates
		private Spine.AnimationState _dokzaSpineState = null;
		private Transform _dokzaSkeletonTransform = null;
		private Action _spineEndAction = null;
		private Transform _boneFollowerTransform = null;
		
		// Debug
		private ILog _log = LogManager.GetLogger(typeof(DokzaProxy));

		// Animations
		private Vector3 _localScaleForFlip = Vector3.zero;

		private void Awake()
		{
			initActionByAnimStateDic();

			DokzaSkeletonAnimation.state.Event += handleSpineEvent;
			_dokzaSkeletonTransform = DokzaSkeletonAnimation.transform;
		}

		private void Start()
		{
			_dokzaSpineState = DokzaSkeletonAnimation.AnimationState;
			_dokzaSpineState.Complete += spineEndEvent;
		}

		private void OnDestroy()
		{
			if(!ReferenceEquals(_boneFollowerTransform, null))
				GlobalService.DokzaBoneFollowerManager.ReleaseBoneFollower(_boneFollowerTransform);
			
			_dokzaSpineState.Complete -= spineEndEvent;
		}

#if UNITY_EDITOR
		public void Reset()
		{
			DokzaSkeletonAnimation = GetComponent<SkeletonAnimation>();
		}
#endif

		/// <summary>
		/// 현재 진행되는 Spine Animation의 종료 시점에 실행할 Action을 일회성으로 추가합니다.
		/// </summary>
		/// <param name="action"></param>
		public void AddActionToSpineEndEvent(Action action)
		{
			_spineEndAction = action;
		}

		private void spineEndEvent(Spine.TrackEntry trackEntry)
		{
			/*
			TrackEntry _animBucket = _dokzaAnimationState.GetCurrent(0);
			if (_animBucket.AnimationTime < _animBucket.AnimationEnd)
				return;
			*/

			if (trackEntry.AnimationTime < trackEntry.AnimationEnd)
				return;

			if (ReferenceEquals(_spineEndAction, null))
				return;

			_spineEndAction();
			_spineEndAction = null;
		}

		[Button]
		public void UpdateAnimation(DokzaAnimationState animationState, bool loop)
		{
			if (ReferenceEquals(_playerCharacter, null))
			{
				if (GlobalService.DokzaSpineDataManager.
				    TryGetDokzaAnimPath(animationState, CurProxyDirection, out string path))
				{
					if (!ReferenceEquals(_boneFollowerTransform, null))
					{
						GlobalService.DokzaBoneFollowerManager.ReleaseBoneFollower(_boneFollowerTransform);
						_boneFollowerTransform = null;
					}
					
					_dokzaSkinHandler.ResetSkinToClearSkin();
					checkBoneSkin(animationState);
					
					_dokzaSpineState ??= DokzaSkeletonAnimation.AnimationState;
					_dokzaSpineState.SetAnimation(0, path, loop);
				}
				else
				{
					_log.Fatal("Dokza Proxy에서 정의되지 않은 Animation 호출됨");
				}	
			}
			else
			{
				if (GlobalService.DokzaSpineDataManager.
				    TryGetDokzaAnimPath(animationState, _playerCharacter.ProxyDirection, out string path))
				{
					_dokzaSpineState ??= DokzaSkeletonAnimation.AnimationState;
					_dokzaSpineState.SetAnimation(0, path, loop);
				}
				else
				{
					_log.Fatal("Dokza Proxy에서 정의되지 않은 Animation 호출됨");
				}	
			}
		}
		
		public void UpdateAnimation(DokzaAnimationState animationState, ProxyDirection proxyDirection, bool loop)
		{
			if (DokzaAnimationSet.TryGetDokzaAnimPath(animationState, _playerCharacter.ProxyDirection, out string path))
			{
				FlipDokza(_dokzaSkeletonTransform, _playerCharacter.ProxyDirection);
				
				_dokzaSpineState ??= DokzaSkeletonAnimation.AnimationState;
				_dokzaSpineState.SetAnimation(0, path, loop);
			}
			else
			{
				_log.Fatal("Dokza Proxy에서 정의되지 않은 Animation 호출됨");
			}	
		}

		/// <summary>
		/// 현재 재생중인 Animation의 Current Time을 받아옵니다.
		/// </summary>
		/// <returns></returns>
		public float GetCurrentAnimationTime()
		{
			return _dokzaSpineState.GetCurrent(0).AnimationTime;
		}

		/// <summary>
		/// 현재 재생중인 Animation의 TrackTime을 변경합니다.
		/// </summary>
		/// <param name="animationTime"></param>
		public void SetAnimationTime(float animationTime)
		{
			_dokzaSpineState.GetCurrent(0).TrackTime = animationTime;
		}

		/// <summary>
		/// Dokza의 좌우를 Scale 기반으로 뒤집습니다.
		/// </summary>
		/// <param name="dokzaTransform"></param>
		/// <param name="direction"></param>
		public void FlipDokza(Transform dokzaTransform, ProxyDirection direction)
		{
			_localScaleForFlip = dokzaTransform.localScale;
			_localScaleForFlip.x = direction switch
			{
				ProxyDirection.RightDown => Mathf.Abs(_localScaleForFlip.x),
				ProxyDirection.LeftDown => Mathf.Abs(_localScaleForFlip.x) * -1f,
				ProxyDirection.RightUp => Mathf.Abs(_localScaleForFlip.x),
				ProxyDirection.LeftUp => Mathf.Abs(_localScaleForFlip.x) * -1f,
				_ => Mathf.Abs(_localScaleForFlip.x)
			};

			dokzaTransform.localScale = _localScaleForFlip;
		}

		public void UpdateProxyDirection(ProxyDirection proxyDirection)
		{
			if (CurProxyDirection != proxyDirection)
			{
				CurProxyDirection = proxyDirection;
				
				if (DokzaAnimationDB.TryGetAnimationInfo(CurDokzaAnimState, out var info))
				{
					UpdateAnimation(CurDokzaAnimState, CurProxyDirection, info.IsLoop);
				}
			}
			else
			{
				CurProxyDirection = proxyDirection;
			}
		}

		/// <summary>
		/// (강제)이동하려는 방향으로 현재 DokzaDirection을 변경합니다.
		/// </summary>
		/// <param name="_moveDirection"></param>
		public void UpdateProxyDirection(Vector2 moveDirection)
		{
			if (moveDirection == Vector2.zero)
				return;

			UpdateProxyDirection(GetDokzaDirection(CurProxyDirection, moveDirection));
			//_playerCharacter.DirectChangeProxyDirection(GetDokzaDirection(_playerCharacter.CurProxyDirection, moveDirection));
		}

		/// <summary>
		/// 현재 DokzaDirection과 MoveDirection을 바탕으로 새로운 DokzaDirection을 Return합니다.
		/// </summary>
		/// <param name="_curDirection"></param>
		/// <param name="_moveDirection"></param>
		/// <returns></returns>
		public ProxyDirection GetDokzaDirection(ProxyDirection direction, Vector2 moveDirection)
		{
			bool _isRight = true;
			switch (moveDirection.x)
			{
				case < 0f:
					_isRight = false;
					break;

				case 0f:
					if (direction is ProxyDirection.LeftDown or ProxyDirection.LeftUp)
						_isRight = false;
					break;
			}

			bool _isDown = true;
			switch (moveDirection.y)
			{
				case > 0f:
					_isDown = false;
					break;

				case 0f:
					if (direction is ProxyDirection.LeftUp or ProxyDirection.RightUp)
						_isDown = false;
					break;
			}

			return _isRight switch
			{
				true when _isDown => ProxyDirection.RightDown,
				true => ProxyDirection.RightUp,
				_ => _isDown ? ProxyDirection.LeftDown : ProxyDirection.LeftUp
			};
		}

		private void checkBoneSkin(DokzaAnimationState animState)
		{
			if (GlobalService.DokzaSpineDataManager.TryGetBoneName(animState, out string bonePath))
			{
				_dokzaSkinHandler.SetSkinBone(animState);
				_boneFollowerTransform = GlobalService.DokzaBoneFollowerManager
					.SpawnBoneFollower(DokzaSkeletonAnimation, bonePath);
			}
		}
		
		
		#region ActionByDokzaAnimationState

		private void handleSpineEvent(TrackEntry trackentry, Event e)
		{
			if (_actionByAnimStateDic.TryGetValue(e.Data.Name, out Action action))
			{
				action?.Invoke();
			}	
		}
		
		private Dictionary<string, Action> _actionByAnimStateDic = new();
		private void initActionByAnimStateDic()
		{
			_actionByAnimStateDic.Add("Redhood_mission/Clean1_effect", Event_Redhood_mission_Clean1);
		}
		
		private void Event_Redhood_mission_Clean1()
		{
			GlobalService.EffectManager.SpawnEffect(EffectType.RedHood_Mission_Clean1, _boneFollowerTransform, 5f);
		}
		
		#endregion
		
	}
}