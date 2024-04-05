using CT.Common.Tools.Data;

public static partial class Global
{
#if UNITY_EDITOR
	public static class Editor
	{
		public static class Credential
		{
			public static string GetData(string key)
			{
				var loadResult = FileHandler.TryReadText(Path.CredentialsDataFile);

				if (loadResult.ResultType != JobResultType.Success)
				{
					_log.Error($"Credentials Data 패치를 위한 패치 파일이 없습니다. 안전한 경로로 다운로드 받아야 합니다.");
					return string.Empty;
				}

				var credentialPairs = DataHandler.ReadAsPairs(loadResult.Value, DataHandler.TSV_PARSE_OPTION);
				if (credentialPairs == null)
					return string.Empty;

				if (credentialPairs.TryGetValue(key, out var value))
				{
					return value;
				}
				else
				{
					_log.Error($"Credential data load failed! Key : {key}");
					return string.Empty;
				}
			}

			public static string GetVersion()
			{
				return GetData("CredentialsVersion");
			}
		}

		public static class URL
		{
			/// <summary>게임 텍스트 데이터 URL입니다.</summary>
			public static string DATA_TEXT_LOCALIZATION
				=> Credential.GetData(nameof(DATA_TEXT_LOCALIZATION).ToLower());
		}

		public static class Path
		{
			public static bool IsExistServerProject =>
				System.IO.Directory.Exists(ServerProject);

			public static string ServerProject =>
				System.IO.Path.Combine(ProjectEditor, @"../../CruelTales-Servers/CTS-GameplayServer");

			public static string ProjectEditor => System.IO.Directory.GetCurrentDirectory();

			public static string ProjectAssets =>
				System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Assets");

			public static string CredentialsDataFile
				=> GetPathFromEditor(@"..\Credentials\data_credentials.tsv");

			public static string DokzaSpineData => GetPathFromAssets("Sprites/Animations/Player/Ani_PC.json");

			public static string EditorFolder => GetPathFromAssets("Editor");

			/// <summary>에디터 위치로 부터의 경로를 반환합니다.</summary>
			/// <param name="relativePath">상대적인 경로</param>
			/// <returns>에디터를 기준으로 한 절대 경로</returns>
			private static string GetPathFromEditor(string relativePath)
			{
				return System.IO.Path.Combine(ProjectEditor, relativePath);
			}

			/// <summary>프로젝트의 Assets 위치로 부터의 경로를 반환합니다.</summary>
			/// <param name="relativePath">상대적인 경로</param>
			/// <returns>에디터를 기준으로 한 절대 경로</returns>
			public static string GetPathFromAssets(string relativePath)
			{
				return System.IO.Path.Combine(ProjectAssets, relativePath);
			}
		}
	}
#endif
}
