#if UNITY_EDITOR

using System.Collections.Generic;
using CT.Logger;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Data;
using CTC.Globalizations;
using CTC.Utils;
using CTC.Utils.Networks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Importer : EditorWindow
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(Importer));

	[MenuItem("Importer/Check Credentials Version", priority = 5)]
	private static void checkCredentialsDataVersion()
	{
		var version = Global.Editor.Credential.GetVersion();
		if (string.IsNullOrWhiteSpace(version))
			_log.Error($"잘못된 Credentials data 파일입니다. 다시 다운로드 받아주세요.");
		else
			_log.Info($"Credentials Data Version : {version}");
	}

	// InGame Unity Scene
	[MenuItem("Importer/Import localization data", priority = 10)]
	private static async void importLocalizationData()
	{
		string filePath =
		AssetLoader.ResourcesPath +
		Global.Path.Localization +
		Global.File.TextExtension;

		string data = await WebUtility.GetDataOnEditorAsync(Global.Editor.URL.DATA_TEXT_LOCALIZATION);
		if (FileHandler.TryWriteText(filePath, data).ResultType == JobResultType.Success)
		{
			AssetDatabase.Refresh();

			_log.Info($"Localization file import success!");
			Localizer.OnUpdateDevelopLocalizationData();
			_log.Info($"Update develop localization data finished!");

			if (makeLocalizationTextEnums())
				_log.Info($"Generate localization text enum success!");
			else
				_log.Error($"Generate localization text enum was failed!");
		}
		else
		{
			_log.Error($"Localiation file import error!");
		}

		bool makeLocalizationTextEnums()
		{
			var textAsset = Resources.Load(Global.Path.Localization) as TextAsset;
			var localizationTable = DataHandler.ReadAsTable(textAsset.text, DataHandler.TSV_PARSE_OPTION);

			string textKey = Global.Data.LocalizationKey;

			List<string> usingItems = new()
			{
				//"System",
			};

			List<string> enumKey = new();

			foreach (var line in localizationTable)
			{
				if (!line.TryGetValue(textKey, out var keyData))
					continue;

				string item = keyData as string;
				if (!string.IsNullOrEmpty(item))
				{
					enumKey.Add(item);
				}
			}

			_log.Info($"Try generate \"{textKey}\" enum code. Count : {enumKey.Count}");

			var codeData = CodeGenerator_Enumerate.Generate(textKey, $"{nameof(CTC)}.{nameof(CTC.Globalizations)}",
															true, true, usingItems, enumKey);
			var codeFilePath = System.IO.Path.Combine(Localizer.GetThisCodeFileDirectory(), $"{textKey}.cs");

			return FileHandler.TryWriteText(codeFilePath, codeData).ResultType == JobResultType.Success;
		}
	}
}

#endif
