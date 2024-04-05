using System.Collections;
using CTC.Utils.Coroutines;
using UnityEngine;

namespace CTC
{
	public class Test_CoroutineRunnerTester : MonoBehaviour
	{
		private CoroutineRunner _testRunner;
		public bool IsRunning;
		public string CurrentMethod;

		private void Awake()
		{
			_testRunner = new(this);
			_testRunner.BindOnStartCallback(() => { Debug.Log($"{nameof(_testRunner)} Start!"); });
			_testRunner.BindOnEndCallback(() => { Debug.Log($"{nameof(_testRunner)} End!"); });
		}

		private void OnDisable()
		{
			_testRunner.Stop();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				Debug.Log($"{nameof(_testRunner)} Click!");
				_testRunner.Start(action_a());
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
				Debug.Log($"{nameof(_testRunner)} Click!");
				_testRunner.Start(action_b());
			}

			IsRunning = _testRunner.IsRunning;
			if (IsRunning == false)
			{
				CurrentMethod = string.Empty;
			}
		}

		private IEnumerator action_a()
		{
			CurrentMethod = nameof(action_a);
			Debug.Log($"{nameof(action_a)} Start!");
			yield return CoroutineCache.GetWaitForSeconds(0.5f);
			Debug.Log($"{nameof(action_a)} End!");
		}

		private IEnumerator action_b()
		{
			CurrentMethod = nameof(action_b);
			Debug.Log($"{nameof(action_b)} Start!");
			yield return CoroutineCache.GetWaitForSeconds(1f);
			Debug.Log($"{nameof(action_b)} End!");
		}
	}
}
