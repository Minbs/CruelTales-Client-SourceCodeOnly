using System;
using System.Collections;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.Tests.Execution;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;


namespace CTC.SystemCore.Manager
{
	public class ExecutionResources : IManager	
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(MapResources));
		
		// Reference
		ResourcesManager _resourcesManager;
		
		[SerializeField]
		private Dictionary<ExecutionCutSceneType, GameObject> _exeSceneByExeType = new();

		private Dictionary<ExecutionCutSceneType, IResourceLocation> _exeCutSceneLocationDic = new();

		public ExecutionResources(ResourcesManager resourcesManager)
		{
			_resourcesManager = resourcesManager;
		}
		
		public void Initialize()
		{
			
		}

		public void Release()
		{
			throw new System.NotImplementedException();
		}

		public void LoadExecutionCutScene(ExecutionCutSceneType cutSceneType)
		{
			Addressables.LoadResourceLocationsAsync(_resourcesManager.ExecutionSceneLabel).Completed +=
				(handle) =>
				{
					var locations = handle.Result;
					foreach (var VARIABLE in locations)
					{
						string executionCutSceneTypeName = System.IO.Path.GetFileNameWithoutExtension(VARIABLE.PrimaryKey).
							Replace("ExecutionCutScene", string.Empty);
					}
				};
			
			
		}
	}
}