using CTC.Utils.Coroutines;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CTC.GUI
{
	[Serializable]
	public class GuiAnimationComponent
	{
		// Animations
		[field: SerializeField]
		public List<GUISequenceElement> ShowTweenList { get; private set; }
		private Sequence mShowAnimation;
		public bool IsShowAnimatinoAble { private set; get; } = false;

		private CoroutineRunner _animationRunner;

		public void Initialize(MonoBehaviour mono)
		{
			_animationRunner = new CoroutineRunner(mono);

			createSequence(mono, out mShowAnimation);
			mShowAnimation.Pause();

			// 비동기
			mShowAnimation.OnStart(() =>
			{
				var tasks = Parallel.ForEach(ShowTweenList, async item =>
				{
					item.TweenAnimation.OnSequenceStart();
					await UniTask.Yield();
				});
			});

			mShowAnimation.OnComplete(() =>
			{
				foreach (var element in ShowTweenList)
				{
					element.TweenAnimation.OnSequenceComplete();
				}
				mShowAnimation = null;
			});

			mShowAnimation.OnPause(() =>
			{
				foreach (var element in ShowTweenList)
				{
					element.TweenAnimation.OnTweenComplete();
				}
			});
		}

		private void createSequence(MonoBehaviour mono, out Sequence sequence)
		{
			foreach (GUISequenceElement element in ShowTweenList)
			{
				element.Initialize(mono);
			}

			sequence = DOTween.Sequence();

			foreach (var element in ShowTweenList)
			{
				if (element.SequenceType == GUISequenceType.Append)
					sequence.Append(element.TweenAnimation.GetTween());
				else if (element.SequenceType == GUISequenceType.Join)
					sequence.Join(element.TweenAnimation.GetTween());
			}
		}

		public void Play(Action callback)
		{
			_animationRunner.Start(this.playAnimation(callback));
		}

		private IEnumerator playAnimation(Action callback)
		{
			mShowAnimation.Play();
			callback?.Invoke();
			yield break;
		}

		public void Stop()
		{
			_animationRunner.Stop();
		}
	}

	//[Serializable]
	//public class ViewAnimation
	//{
	//	[field: TabGroup("Hide Animation"), SerializeField]
	//	public List<GUISequenceElement> HideTweenList { get; private set; }

	//	private Sequence mHideAnimation;

	//	public bool IsHideAnimatinoAble { private set; get; } = false;

	//	public void Initialize(MonoBehaviour mono)
	//	{
	//		createSequence(ShowTweenList, mono, out mShowAnimation);
	//		mShowAnimation.Pause();

	//		// 비동기
	//		mShowAnimation.OnStart(() =>
	//		{
	//			var tasks = Parallel.ForEach(ShowTweenList, async item =>
	//			{
	//				item.TweenAnimation.OnSequenceStart();
	//			});
	//		});

	//		mShowAnimation.OnComplete(() =>
	//		{
	//			foreach (var element in ShowTweenList)
	//			{
	//				element.TweenAnimation.OnSequenceComplete();
	//			}
	//			mShowAnimation = null;
	//		});
			
	//		mShowAnimation.OnPause(() =>
	//		{
	//			foreach (var element in ShowTweenList)
	//			{
	//				element.TweenAnimation.OnTweenComplete();
	//			}
	//		});
			

	//	}

	//	public void PlaySequence() => mShowAnimation.Play();

	//	public void OnHide() => mShowAnimation.Pause();

	//	private void createSequence(List<GUISequenceElement> tweenList, MonoBehaviour mono, out Sequence sequence)
	//	{
	//		foreach (GUISequenceElement element in tweenList)
	//		{
	//			element.Initialize(mono);
	//		}

	//		sequence = DOTween.Sequence();

	//		foreach (var element in tweenList)
	//		{
	//			if (element.SequenceType == GUISequenceType.Append)
	//				sequence.Append(element.TweenAnimation.GetTween());
	//			else if (element.SequenceType == GUISequenceType.Join)
	//				sequence.Join(element.TweenAnimation.GetTween());
	//		}
	//	}
	//}
}
