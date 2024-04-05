using System;
using System.Collections;
using CT.Common.Gameplay.Players;
using CTC.Data.Spine;
using Sirenix.OdinInspector;
using UnityEngine;

/*
[Serializable]
public class DokzaAnimationSet
{
	[ValueDropdown(nameof(DokzaVDL), ExpandAllMenuItems = true)]
	public string Front = "None";
	[ValueDropdown(nameof(DokzaVDL), ExpandAllMenuItems = true)]
	public string Back = "None";

	public IEnumerable DokzaVDL => DokzaAnimationVDLExtension.DokzaAnimationVDL;

	public string GetAnimationBy(bool isLookUp)
	{
		return isLookUp ? Back : Front;
	}

	public string GetAnimationBy(ProxyDirection dokzaDirection)
	{
		return dokzaDirection switch
		{
			ProxyDirection.RightDown => Front,
			ProxyDirection.LeftDown => Front,
			ProxyDirection.RightUp => Back,
			ProxyDirection.LeftUp => Back,
			_ => Front
		};
	}
	
	public string GetAnimationBy(int isLookUp)
	{
		return isLookUp == 0 ? Front : Back;
	}
}

[CreateAssetMenu(menuName = "Data/Gameplay/Data_DokzaAnimationSet")]
public class Data_DokzaAnimationSet : ScriptableObject
{
	public DokzaAnimationSet Walk;
	public DokzaAnimationSet Idle;
	public DokzaAnimationSet Run;
	public DokzaAnimationSet Action;
	public DokzaAnimationSet Push;
	public DokzaAnimationSet Pushed;
	public DokzaAnimationSet AFK;
}
*/