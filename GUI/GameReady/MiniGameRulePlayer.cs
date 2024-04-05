using Spine;
using UnityEngine.Video;

namespace CTC.GUI.GameReady
{
	public abstract class MiniGameRulePlayer
	{
		public abstract void Play();
		public abstract void Stop();
		public abstract bool SetIndex(int index); // 해당 순서의 영상 또는 이미지 재생
		public abstract void SetLoop(bool isLoop);
	}

	#region VideoPlayer
	public class MiniGameRuleVideoPlayer : MiniGameRulePlayer
	{
		public VideoPlayer videoPlayer;

		public override void Play()
		{
			videoPlayer.Play();
		}

		public override void Stop() 
		{
			videoPlayer.Stop();
		}

		public override bool SetIndex(int index)
		{
			return true;
		}

		public override void SetLoop(bool isLoop)
		{
			videoPlayer.isLooping = isLoop;
		}
	}
	#endregion

	#region SpineAnimation
	public class MiniGameRuleSpineAnimationPlayer : MiniGameRulePlayer
	{
		//public SkeletonAnimation SpineAnimation;
		public AnimationState AnimationState;

		public override void Play()
		{
			AnimationState.TimeScale = 1;
			AnimationState.SetAnimation(0, "animation", false);
		}

		public override void Stop()
		{
			AnimationState.TimeScale = 0;
		}

		public override bool SetIndex(int index)
		{
			return true;
		}

		public override void SetLoop(bool isLoop)
		{
			AnimationState.GetCurrent(0).Loop = isLoop;
		}
	}
	#endregion

	#region Image
	public class MiniGameRuleImagePlayer : MiniGameRulePlayer
	{
		//public SkeletonAnimation SpineAnimation;

		public override void Play()
		{
			return;
		}

		public override void Stop()
		{
			return;
		}

		public override bool SetIndex(int index)
		{
			return true;
		}

		public override void SetLoop(bool isLoop)
		{
			return;
		}
	}
	#endregion
}
