using System.Collections.Generic;
using UnityEngine;

namespace CTC.Utils
{
	public class MonoObjectPool
	{
		public int Count => _objectStack.Count;
		private Stack<GameObject> _objectStack = new();
		private GameObject _referenceInstance;
		private Transform _baseTransform;

		public MonoObjectPool(GameObject referenceInstance,
							  Transform baseTransform)
		{
			_referenceInstance = referenceInstance;
			_baseTransform = baseTransform;
		}

		public GameObject Get()
		{
			return Get(Vector3.zero, Quaternion.identity);
		}

		public GameObject Get(Vector3 position, Quaternion rotation)
		{
			if (_objectStack.TryPop(out GameObject instance))
			{
				instance.SetActive(true);
				instance.transform.position = position;
				instance.transform.rotation = rotation;
				return instance;
			}

			var go = Object.Instantiate(_referenceInstance, position, rotation, _baseTransform);
			go.SetActive(true);
			return go;
		}

		public void Release(GameObject instance)
		{
			instance.SetActive(false);
			_objectStack.Push(instance);
		}

		public void Clear()
		{
			while (_objectStack.Count > 0)
			{
				var instance = _objectStack.Pop();
				Object.Destroy(instance);
			}
		}
	}
}