using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace CTC.Tests.Execution
{
	public class ExecutionManager : MonoBehaviour
	{
		public GameObject[] ExecutionScenes;
		
		[ShowInInspector]
		private Dictionary<Tuple<ExecutionCutSceneType, int>, ExecutionScene> _executionDic = new();
		
		private ExecutionScene _curExectionScene = null;
		private GameObject _originalCam = null;
		
		private ILog _log = LogManager.GetLogger(typeof(ExecutionManager));
		
		private void Awake()
		{
			foreach (var VARIABLE in ExecutionScenes)
			{
				if (VARIABLE.TryGetComponent(out ExecutionScene exeScene))
				{
					_executionDic.Add(new Tuple<ExecutionCutSceneType, int>
						(exeScene.ExeCutSceneType, exeScene.CurDokza), exeScene);
				}
			}

			_originalCam = GameObject.FindGameObjectWithTag("MainCamera");
		}

		/// <summary>
		/// 처형 씬을 재생합니다.
		/// </summary>
		/// <param name="exeCutSceneType"></param>
		/// <param name="skinSetArr"></param>
		public void PlayExecutionScene(ExecutionCutSceneType exeCutSceneType, params SkinSet[] skinSetArr)
		{
			// 기존 처형씬 제거
			if (!ReferenceEquals(_curExectionScene, null))
			{
				Destroy(_curExectionScene.gameObject);
				_curExectionScene = null;
			}

			
			// 우선 1명일 때 처형씬 존재 여부를 확인
			if (!_executionDic.TryGetValue(new Tuple<ExecutionCutSceneType, int>(exeCutSceneType, 1),
				    out ExecutionScene confirmExeScene))
			{
				activeOriginalCam();
				_log.Error("There is no such ExeScene in ExecutionManager");
				return;
			}


			ExecutionScene resultScene = null;
			if (skinSetArr.Length > confirmExeScene.MaxDokza)
				_executionDic.TryGetValue(new Tuple<ExecutionCutSceneType, int>
					(exeCutSceneType, confirmExeScene.MaxDokza), out resultScene);
			else
				_executionDic.TryGetValue(new Tuple<ExecutionCutSceneType, int>
					(exeCutSceneType, skinSetArr.Length), out resultScene);


			if (ReferenceEquals(resultScene, null))
			{
				activeOriginalCam();
				_log.Error($"There is {confirmExeScene.ExeCutSceneType}in ExecutionManager, But There is an error.");
				return;
			}
			
			_curExectionScene = Instantiate(resultScene, Vector3.zero, Quaternion.identity, transform);
			_curExectionScene.Init(this);
			_curExectionScene.Play(skinSetArr);

			_originalCam.SetActive(false);
		}

		/// <summary>
		/// 테스트용 함수입니다. 코드로 호출 금지
		/// </summary>
		/// <param name="exeCutSceneType"></param>
		/// <param name="number"></param>
		/// <param name="skinSetIDX"></param>
		[Button]
		public void Test_PlayExecutionScene(ExecutionCutSceneType exeCutSceneType, int number, int skinSetIDX)
		{
			List<SkinSet> skinSets = new();
			for (int i = 0; i < number; i++)
				skinSets.Add(SkinSetHelper.GetDefaultSkinSet(skinSetIDX));
			
			PlayExecutionScene(exeCutSceneType, skinSets.ToArray());
		}

		public void OnSceneEnd()
		{
			Destroy(_curExectionScene.gameObject);
			_curExectionScene = null;
			
			_originalCam.SetActive(true);
		}

		private void activeOriginalCam()
		{
			if (!ReferenceEquals(_originalCam, null))
			{
				_originalCam.SetActive(true);
			}
		}
	}
}