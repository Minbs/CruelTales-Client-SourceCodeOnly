#if UNITY_EDITOR

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace CTC.Tests
{
	public class Test_PrefabReader : MonoBehaviour
	{
		[Button]
		public void ReadnApplyAllRenderers()
		{
			GameObject[] objList = GameObject.FindObjectsOfType<GameObject>();
			
			foreach (var VARIABLE in objList)
			{
				if (VARIABLE.TryGetComponent(out SpriteRenderer renderer))
				{
					if (renderer.shadowCastingMode != ShadowCastingMode.On || renderer.receiveShadows == false)
					{
						PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(VARIABLE);

						var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
							PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(VARIABLE));

						foreach (var spriteRenderer in prefab.GetComponentsInChildren<SpriteRenderer>())
						{
							spriteRenderer.shadowCastingMode = ShadowCastingMode.On;
							spriteRenderer.receiveShadows = true;
						}

						PrefabUtility.SavePrefabAsset(prefab);
						//PrefabUtility.ApplyObjectOverride(prefab, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(VARIABLE), InteractionMode.UserAction);
						//EditorUtility.SetDirty(prefab);

						/*
						var asset =
							AssetDatabase.LoadAssetAtPath(
								PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(VARIABLE));
						*/
						//asset.Getcomponent<SpriteRenderer>()

						//PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.UserAction);
						//PrefabUtility.SavePrefabAsset(prefab);
					}
				}
			}
		}
	}
}

#endif