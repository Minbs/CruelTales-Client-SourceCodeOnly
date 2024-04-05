using System.Collections.Generic;
using System;
using UnityEngine;

public static class UnityObjectExtensions
{
	public static bool IsNull(this UnityEngine.Object obj)
	{
		return ReferenceEquals(obj, null);
	}

	public static bool IsFakeNull(this UnityEngine.Object obj)
	{
		return !ReferenceEquals(obj, null) && obj;
	}

	public static bool IsAssigned(this UnityEngine.Object obj)
	{
		return obj;
	}

	public static bool IsMatch(this GameObject obj, string matchString)
	{
		return obj.name.ToLower().Contains(matchString.ToLower());
	}

	public static List<GameObject> FindGameObjects(this MonoBehaviour mono,
												   Predicate<GameObject> predicate)
	{
		List<GameObject> results = new();
		Queue<Transform> retrieve = new();
		retrieve.Enqueue(mono.transform);

		while (retrieve.TryDequeue(out var curTransform))
		{
			int childCount = curTransform.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				var curChild = curTransform.transform.GetChild(i);
				retrieve.Enqueue(curChild);
			}

			if (predicate(curTransform.gameObject))
			{
				results.Add(curTransform.gameObject);
			}
		}

		return results;
	}

	public static List<T> FindComponents<T>(this MonoBehaviour mono,
											Predicate<GameObject> predicate)
											where T : UnityEngine.Component
	{
		List<T> results = new();
		Queue<Transform> retrieve = new();
		retrieve.Enqueue(mono.transform);

		while (retrieve.TryDequeue(out var curTransform))
		{
			int childCount = curTransform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				var curChild = curTransform.GetChild(i);
				retrieve.Enqueue(curChild);
			}

			if (predicate(curTransform.gameObject))
			{
				results.AddRange(curTransform.GetComponents<T>());
			}
		}

		return results;
	}

	public static List<T> GetComponentList<T>(this MonoBehaviour mono)
		where T : UnityEngine.Component
	{
		List<T> results = new();
		Queue<Transform> retrieve = new();
		retrieve.Enqueue(mono.transform);

		while (retrieve.TryDequeue(out var curTransform))
		{
			int childCount = curTransform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				var curChild = curTransform.GetChild(i);
				retrieve.Enqueue(curChild);
			}

			results.AddRange(curTransform.GetComponents<T>());
		}

		return results;
	}

}