#nullable enable

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CTC.Utils.Coroutines
{
	/// <summary>
	/// 하나의 코루틴만 동작시키는 코루틴 래퍼 클래스입니다.
	/// MonoBehaviour의 Awake에서 new(this)로 할당합니다.
	/// OnDisable에서 Stop을 호출하면 안전하게 종료됩니다.
	/// </summary>
	public class CoroutineRunner
	{
		private MonoBehaviour _mono;
		private Coroutine? _coroutine;
		private Action? _onStartCallback;
		private Action? _onEndCallback;
		[AllowNull] private IEnumerator _action;

		public bool IsRunning { get; private set; } = false;

		public CoroutineRunner(MonoBehaviour mono)
		{
			_mono = mono;
		}

		/// <summary>
		/// 코루틴이 끝났을 때 호출할 함수를 바인딩합니다.
		/// 하나의 함수만 바인딩됩니다.
		/// </summary>
		/// <param name="onEndCallback">바인딩할 함수 입니다.</param>
		public void BindOnEndCallback(Action onEndCallback)
		{
			_onEndCallback = onEndCallback;
		}

		/// <summary>
		/// 코루틴이 시작될 때 호출할 함수를 바인딩합니다.
		/// 하나의 함수만 바인딩됩니다.
		/// </summary>
		/// <param name="onStartCallback">바인딩할 함수 입니다.</param>
		public void BindOnStartCallback(Action onStartCallback)
		{
			_onStartCallback = onStartCallback;
		}

		[Obsolete("Clear대신 Stop을 사용하세요.")]
		public void Clear() => Stop();

		/// <summary>코루틴을 시작합니다. 이전에 동작중인 코루틴이 있다면 정지되고 새로 시작합니다.</summary>
		/// <param name="action">코루틴 입니다.</param>
		public void Start(IEnumerator action)
		{
			// Stop coroutine before fresh start
			Stop();

			// Bind action
			_action = action;

			// Start coroutine if it's okay to run
			if (_mono.isActiveAndEnabled)
			{
				_coroutine = _mono.StartCoroutine(actionWrapper());
			}
		}

		/// <summary>코루틴을 정지합니다.</summary>
		public void Stop()
		{
			if (_coroutine != null)
			{
				_mono.StopCoroutine(_coroutine);
				_coroutine = null;
				if (IsRunning == true)
				{
					_onEndCallback?.Invoke();
					IsRunning = false;
				}
			}
		}

		private IEnumerator actionWrapper()
		{
			_onStartCallback?.Invoke();
			IsRunning = true;
			yield return _action;
			IsRunning = false;
			_coroutine = null;
			_onEndCallback?.Invoke();
		}
	}
}
