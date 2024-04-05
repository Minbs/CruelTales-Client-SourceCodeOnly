using System;
using System.Collections;
using CT.Common.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CTC.Tests
{
	public enum EffectVisualType
	{
		None = 0,
		Sprite,
		SpriteAnimation,
		Spine,
		Particle
	}
	public class Test_Effect : MonoBehaviour
	{
		[field: SerializeField] public EffectVisualType EffectVisualType { get; protected set; }
		[field: SerializeField] public EffectType EffectType { get; protected set; }
		[field: SerializeField] public float EffectDuration { get; protected set; } = 0f;
		[field: SerializeField] public Animator Animator { get; protected set; }

		private EffectManager _effectManager = null;
		
		protected Coroutine _endTimerCoroutine = null;
		

		public void InitEffect(EffectManager effectManager)
		{
			_effectManager = effectManager;
		}
		
		/// <summary>
		/// SFX를 초기화합니다.
		/// </summary>
		[Button]
		public virtual void Reset(float duration)
		{
			EffectDuration = duration;
			
			if (!ReferenceEquals(_endTimerCoroutine, null))
			{
				StopCoroutine(_endTimerCoroutine);
				_endTimerCoroutine = null;
			}

			if (EffectDuration != 0f)
			{
				_endTimerCoroutine = StartCoroutine(endTimerEnumerator(EffectDuration));
			}
		}

		protected IEnumerator endTimerEnumerator(float duration)
		{
			float timer = 0f;
			while (true)
			{
				timer += Time.deltaTime;
				if (timer >= duration)
					break;
				
				yield return null;
			}
			
			_endTimerCoroutine = null;
			OnEnd();

			yield break;
		}
		
		protected void OnEnd()
		{
			transform.parent = _effectManager.transform;
			gameObject.SetActive(false);
		}
	}
}