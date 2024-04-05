using CT.Common.Gameplay;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CTC.GUI.Components
{
	public class PC_IconSetter : MonoBehaviour
	{
		public void ChangeCurrentSkin(SkinSet skinSet)
		{
			List<int> skinList = new List<int>
			{
				skinSet.Back,
				skinSet.Cheek,
				skinSet.Dress,
				skinSet.Eyes,
				skinSet.Eyebrow,
				skinSet.FaceAcc,
				skinSet.Hair,
				skinSet.HairAcc,
				skinSet.HairHelmet,
				skinSet.Headgear,
				skinSet.Lips,
				skinSet.Nose,
				skinSet.Shoes,
				skinSet.Hammer
			};

			/*
			Skin _newSkin = new Skin("_newSkin");

			foreach (var VARIABLE in skinList)
			{
				if (VARIABLE == 0) continue;

				Debug.Log(CostumeTable.GetValueOrDefault(VARIABLE));
				Skin skin = DokzaSkeletonAnimation.SkeletonData.FindSkin(CostumeTable.GetValueOrDefault(VARIABLE));
				if (skin != null)
				{
					_newSkin.AddSkin(skin);
					Debug.Log(skin.Name);
				}
			}
			DokzaSkeletonAnimation.Skeleton.SetSkin(_newSkin);
			DokzaSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
			DokzaSkeletonAnimation.AnimationState.Apply(_dokzaSkeleton);
			_currentSkinSet = skinSet;
			*/
		}
	}
}
