using CTC.GUI;
using Slash.Unity.DataBind.Core.Presentation;

namespace CTC.GUI.MiniGameSelect
{
	public class View_MiniGameVoteResult : ViewBaseWithContext
	{
		public Context_MiniGameVoteResult BindedContext { get; private set; }
		public MIniGameInfo SelectedMiniGameInfo;

		private void Start()
		{
			BindedContext = GetComponent<ContextHolder>().Context as Context_MiniGameVoteResult;
			BindedContext.MiniGameName = SelectedMiniGameInfo.Name;
			BindedContext.MiniGameSprite = SelectedMiniGameInfo.Sprite;
		}

		protected override void onBeginShow()
		{
			base.onBeginShow();
			this.BindedContext = this.CurrentContext as Context_MiniGameVoteResult;
		}
	}
}
