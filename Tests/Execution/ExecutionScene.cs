using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace CTC.Tests.Execution
{
	public abstract class ExecutionScene : MonoBehaviour
	{
		public abstract ExecutionCutSceneType ExeCutSceneType { get; protected set; }
		public abstract int MaxDokza { get; protected set; }
		public abstract int CurDokza { get; protected set; }

		public PlayableDirector Director;
		public List<SkeletonAnimation> DokzaModelList;
		public List<DokzaSkinHandler> DokzaSkinHandlerList;
		[HideInInspector] public ExecutionManager ExeManager;
		
		protected GameObject _dokzaModel;

		public void Init(ExecutionManager manager)
		{
			ExeManager = manager;
		}

		[Button]
		public abstract void Play(params SkinSet[] skinSets);

		public abstract void OnEnd();
	}
}