#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Text;
using CT.Common.Tools.Data;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
internal class EditorTools
{

	[MenuItem("Tools/Compress All Texture2D", priority = 20)]
	private static void setEveryTexture2DCompression()
	{
		Queue<string> retrieve = new();
		retrieve.Enqueue(Directory.GetCurrentDirectory() + @"\Assets");

		int changeCount = 0;

		StringBuilder log = new();

		while (retrieve.Count > 0)
		{
			string curPath = retrieve.Dequeue();
			foreach (var child in Directory.GetDirectories(curPath))
			{
				retrieve.Enqueue(child);
			}

			var filePaths = Directory.GetFiles(curPath);

			foreach (string filePath in filePaths)
			{
				if (!filePath.ToLower().Contains("png.meta"))
				{
					continue;
				}

				var readResult = FileHandler.TryReadText(filePath);
				if (readResult.ResultType != JobResultType.Success)
				{
					continue;
				}
				string loadedMetaData = readResult.Value;

				string pattern = "textureCompression: ";
				var targetIndex = loadedMetaData.IndexOf(pattern);

				if (targetIndex < 0)
				{
					continue;
				}

				var index = targetIndex + pattern.Length;

				StringBuilder sb = new StringBuilder(loadedMetaData);
				sb[index] = '2';

				var writeResult = FileHandler.TryWriteText(filePath, sb.ToString());
				if (writeResult.ResultType == JobResultType.Success)
				{
					changeCount++;
					log.AppendLine(filePath);
				}
			}
		}

		Debug.Log(log.ToString());
		Debug.Log($"Setup every texture2D compression option. Count : {changeCount}");
	}
}

#endif