using CTC.SystemCore;
using TMPro;

namespace CTC.GUI.Errors
{
	public class View_ClientError : ViewBase
	{
		public TextMeshProUGUI ErrorText;
		protected override void onBeginShow()
		{
			string message = GlobalService.ErrorManager.GetErrorMessage();
			message = $"<size=20>{message}</size>";
			ErrorText.text += message;
		}

		public void OnClickStopProcess()
		{
			ProcessHandler.Instance.StopProcess();
		}
	}
}
