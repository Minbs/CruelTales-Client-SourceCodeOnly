using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using Spine.Unity;
using UnityEngine;

namespace CTC.Tests.Execution
{
	public class ExecutionScene_Dueok_1 : ExecutionScene
	{
		public override ExecutionCutSceneType ExeCutSceneType { get; protected set; } = ExecutionCutSceneType.Dueoksini;
		public override int MaxDokza { get; protected set; } = 2;
		public override int CurDokza { get; protected set; } = 1;

		private static readonly ILog _log = LogManager.GetLogger(typeof(ExecutionScene_Dueok_1));

		
		private const string DOKZA_SINGLE_0 = "Execution/dowoksini_1";
		
		public override void Play(params SkinSet[] skinSets)
		{
			if (skinSets is null || skinSets.Length <= 0)
			{
				_log.Warn("ExecutionScene_Dueok_1 SkinSet Array's length is suspicious");
				return;
			}

			// 즉시 발동
			DokzaSkinHandlerList[0].LoadDefaultSkin(0);
			var entry = DokzaModelList[0].state.SetAnimation(0, DOKZA_SINGLE_0, false);
			entry.MixDuration = 0f;
			
			Director.Play();
		}
		
		public override void OnEnd()
		{
			if (ReferenceEquals(ExeManager, null))
				return;
			
			ExeManager.OnSceneEnd();
			_log.Debug("Dueok_1_Ended");
		}
	}
}