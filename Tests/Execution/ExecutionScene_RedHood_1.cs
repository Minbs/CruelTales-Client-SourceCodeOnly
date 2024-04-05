using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace CTC.Tests.Execution
{
	public class ExecutionScene_RedHood_1 : ExecutionScene
	{
		public override ExecutionCutSceneType ExeCutSceneType { get; protected set; } = ExecutionCutSceneType.RedHood;
		public override int MaxDokza { get; protected set; } = 2;
		public override int CurDokza { get; protected set; } = 1;

		private static readonly ILog _log = LogManager.GetLogger(typeof(ExecutionScene_RedHood_1));
		
		private const string DOKZA_SINGLE_0 = "Execution/Redhood_1";


		public override void Play(params SkinSet[] skinSets)
		{
			if (skinSets is null || skinSets.Length <= 0)
			{
				_log.Warn("ExecutionScene_RedHood_1 SkinSet Array's length is suspicious");
				return;
			}


			DokzaSkinHandlerList[0].ApplySkin(skinSets[0]);
			DokzaModelList[0].state.SetAnimation
				(0, DOKZA_SINGLE_0, false).MixDuration = 0f;
			Director.Play();
		}

		public override void OnEnd()
		{
			if (ReferenceEquals(ExeManager, null))
				return;
			
			ExeManager.OnSceneEnd();
			_log.Debug("RedHood_1_Ended");
		}
	}
}