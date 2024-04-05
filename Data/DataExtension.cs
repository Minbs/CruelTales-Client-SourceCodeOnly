using Steamworks;

namespace CTC.Data
{
	public static class DataExtension
	{
		public static bool TryParse(string s, out SteamId result)
		{
			if (ulong.TryParse(s, out var value))
			{
				result = value;
				return true;
			}

			result = 0;
			return false;
		}
	}
}
