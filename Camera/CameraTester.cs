using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTester : MonoBehaviour
{
	public float Angle = 45f;
	
	private CameraTransformer _screenInfo = new();
	private Camera _mainCamera;
	
    // Start is called before the first frame update
    void Start()
    {
	    _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
	    transform.localRotation = Quaternion.Euler(Angle, 0f, 0f);
	    _screenInfo.ScreenSize = new Vector2(Screen.width, Screen.height);
	    _screenInfo.Scale = _mainCamera.orthographicSize;
	    _screenInfo.Angle = transform.rotation.eulerAngles.x;
	    _mainCamera.projectionMatrix = _screenInfo.OrthoMatrix;
    }
}
