using CT.Common.DataType;
using CTC.Networks.SteamworksCore;
using CTC.SystemCore;

namespace CTC.Networks
{
	public class BackendManager : IManager
	{
		public UserId UserId { get; private set; }
		public NetString Username
		{
			get => GlobalService.UserDataManager.UserData.UserInfo.Username;
			set => GlobalService.UserDataManager.UserData.UserInfo.Username = value;
		}
		public UserToken UserToken;

		private static SteamManager _steamManager;

		public bool IsValid { get; private set; }

		public void Initialize()
		{
			UserId = new UserId((ulong)RandomHelper.NextInt(1000000));
			UserToken = new UserToken((ulong)RandomHelper.NextInt(100000));

			var userInfo = GlobalService.UserDataManager.UserData.UserInfo;
			if (string.IsNullOrEmpty(userInfo.Username))
			{
				userInfo.Username = $"User_{UserId}";
			}

			Username = userInfo.Username;

#if CT_PLATFORM_STEAM
			_steamManager = new(ProcessHandler.Instance.ID.AppID);
#endif

			IsValid = true;
		}

		public void Release()
		{
			IsValid = false;

#if CT_PLATFORM_STEAM
			_steamManager.Release();
#endif
		}
	}
}
