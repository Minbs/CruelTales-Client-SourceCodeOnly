using CT.Common.Gameplay;
using CT.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Video;
using System.Collections;
using CTC.Utils.Coroutines;

namespace CTC.SystemCore
{
	[Serializable]
	public class VideoResources : IManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(VideoResources));

		// Reference
		ResourcesManager _resourcesManager;

		[SerializeField]
		public Dictionary<GameModeType, List<AssetReferenceGameObject>> _videoRefTable { get; private set; } = new();

		//private GameObject _videoObject;
		public List<VideoClip> _currentMiniGameVideos { get; private set; } = new();

		public VideoResources(ResourcesManager resourcesManager)
		{
			_resourcesManager = resourcesManager;
		}

		public void Initialize()
		{
			loadMiniGameRuleVideoReferences();
		}

		public void Release() { }

		/// <summary>
		/// 미니 게임 규칙용 비디오만 로드하기 위한 함수입니다
		/// 파일 형식 : Video_Rule_MapName_0
		/// </summary>
		private void loadMiniGameRuleVideoReferences()
		{
			var loadOperation = Addressables.LoadResourceLocationsAsync(_resourcesManager.MiniGameRuleVideoLabel);
			_resourcesManager.RegisterOperationHandle(loadOperation);

			loadOperation.Completed += (operation) =>
			{
				foreach (IResourceLocation videoRef in operation.Result)
				{
					string videoPath = videoRef.PrimaryKey;
					string modeName = System.IO.Path.GetFileNameWithoutExtension(videoPath).Replace("Video_Rule_", string.Empty);
					string[] str = modeName.Split('_');
					modeName = str[0];
					int index = int.Parse(str[1]);

					if (!Enum.TryParse<GameModeType>(modeName, true, out var modeType))
					{
						_log.Fatal($"There is no such MiniGameRuleVideo {modeName}");
						throw new Exception($"There is no such MiniGameRuleVideo {modeName}");
					}

					AssetReferenceGameObject assetRef = new(videoPath);
					if (!_videoRefTable.ContainsKey(modeType))
					{
						_videoRefTable.Add(modeType, new List<AssetReferenceGameObject>());
					}
					_videoRefTable.TryGetValue(modeType, out var list);
					list.Add(assetRef);
				}

				_resourcesManager.StartCoroutine(loadMiniGameRuleVideo(GameModeType.RedHood));
			};


		}


		public IEnumerator loadMiniGameRuleVideo(GameModeType modeType) 
		{
			if (!_videoRefTable.TryGetValue(modeType, out var videoAssets))
			{
				_log.Error($"There is no such MiniGameRuleVideo in the table! map type : {modeType}");
				yield break;
			}

			foreach (var asset in videoAssets) 
			{
				var asyncOperationHandle = asset.LoadAssetAsync<VideoClip>();
				yield return asyncOperationHandle;
				var clip = asyncOperationHandle.Result;
				_currentMiniGameVideos.Add(clip);
			}

			yield return null;
		}
	}
}
