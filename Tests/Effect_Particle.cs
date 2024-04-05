

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CTC.Tests
{
	public class Effect_Particle : Test_Effect
	{
		public List<ParticleSystem> ParticleList;

		public override void Reset(float duration)
		{
			if (!ReferenceEquals(_endTimerCoroutine, null))
			{
				StopCoroutine(_endTimerCoroutine);
				_endTimerCoroutine = null;
			}

			foreach (var VARIABLE in ParticleList)
			{
				VARIABLE.Play();
			}
			
			_endTimerCoroutine = StartCoroutine(endTimerEnumerator(EffectDuration));
		}
	}
}