using System.Collections.Generic;
using CT.Common.DataType.Input;
using CTC.Globalizations;
using CTC.GUI.Components;
using CTC.SystemCore;
using UnityEngine;
using UnityEngine.Video;

namespace CTC.GUI.GameReady
{
	public class View_GameReady : ViewBaseWithContext
	{
		[field : SerializeField]
		private Pagination MiniGameRulePagination;

		[field: SerializeField]
		private List<VideoClip> MiniGameRuleVideosClip = new();

		public Context_GameReady BindedContext { get; private set; }

		public VideoPlayer VideoPlayer;

		// Manager
		private ResourcesManager _resourcesManager;
		private List<object> MiniGameRuleText = new()
		{
			TextKey.DialogResult_OK,
			TextKey.DialogResult_No,
			TextKey.DialogResult_Apply,
		};

		protected override void Start()
		{
			base.Start();
			MiniGameRulePagination.Initialize(MiniGameRuleText);
			MiniGameRulePagination.OnPageChanged += ChangeRuleVideo;
		}

		protected override void onBeginShow()
		{
			base.onBeginShow();
			this.BindedContext = this.CurrentContext as Context_GameReady;
			MiniGameRuleVideosClip.AddRange(GlobalService.ResourcesManager.VideoResources._currentMiniGameVideos);
			ChangeRuleVideo(0);
		}

		public void ChangeRuleVideo(int index)
		{
			VideoPlayer.clip = MiniGameRuleVideosClip[index];
			VideoPlayer.Play();
		}
	}
}