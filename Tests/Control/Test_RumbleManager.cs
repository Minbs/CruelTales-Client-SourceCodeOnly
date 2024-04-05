using System.Collections;
using System.Collections.Generic;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem;


namespace CTC.Tests.Control
{
	public class Test_RumbleManager : MonoBehaviour
	{
		public SkeletonAnimation DokzaSkeleton;
		public Data_RumblePattern RumblePattern;
	
		private CoroutineRunner rumbleCoroutine;

		private void Awake()
		{
			rumbleCoroutine = new CoroutineRunner(this);
		}

		[Button]
		public void StopRumble()
		{
			rumbleCoroutine.Stop();
			Gamepad.current.SetMotorSpeeds(0f,0f);
		}
	
		[Button]
		public void ExecuteRumble()
		{
			StopRumble();

			DokzaSkeleton.state.SetAnimation(0, RumblePattern.BasedAnimation,RumblePattern.IsLoop);
			rumbleCoroutine.Start(rumbleEnumerator(DokzaSkeleton, RumblePattern.RumbleCurve, RumblePattern.RumbleCurve));
		}
	

		private IEnumerator rumbleEnumerator(SkeletonAnimation skeleton, AnimationCurve lCurve, AnimationCurve rCurve)
		{
			float _curanimTime = 0f;
			float _bufanimTime = 0f;
		
			float _timer = 0f;
			float _endTime = skeleton.state.GetCurrent(0).AnimationEnd;

			while (true)
			{
				yield return null;
			
				_bufanimTime = _curanimTime;
				_curanimTime = skeleton.state.GetCurrent(0).AnimationTime;

				if (_curanimTime == _bufanimTime)
					break;
			
				Gamepad.current.SetMotorSpeeds(
					lCurve.Evaluate(_curanimTime / _endTime), 
					rCurve.Evaluate(_curanimTime / _endTime)
				);
			}

			Debug.Log("Rumble ENded");
			StopRumble();
		
			yield break;
		}
	}
}