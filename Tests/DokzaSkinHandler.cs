using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CT.Logger;
using CTC.Data.Spine;
using CTC.SystemCore;
using CTC.Tests;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Mono.Data.Sqlite;
using AnimationState = Spine.AnimationState;
using Random = UnityEngine.Random;

namespace CTC
{
    public class DokzaSkinHandler : MonoBehaviour, IHasSkeletonDataAsset
    {
	    [SpineSlot] public string spineSlot;
	    public SkeletonAnimation DokzaSkeletonAnimation;
	    public SkeletonGraphic DokzaSkeletonGraphic;
	    
	    private Skeleton _dokzaSkeleton;
	    private SkeletonData _dokzaSkeletonData;
	    
		public SkeletonDataAsset SkeletonDataAsset { get; set; }
	    public bool IsSkinInitiated { get; private set; } = false;

	    private Skin _clearSkin = new Skin("_clearSkin");
	    private Color _HairColor = Color.white;
	    private Color _BodyColor = Color.white;
	    private Color _EyeColor = Color.white;

	    private static readonly ILog _log = LogManager.GetLogger(typeof(DokzaSkinHandler));
	    
	    #region DEFAULT_SKINSET
	    
	    public readonly int[] DEFAULT_SKINSET = new[]
	    {
		    2000003,
		    3000005,
		    4000001,
		    5000002,
		    6000001,
		    10000001,
		    13000001,
		    14000002,
		    15000005
	    };

	    public readonly int[] DEFAULT_WOLFSKINSET = new[]
	    {
		    2000003,
		    3000005,
		    4000001,
		    5000002,
		    6000001,
		    7000002,
		    13000001,
		    14000002,
		    15000005
	    };

	    public readonly int[] MISSION_SKINSET = new[]
	    {
		    2000003,
		    3000005,
		    4000001,
		    5000002,
		    6000001,
		    7000001,
		    13000001,
		    14000002,
		    15000005
	    };

	    #endregion

	    private void Start()
	    {
		    SkeletonDataAsset = getSkeletonDataAsset();
		    
		    // Dokza Skin Init
		    getSkeleton().SetSkin(_clearSkin);
		    getSkeleton().SetSlotsToSetupPose();
		    updateSkeletonAnimation();
	    }

	    private Vector3 localScale = Vector3.one;
	    /// <summary>
	    /// Dokza의 Still 애니메이션을 출력합니다.
	    /// </summary>
	    /// <param name="playStill">false일 경우 IDLE로 감</param>
	    [Button]
	    public void PlayStillPose(bool playStill)
	    {
		    if (!playStill)
		    {
			    localScale = transform.localScale;
			    localScale.x = Mathf.Abs(localScale.x);
			    transform.localScale = localScale;

			    getAnimationState().SetEmptyAnimation(0, 0f);
			    var idleEntry = getAnimationState().AddAnimation(0, "Idle_Front", true, 0f);
			    idleEntry.MixDuration = 1f;
			    return;
		    }

		    float isRight = Random.Range(0, 2) == 0 ? -1f : 1f;
		    
		    localScale = transform.localScale;
		    localScale.x *= isRight;
		    transform.localScale = localScale;

		    int _stillPoseIdx = Random.Range(1, 8);

		    string animName = "StillPose/StillPose_" + _stillPoseIdx.ToString();
		    var stillEntry = getAnimationState().SetAnimation(0, animName, true);
		    stillEntry.MixDuration = 0f;
	    }
	    
	    /// <summary>
	    /// 기본 스킨을 로드하여 적용합니다.
	    /// </summary>
	    [Button]
	    public void LoadDefaultSkin(int defaultSkinIdx)
	    {
		    if (GlobalService.DokzaSpineDataManager is null)
			    return;
		    
		    List<int> _defaultSkinSet = new List<int>();
		    switch (defaultSkinIdx)
		    {
			    case 0:
				    _defaultSkinSet.AddRange(DEFAULT_SKINSET);
				    break;
			    
			    case 1:
				    _defaultSkinSet.AddRange(DEFAULT_WOLFSKINSET);
				    break;
			    
			    case 2:
				    _defaultSkinSet.AddRange(MISSION_SKINSET);
				    break;
		    }
		    
		    ApplySkin(_defaultSkinSet);
		    
		    ChangeSlotColor(DokzaSkinSlot.HairnEyebrow, _HairColor);
		    ChangeSlotColor(DokzaSkinSlot.Body, _BodyColor);
		    ChangeSlotColor(DokzaSkinSlot.Eye, _EyeColor);
	    }

	    public enum DokzaSkinSlot
	    {
		    None = 0,
		    HairnEyebrow,
		    Body,
		    Eye
	    }
	    
