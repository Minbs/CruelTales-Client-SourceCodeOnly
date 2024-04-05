using CT.Common.DataType;
using CTC.DataBind.Contexts;
using CTC.Networks;
using CTC.SystemCore;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.Gameplay
{
	public class Context_GameplayHUD : ContextWithView<View_GameplayHUD>
	{
		private readonly NetworkManager _networkManager;

		private readonly Property<string> connectInfoProperty = new();

		public string ConnectInfo
		{
			get => connectInfoProperty.Value;
			set => connectInfoProperty.Value = value;
		}

		public Context_GameplayHUD()
		{
			_networkManager = GlobalService.NetworkManager;
			SetConnectInfoText(UserSessionState.NoConnection);
			_networkManager.OnSessionStateChanged += SetConnectInfoText;
		}

		public void GUI_OnClick_Something()
		{

		}

		public void SetConnectInfoText(UserSessionState sessionState)
		{
			ConnectInfo = sessionState.GetText();
		}
	}
}