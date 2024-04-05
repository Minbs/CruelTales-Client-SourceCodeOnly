#if UNITY_EDITOR

using System.IO;
using CT.Logger;
using UnityEditor;
using UnityEngine;

public class EmptyFolderCleaner : Editor
{
	private static ILog _log = LogManager.GetLogger(typeof(EmptyFolderCleaner));

	[MenuItem("Tools/Clean Empty Folder &F9")]
	public static void EmptyFolderClear()
	{
		_log.Info($"Clean Start");
		cleanDirectory(Application.dataPath);
		_log.Info($"Clean End");
	}

	private static void cleanDirectory(string path)
	{
		var info = new DirectoryInfo(path);

		if (!info.Exists)
			return;

		var allDirectories = info.GetDirectories("*", SearchOption.AllDirectories);
		foreach (var dirInfo in allDirectories)
		{
			if (hasFiles(dirInfo.FullName))
				continue;

			_log.Info($"Delete folder : {dirInfo.FullName}");

			var di = new DirectoryInfo(dirInfo.FullName);
			di.Delete(true);

			var file = new FileInfo(dirInfo.FullName + ".meta");
			file.Delete();
		}

		bool hasFiles(string dir)
		{
			var directories = Directory.GetDirectories(dir);
			var files = Directory.GetFiles(dir);
			return files.Length != 0;
		}
	}

}

#endif