using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CTC;

namespace CTC
{
    public class GUIAnimationWithAnimator : GUIAnimationBase
	{
		public AnimationClip mAnimation;
		private Animator mAnimator;
		private Tween tween;
		public override void Initialize()
		{
			base.Initialize();

			if (!TryGetComponent<Animator>(out mAnimator))
			{
				//Ulog.LogNoComponent(this, mImage);
				return;
			}

			IsAvailable = true;
		}
		public override Tween GetTween()
		{
			if (!IsAvailable)
			{
				return null;
			}


			tween = DOVirtual.DelayedCall(mAnimation.length * loopCount, null, false);

			tween.SetDelay(Delay);
			tween.OnStart(() => OnTweenStart());
			tween.OnComplete(() => OnTweenComplete());


			return tween;
		}

		private IEnumerator PlayAnimation()
		{
			mAnimator.Play(mAnimation.name, -1, 0);
			int count = loopCount;
			yield return null;

			while (true)
			{
				if (mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
				{
					if(count > 1)
					{
						mAnimator.Play(mAnimation.name, -1, 0);
						Debug.Log(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
						count--;

						yield return null;
					}
					else
					{
						break;
					}
				}

				yield return null;
			}

			Debug.Log("끝");
			yield return null;
		}

		public override void OnSequenceStart()
		{
			base.OnSequenceStart();
			mAnimator.Play(mAnimation.name, -1, 0);
			mAnimator.speed = 0;
		}

		public override void OnTweenStart()
		{
			base.OnTweenStart();
			mAnimator.speed = 1;
			StartCoroutine(PlayAnimation());
		}

		public override void OnTweenComplete()
		{
			base.OnTweenComplete();
			StopCoroutine(PlayAnimation());
		}
	}
}
