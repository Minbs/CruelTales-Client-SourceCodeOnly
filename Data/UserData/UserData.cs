using CT.Common.Gameplay;
using CTC.Networks;

namespace CTC.Data
{
	public class UserData
	{
		public class UserInfoData
		{
			public string Username;
		}

		public class SkinSetData
		{
			public SkinSet SelectedSkinSet;

			public SkinSetData() {}
		}

		public class InputCacheData
		{
			public GameServerEndPoint ServerEndPoint = new();

			public InputCacheData() { }
		}

		public UserInfoData UserInfo = new();
		public InputCacheData InputCache = new();
		public SkinSetData SkinSet = new();

		public UserData() {}
	}
}
