//#define CUSTOM_SPINE
#if CUSTOM_SPINE && UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CT.Logger;
using CT.Common.Tools;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Data;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Spine;
using UnityEditor;
using UnityEngine;

public class EnumPatchManager : OdinEditorWindow
{
	private static ILog _log = LogManager.GetLogger(typeof(EnumPatchManager));

	class SpineEnumOptionSet
	{
		public string SpineName = "";
		public string SpineDataPath = "";
		public string CodeFolderPath = "";
		public string NameSpace = "";
	}

	class SpineEnumPatchOption
	{
		public SpineEnumOptionSet Dokza = new();
	}

	public string OptionFileName => nameof(SpineEnumPatchOption) + ".json";
	public string OptionPath => Path.Combine(Global.Editor.Path.EditorFolder, OptionFileName);
	private SpineEnumPatchOption mOptionData = new SpineEnumPatchOption();

	// Options
	private SpineEnumOptionSet mDokza => mOptionData.Dokza;

	[InfoBox("스파인 데이터 위치는 Assets으로 부터 상대적인 경로입니다.")]
	[SerializeField]
	public string DokzaSpineName { get => mDokza.SpineName; set => mDokza.SpineName = value; }

	[SerializeField]
	public string DokzaSpineDataPath { get => mDokza.SpineDataPath; set => mDokza.SpineDataPath = value; }

	[SerializeField]
	public string DokzaEnumCodeFolderPath { get => mDokza.CodeFolderPath; set => mDokza.CodeFolderPath = value; }

	[SerializeField]
	public string DokzaNameSpace { get => mDokza.NameSpace; set => mDokza.NameSpace = value; }

	[Title("생성")]
	[Button("Enum 생성", ButtonHeight = 40)]
	public void GenerateEnum()
	{
		generateSpineEnum(mDokza);
	}

	private void generateSpineEnum(SpineEnumOptionSet optionSet)
	{
		var spineDataPath = Global.Editor.Path.GetPathFromAssets(optionSet.SpineDataPath);
		var enumPath = Global.Editor.Path.GetPathFromAssets(optionSet.CodeFolderPath);
		var codeNamespcae = $"{nameof(CTC)}.{optionSet.NameSpace}";

		// Parse spine
		SkeletonJson_CruelTales parser = new SkeletonJson_CruelTales();
		var loadData = parser.ReadSkeletonData(spineDataPath);

		// Get items
		var animationItems = loadData.Animations.Select(a => a.Name).ToList();
		var skinItems = loadData.Skins.Select(a => a.Name).ToList();

		var replacePattern = @"-|!|@|\?|#|\$|%|\^|&|\*|\(|\)";

		generateCode(animationItems, "Animation", true);
		generateCode(skinItems, "Skin", true);

		void generateCode(IList<string> items, string enumSuffix = "Animation", bool createDropdownList = false)
		{
			List<string> names = new List<string>();
			foreach (var a in items)
			{
				string name = a;
				
				name = Regex.Replace(a, replacePattern, "").Replace("/", "_");

				if (KeywordChecker.IsKeyword(a))
				{
					name += "_";
				}

				names.Add(name);
			}

			// Generate spine enum
			// Spine의 경우 item의 이름이 name이 되어야함으로 생성 인자를 서로 바꾸어야한다.
			var enumName = optionSet.SpineName + enumSuffix;
			var targetPath = Path.Combine(enumPath, enumName + ".cs");
			var codeData = CodeGenerator_Enumerate
				.GenerateWithNameTable(enumName, codeNamespcae, true, true,
									   new List<string>(), names, items, "none");

			var writeEnumResult = FileHandler.TryWriteText(targetPath, codeData);
			if (writeEnumResult.ResultType == JobResultType.Success)
				_log.Info($"Create enum \"{enumName}\" success!");
			else
				_log.Error($"Create enum \"{enumName}\" failed!");

			if (createDropdownList)
			{
				// Generate dropdown values
				var collectionName = optionSet.SpineName + enumSuffix + "VDL";
				var collectionTargetPath = Path.Combine(enumPath, collectionName + ".cs");

				var dropdownCodeData = CodeGenerator_Enumerate
					.GenerateValueDropdownList(collectionName, codeNamespcae, true, true,
										   new List<string>(), items);

				var writeDlResult = FileHandler.TryWriteText(collectionTargetPath, dropdownCodeData);
				if (writeDlResult.ResultType == JobResultType.Success)
				{
					_log.Info($"Create dropdown value list \"{collectionName}\" success!");
				}
				else
				{
					_log.Error($"Create dropdown value list \"{collectionName}\" failed!");
				}
			}

			AssetDatabase.Refresh();
		}
	}

	void printStrings(string title, IEnumerable<string> strings)
	{
		string print = $"{title}\n";
		foreach (var s in strings)
		{
			print += $"({s})\n";
		}

		_log.Info(print);
	}

	#region Option and Window

	[MenuItem("Editor/Enum Patch Manager")]
	private static void initialize()
	{
		var window = GetWindow<EnumPatchManager>(true, "Enum Patch Manager");
		window.minSize = new Vector2(500, 400);
		window.Show();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		loadOption();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		saveOption();
	}

	private void loadOption()
	{
		var result = JsonHandler.TryReadObject<SpineEnumPatchOption>(OptionPath);
		if (result.ResultType == JobResultType.Success)
			mOptionData = result.Value;
		else
			_log.Warn("There is no option to load.");
	}

	private void saveOption()
	{
		var result = JsonHandler.TryWriteObject(OptionPath, mOptionData);
		if (result.ResultType == JobResultType.Success)
			_log.Info("Saving option success!");
		else
			_log.Error("Saving option failed!");
	}

	#endregion
}

#endif
