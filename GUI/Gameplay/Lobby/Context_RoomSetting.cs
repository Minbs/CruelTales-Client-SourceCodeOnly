using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.Gameplay.Lobby
{
	public class Context_RoomSetting : ContextWithView<View_RoomSetting>
	{
		private readonly Property<string> roomNameProperty = new();
		public string RoomName
		{
			get => this.roomNameProperty.Value;
			set => this.roomNameProperty.Value = value;
		}

		private readonly Property<string> roomDiscriptionProperty = new();
		public string RoomDiscription
		{
			get => this.roomDiscriptionProperty.Value;
			set => this.roomDiscriptionProperty.Value = value;
		}

		private readonly Property<string> passwordProperty = new();
		public int Password
		{
			get => int.TryParse(passwordProperty.Value, out int value) ? value : -1;
			set => this.passwordProperty.Value = value.ToString();
		}

		private readonly Property<string> callBackMessageProperty = new();
		public string CallBackMessage
		{
			get => this.callBackMessageProperty.Value;
			set => this.callBackMessageProperty.Value = value;
		}

		public void GUI_OnClick_Apply()
		{
			this.CurrentView.OnClick_Apply(this);
		}

		public void GUI_OnClick_Cancel()
		{
			this.CurrentView.OnClick_Cancel();
		}
	}
}
