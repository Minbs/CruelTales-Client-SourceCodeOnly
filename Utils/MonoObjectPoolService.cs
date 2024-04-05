using System.Collections.Generic;
using UnityEngine;

namespace CTC.Utils
{
	public class MonoObjectPoolService
	{
		private Dictionary<GameObject, MonoObjectPool> _monoObjectPoolTable = new();
		private Dictionary<GameObject, MonoObjectPool> _monoInstancePoolTable = new();

		private Transform _baseTrasnform;

		public MonoObjectPoolService(Transform transform)
		{
			_baseTrasnform = transform;
		}

		public GameObject CreateObject(GameObject prefab)
		{
			return CreateObject(prefab, Vector3.zero, Quaternion.identity);
		}

		public GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			GameObject poolInstance;
			MonoObjectPool monoPool;

			if (!_monoObjectPoolTable.TryGetValue(prefab, out monoPool))
			{
				monoPool = new MonoObjectPool(prefab, _baseTrasnform);
				_monoObjectPoolTable.Add(prefab, monoPool);
			}

			poolInstance = _monoObjectPoolTable[prefab].Get(position, rotation);
			_monoInstancePoolTable.Add(poolInstance, monoPool);

			return poolInstance;
		}

		public void Release(GameObject instance)
		{
			if (_monoInstancePoolTable.ContainsKey(instance))
			{
				_monoInstancePoolTable[instance].Release(instance);
				_monoInstancePoolTable.Remove(instance);
				return;
			}

			if (!ReferenceEquals(instance, null))
			{
				Object.Destroy(instance);
			}
		}

		public void OnRegistered()
		{
		}

		public void OnUnregistered()
		{
			List<GameObject> destroyObjectList = new();

			foreach (var instance in _monoInstancePoolTable.Keys)
			{
				destroyObjectList.Add(instance);
			}

			foreach (var instance in destroyObjectList)
			{
				Release(instance);
			}
		}
	}
}
