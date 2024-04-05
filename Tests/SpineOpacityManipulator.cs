using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace CTC.Tests
{
	public class SpineOpacityManipulator : MonoBehaviour
	{
		private SkeletonAnimation _skeletonAnimation;
		private SkeletonGraphic _skeletonGraphic;
		private int _isInit = 0;

		[Button]
		public void ManipulateOpacity(float alpha)
		{
			alpha = alpha switch
			{
				< 0f => 0f,
				> 1f => 1f,
				_ => alpha
			};

			if (_isInit == 0)
			{
				if (this.TryGetComponent(out _skeletonAnimation))
					_isInit = 1;
				else if (this.TryGetComponent(out _skeletonGraphic))
					_isInit = 2;
			}
			
			switch (_isInit)
			{
				case 1:
					_skeletonAnimation.skeleton.A = alpha;
					break;
				
				case 2:
					_skeletonGraphic.Skeleton.A = alpha;
					break;
			}
		}
	}
}