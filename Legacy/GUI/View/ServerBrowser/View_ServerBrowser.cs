//using CruelTales.GUI;
//using CruelTales.GUI.Contexts.ServerBrowser;
//using CruelTales.Net;
//using Legacy.CruelTales.GUI.View;

//namespace CruelTales.GUI.View
//{
//	public class View_ServerBrowser : ViewBaseWithContext
//	{
//		public Context_ServerBrowser BindedContext { get; private set; }

//		protected override void onBeginShow()
//		{
//			base.onBeginShow();
//			this.BindedContext = this.CurrentContext as Context_ServerBrowser;
//		}

//		protected override void onAfterShow()
//		{
//			base.onAfterShow();
//			BindedContext.GUI_OnClick_RefreshServerList();
//		}

//		public void OnClick_ShowLobbyCreatePopup()
//		{
//			this.ParentNavigation.Push<View_CreateLobby>();
//		}
//	}
//}
