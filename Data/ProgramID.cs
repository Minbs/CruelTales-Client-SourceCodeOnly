using System;

namespace CTC.Data
{
	/// <summary>프로그램 식별 구조체입니다.</summary>
	public readonly struct ProgramID
	{
		public readonly string GameName;
		public readonly string Version;
		public readonly uint AppID;

		public ProgramID(string gameName, string version, uint appID)
		{
			GameName = gameName;
			Version = version;
			AppID = appID;
		}

		public override string ToString()
		{
			return
				$"Name : {GameName}\n" +
				$"Version : {Version}\n" +
				$"AppID : {AppID}";
		}

		public static bool operator ==(ProgramID lhs, ProgramID rhs)
		{
			return lhs.GameName == rhs.GameName &&
				lhs.Version == rhs.Version &&
				lhs.AppID == rhs.AppID;
		}

		public static bool operator !=(ProgramID lhs, ProgramID rhs)
		{
			return !(lhs == rhs);
		}

		public override bool Equals(object obj)
		{
			if (obj is ProgramID)
			{
				return this == (ProgramID)obj;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return new Tuple<string, string, ulong>
				(GameName, Version, AppID).GetHashCode();
		}
	}
}