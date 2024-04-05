using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using CTC;

namespace CTC
{
    public class GUIViewSequenceGenerator : MonoBehaviour
    {
		[SerializeField] private List<GUISequenceElement> _ShowTweenList;
		[SerializeField] private List<GUISequenceElement> _HideTweenList;

		public bool IsShowAnimatnioAvailable { private set; get; } = false;
		private bool _IsShowPlay = false;
		private Sequence _ShowSequence = null;

		public bool IsHideAnimationAvailable { private set; get; } = false;
		private bool _IsHidePlay = false;
		private Sequence _HideSequence = null;

		private event Action _OnShowComplete;

		public event Action OnShowComplete
		{
			add
			{
				_OnShowComplete += value;
			}
			remove
			{
				_OnShowComplete -= value;
			}
		}

		private event Action _OnHideComplete;

		public event Action OnHideComplete
		{
			add
			{
				_OnHideComplete += value;
			}
			remove
			{
				_OnHideComplete -= value;
			}
		}

		public void Initialize()
		{
			#region Show Seqence Setting

			if (_ShowTweenList.Count > 0)
			{
				foreach (var element in _ShowTweenList)
				{
					element.Initialize(this);
				}

				IsShowAnimatnioAvailable = true;
			}

			#endregion

			#region Hide Seqence Setting

			if (_HideTweenList.Count > 0)
			{
				foreach (var element in _HideTweenList)
				{
					element.Initialize(this);
				}

				IsHideAnimationAvailable = true;
			}

			#endregion
		}

		private void createSequence(List<GUISequenceElement> tweenList, out Sequence sequence)
		{
			sequence = DOTween.Sequence();

			foreach (var element in tweenList)
			{

				if (element.SequenceType == GUISequenceType.Append)
					sequence.Append(element.TweenAnimation.GetTween());
				else if (element.SequenceType == GUISequenceType.Join)
					sequence.Join(element.TweenAnimation.GetTween());
			}
		}

		public void PlayShow(Action callback = null)
		{
			if (!IsShowAnimatnioAvailable)
				return;

			if (_IsShowPlay)
			{
				_ShowSequence.Kill();
			}

			createSequence(_ShowTweenList, out _ShowSequence);

			_ShowSequence.OnStart(() =>
			{
				foreach (var element in _ShowTweenList)
				{
					element.TweenAnimation.OnSequenceStart();
				}
			});

			_ShowSequence.OnComplete(() =>
			{
				foreach (var element in _ShowTweenList)
				{
					element.TweenAnimation.OnSequenceComplete();
				}

				callback?.Invoke();
				_OnShowComplete?.Invoke();

			});

			_ShowSequence.OnKill(() =>
			{
				_IsShowPlay = false;
				_ShowSequence = null;
			});

			_ShowSequence.Play();

			_IsShowPlay = true;
		}

		public void PlayHide(Action callback = null)
		{
			if (!IsHideAnimationAvailable)
				return;

			if (_IsHidePlay)
			{
				DOTween.Kill(_HideSequence);
				_HideSequence = null;
			}

			createSequence(_HideTweenList, out _HideSequence);

			_HideSequence.OnStart(() =>
			{
				foreach (var element in _HideTweenList)
				{
					element.TweenAnimation.OnSequenceStart();
				}
			});

			_HideSequence.OnComplete(() =>
			{
				foreach (var element in _HideTweenList)
				{
					element.TweenAnimation.OnSequenceComplete();
				}
				_OnHideComplete?.Invoke();
				callback?.Invoke();

			});

			_HideSequence.OnKill(() =>
			{
				_IsHidePlay = false;
				_HideSequence = null;
			});

			_HideSequence.Play();
			_IsHidePlay = true;
		}


	}
}
