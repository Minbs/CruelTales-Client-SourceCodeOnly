using System;
using System.Collections;
using CT.Logger;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace CTC.Graphics
{
	public class VolumeManager : MonoBehaviour
	{
		public Volume[] VolumeProfiles;

		private int _curVolumeIdx = 0;
		private int _changeVolumeIdx = 0;
		private float _lerpSpeed = 1.5f;
		
		private CoroutineRunner _volumeRunner;
		private static readonly ILog _log = LogManager.GetLogger(typeof(VolumeManager));

		public void Awake()
		{
			_volumeRunner = new CoroutineRunner(this);
			
			if (ReferenceEquals(VolumeProfiles, null) || VolumeProfiles.Length <= 0)
			{
				_log.Warn("VolumeManager에 Volume이 할당되어 있지 않습니다.");
				return;	
			}
			
			foreach (var VARIABLE in VolumeProfiles)
			{
				VARIABLE.gameObject.SetActive(true);
				VARIABLE.weight = 0f;
				VARIABLE.gameObject.SetActive(false);
			}
			
			VolumeProfiles[0].gameObject.SetActive(true);
			VolumeProfiles[0].weight = 1f;
		}

		/// <summary>
		/// 해당 Index로 Volume을 자연스럽게 교체합니다.
		/// </summary>
		/// <param name="volumeIdx"></param>
		[Button]
		public void ChangeVolumeTo(int volumeIdx)
		{
			if (volumeIdx < 0 || volumeIdx >= VolumeProfiles.Length)
			{
				_log.Warn("교체하려는 Volume의 Index가 범위를 벗어났습니다.");
				return;	
			}
			else if (volumeIdx == _curVolumeIdx)
				return;
			
			if (_volumeRunner.IsRunning)
			{
				VolumeProfiles[_curVolumeIdx].weight = 0f;
				VolumeProfiles[_changeVolumeIdx].weight = 1f;
				VolumeProfiles[_curVolumeIdx].gameObject.SetActive(false);
				_curVolumeIdx = _changeVolumeIdx;
			}
			
			_changeVolumeIdx = volumeIdx;
			VolumeProfiles[volumeIdx].gameObject.SetActive(true);
			_volumeRunner.Start(volumeEnumerator(_changeVolumeIdx));
		}

		private IEnumerator volumeEnumerator(int changeVolumeIdx)
		{
			float lerpValue = 0f;
			while (true)
			{
				if (lerpValue >= 1f)
				{
					lerpValue = 1f;
					break;
				}
				
				VolumeProfiles[_curVolumeIdx].weight = Mathf.Lerp(1f, 0f, lerpValue);
				VolumeProfiles[changeVolumeIdx].weight = Mathf.Lerp(0f, 1f, lerpValue);
				lerpValue += Time.deltaTime * _lerpSpeed;
				yield return null;
			}
			
			VolumeProfiles[_curVolumeIdx].weight = Mathf.Lerp(1f, 0f, lerpValue);
			VolumeProfiles[changeVolumeIdx].weight = Mathf.Lerp(0f, 1f, lerpValue);
			VolumeProfiles[_curVolumeIdx].gameObject.SetActive(false);
			_curVolumeIdx = changeVolumeIdx;
			
			yield break;
		}
	}
}