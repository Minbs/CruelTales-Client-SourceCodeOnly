using System;
using System.Collections;
using System.Collections.Generic;
using CT.Logger;
using CTC.Utils.Coroutines;
using UnityEngine;
using UnityEngine.Assertions;

namespace CTC.SystemCore
{
	public class SystemJobQueue
	{
		private static ILog _log = LogManager.GetLogger(typeof(SystemJobQueue));

		// Lock
		private readonly object _lock = new object();

		// Normal Job
		private Queue<SystemEventHandle> _jobQueue = new();

		public bool IsEmpty
		{
			get
			{
				lock (_lock)
				{
					return _jobQueue.Count == 0;
				}
			}
		}

		/// <summary>실행할 Job을 추가합니다.</summary>
		public void Push(SystemEventHandle job)
		{
			lock (_lock)
			{
				_jobQueue.Enqueue(job);
			}
		}

		/// <summary>Job을 수행합니다.</summary>
		public void Flush()
		{
			SystemEventHandle handle = null;

			lock (_lock)
			{
				if (_jobQueue.TryPeek(out var peekHandle))
				{
					if (peekHandle.State == SystemEventState.Awaiting)
					{
						handle = peekHandle;
					}
					else if (peekHandle.State == SystemEventState.Completed ||
							 peekHandle.State == SystemEventState.Canceled)
					{
						_jobQueue.Dequeue();
						return;
					}
				}
			}

			if (handle != null)
			{
				if (handle.State == SystemEventState.Awaiting)
				{
					handle.Execute();
				}
				else
				{
					_log.Warn($"Job execution failed! Job state : {handle.State}");
				}
			}
		}

		/// <summary>모든 Job을 삭제합니다.</summary>
		public void Clear()
		{
			lock (_lock)
			{
				_jobQueue.Clear();
			}
		}
	}

	public enum SystemEventState
	{
		None = 0,
		Awaiting,
		Running,
		Canceled,
		Completed,
	}

	public enum SystemEventResult
	{
		None,
		Completed,
		Canceled,
	}

	public class SystemEventHandle
	{
		private static ILog _log = LogManager.GetLogger(typeof(SystemEventHandle));

		private SystemJobQueue _jobQueue;
		private Action _systemEvent;
		private Action<SystemEventResult> _callback;
		public SystemEventState State { get; private set; }

		public static SystemEventHandle Create(SystemJobQueue jobQueue)
		{
			return new SystemEventHandle(jobQueue);
		}

		private SystemEventHandle(SystemJobQueue jobQueue)
		{
			_jobQueue = jobQueue;
		}

		/// <summary>
		/// 시스템 이벤트 실행을 예약합니다.
		/// 이벤트가 끝났다면 OnCompleted를 호출해야합니다.
		/// 이벤트를 취소했다면 Cancel을 호출해야합니다.
		/// </summary>
		/// <param name="systemEvent"></param>
		public void Push(Action systemEvent, Action<SystemEventResult> callback = null)
		{
			_systemEvent = systemEvent;
			_callback = callback;
			State = SystemEventState.Awaiting;
			_jobQueue.Push(this);
		}

		public void Execute()
		{
			if (State != SystemEventState.Awaiting)
			{
				_log.Fatal($"Wrong system event flow!");
				Assert.IsTrue(State != SystemEventState.Awaiting);
			}

			State = SystemEventState.Running;
			if (_systemEvent == null)
			{
				_log.Fatal($"There is no completed event!");
				Assert.IsTrue(_systemEvent == null);
			}
			_systemEvent();
		}

		public void OnCancel()
		{
			State = SystemEventState.Canceled;
			_callback?.Invoke(SystemEventResult.Canceled);
		}

		public void OnCompleted()
		{
			State = SystemEventState.Completed;
			_callback?.Invoke(SystemEventResult.Completed);
		}
	}

	public class SystemEventManager : MonoBehaviour, IManager, IFinalizable
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(SystemEventManager));

		private SystemJobQueue _systemJobQueue = new();
		private CoroutineRunner _runner;

		private void Awake()
		{
			_runner = new CoroutineRunner(this);
		}

		public void Initialize()
		{
			_runner.Start(flushTick());
		}

		public void Release()
		{
		}

		private IEnumerator flushTick()
		{
			while (true)
			{
				_systemJobQueue.Flush();
				yield return CoroutineCache.WaitForEndOfFrame;
			}
		}

		public SystemEventHandle CreateHandle()
		{
			return SystemEventHandle.Create(_systemJobQueue);
		}

		public bool IsFinalized()
		{
			return _systemJobQueue.IsEmpty;
		}
	}
}
