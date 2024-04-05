using UnityEngine;

public class CameraTransformer
{
	private Vector2 _screenSize;
	/// <summary>Actual camera view size</summary>
	public Vector2 ScreenSize
	{
		get => _screenSize;
		set
		{
			if (value == _screenSize)
				return;

			_screenSize = value;
			if (_screenSize.x < 100 || _screenSize.y < 100)
			{
				_screenSize = new Vector2(100, 100);
			}

			Reset();
		}
	}

	private float _scale;
	/// <summary>Scale of camera</summary>
	public float Scale
	{
		get => _scale;
		set
		{
			if (value == _scale)
				return;

			_scale = Mathf.Max(value, 1);
			Reset();
		}
	}

	private float _angle;
	public float Angle
	{
		get => _angle;
		set
		{
			if (value == _angle)
			{
				return;
			}
			
			_angle = value;
			Reset();
		}
	}

	/// <summary>Ratio of screen</summary>
	public float ScreenRatio { get; private set; }

	/// <summary>Orthographic camera matrix</summary>
	public Matrix4x4 OrthoMatrix { get; private set; }

	public void Reset()
	{
		ScreenRatio = Mathf.Max(ScreenSize.x / ScreenSize.y, 0.1f);

		float transformRatio = Mathf.Cos(Mathf.Deg2Rad * _angle);

		var hw = ScreenRatio * _scale;
		var hh = _scale * transformRatio;

		OrthoMatrix = Matrix4x4.Ortho(-hw, hw, -hh, hh, 0.3f, 1000);
	}
}
