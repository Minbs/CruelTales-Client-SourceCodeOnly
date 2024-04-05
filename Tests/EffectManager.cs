using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CTC.SystemCore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CTC.Tests
{
	public class EffectManager : MonoBehaviour, IManager
	{
		public Test_Effect[] PoolingEffects;

		private int _poolSize = 10;
		private Test_Effect[] _pooledEffectArr;
		private Dictionary<EffectType, int> _pooledEffectIdx = new();
		private Dictionary<EffectType, int> _pooledLimitIdx = new();
		
		private void Awake()
		{
			if (PoolingEffects.Length <= 0)
			{
				Debug.LogError("Empty on PoolingEffectsArr");
				return;
			}
			
			_pooledEffectArr = new Test_Effect[PoolingEffects.Length * _poolSize];

			for (int i = 0; i < PoolingEffects.Length; i++)
			{
				for (int j = 0; j < _poolSize; j++)
				{
					Test_Effect createdEffect = GameObject.Instantiate(PoolingEffects[i], transform);
					createdEffect.InitEffect(this);
					_pooledEffectArr[i * _poolSize + j] = createdEffect;
					_pooledEffectArr[i * _poolSize + j].gameObject.SetActive(false);
				}
			}
		
			for (int i = 0; i < PoolingEffects.Length; i++)
			{
				_pooledEffectIdx.Add(PoolingEffects[i].EffectType, i * _poolSize);
				_pooledLimitIdx.Add(PoolingEffects[i].EffectType, (i + 1) * _poolSize);
			}
		}

		[Button]
		public void SpawnEffect(EffectType effectType, Transform parent, float duration)
		{
			if (_pooledEffectIdx.TryGetValue(effectType, out var effectIdx))
			{
				if(_pooledEffectArr[effectIdx].isActiveAndEnabled)
					_pooledEffectArr[effectIdx].gameObject.SetActive(false);
				
				_pooledEffectArr[effectIdx].gameObject.SetActive(true);
				_pooledEffectArr[effectIdx].transform.parent = parent;
				_pooledEffectArr[effectIdx].transform.localPosition = Vector3.zero;
				_pooledEffectArr[effectIdx].Reset(duration);

				_pooledEffectIdx[effectType]++;
				if (_pooledEffectIdx[effectType] >= _pooledLimitIdx[effectType])
				{
					_pooledEffectIdx[effectType] = _pooledLimitIdx[effectType] - _poolSize;
				}
			}
			else
			{
				Debug.LogError($"There is no effect such as : {effectType}");
			}
		}
		
		/// <summary>
		/// Effect를 원하는 위치에 소환합니다.
		/// </summary>
		/// <param name="effectType"></param>
		/// <param name="position"></param>
		/// <param name="action"></param>
		[Button]
		public void SpawnEffect(EffectType effectType, Vector2 position, float duration)
		{
			SpawnEffect(effectType, new Vector3(position.x, 0f, position.y), duration);
		}

		/// <summary>
		/// Effect를 원하는 위치에 소환합니다.
		/// </summary>
		/// <param name="effectType"></param>
		/// <param name="position"></param>
		/// <param name="action"></param>
		public void SpawnEffect(EffectType effectType, Vector3 position, float duration)
		{
			if (_pooledEffectIdx.TryGetValue(effectType, out var effectIdx))
			{
				if(_pooledEffectArr[effectIdx].isActiveAndEnabled)
					_pooledEffectArr[effectIdx].gameObject.SetActive(false);
				
				_pooledEffectArr[effectIdx].gameObject.SetActive(true);
				_pooledEffectArr[effectIdx].transform.parent = null;
				_pooledEffectArr[effectIdx].transform.position = position;
				_pooledEffectArr[effectIdx].Reset(duration);

				_pooledEffectIdx[effectType]++;
				if (_pooledEffectIdx[effectType] >= _pooledLimitIdx[effectType])
				{
					_pooledEffectIdx[effectType] = _pooledLimitIdx[effectType] - _poolSize;
				}
			}
			else
			{
				Debug.LogError($"There is no effect such as : {effectType}");
			}
		}

		public void Initialize()
		{
			
		}

		public void Release()
		{
			
		}
	}
}