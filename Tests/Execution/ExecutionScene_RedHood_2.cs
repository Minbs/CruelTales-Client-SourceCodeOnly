using CT.Common.Gameplay;
using CT.Logger;

namespace CTC.Tests.Execution
{
	public class ExecutionScene_RedHood_2 : ExecutionScene
	{
		public override ExecutionCutSceneType ExeCutSceneType { get; protected set; } = ExecutionCutSceneType.RedHood;
		public override int MaxDokza { get; protected set; } = 2;
		public override int CurDokza { get; protected set; } = 2;

		
		
		private static readonly ILog _log = LogManager.GetLogger(typeof(ExecutionScene_RedHood_2));
		
		private const string DOKZA_FIRSTANIMNAME = "Execution/Redhood_1";
		private const string DOKZA_SECONDANIMNAME = "Execution/Redhood_2";
		
		
		public override void Play(params SkinSet[] skinSets)
		{
			if (skinSets is null || skinSets.Length <= 1)
			{
				_log.Warn("ExecutionScene_RedHood_2 SkinSet Array's length is suspicious");
				return;
			}
			
			DokzaSkinHandlerList[0].ApplySkin(skinSets[0]);
			DokzaModelList[0].state.SetAnimation
				(0, DOKZA_FIRSTANIMNAME, false).MixDuration = 0f;
			
			DokzaSkinHandlerList[1].ApplySkin(skinSets[1]);
			DokzaModelList[1].state.SetAnimation
				(0, DOKZA_SECONDANIMNAME, false).MixDuration = 0f;
			
			Director.Play();
		}

		public override void OnEnd()
		{
			if (ReferenceEquals(ExeManager, null))
				return;
			
			ExeManager.OnSceneEnd();
			_log.Debug("RedHood_2_Ended");
		}
	}
}