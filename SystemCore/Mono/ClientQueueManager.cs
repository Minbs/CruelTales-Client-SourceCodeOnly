#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CT.Common.DataType;
using CT.Common.DataType.Input;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Logger;
using CTC.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CTC.SystemCore
{
	public class ClientQueueManager : MonoBehaviour, IManager
	{
		private Queue<Tuple<Action, int, int>> _dataStreamQueue = new();
		
		public bool IsRunning { get; private set; } = false;
		
		public void RequestDataStreamToServer(Action startAction, int objID, int endValue)
		{
			_dataStreamQueue.Enqueue(
				new Tuple<Action, int, int>(startAction, objID, endValue)
				);
			
			if (!IsRunning)
			{
				_dataStreamQueue.Peek().Item1();
				IsRunning = true;
			}
		}

		public void GetDataFromServer(int objID, int value)
		{
			if (!IsRunning)
				return;

			if (_dataStreamQueue.Peek().Item2 == objID &&
			    _dataStreamQueue.Peek().Item3 == value)
			{
				_dataStreamQueue.Dequeue();

				if (_dataStreamQueue.Count > 0)
				{
					_dataStreamQueue.Peek().Item1();
				}
				else
				{
					IsRunning = false;
				}
			}
		}

		public void Initialize()
		{
			
		}

		public void Release()
		{
			
		}

		public void Update()
		{
			//Debug.LogError(_dataStreamQueue.Count);
		}
	}
}