using System;
using System.Collections.Generic;
using CT.Logger;
using CTC.Gameplay.Proxies;
using CTC.SystemCore;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace CTC.Tests
{
	public class Dokza_BoneFollowerManager : MonoBehaviour, IManager
	{
		public GameObject BoneFollowerPrefab;

		private Dokza_BoneFollower[] _boneFollowerPool = new Dokza_BoneFollower[15];
		private Dictionary<Transform, Dokza_BoneFollower> _boneFollowerDic = new();
		private int _boneFollowerIdx = 0;
		
		private ILog _log = LogManager.GetLogger(typeof(Dokza_BoneFollowerManager));
		
		public void Initialize()
		{
			for (int i = 0; i < _boneFollowerPool.Length; i++)
			{
				GameObject instancedObj = Instantiate(BoneFollowerPrefab, transform);

				if (instancedObj.TryGetComponent(out Dokza_BoneFollower boneFollower))
				{
					boneFollower.Init(transform);
					_boneFollowerPool[i] = boneFollower;
					_boneFollowerDic.Add(boneFollower.transform, boneFollower);
					boneFollower.gameObject.SetActive(false);
				}
				else
				{
					_log.Error("There is no BoneFollower");
					return;
				}
			}
		}

		[Button]
		public Transform SpawnBoneFollower(SkeletonRenderer skeletonRenderer, string boneName)
		{
			Transform returnTransform = _boneFollowerPool[_boneFollowerIdx].transform;
			_boneFollowerPool[_boneFollowerIdx].gameObject.SetActive(true);
			_boneFollowerPool[_boneFollowerIdx].SetBoneFollower(skeletonRenderer, boneName);

			_boneFollowerIdx++;
			if (_boneFollowerIdx >= _boneFollowerPool.Length)
			{
				_boneFollowerIdx = 0;
			}

			return returnTransform;
		}

		[Button]
		public void ReleaseBoneFollower(Transform boneFollowerTransform)
		{
			if (_boneFollowerDic.TryGetValue(boneFollowerTransform, out Dokza_BoneFollower follower))
			{
				follower.Release(transform);
				follower.gameObject.SetActive(false);
			}
			else
			{
				_log.Error("Transform Param Error");
			}
		}

		public void Release()
		{
			_boneFollowerDic.Clear();
			foreach (var VARIABLE in _boneFollowerPool)
			{
				if(!ReferenceEquals(VARIABLE, null))
					GameObject.Destroy(VARIABLE);
			}
		}
	}
}