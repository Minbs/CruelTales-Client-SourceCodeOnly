using CTC.SystemCore;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace CTC.Tests.Tools
{
	public class SceneStarter : MonoBehaviour
	{
		public void Awake()
		{
			var scenecontrollers = FindObjectsOfType<SceneController>();
			foreach (var scene in scenecontrollers)
			{
				scene.Startup(null);
			}
		}
	}
}
