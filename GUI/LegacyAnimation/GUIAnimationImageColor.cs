using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CTC;

namespace CTC
{
    public class GUIAnimationImageColor : GUIAnimationBase
	{
		private Image mImage;

		[field: Header("Color Option")]
		[field: ColorUsage(true, false), SerializeField] private Color StartColor;
		[field: ColorUsage(true, false), SerializeField] private Color EndColor;

		public override void Initialize()
		{
			base.Initialize();

			if (!TryGetComponent<Image>(out mImage))
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

			Tween tween = mImage.DOColor(EndColor, Duration);


			tween.SetLoops(loopCount, loopType);
			tween.SetDelay(Delay);

			tween.OnStart(() => OnTweenStart());
			tween.OnComplete(() => OnTweenComplete());

			return tween;
		}
		public override void OnSequenceStart()
		{
			base.OnSequenceStart();
			mImage.color = StartColor;
		}

		public override void OnTweenComplete()
		{
			base.OnTweenComplete();
			mImage.color = EndColor;
		}
	}
}