	    /// <summary>
	    /// 특정 Slot의 컬러를 바꿉니다.
	    /// </summary>
	    /// <param name="skinSlot"></param>
	    /// <param name="color"></param>
	    [Button]
	    public void ChangeSlotColor(DokzaSkinSlot skinSlot, Color color)
	    {
		    _dokzaSkeleton = getSkeleton();
		    
		    switch (skinSlot)
		    {
			    case DokzaSkinSlot.HairnEyebrow:
				    _HairColor = color;
				    color.a = 1f;
				    _dokzaSkeleton.FindSlot("hair").SetColor(color);
				    _dokzaSkeleton.FindSlot("hair_B_long_L").SetColor(color);
				    _dokzaSkeleton.FindSlot("hair_B_long_R").SetColor(color);
				    _dokzaSkeleton.FindSlot("hair_B").SetColor(color);
				    _dokzaSkeleton.FindSlot("hairB_F").SetColor(color);
				    _dokzaSkeleton.FindSlot("hair_S_L").SetColor(color);
				    _dokzaSkeleton.FindSlot("hair_S_R").SetColor(color);
				    _dokzaSkeleton.FindSlot("eyebrow_R").SetColor(color);
				    _dokzaSkeleton.FindSlot("eyebrow_L").SetColor(color);
				    break;
			    
			    case DokzaSkinSlot.Body:
				    _BodyColor = color;
				    color.a = 1f;
				    _dokzaSkeleton.FindSlot("body").SetColor(color);
				    _dokzaSkeleton.FindSlot("head").SetColor(color);
				    _dokzaSkeleton.FindSlot("ear_L").SetColor(color);
				    _dokzaSkeleton.FindSlot("ear_R").SetColor(color);
				    _dokzaSkeleton.FindSlot("hand_L").SetColor(color);
				    _dokzaSkeleton.FindSlot("hand_R").SetColor(color);
				    _dokzaSkeleton.FindSlot("arm_L").SetColor(color);
				    _dokzaSkeleton.FindSlot("arm_R").SetColor(color);
				    break;
			    
			    case DokzaSkinSlot.Eye:
				    _EyeColor = color;
				    color.a = 1f;
				    _dokzaSkeleton.FindSlot("eye_L_color").SetColor(color);
				    _dokzaSkeleton.FindSlot("eye_R_color").SetColor(color);
				    break;
		    }
	    }

	    /// <summary>
	    /// SkinSet을 통해 Dokza의 Skin을 업데이트합니다.
	    /// 잘못된 Skin의 경우 자동 필터링됩니다.
	    /// </summary>
	    /// <param name="skinSet"></param>
	    public void ApplySkin(SkinSet skinSet)
	    {
		    List<int> skinList = new List<int>();
		    skinList.Add(skinSet.Back);
		    skinList.Add(skinSet.Cheek);
		    skinList.Add(skinSet.Dress);
		    skinList.Add(skinSet.Eyes);
		    skinList.Add(skinSet.Eyebrow);
		    skinList.Add(skinSet.FaceAcc);
		    skinList.Add(skinSet.Hair);
		    skinList.Add(skinSet.HairAcc);
		    skinList.Add(skinSet.HairHelmet);
		    skinList.Add(skinSet.Headgear);
		    skinList.Add(skinSet.Lips);
		    skinList.Add(skinSet.Nose);
		    skinList.Add(skinSet.Shoes);
		    skinList.Add(skinSet.Hammer);
		    
		    ApplySkin(skinList);
		    
		    ChangeSlotColor(DokzaSkinSlot.HairnEyebrow, skinSet.HairColor.ToUnityColor());
		    ChangeSlotColor(DokzaSkinSlot.Eye, skinSet.EyesColor.ToUnityColor());
		    ChangeSlotColor(DokzaSkinSlot.Body, skinSet.SkinColor.ToUnityColor());
	    }
	    
