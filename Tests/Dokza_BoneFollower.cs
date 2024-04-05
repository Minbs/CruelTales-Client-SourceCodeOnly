using System;
using Spine.Unity;
using UnityEngine;

namespace CTC.Tests
{
	public class Dokza_BoneFollower : MonoBehaviour
	{
		public BoneFollower BoneFollowerScript;

		public void Init(Transform poolTransform)
		{
			transform.parent = poolTransform;
			BoneFollowerScript.enabled = false;
		}
		
		public void SetBoneFollower(SkeletonRenderer skeletonRenderer, string boneName)
		{
			transform.parent = null;
			BoneFollowerScript.enabled = true;
			BoneFollowerScript.skeletonRenderer = skeletonRenderer;
			BoneFollowerScript.SetBone(boneName);
			
			BoneFollowerScript.followBoneRotation = true;
			BoneFollowerScript.followXYPosition = true;
			BoneFollowerScript.followZPosition = true;
			BoneFollowerScript.followLocalScale = true;
			BoneFollowerScript.followParentWorldScale = false;
			BoneFollowerScript.followSkeletonFlip = true;
		}

		public void Release(Transform poolTransform)
		{
			transform.parent = poolTransform;
			BoneFollowerScript.enabled = false;
		}
	}
}