using System;

namespace CTC.SystemCore
{
	[Flags]
	public enum StateFlag
	{
		None = 0,
		System = 1,
		Escape = 2,
		Gameplay = 4,
		Chat = 8,
		Event = 16,
	}

	public static class StateFlagExtension
	{
		private static StateFlag[] mStateFlagArray = null;

		public static StateFlag[] GetArray()
		{
			if (mStateFlagArray == null)
			{
				mStateFlagArray = (StateFlag[])Enum.GetValues(typeof(StateFlag));
			}

			return mStateFlagArray;
		}
	}
}