	    /// <summary>
	    /// Skin의 Idx를 통해 Dokza의 Skin을 업데이트합니다.
	    /// 잘못된 Idx는 자동 필터링됩니다.
	    /// </summary>
	    /// <param name="skinIdxList"></param>
	    public void ApplySkin(List<int> skinIdxList, bool debug = false)
	    {
		    if (GlobalService.DokzaSpineDataManager is null)
			    return;
		    
		    applySkinUsingSkin(GlobalService.DokzaSpineDataManager.GetSkinList(skinIdxList, debug));
	    }
	    
	    
	    /// <summary>
	    /// 정확한 Skin Path를 이용해 Dokza의 Skin을 업데이트합니다.
	    /// List는 정확해야 합니다.
	    /// </summary>
	    /// <param name="skinPathList"></param>
	    private void applySkinUsingSkin(List<Skin> skinList)
	    {
		    _clearSkin.Clear();
		    
		    _dokzaSkeleton = getSkeleton();
		    _dokzaSkeletonData = getSkeleton().Data;

		    foreach (var VARIABLE in skinList)
			    _clearSkin.AddSkin(VARIABLE);
		    
		    _resultSkin.Clear();
		    _resultSkin.AddSkin(_clearSkin);

		    if (_isAnimSkinUsing)
			    _resultSkin.AddSkin(_currentOnlyBoneSkin);

		    getSkeleton().SetSkin(_resultSkin);
		    _dokzaSkeleton.SetSlotsToSetupPose();
		    updateSkeletonAnimation();

		    IsSkinInitiated = true;
	    }

	    private void updateSkeletonAnimation()
	    {
		    if (!ReferenceEquals(DokzaSkeletonAnimation, null))
		    {
			    DokzaSkeletonAnimation.LateUpdate();
		    }
		    else if (!ReferenceEquals(DokzaSkeletonGraphic, null))
		    {
			    DokzaSkeletonGraphic.LateUpdate();
		    }
		    else
		    {
			    _log.Fatal("There is no SkeletonData");
		    }
	    }

	    private Skeleton getSkeleton()
	    {
		    if (!ReferenceEquals(DokzaSkeletonAnimation, null))
		    {
			    return DokzaSkeletonAnimation.skeleton;
		    }
		    else if (!ReferenceEquals(DokzaSkeletonGraphic, null))
		    {
			    return DokzaSkeletonGraphic.Skeleton;
		    }
		    else
		    {
			    _log.Fatal("There is no Skeleton");
			    return null;
		    }
	    }

	    private AnimationState getAnimationState()
	    {
		    if (!ReferenceEquals(DokzaSkeletonAnimation, null))
		    {
			    return DokzaSkeletonAnimation.state;
		    }
		    else if (!ReferenceEquals(DokzaSkeletonGraphic, null))
		    {
			    return DokzaSkeletonGraphic.AnimationState;
		    }
		    else
		    {
			    _log.Fatal("There is no AnimationState");
			    return null;
		    }
	    }

	    private SkeletonDataAsset getSkeletonDataAsset()
	    {
		    if (!ReferenceEquals(DokzaSkeletonAnimation, null))
		    {
			    return DokzaSkeletonAnimation.SkeletonDataAsset;
		    }
		    else if (!ReferenceEquals(DokzaSkeletonGraphic, null))
		    {
			    return DokzaSkeletonGraphic.SkeletonDataAsset;
		    }
		    else
		    {
			    _log.Fatal("There is no AnimationState");
			    return null;
		    }
	    }

	    private Skin _resultSkin = new Skin("_resultSkin");
	    private Skin _currentOnlyBoneSkin = new Skin("_currentOnlyBoneSkin");
	    private bool _isAnimSkinUsing = false;
	    
	    [Button]
	    public void SetSkinBone(DokzaAnimationState animState)
	    {
		    _currentOnlyBoneSkin.Clear();
		    ResetSkinToClearSkin();
		    
		    if (GlobalService.DokzaSpineDataManager.TryGetSkinBone(animState, out Skin onlyBoneSkin))
		    {
			    _currentOnlyBoneSkin.CopySkin(onlyBoneSkin);
			    
			    _resultSkin.Clear();
			    _resultSkin.AddSkin(_clearSkin);
			    _resultSkin.AddSkin(_currentOnlyBoneSkin);
			    
			    getSkeleton().SetSkin(_resultSkin);
			    updateSkeletonAnimation();
			    
			    ChangeSlotColor(DokzaSkinSlot.HairnEyebrow, _HairColor);
			    ChangeSlotColor(DokzaSkinSlot.Body, _BodyColor);
			    ChangeSlotColor(DokzaSkinSlot.Eye, _EyeColor);
			    
			    _isAnimSkinUsing = true;
		    }
		    else
		    {
			    _log.Warn($"해당 {animState}에 맞는 Skin이 없습니다.");
		    }
	    }

	    [Button]
	    public void ResetSkinToClearSkin()
	    {
		    getSkeleton().SetSkin(_clearSkin);
		    updateSkeletonAnimation();
		    
		    ChangeSlotColor(DokzaSkinSlot.HairnEyebrow, _HairColor);
		    ChangeSlotColor(DokzaSkinSlot.Body, _BodyColor);
		    ChangeSlotColor(DokzaSkinSlot.Eye, _EyeColor);
		    
		    _isAnimSkinUsing = false;
	    }
    }
}
