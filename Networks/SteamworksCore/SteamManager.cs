#define CT_PLATFORM_STEAM

using System;
using System.Collections.Generic;
using CT.Logger;
using CTC.SystemCore;
using Steamworks;
using Steamworks.Data;

namespace CTC.Networks.SteamworksCore
{
	public class SteamManager : IManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(SteamManager));

#if CT_PLATFORM_STEAM
		public SteamId ClientSteamID => IsValid ? SteamClient.SteamId : 0;
		public string ClientName;
#else
		public SteamId ClientSteamID => 0;
#endif

		public bool IsValid { get; private set; }
		public uint AppID { get; private set; }

		public SteamManager(uint appID)
		{
			AppID = appID;
		}

		public void Initialize()
		{
#if CT_PLATFORM_STEAM
			try
			{
				SteamClient.Init(AppID, true);
				IsValid = SteamClient.IsValid;

				if (!IsValid)
				{
					Random rand = new Random();
					ClientName = $"User_{rand.Next(1, 100)}";
					_log.Error("SteamClient is not valid! It's not initialized!");
					return;
				}

				ClientName = SteamClient.Name;

				_log.Info("SteamService is up and running!");
			}
			catch (Exception e)
			{
				_log.Error($"SteamService initialize faild!\n{e}");
				return;
			}
#else
			IsValid = true;
#endif
		}

		public void Release()
		{
#if CT_PLATFORM_STEAM
			try
			{
				SteamClient.Shutdown();
				_log.Info("SteamService ended!");
			}
			catch
			{
				_log.Error("SteamClient Shutdown Error");
			}
#endif
		}

		public bool TryGetFriendList(out IEnumerable<Friend> friends)
		{
#if CT_PLATFORM_STEAM
			friends = new List<Friend>();
			if (!IsValid)
			{
				return false;
			}

			friends = SteamFriends.GetFriends();
			return true;
#else
			friends = new List<Friend>();
			return true;
#endif
		}

		#region Operation

		public void OpenGameInviteOverlay(Lobby lobby)
		{
#if CT_PLATFORM_STEAM
			SteamFriends.OpenGameInviteOverlay(lobby.Id);
#else
#endif
		}

		public void OpenGameOverlay(OverlayType overlayType)
		{
#if CT_PLATFORM_STEAM
			if (overlayType == OverlayType.None)
				return;

			SteamFriends.OpenOverlay(overlayType.ToString());
#else
#endif
		}

		public void OpenUserOverlay(SteamId steamID)
		{
#if CT_PLATFORM_STEAM
			SteamFriends.OpenUserOverlay(steamID, OverlayType.SteamID.ToString());
#else
#endif
		}

		public void OpenShopPage()
		{
#if CT_PLATFORM_STEAM
			SteamFriends.OpenStoreOverlay(ProcessHandler.Instance.ID.AppID);
#else
#endif
		}

		#endregion
	}
}