using System;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.SystemCore;
using Spine.Unity;
using UnityEngine;

namespace CTC.Gameplay.Proxies
{
	public class DokzaEffectProxy : MonoBehaviour
	{
		public SkeletonAnimation DokzaSkeletonAnimation = null;

		private ILog _log = LogManager.GetLogger(typeof(DokzaEffectProxy));
		
		private void Awake()
		{
			if (ReferenceEquals(DokzaSkeletonAnimation, null))
				_log.Error("There is no SkeletonAnimation");
		}

		private void Start()
		{
			DokzaSkeletonAnimation.state.Event += receiveSpineEvent;
		}

		private void OnDisable()
		{
			DokzaSkeletonAnimation.state.Event -= receiveSpineEvent;
		}

		private void receiveSpineEvent(Spine.TrackEntry trackEntry, Spine.Event e)
		{
			if (ReferenceEquals(GlobalService.GameplayManager.EffectManager, null))
				return;
			
			switch (e.Data.Name)
			{
				case "OnStepRun":
					GlobalService.GameplayManager.EffectManager.SpawnEffect(EffectType.RunDust,
						transform.position, 1.5f);
					break;
			}
		}
	}
}