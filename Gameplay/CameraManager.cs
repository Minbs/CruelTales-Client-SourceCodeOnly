#nullable enable

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CTC.Gameplay
{
	public enum CamMode
	{
		None = 0,
		NoTarget,
		TargetOnly,
		TargetnBoundary,
		SetTo,
		MoveTo,
		MoveTo_Instant,
	}
	
	public class CameraManager : MonoBehaviour
	{
		private const string PROPERTIES = "Properties";

		// References
		private GameplayManager? _gameplayManager = null;

		[field: SerializeField, TitleGroup("Reference")]
		[AllowNull] public Camera MainCamera { get; private set; }

		public Transform? Target { get; private set; } = null;

		// Properties
		[TitleGroup(PROPERTIES)] public float DampLength = 1f;
		[TitleGroup(PROPERTIES)] public float DampPower = 1f;
		public float FollowSpeed { get; private set; } = 10;

		private CameraTransformer _cameraTransforer = new();
		private CamBoundMgr _camBoundMgr = null;
		public CamEdge _camEdge { get; private set; }

		private bool _isCamBoundMgrExist = true;
		
		private Coroutine? _camShakeCoroutine = null;

		private CamMode _camMode = CamMode.None;
		// Boundary
		private Vector3 _targetPos = Vector3.zero;
		private Vector3 _curCamPos = Vector3.zero;
		private Vector3 _destCamPos = Vector3.zero;
		private Vector3 _lerpCamPos = Vector3.zero;
		private Vector3 _effectCamPos = Vector3.zero;

		private CoroutineRunner _orthoRunner;
		
		public void Awake()
		{
			_orthoRunner = new CoroutineRunner(this);
			
			_camBoundMgr = FindObjectOfType<CamBoundMgr>();
			_camEdge = new CamEdge(this);

			if (ReferenceEquals(_camBoundMgr, null))
				_isCamBoundMgrExist = false;
			
			updateCameraMatrix();
		}

		public void Start()
		{
			updateCameraMatrix();
		}

		public void Update()
		{
			calculateCamMode();

			switch (_camMode)
			{
				case CamMode.NoTarget:
					_curCamPos = transform.position;
					_destCamPos = _targetPos;
					_lerpCamPos = Vector3.Lerp(_curCamPos, _destCamPos, Time.deltaTime * FollowSpeed);
					break;
				
				case CamMode.TargetnBoundary:
					_targetPos = Target.position;
					_camEdge.OrganizePos(_targetPos, MainCamera);
					
					// 포지션 보정
					_camEdge.PosCompensation(ref _camBoundMgr.CurCamBound.GetLimitArr());
			
					_destCamPos = _camEdge.Center;
					//_camEdge.DebugRayEdge(MainCamera.transform.forward);

					_curCamPos = transform.position;
				
					//_lerpCamPos.x = Mathf.Lerp(_curCamPos.x, _destCamPos.x, Time.deltaTime * (CamSpeed - _camEdge.DampXValue * DampPower));
					//_lerpCamPos.z = Mathf.Lerp(_curCamPos.z, _destCamPos.z, Time.deltaTime * (CamSpeed - _camEdge.DampZValue * DampPower));

					_lerpCamPos = Vector3.Lerp(_curCamPos, _destCamPos,Time.deltaTime * (FollowSpeed - _camEdge.DampZValue * DampPower));
					_lerpCamPos.y = 0f;
					break;
				
				case CamMode.TargetOnly:
					_targetPos = Target.position;
					
					_curCamPos = transform.position;
					_destCamPos = _targetPos;
					_lerpCamPos = Vector3.Lerp(_curCamPos, _destCamPos, Time.deltaTime * FollowSpeed);
					break;
				
				case CamMode.SetTo:
					// LookAt 함수 호출중
					_curCamPos = transform.position;
					_destCamPos = _targetPos;
					_lerpCamPos = Vector3.Lerp(_curCamPos, _destCamPos, Time.deltaTime * FollowSpeed);
					
					if (!ReferenceEquals(_setToCoroutine, null))
						break;

					if (ReferenceEquals(Target, null))
						_camMode = CamMode.NoTarget;
					else if (!ReferenceEquals(Target, null) && _isCamBoundMgrExist)
						_camMode = CamMode.TargetnBoundary;
					else if (!ReferenceEquals(Target, null) && !_isCamBoundMgrExist)
						_camMode = CamMode.TargetOnly;
					break;
				
				case CamMode.MoveTo:
					_curCamPos = transform.position;
					_destCamPos = _targetPos;
					_lerpCamPos = Vector3.Lerp(_curCamPos, _destCamPos, Time.deltaTime * FollowSpeed);

					if (Vector2.Distance(_curCamPos, _lerpCamPos) <= 0.1f)
						_isMoveTo = false;
					break;
				
				case CamMode.MoveTo_Instant:
					_curCamPos = transform.position;
					_destCamPos = _targetPos;
					_lerpCamPos = Vector3.Lerp(_curCamPos, _destCamPos, Time.deltaTime * FollowSpeed);
					_camMode = CamMode.TargetOnly;
					break;
				
				case CamMode.None:
					Debug.LogError("CamMode is None");
					break;
			}
			
			transform.position = _lerpCamPos + _effectCamPos;
			updateCameraMatrix();
		}

		private void calculateCamMode()
		{
			if (_camMode == CamMode.MoveTo_Instant)
				return;
			
			if (_isMoveTo)
				_camMode = CamMode.MoveTo;
			else if (!ReferenceEquals(_setToCoroutine, null))
				_camMode = CamMode.SetTo;
			else if (ReferenceEquals(Target, null))
				_camMode = CamMode.NoTarget;
			else if (!ReferenceEquals(Target, null) && _isCamBoundMgrExist)
				_camMode = CamMode.TargetnBoundary;
			else if (!ReferenceEquals(Target, null) && !_isCamBoundMgrExist)
				_camMode = CamMode.TargetOnly;
			else
			{
				_camMode = CamMode.None;
			}
		}

		/// <summary>
		/// Camera의 Matrix를 갱신합니다.
		/// </summary>
		private void updateCameraMatrix()
		{
			_cameraTransforer.ScreenSize = new Vector2(Screen.width, Screen.height);
			_cameraTransforer.Scale = MainCamera.orthographicSize;
			_cameraTransforer.Angle = transform.rotation.eulerAngles.x;
			MainCamera.projectionMatrix = _cameraTransforer.OrthoMatrix;
		}

		[Button]
		public void ShakeCam(float shakePower, float shakeSpeed = 10f)
		{
			_effectCamPos = Vector3.zero;
			
			if (!ReferenceEquals(_camShakeCoroutine, null))
			{
				StopCoroutine(_camShakeCoroutine);
				_camShakeCoroutine = null;
			}

			shakePower /= 2f;
			_camShakeCoroutine = StartCoroutine(ShakeEnumerator(shakePower, shakeSpeed));
		}

		private IEnumerator ShakeEnumerator(float shakePower, float shakeSpeed)
		{
			Vector3 startPos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
			Vector3 endPos = -startPos;

			startPos *= shakePower;
			endPos *= shakePower;
			
			int _phase = 0;
			float _lerpVal = 0f;
			
			while (true)
			{
				switch (_phase)
				{
					case 0:
						_effectCamPos = Vector3.Lerp(startPos, endPos, _lerpVal);
						_lerpVal += Time.deltaTime * shakeSpeed;

						if (_lerpVal >= 1f)
						{
							_lerpVal = 0f;
							_phase = 1;
						}
						break;
					
					case 1:
						_effectCamPos = Vector3.Lerp(endPos, startPos, _lerpVal);
						_lerpVal += Time.deltaTime * shakeSpeed;

						if (_lerpVal >= 1f)
						{
							_lerpVal = 0f;
							_phase = 2;
						}
						break;
				}

				if (_phase == 2)
					break;
				
				yield return null;
			}
			
			_effectCamPos = Vector3.zero;
			_camShakeCoroutine = null;
			yield break;
		}
		
#if UNITY_EDITOR
		public void OnValidate()
		{
			//StartRepaintGameView();
		}
	
		private void StartRepaintGameView()
		{
		
			if (Application.isPlaying == false)
			{
				var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
				var repaintToolbar = toolbarType.GetMethod("RepaintToolbar",
					BindingFlags.NonPublic | BindingFlags.Static);

				CameraTransformer _tempScreenInfo = new();
				_tempScreenInfo.ScreenSize = new Vector2(Screen.width, Screen.height);
				_tempScreenInfo.Scale = MainCamera.orthographicSize;
				_tempScreenInfo.Angle = transform.rotation.eulerAngles.x;
				MainCamera.projectionMatrix = _tempScreenInfo.OrthoMatrix;
			
				repaintToolbar.Invoke(null, null);
			}
		}
#endif

		public void SetProperties(float followSpeed)
		{
			FollowSpeed = followSpeed;
		}

		public void SetProperties(float followSpeed, float dampLength, float dampPower)
		{
			FollowSpeed = followSpeed;
			DampLength = dampLength;
			DampPower = dampPower;
		}

		private Coroutine? _setToCoroutine = null;
		
		/// <summary>
		/// 해당 위치를 잠시동안 바라본 후 타겟으로 돌아옵니다.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="time"></param>
		[Button]
		public void LookAt(Vector2 position, float time)
		{
			_isMoveTo = false;
			
			if (!ReferenceEquals(_setToCoroutine, null))
			{
				StopCoroutine(_setToCoroutine);
				_setToCoroutine = null;
			}
			
			_setToCoroutine = StartCoroutine(setToEnumerator(time));
			_targetPos = new Vector3(position.x, 0f, position.y);
		}

		private IEnumerator setToEnumerator(float timer)
		{
			float _timer = 0f;
			while (true)
			{
				_timer += Time.deltaTime;

				if (_timer >= timer)
					break;
				
				yield return null;
			}
			
			_setToCoroutine = null;
			yield break;
		}


		private bool _isMoveTo = false;
		
		/// <summary>
		/// 카메라를 강제로 이동시킵니다.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="isInstant">즉시 해당 위치로 이동 여부</param>
		[Button]
		public void MoveTo(Vector2 position, bool isInstant = false)
		{
			if (isInstant)
			{
				_targetPos =  new Vector3(position.x, 0f, position.y);
				transform.position = new Vector3(position.x, 0f, position.y);
				_camMode = CamMode.MoveTo_Instant;
			}
			else
			{
				if (!ReferenceEquals(_setToCoroutine, null))
				{
					StopCoroutine(_setToCoroutine);
					_setToCoroutine = null;
				}

				_isMoveTo = true;
				_targetPos = new Vector3(position.x, 0f, position.y);
			}
		}


		[Button]
		public void SetZoomTarget(float zoom, bool instant = false)
		{
			if (instant)
				MainCamera.orthographicSize = zoom;
			else
			{
				_orthoRunner.Start(orthoEnumerator(zoom));
			}
		}

		private IEnumerator orthoEnumerator(float orthoValue)
		{
			while (true)
			{
				MainCamera.orthographicSize =
					Mathf.LerpUnclamped(MainCamera.orthographicSize, orthoValue, Time.deltaTime * 4f);

				if (Mathf.Abs(orthoValue - MainCamera.orthographicSize) <= 0.001f)
				{
					MainCamera.orthographicSize = orthoValue;
					break;
				}
				yield return null;
			}
		}
		
		
		[Button]
		public void ReleaseTarget(Transform target)
		{
			if (Target != target)
				return;

			Target = null;
		}

		[Button]
		public void BindTarget(Transform target)
		{
			Target = target;
		}

		public void ReleaseForce()
		{
			Target = null;
		}
	}
}