using System;
using UnityEngine;

namespace CTC
{
	public enum GUISequenceType
	{
		None = 0,
		Append,
		Join,

	}

	[Serializable]
	public class GUISequenceElement
    {
		[SerializeField] private string ID;
		[field: SerializeField] public GUISequenceType SequenceType { private set; get; } = GUISequenceType.None;

		private GUIAnimationBase mTweenAnimation;

		public GUIAnimationBase TweenAnimation { get => mTweenAnimation; }

		public void Initialize(MonoBehaviour mono)
		{
			if(TryGetUiAnimation<GUIAnimationBase>(mono, out mTweenAnimation))
			{
				mTweenAnimation.Initialize();
			}
			else
			{
				Debug.Log("Initize Failed");
			}
		}

		private bool TryGetUiAnimation<T>(MonoBehaviour mono, out GUIAnimationBase tweenAnimation) where T : GUIAnimationBase
		{
			tweenAnimation = null;

			#region Find component in mono

			var findedComponentsInMono = mono.GetComponents<T>();

			foreach (var findedMono in findedComponentsInMono)
			{
				if (findedMono.ID.EndsWith(ID))
				{
					tweenAnimation = findedMono;
					return true;
				}
			}

			#endregion

			#region Find component in mono children

			var findedAnimatniosInChild = mono.GetComponentsInChildren<T>();
			foreach (var findedAnimationInChild in findedAnimatniosInChild)
			{
				if (findedAnimationInChild.ID.Equals(ID))
				{
					tweenAnimation = findedAnimationInChild;
					return true;
				}
			}

			#endregion

			return false;
		}

	}
}
