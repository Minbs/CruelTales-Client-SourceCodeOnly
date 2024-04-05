//using UnityEngine;
//using Slash.Unity.DataBind.Core.Data;
//using CruelTales.Net;
//using System.Threading.Tasks;
//using System;
//using CruelTales;

//namespace Legacy.CruelTales.GUI.Contexts.Gameplay
//{
//	[Obsolete("Legacy")]
//	public class Context_UserStateLabel : Context
//	{
//		private readonly Property<ulong> mSteamID = new();
//		private readonly Property<ulong> mFriend = new();
//		private readonly Property<Texture2D> mUserProfileAvatar = new();

//		public Context_UserStateLabel(ulong steamId)
//		{
//			mSteamID.Value = steamId;
//			mFriend.Value = steamId;
//			var task = SetupSteamProfileAvatar();
//		}

//		public async Task SetupSteamProfileAvatar()
//		{
//			mUserProfileAvatar.Value = await SteamUtilsExtension.GetTextureFromSteamIDAsync(mSteamID.Value);
//		}

//		public string Username => mFriend.Value.ToString();

//		public Texture2D UserProfileAvatar => mUserProfileAvatar.Value;

//		public void GUI_OnClick_OpenUserOverlay()
//		{
//			if (!GlobalService.SteamManager.IsValid)
//				return;

//			GlobalService.SteamManager.OpenUserOverlay(mSteamID.Value);
//		}
//	}
//}
