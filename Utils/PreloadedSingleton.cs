using UnityEngine;

namespace CTC.Utils
{
	public class PreloadedSingleton<T> : MonoBehaviour
		where T : PreloadedSingleton<T>
	{
		private static T mInstance;
		public static T Instance => mInstance;

		private void Awake()
		{
			if (mInstance == null)
			{
				mInstance = this as T;
				DontDestroyOnLoad(gameObject);
				OnAwake();
			}
			else
			{
				if (mInstance == this)
				{
					return;
				}

				Destroy(gameObject);
			}
		}

		public virtual void OnAwake() { }
	}
}