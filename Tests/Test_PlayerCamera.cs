using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CT.Common.Tools;
using CTC.Networks.SyncObjects.SyncObjects;
using UnityEditor;
using UnityEngine;

public class Test_PlayerCamera : MonoBehaviour
{
	public List<Transform> VisualizeList;

	private CamBoundMgr CamBoundManager;
	
	public float SideAngle = 0;
	public float AngleDestination = 45;
	public float AngleSpeed = 2.0f;
	public float AngleStart = 10.0f;
	public float CamSpeed = 3f;
	public float DampLength = 1f;
	public float DampPower = 1f;

	public Camera MainCamera;

	
	private PlayerCharacter DokzaController;

	private Transform _playerTransform;

	private CameraTransformer _screenInfo = new();
	private float _angleResult = 0;

	public CamEdge _camEdge { get; private set; }

	private Coroutine _startCamActionCoroutine = null;
	
	public void Awake()
	{
		// 개선 필요 - 렙디를 위한 좋은 오토 바인딩 방법이 있나?
		foreach (var VARIABLE in GameObject.FindObjectsOfType<PlayerCharacter>())
		{
			if (VARIABLE.name == "Entity_Player")
				DokzaController = VARIABLE;
		}
		CamBoundManager = GameObject.FindObjectOfType<CamBoundMgr>();
		
		
		
		//_camEdge = new CamEdge(this);
		
		_playerTransform = DokzaController.transform;

		_screenInfo.ScreenSize = new Vector2(Screen.width, Screen.height);
		_screenInfo.Scale = MainCamera.orthographicSize;
		_screenInfo.Angle = transform.rotation.eulerAngles.x;

		_startCamActionCoroutine = StartCoroutine(startCamAction());
	}
	
	private IEnumerator startCamAction()
	{
		float lerpVal = 0f;
		float angleInit = 10f;
		
		while (true)
		{
			if (lerpVal >= 1f)
			{
				transform.rotation = Quaternion.Euler(AngleDestination, SideAngle, 0);
				break;
			}
			
			_angleResult = Mathf.Lerp(angleInit, AngleDestination, lerpVal);
			transform.rotation = Quaternion.Euler(_angleResult, SideAngle, 0);

			lerpVal += Time.deltaTime * AngleSpeed;
			yield return null;
		}
	}
	
	public void OnEnable()
	{
		ResetCamera();
	}

	public void ResetCamera()
	{
		_angleResult = AngleStart;
	}


	private Vector3 _curCamPos = Vector3.zero;
	private Vector3 _destCamPos = Vector3.zero;
	private Vector3 _lerpCamPos = Vector3.zero;


	private Vector2 _dampValue;
	public void Update()
	{

		// Camera Reset
		if (Input.GetKeyDown(KeyCode.R))
			_startCamActionCoroutine = StartCoroutine(startCamAction());
		
		
		// 카메라 위치 계산
		_camEdge.OrganizePos(DokzaController.transform.position, MainCamera);


		if (CamBoundManager && CamBoundManager.IsFunctional)
		{
			// 포지션 보정
			_camEdge.PosCompensation(ref CamBoundManager.CurCamBound.GetLimitArr());
			
			_destCamPos = _camEdge.Center;
			_camEdge.DebugRayEdge(MainCamera.transform.forward);

			_curCamPos = transform.position;

			
			_lerpCamPos.x = Mathf.Lerp(_curCamPos.x, _destCamPos.x, Time.deltaTime * (CamSpeed - _camEdge.DampXValue * DampPower));
			_lerpCamPos.z = Mathf.Lerp(_curCamPos.z, _destCamPos.z, Time.deltaTime * (CamSpeed - _camEdge.DampZValue * DampPower));
			
			transform.position = _lerpCamPos;
		}
		else
		{
			transform.position = DokzaController.transform.position;
		}

		updateCameraMatrix();
	}
	
	private void updateCameraMatrix()
	{
		_screenInfo.Scale = MainCamera.orthographicSize;
		_screenInfo.Angle = transform.rotation.eulerAngles.x;
		MainCamera.projectionMatrix = _screenInfo.OrthoMatrix;
	}
	
	public void OnValidate()
	{
		StartRepaintGameView();
	}
	
	private void StartRepaintGameView()
	{
#if UNITY_EDITOR
		
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
		
#endif
	}
}
