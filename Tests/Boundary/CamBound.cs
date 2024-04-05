using CTC.DebugTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CamBound : MonoBehaviour
{
	[HideInInspector]	// 나중에 private 할 것
	public CamBoundMgr _camBoundMgr;

	public bool IsFunctional { get; private set; } = false;
	private BoxGizmo _boxGizmo;
	private Vector4 _dampPos;
	
	/// <summary>
	/// 왼쪽, 오른쪽, 위, 아래
	/// </summary>
	private Vector2[] _limitPosArr = new Vector2[4];
	
	private void Awake()
	{
		if (this.TryGetComponent(out BoxGizmo _gizmo))
		{
			IsFunctional = true;
			Vector3 _pos = _gizmo.transform.position;
			float _xDistance = _gizmo.transform.localScale.x / 2f;
			float _zDistance = _gizmo.transform.localScale.z / 2f;

			_limitPosArr[0] = new Vector2(_pos.x - _xDistance, _pos.z);
			_limitPosArr[1] = new Vector2(_pos.x + _xDistance, _pos.z);
			_limitPosArr[2] = new Vector2(_pos.x, _pos.z + _zDistance);
			_limitPosArr[3] = new Vector2(_pos.x, _pos.z - _zDistance);
		}
		else
		{
			IsFunctional = false;
			Debug.Log("ERR : Cambound does not have BoxGizmo");
		}
	}

	[Button]
	public void ResetBound()
	{
		Awake();
	}

	public ref Vector2[] GetLimitArr()
	{
		return ref _limitPosArr;
	}
}