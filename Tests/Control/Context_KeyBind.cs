using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;




namespace CTC.Tests.Control
{
	public class Context_KeyBind : ContextWithView<View_KeyBind>
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

		public void GUI_OnClick_Gamepad()
		{
			CurrentView.OnClick_Gamepad();
		}

		public void GUI_OnClick_Keyboard()
		{
			CurrentView.OnClick_Keyboard();
		}

		public void GUI_OnClick_RebindAction()
		{
			CurrentView.OnClick_RebindAction();
		}

		public void GUI_OnClick_RebindInteraction()
		{
			CurrentView.OnClick_RebindInteraction();
		}
	}
}