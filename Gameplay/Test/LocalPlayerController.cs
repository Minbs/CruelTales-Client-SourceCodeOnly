using CT.Common.Gameplay.Players;
using UnityEngine;

namespace CTC.Gameplay.Test
{

	public class LocalPlayerController : MonoBehaviour
	{
		public float MoveSpeed = 3.0f;
		public float WalkSpeed = 1.5f;

		private Vector3 _inputDirection = Vector3.zero;
		public Vector3 InputDirection
		{
			get => _inputDirection;
			set
			{
				if (_inputDirection == value)
					return;

				_inputDirection = value;
				_isStateChanged = true;
			}
		}

		private DokzaAnimationState _animationState = DokzaAnimationState.Idle;
		public DokzaAnimationState AnimationState
		{
			get => _animationState;
			set
			{
				if (_animationState == value)
					return;

				_animationState = value;
				_isStateChanged = true;
			}
		}

		private bool _isStateChanged = false;
		private bool _isWalk = false;

		public void Update()
		{
			updateInput();
			updateAnimationState();
			updateMovement();
			updateAnimation();
		}

		private void updateInput()
		{
			Vector3 inputDir = Vector3.zero;

			if (Input.GetKey(KeyCode.W))
			{
				inputDir += new Vector3(0, 0, 1);
			}
			if (Input.GetKey(KeyCode.S))
			{
				inputDir += new Vector3(0, 0, -1);
			}
			if (Input.GetKey(KeyCode.D))
			{
				inputDir += Vector3.right;
			}
			if (Input.GetKey(KeyCode.A))
			{
				inputDir += Vector3.left;
			}

			InputDirection = inputDir.normalized;
			_isWalk = Input.GetKey(KeyCode.LeftShift);
		}

		private void updateAnimationState()
		{
			float moveAmount = InputDirection.magnitude;
			if (moveAmount > 0)
			{
				AnimationState = _isWalk ? DokzaAnimationState.Walk : DokzaAnimationState.Run;
			}
			else
			{
				AnimationState = DokzaAnimationState.Idle;
			}
		}

		private void updateMovement()
		{
			Vector3 move = InputDirection * Time.deltaTime;
			if (_isWalk)
			{
				move *= WalkSpeed;
			}
			else
			{
				move *= MoveSpeed;
			}
			transform.position = transform.position + move;
		}

		public void updateAnimation()
		{
			if (_isStateChanged == false)
				return;

			Debug.Log(nameof(updateAnimation));

			_isStateChanged = false;
			//AnimationController.OnModelChanged(AnimationState, InputDirection);
		}
	}
}