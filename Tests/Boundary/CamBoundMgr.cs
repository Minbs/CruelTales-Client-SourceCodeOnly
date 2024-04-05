using System;
using CTC.Gameplay;
using UnityEngine;

public class CamBoundMgr : MonoBehaviour
{
	public CamBound InitCamBound;
	public float DampLength = 0.7f;
	public float DampMin = 0f;
	
	
	private Transform CamTransform;
	private CamBound[] _camBoundArr;

	public bool IsFunctional { get; private set; } = false;
	public CamBound CurCamBound { get; private set; }

	private void Awake()
	{
		CamTransform = GameObject.FindObjectOfType<CameraManager>().transform;
		_camBoundArr = GameObject.FindObjectsOfType<CamBound>();

		foreach (var VARIABLE in _camBoundArr)
		{
			VARIABLE._camBoundMgr = this;
		}
	}

	private void Start()
	{
		if (InitCamBound)
		{
			IsFunctional = true;
			CurCamBound = InitCamBound;

			if (!InitCamBound.IsFunctional)
			{
				IsFunctional = false;
				Debug.Log("ERR : CamBoundMgr found that InitCamBound is not Functional");
			}
		}
		else
		{
			IsFunctional = false;
			Debug.Log("ERR : CamBoundMgr found that there is no InitCamBound");
		}
	}


	private int _camBoundIdx = -1;
	
	/*
	public Vector2 GetDamp(CamBound _curBound, Vector3 _camPos)
	{
		if (_camBoundIdx == -1)
		{
			for (int i = 0; i < _camBoundArr.Length; i++)
			{
				if (_camBoundArr[i] == _curBound)
				{
					_camBoundIdx = i;
				}
			}
		}
		else if(_camBoundArr[_camBoundIdx] != _curBound)
		{
			for (int i = 0; i < _camBoundArr.Length; i++)
			{
				if (_camBoundArr[i] == _curBound)
				{
					_camBoundIdx = i;
				}
			}
		}

		return _camBoundArr[_camBoundIdx].GetDampValue(_camPos);
	}
	*/

	public void ChangeCamBound(CamBound _cBound, Vector3 _resetPos)
	{
		CurCamBound = _cBound;
		IsFunctional = CurCamBound.IsFunctional;
		
		if (_resetPos != Vector3.zero)
		{
			CamTransform.position = 
				new Vector3(_resetPos.x, CamTransform.position.y, _resetPos.z);
		}
	}
}