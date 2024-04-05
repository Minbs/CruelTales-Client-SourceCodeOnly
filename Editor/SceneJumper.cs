#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CT.Logger;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class SceneJumper : EditorWindow
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(SceneJumper));

	private static readonly string mSceneExtension = ".unity";

	private static string mSceneAbsolutePath => Application.dataPath + "/Scenes";
	private static readonly string mSceneRelativePath = "Assets/Scenes";

	private static string mAllSceneAbsolutePath => Application.dataPath;
	private static readonly string mAllSceneRelativePath = "Assets";

	private static string mExtensionFilePath => Path.Combine
	(
		GetThisCodeFileDirectory(),
		$"{GetThisCodeFileNameWithoutExtension()}{"Extension.cs"}"
	);

	public class ScenePathData
	{
		public string SceneAbsolutePath;
		public string SceneEditorPath;
		public string QuickSlotPath;
		public string FunctionSignature;
		public string SceneName;

		public ScenePathData(string sceneAbsolutePath, string relativeScenePath)
		{
			SceneName = Path.GetFileNameWithoutExtension(sceneAbsolutePath);

			string relativePath = Directory.GetParent(Application.dataPath).ToString();
			SceneAbsolutePath = sceneAbsolutePath.Replace("\\", "/");

			SceneEditorPath = Path.GetRelativePath(relativePath, SceneAbsolutePath).Replace("\\", "/"); ;
			QuickSlotPath = SceneEditorPath.Replace(relativeScenePath, "Jumper");
			QuickSlotPath = QuickSlotPath[..^mSceneExtension.Length];
			FunctionSignature = "ChangeTo_" + QuickSlotPath.Replace("/", "_");
			FunctionSignature = Regex.Replace(FunctionSignature, @" |\.|-|\(|\)", "_");
		}

		public string GetFunctionDeclaration(int priority = 0)
		{
			return $"\n\t[MenuItem(\"{QuickSlotPath}\", priority = {priority.ToString()})]\n" +
				$"\tprivate static void {FunctionSignature}()\n" +
				"\t{\n" +
				$"\t\tSceneJumper.ChangeScene(\"{SceneEditorPath}\");\n" +
				"\t}\n";
		}
	}

	[MenuItem("Jumper/Play Game")]
	public static void PlaySceneMenu()
	{
		PlayScene("Assets/Scenes/Game/scn_game_initial.unity");
	}

	/// <summary>Extrack scene path data by relative data</summary>
	private static List<ScenePathData> getScenePathData(string targetDirectory, string relativePath)
	{
		List<ScenePathData> scenePathDataList = new();
		Queue<string> sceneDirectories = new(Directory.GetDirectories(targetDirectory));

		while (sceneDirectories.Count != 0)
		{
			string curDirectory = sceneDirectories.Dequeue();
			foreach (var d in Directory.GetDirectories(curDirectory))
			{
				sceneDirectories.Enqueue(d);
			}

			var files = Directory.GetFiles(curDirectory);

			foreach (var filePath in files)
			{
				string curExtension = Path.GetExtension(filePath);

				if (mSceneExtension == curExtension)
				{
					scenePathDataList.Add(new ScenePathData(filePath, relativePath));
				}
			}
		}

		return scenePathDataList;
	}

	[MenuItem("Jumper/Setup scene quick menu", priority = 300)]
	public static void SetupSceneQuickMenu()
	{
		// Generate scene jump code
		string header = "#if UNITY_EDITOR\n" +
			"using UnityEditor;\n" +
			"\n" +
			"[InitializeOnLoad]\n" +
			"public class SceneJumperExtension : EditorWindow\n" +
			"{\n" +
			"\n";

		string footer = "\n" +
			"}\n" +
			"\n" +
			"#endif";

		string data = header;

		var inScenePath = getScenePathData(mSceneAbsolutePath, mSceneRelativePath);
		List<string> sceneEnumList = new();
		foreach (var sceneData in inScenePath)
		{
			data += sceneData.GetFunctionDeclaration(100);
			sceneEnumList.Add($"{sceneData.SceneName} = {sceneData.SceneName.GetHashCode()}");
		}

		var allScenePath = getScenePathData(mAllSceneAbsolutePath, mAllSceneRelativePath);
		foreach (var sceneData in allScenePath)
		{
			data += sceneData.GetFunctionDeclaration(200);
		}

		data += footer;

		
		if (FileHandler.TryWriteText(mExtensionFilePath, data).ResultType == JobResultType.Success)
			_log.Info($"Save scene jumper file success at {mExtensionFilePath}");
		else
			_log.Info($"Saveing scene jumper failed!");

		// Generate scene enum
		string enumName = "SceneType";
		var sceneEnumCode = CodeGenerator_Enumerate.Generate(enumName, nameof(CTC), true, true,
															 new List<string>(), sceneEnumList);
		var sceneEnumCodePath = Path.Combine(Global.GetThisCodeFileDirectory(), $"{enumName}.cs");
		if (FileHandler.TryWriteText(sceneEnumCodePath, sceneEnumCode).ResultType == JobResultType.Success)
			_log.Info($"Save scene enum file success at {sceneEnumCodePath}");
		else
			_log.Info($"Saveing scene enum failed!");
		
		AssetDatabase.Refresh();
	}

	#region Scene Management Functions

	public static void PlayScene(string scenePath)
	{
		if (EditorApplication.isPlaying)
		{
			EditorApplication.ExitPlaymode();
		}

		ChangeScene(scenePath);
		EditorApplication.isPlaying = true;
	}

	public static void ChangeScene(string scenePath)
	{
		if (EditorApplication.isPlaying)
		{
			EditorApplication.ExitPlaymode();
		}

		// If the scene has been modified, Editor ask to you want to save it.
		if (EditorSceneManager.GetActiveScene().isDirty)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		EditorSceneManager.OpenScene(scenePath);
	}

	#endregion

	public static string GetThisCodeFilePath([CallerFilePath] string path = null)
	{
		return path;
	}

	public static string GetThisCodeFileDirectory()
	{
		return Path.GetDirectoryName(GetThisCodeFilePath());
	}

	public static string GetThisCodeFileNameWithoutExtension()
	{
		return Path.GetFileNameWithoutExtension(GetThisCodeFilePath());
	}
}

#endif