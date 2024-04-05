using CTC.DataBind.Contexts;
using CTC.Globalizations;
using CTC.SystemCore;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.StaticGUI.DialogPopup
{
	public class Context_SystemDialogPopup : ContextWithView<View_SystemDialogPopup>
	{
		private readonly Property<string> titleProperty = new Property<string>();
		public string Title
		{
			get => titleProperty.Value;
			set => titleProperty.Value = value;
		}

		private readonly Property<string> contentProperty = new Property<string>();
		public string Content
		{
			get => contentProperty.Value;
			set => contentProperty.Value = value;
		}

		public void GUI_OnClick_OK() => CurrentView.OnClick(DialogResult.OK);
		public void GUI_OnClick_Yes() => CurrentView.OnClick(DialogResult.Yes);
		public void GUI_OnClick_Apply() => CurrentView.OnClick(DialogResult.Apply);
		public void GUI_OnClick_No() => CurrentView.OnClick(DialogResult.No);
		public void GUI_OnClick_Cancel() => CurrentView.OnClick(DialogResult.Cancel);
	}
}
