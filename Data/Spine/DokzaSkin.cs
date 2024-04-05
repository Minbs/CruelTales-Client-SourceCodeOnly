using System.Collections.Generic;

namespace CTC.Data.Spine
{
	public enum DokzaSkin
	{
		None = 0,
		default_,
		Test,
		wolf,
	}

	public static class DokzaSkinExtension
	{
		private static Dictionary<DokzaSkin, string> mDokzaSkinTable = new()
		{
			{ DokzaSkin.None, "none" },
			{ DokzaSkin.default_, "default" },
			{ DokzaSkin.Test, "Test" },
			{ DokzaSkin.wolf, "wolf" },
		};

		public static string GetName(this DokzaSkin value)
		{
			return mDokzaSkinTable[value];
		}
	}
}
