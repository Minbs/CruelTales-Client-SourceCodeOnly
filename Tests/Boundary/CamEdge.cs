using System.Linq;
using CTC.Gameplay;
using UnityEngine;




public class CamEdge
{
	public Vector3 Left;
	public Vector3 Right;
	public Vector3 Up;
	public Vector3 Down;
	public Vector3 Center;

	public float DampXValue;
	public float DampZValue;

	private CameraManager _playerCam;
	
	public CamEdge(Vector3 _left, Vector3 _right, Vector3 _up, Vector3 _down)
	{
		Left = _left;
		Right = _right;
		Up = _up;
		Down = _down;

		Center = Left;
		Left.x += Right.x - Left.x;
	}

	public CamEdge(CameraManager _playerCamera)
	{
		_playerCam = _playerCamera;
		
		Left = Vector3.zero;
		Right = Vector3.zero;
		Up = Vector3.zero;
		Down = Vector3.zero;
		Center = Vector3.zero;
	}

	private bool _isDamp = false;
	/// <summary>
	/// Limit Arr을 받아서 좌표 보정과 댐핑값 산출을 개시합니다.
	/// </summary>
	/// <param name="_limitPosArr">왼쪽 오른쪽 위 아래 순서 배열</param>
	public void PosCompensation(ref Vector2[] _limitPosArr)
	{
		DampXValue = 1f;
		DampZValue = 1f;
			
		if (_limitPosArr.Length != 4)
		{
			Debug.LogError("ERR : CamEdge - PosCompensation Argument size is not 4");
			return;
		}

		for (int i = 0; i < _limitPosArr.Length; i++)
		{
			switch (i)
			{
				case 0:
					// Left
					if (Left.x < _limitPosArr[0].x + _playerCam.DampLength)
					{
						_isDamp = true;

						if (Left.x < _limitPosArr[0].x)
						{
							moveAllPosition(_limitPosArr[0].x - Left.x, 0f);
						}
						
						float result = _limitPosArr[0].x + _playerCam.DampLength - Left.x;
						DampXValue -= result / _playerCam.DampLength;
					}
					break;
				
				case 1:
					// Right
					if (Right.x > _limitPosArr[1].x - _playerCam.DampLength)
					{
						_isDamp = true;

						if (Right.x > _limitPosArr[1].x)
						{
							moveAllPosition(-(Right.x - _limitPosArr[1].x), 0f);
						}
						
						float result = Right.x - _limitPosArr[1].x - _playerCam.DampLength;
						DampXValue -= result / _playerCam.DampLength;
					}
					break;
				
				case 2:
					// Up
					if (Up.z > _limitPosArr[2].y - _playerCam.DampLength)
					{
						_isDamp = true;

						if (Up.z > _limitPosArr[2].y)
						{
							moveAllPosition(0f, -(Up.z - _limitPosArr[2].y));
						}
						
						float result = _limitPosArr[2].y - _playerCam.DampLength - Up.z;
						DampZValue -= result / _playerCam.DampLength;
					}
					break;
				
				case 3:
					// Down
					if (Down.z < _limitPosArr[3].y + _playerCam.DampLength)
					{
						_isDamp = true;

						if (Down.z < _limitPosArr[3].y)
						{
							moveAllPosition(0f, _limitPosArr[3].y - Down.z);
						}
						
						float result = Down.z - _limitPosArr[3].y - _playerCam.DampLength;
						DampZValue -= result / _playerCam.DampLength;
					}
					break;
			}
		}

		if (DampXValue < 0.1f)
			DampXValue = 0.1f;

		if (DampZValue < 0.1f)
			DampZValue = 0.1f;
	}
	
	
	private float _camAngle;
	private float _orthoSize;
	private float _aspect;
	
	private float _orthoRatio;
	private float _targetAngle;

	private float _xDistance;
	private float _zDistance;

	/// <summary>
	/// 현재 카메라가 보고 있는 월드의 영역을 새롭게 계산합니다.
	/// </summary>
	/// <param name="_centerPos"></param>
	/// <param name="_mainCamera"></param>
	public void OrganizePos(Vector3 _centerPos, Camera _mainCamera)
	{
		Center = _centerPos;
		Left = Center;
		Right = Center;
		Up = Center;
		Down = Center;

		if (_camAngle != _mainCamera.transform.eulerAngles.x ||
		    _orthoSize != _mainCamera.orthographicSize ||
		    _aspect != _mainCamera.aspect)
		{
			//Debug.Log("OrganizePos");
			
			_camAngle = _mainCamera.transform.eulerAngles.x;
			_orthoSize = _mainCamera.orthographicSize;
			_aspect = _mainCamera.aspect;
			
			_orthoRatio = Mathf.Cos(_camAngle * Mathf.Deg2Rad);
			_targetAngle = 90f - _camAngle;

			_xDistance = _orthoSize * _aspect;
			_zDistance = (_orthoSize * _orthoRatio) / Mathf.Cos(_targetAngle * Mathf.Deg2Rad);
		}

		Left.x -= _xDistance;
		Right.x += _xDistance;
		Up.z += _zDistance;
		Down.z -= _zDistance;
	}

	private void moveAllPosition(float _posX, float _posY)
	{
		Center.x += _posX;
		Center.z += _posY;
		
		Left.x += _posX;
		Left.z += _posY;

		Right.x += _posX;
		Right.z += _posY;

		Up.x += _posX;
		Up.z += _posY;

		Down.x += _posX;
		Down.z += _posY;
	}

	public void DebugRayEdge(Vector3 _direction)
	{
		Debug.DrawRay(Left, _direction * 100f, Color.green);
		Debug.DrawRay(Right, _direction * 100f, Color.green);
		Debug.DrawRay(Up, _direction * 100f, Color.green);
		Debug.DrawRay(Down, _direction * 100f, Color.green);
	}
}