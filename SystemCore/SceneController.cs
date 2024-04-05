using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CTC.SystemCore
{
	public abstract class SceneController : MonoBehaviour
	{
		public abstract UniTask Startup(Action callback);
		public abstract UniTask Release(Action callback);
	}
}