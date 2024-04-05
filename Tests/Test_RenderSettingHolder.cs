using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace CTC.Tests
{
	public class Test_RenderSettingHolder : MonoBehaviour
	{
		public bool CheckWhenInit = false;
		public AmbientMode SceneAmbientMode;
		public Color SkyColor;
		public Color EquatorColor;
		public Color GroundColor;

		
		[Button]
		public void SaveRenderSettings()
		{
			SceneAmbientMode = RenderSettings.ambientMode;
			
			SkyColor = RenderSettings.ambientSkyColor;
			EquatorColor = RenderSettings.ambientEquatorColor;
			GroundColor = RenderSettings.ambientGroundColor;
		}

		[Button]
		public void ApplyRenderSettings()
		{
			RenderSettings.ambientMode = SceneAmbientMode;
			
			RenderSettings.ambientSkyColor = SkyColor;
			RenderSettings.ambientEquatorColor = EquatorColor;
			RenderSettings.ambientGroundColor = GroundColor;
		}

		public void OnEnable()
		{
			if(CheckWhenInit)
				ApplyRenderSettings();
		}
	}
}