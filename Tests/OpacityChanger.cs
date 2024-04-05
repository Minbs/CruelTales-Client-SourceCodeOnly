using System;
using System.Collections;
using System.Collections.Generic;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Tests
{
	public class OpacityChanger : MonoBehaviour
	{
		public GameObject[] OpacityChangeObjs;
		public float Speed = 4f;
		
		// Privates
		private CoroutineRunner _fadeRunner;
		private List<SpineOpacityManipulator> _spineList = new();
		private List<SpriteRenderer> _spriteList = new();
		
		private void Awake()
		{
			_fadeRunner = new CoroutineRunner(this);
			
			foreach (var VARIABLE in OpacityChangeObjs)
			{
				if (VARIABLE.TryGetComponent(out SpineOpacityManipulator spineObj))
				{
					_spineList.Add(spineObj);
				}
				else if (VARIABLE.TryGetComponent(out SpriteRenderer spriteObj))
				{
					_spriteList.Add(spriteObj);
				}
			}
		}

		Color _spriteColor = Color.white;
		[Button]
		public void ChangeOpacity(float alphaValue)
		{
			if (alphaValue < 0f)
				alphaValue = 0f;
			else if (alphaValue > 1f)
				alphaValue = 1f;

			foreach (var VARIABLE in _spineList)
			{
				VARIABLE.ManipulateOpacity(alphaValue);
			}

			foreach (var VARIABLE in _spriteList)
			{
				_spriteColor = VARIABLE.color;
				_spriteColor.a = alphaValue;
				VARIABLE.color = _spriteColor;
			}
		}

		[Button]
		public void LerpOpacity(bool toFadeOut)
		{
			_fadeRunner.Start(fadeEnumerator(toFadeOut));
		}

		private IEnumerator fadeEnumerator(bool toFadeOut)
		{
			float startValue = toFadeOut ? 1f : 0f;
			float endValue = toFadeOut ? 0f : 1f;
			float lerpValue = 0f;
			
			while (true)
			{
				ChangeOpacity(Mathf.Lerp(startValue, endValue, lerpValue));

				lerpValue += Time.deltaTime * Speed;
				if (lerpValue >= 1f)
				{
					lerpValue = 1f;
					ChangeOpacity(Mathf.Lerp(startValue, endValue, lerpValue));
					break;
				}
				yield return null;
			}

			yield break;
		}
	}
}