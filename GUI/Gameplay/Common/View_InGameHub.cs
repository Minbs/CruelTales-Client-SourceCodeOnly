using CTC.DataBind.Contexts;
using CTC.Globalizations;
using CTC.SystemCore;

namespace CTC.GUI.Gameplay.Common
{
	public class Context_InGameHub : ContextWithView<View_InGameHub>
	{
		public void GUI_OnClick_Leave()
		{
			CurrentView.OnClick_Leave();
		}
	}

	public class View_InGameHub : ViewBaseWithContext
	{
		public void OnClick_Leave()
		{
			string title = TextKey.Dialog_WantLeaveGame_Title.GetText();
			string content = TextKey.Dialog_WantLeaveGame_Content.GetText();
			GlobalService.StaticGUI
				.OpenSystemDialogPopup(isTemporary: true, title, content, responseCallback: (response) =>
				{
					if (response == DialogResult.Yes)
					{
						GlobalService.NetworkManager.Disconnect();
					}
				},
				DialogResult.Yes, DialogResult.No);
		}
	}
}
