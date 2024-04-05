using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CTC.DebugTools
{
	public class DebugCamera : MonoBehaviour
	{
		public float OrthoSize = 6;
		
		public Camera MainCamera { get; private set; }
		private CameraTransformer _cameraTransformer = new CameraTransformer();
		public Transform Target { get; set; }
		public float CameraSpeed = 10.0f;

		private void Awake()
		{
			MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			
			_cameraTransformer.ScreenSize = new Vector2(Screen.width, Screen.height);
			_cameraTransformer.Scale = MainCamera.orthographicSize;
			_cameraTransformer.Angle = transform.root.eulerAngles.x;
		}

		public void Update()
		{
			MainCamera.orthographicSize = OrthoSize;
			_cameraTransformer.ScreenSize = new Vector2(Screen.width, Screen.height);
			_cameraTransformer.Scale = MainCamera.orthographicSize;
			_cameraTransformer.Angle = transform.root.eulerAngles.x;
			MainCamera.projectionMatrix = _cameraTransformer.OrthoMatrix;

			Vector3 curPos = transform.position;
			Vector3 targetPosition = Target == null ? curPos : Target.position;
			Vector3 lerpPos = Vector3.Lerp(curPos, targetPosition,
										   CameraSpeed * Time.deltaTime);
			lerpPos = new Vector3(lerpPos.x, curPos.y, lerpPos.z);
			transform.position = lerpPos;
		}
		
#if UNITY_EDITOR
		public void OnValidate()
		{
			StartRepaintGameView();
		}
	
		[Button(ButtonSizes.Gigantic)]
		public void StartRepaintGameView()
		{
		
			if (Application.isPlaying == false)
			{
				var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
				var repaintToolbar = toolbarType.GetMethod("RepaintToolbar",
					BindingFlags.NonPublic | BindingFlags.Static);
				
				
				
				MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
				MainCamera.orthographicSize = OrthoSize;
				
				CameraTransformer _tempScreenInfo = new();
				_tempScreenInfo.ScreenSize = new Vector2(Handles.GetMainGameViewSize().x,
					Handles.GetMainGameViewSize().y);
				_tempScreenInfo.Scale = MainCamera.orthographicSize;
				_tempScreenInfo.Angle = transform.rotation.eulerAngles.x;
				MainCamera.projectionMatrix = _tempScreenInfo.OrthoMatrix;
				
				repaintToolbar.Invoke(null, null);
			}
		}
#endif
	}
}