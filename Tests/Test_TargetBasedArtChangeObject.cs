using System;
using System.Collections;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace CTC.Tests
{
	public class Test_TargetBasedArtChangeObject : MonoBehaviour
	{
		private Coroutine _artChangeCoroutine = null;
		public SpriteRenderer _renderer = null;
		public DirectionalLight? _directionalLight = null;

		[Button]
		public void LerpSpriteRenderer(float alphaValue)
		{
			if (ReferenceEquals(_renderer, null))
				return;
			
			if (!ReferenceEquals(_artChangeCoroutine, null))
			{
				StopCoroutine(_artChangeCoroutine);
				_artChangeCoroutine = null;
			}

			StartCoroutine(artChangeEnumerator(_renderer, alphaValue));
		}

		public void LerpDirectionalLight(Vector3 angle, float intensity, Color color)
		{
			if (ReferenceEquals(_directionalLight, null))
				return;
			
		}

		private IEnumerator artChangeEnumerator(SpriteRenderer renderer,
			float alphaValue)
		{
			Color startColor = renderer.color;
			Color endColor = startColor;
			endColor.a = alphaValue;

			float timer = 0f;
			
			while (true)
			{
				renderer.color = Color.Lerp(startColor, endColor, timer);
				timer += Time.deltaTime;

				if (timer >= 1f)
				{
					renderer.color = endColor;
					break;
				}
				yield return null;
			}

			_artChangeCoroutine = null;
			yield break;
		}
	}
}