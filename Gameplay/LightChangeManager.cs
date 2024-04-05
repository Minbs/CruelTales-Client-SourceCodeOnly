using System;
using System.Collections;
using System.Collections.Generic;
using CT.Logger;
using CTC.Tests.Execution;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Gameplay
{
	public class LightChangeManager : MonoBehaviour
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(LightChangeManager));
		
		public Light[] LightArray;

		public AnimationCurve FadeInCurve;
		public AnimationCurve FadeOutCurve;
		
		private Light _curLight = null;
		[ShowInInspector]private Dictionary<Light, float> _lightIntensityDic = new Dictionary<Light, float>();
		private CoroutineRunner _lightRunner;
		
		public void Awake()
		{
			_lightRunner = new CoroutineRunner(this);

			if (ReferenceEquals(LightArray, null) || LightArray.Length <= 0)
				return;

			foreach (var VARIABLE in LightArray)
			{
				VARIABLE.gameObject.SetActive(true);
				_lightIntensityDic.Add(VARIABLE, VARIABLE.intensity);
				VARIABLE.intensity = 0f;
			}

			LightArray[0].intensity = _lightIntensityDic[LightArray[0]];
			_curLight = LightArray[0];
		}

		[Button]
		public void ChangeLightTo(int index)
		{
			if (ReferenceEquals(LightArray, null) || index < 0 || index >= LightArray.Length)
			{
				_log.Warn("Index OOR in LightChanger");
				return;
			}

			if (LightArray[index] == _curLight)
				return;
			
			_lightRunner.Start(changeLightEnumerator(LightArray[index]));
		}

		[Button]
		public void ChangeLightTo(Light light)
		{
			if (light == _curLight)
				return;
			
			if (!_lightIntensityDic.TryGetValue(light, out float intensity))
			{
				_log.Warn("Index OOR in LightChanger");
				return;
			}
			
			Debug.Log("LightChange");
			_lightRunner.Start(changeLightEnumerator(light));
		}

		private IEnumerator changeLightEnumerator(Light targetLight)
		{
			float lerpTimer = 0f;
			while (true)
			{
				_curLight.intensity = Mathf.Lerp(_lightIntensityDic[_curLight], 0f, FadeOutCurve.Evaluate(lerpTimer));
				targetLight.intensity = Mathf.Lerp(0f, _lightIntensityDic[targetLight], FadeInCurve.Evaluate(lerpTimer));
				
				lerpTimer += Time.deltaTime;
				
				if (lerpTimer >= 1f)
				{
					lerpTimer = 1f;
					_curLight.intensity = Mathf.Lerp(_lightIntensityDic[_curLight], 0f, lerpTimer);
					targetLight.intensity = Mathf.Lerp(0f, _lightIntensityDic[targetLight], lerpTimer);
					break;
				}
				yield return null;
			}

			_curLight = targetLight;
			yield break;
		}
	}
}