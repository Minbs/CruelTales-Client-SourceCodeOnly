using System.Runtime.CompilerServices;
using CT.Logger;
using UnityEngine;

public static partial class Global
{ 
	private static readonly ILog _log = LogManager.GetLogger(typeof(Global));

	public static class Environment
	{
		public const int SCREEN_MINIMUM_WIDTH = 960;
		public const int SCREEN_MINIMUM_HEIGHT = 540;
		public const int MINIMUM_FRAMERATE = 60;
	}

	public static class DataBind
	{
		public static class ComponentPrefix
		{
			public static readonly string VIEW = "View_";
			public static readonly string PANEL = "Panel_"; 
			public static readonly string LAYOUT = "Layout_";
			public static readonly string TEXT = "Text_";
			public static readonly string IMAGE = "Img_";
			public static readonly string RAWIMAGE = "RawImg_";
			public static readonly string ITEM = "Item_";
			public static readonly string BUTTON = "Btn_";
			public static readonly string INPUT = "Input_";
			public static readonly string SCROLL = "Scroll_";
		}

		public static class Path
		{
			public static readonly string GUI_ON_CLICK = "GUI_OnClick_";
		}
	}

	public static class Data
	{
		public static readonly string LocalizationKey = "TextKey";
	}

	public static class Texts
	{
		public static readonly string WRONG_VERSION = "UNKNOWN";
		public const char PASSWORD_MASK_CHARACTER = '*';
	}

	public static class UnityTexts
	{
		public const string NewLine = "<br>";
	}

	public static class Path
	{
		public static string ProcessDataPath = Application.dataPath;
		public static string UserDataPath = ProcessDataPath + @"/Userdata";

		public static string GetUserDataFilePath(string fileNameWithExtension)
		{
			return UserDataPath + "/" + fileNameWithExtension;
		}

		public static readonly string Localization = @"data_text_localization";
		public static readonly string ClientConfig = @"data_client_config";
	}

	public static class File
	{
		public static readonly string OptionFileName = @"option";
		public static readonly string InputDataFileName = @"input_data";

		public static readonly string TextExtension = @".txt";
		public static readonly string JsonExtension = @".json";
	}

	public static class GUI
	{
		public const int GUI_REFERENCE_PPU = 100;
		public const int GUI_REFERENCE_RESOLUTION_X = 1920;
		public const int GUI_REFERENCE_RESOLUTION_Y = 1080;
	}

	public static class Net
	{
		//[Obsolete("Legacy")]
		//public static class Lobby
		//{
		//	/// <summary>최대 접속 가능한 플레이어의 수 입니다.</summary>
		//	public const int DEFAULT_MAX_PLAYER = 5;
		//	/// <summary>접속 가능 여부입니다. 싱글플레이인 경우 비활성화 해야 합니다.</summary>
		//	public const bool DEFAULT_IS_JOINABLE = true;
		//	public const Legacy.CruelTales.Net.LobbyType DEFAULT_LOBBY_TYPE = Legacy.CruelTales.Net.LobbyType.Public;

		//	public const int MAX_METADATA_KEY_LENGTH = 255;
		//	public const int MAX_METADATA_VALUE_LENGTH = 8192;

		//	public static readonly Dictionary<Legacy.CruelTales.Net.LobbyMetadataKey, int> ValueLimitsTable = new()
		//	{
		//		{ Legacy.CruelTales.Net.LobbyMetadataKey.Title, 60 },
		//		{ Legacy.CruelTales.Net.LobbyMetadataKey.Password, 24 },
		//		{ Legacy.CruelTales.Net.LobbyMetadataKey.Description, 1024 },
		//	};

		//	public static int GetMetadataLimitation(Legacy.CruelTales.Net.LobbyMetadataKey dataKey)
		//	{
		//		if (ValueLimitsTable.TryGetValue(dataKey, out int length))
		//		{
		//			return length;
		//		}

		//		return MAX_METADATA_VALUE_LENGTH;
		//	}
		//}

		public const int SYSTEM_MIN_PLAYER = 2;
		public const int SYSTEM_MAX_PLAYER = 64;

		public static int GetValidMaxPlayerCount(int count)
		{
			return Mathf.Clamp(count, SYSTEM_MIN_PLAYER, SYSTEM_MAX_PLAYER);
		}

		public static readonly string ERROR_VERSION = "ERROR_VERSION";

		public static readonly int DEFALUT_GUI_OPERATION_DELAY = 150;

		// Low Level Network
		public const int CONNECTION_TIMEOUT_INTERVAL = 10000; // ms
		public const int DEFAULT_MTU = 1200;
		public const int UDP_HEADER_SIZE = 8;

		public const int VALID_PORT_FIRST = 49152;
		public const int VALID_PORT_LAST = 65535;

		public const float HEARTBEAT_CHECK_INTERVAL_SEC = 5.0f;
		public const float HEARTBEAT_SEND_INTERVAL_SEC = 3.0F;
		public const int HEARTBEAT_TIMEOUT_INTERVAL_MS = (int)(HEARTBEAT_CHECK_INTERVAL_SEC * 1000) * 2000;

		// Synchronizer
		public const int INITIAL_OBJECT_COUNT = 1000;
		public const int NETWORK_TICK = 30;
		public const int NETWORK_INTERPOLATION_VALUE = NETWORK_TICK;
		public const float NETWORK_TICK_INTERVAL_SEC = 1.0F / NETWORK_TICK;
		public const int NETWORK_TICK_INTERVAL_MS = (int)(NETWORK_TICK_INTERVAL_SEC * 1000);

		public static bool IsValidPort(int portNumber)
		{
			return portNumber >= VALID_PORT_FIRST && portNumber <= VALID_PORT_LAST;
		}
	}

	public static string GetThisCodeFilePath([CallerFilePath] string path = null)
	{
		return path;
	}

	public static string GetThisCodeFileDirectory()
	{
		return System.IO.Path.GetDirectoryName(GetThisCodeFilePath());
	}

	public static string GetThisCodeFileNameWithoutExtension()
	{
		return System.IO.Path.GetFileNameWithoutExtension(GetThisCodeFilePath());
	}
}