//using System;
//using CruelTales.GUI;
//using Legacy.CruelTales.Net;

//namespace Legacy.CruelTales.GUI.View.Gameplay
//{
//	[Obsolete("Legacy")]
//	public class View_LobbyInformation : ViewBaseWithContext
//	{
//		private NetworkManagerService mNetworkManagerService;
//		public Context_LobbyInformation BindedContext;

//		protected override void onBeginShow()
//		{
//			base.onBeginShow();

//			BindedContext = CurrentContext as Context_LobbyInformation;
//			BindedContext.Initialize();

//			mNetworkManagerService = GlobalService.GetOrNull<NetworkManagerService>();
//			mNetworkManagerService.OnMemberChanged += onLobbyMemberChanged;
//			onLobbyMemberChanged();
//		}

//		protected override void onAfterHide()
//		{
//			base.onAfterHide();

//			mNetworkManagerService.OnMemberChanged -= onLobbyMemberChanged;
//		}

//		private void onLobbyMemberChanged()
//		{
//			BindedContext.UserLabelItems.Clear();

//			//if (mNetworkManagerService.TryGetCurrentLobby(out var currentLobby))
//			//{
//			//	foreach (var f in currentLobby.Members)
//			//	{
//			//		BindedContext.UserLabelItems.Add(new Context_UserStateLabel(f.Id));
//			//	}
//			//}
//		}
//	}
//}
