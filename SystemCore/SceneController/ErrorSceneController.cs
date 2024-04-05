using System;
using CT.Logger;
using CTC.GUI;
using CTC.GUI.Errors;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CTC.SystemCore
{
	public class ErrorSceneController : SceneController
	{
		[field: SerializeField]
		public ViewNavigation Navigation { get; private set; }

		public override async UniTask Startup(Action callback)
		{
			await UniTask.Yield();

			this.LogError($"Error occur! : {GlobalService.ErrorManager.GetErrorMessage()}");
			Navigation.Push<View_ClientError>();

			callback?.Invoke();
		}

		public override async UniTask Release(Action callback)
		{
			await UniTask.Yield();

			callback?.Invoke();
		}
	}
}
