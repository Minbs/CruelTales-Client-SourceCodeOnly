using System.Collections;
using System.Collections.Generic;
using CTC.Data.Spine;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Data/Gameplay/Data_RumblePattern")]
public class Data_RumblePattern : ScriptableObject
{
	public bool IsNotBasedAnim = false;
	public float RumbleTime = 0f;
	public AnimationCurve RumbleCurve;
	
	[ValueDropdown(nameof(dokzaAnimList), ExpandAllMenuItems = true)]
	public string BasedAnimation;
	public bool IsLoop = false;

	private IEnumerable dokzaAnimList => DokzaAnimationVDLExtension.DokzaAnimationVDL;
}