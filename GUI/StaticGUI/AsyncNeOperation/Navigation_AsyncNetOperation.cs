using System.Collections.Generic;
using CTC.Networks;
using CT.Logger;

namespace CTC.GUI.StaticGUI.AsyncNetOperation
{
	public class Navigation_AsyncNetOperation : ViewNavigation
	{
		private readonly Dictionary<AsyncOperationType, ViewBase> _popupTable = new();

		public void OnAsyncOperationStarted(AsyncOperationType operationType)
		{
			if (_popupTable.ContainsKey(operationType))
			{
				this.LogError($"There is same operation popup exsist : {operationType.ToString()}");
				return;
			}

			var view = this.Push<View_AsyncNetOperation>();
			view.Initialize(operationType);
			_popupTable.Add(operationType, view);
		}

		public void OnAsyncOperationCompleted(AsyncOperationType operationType)
		{
			if (_popupTable.TryGetValue(operationType, out ViewBase view))
			{
				this.PopByObject(view.gameObject);
				_popupTable.Remove(operationType);
			}
		}
	}
}
