using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CT.Logger;
using CTC.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.SystemCore
{
	[Serializable]
	public struct FieldItemSpriteSet
	{
		public FieldItemType ItemType;
		public Sprite Sprite;

		public FieldItemSpriteSet(FieldItemType itemType, Sprite sprite)
		{
			ItemType = itemType;
			Sprite = sprite;
		}
	}

	public class GameplayResourcesManager : MonoBehaviour, IManager, IInitializable
	{
		private readonly static ILog _log = LogManager.GetLogger(typeof(GameplayResourcesManager));

		private bool _isInitialize = false;

		[SerializeField]
		private List<FieldItemSpriteSet> _fieldItemSpriteSetList = new();
		private Dictionary<FieldItemType, Sprite> _spriteByItemType = new();

		public void Awake()
		{
			Initialize();
		}

		public void Initialize()
		{
			if (_isInitialize)
				return;

			foreach (var i in _fieldItemSpriteSetList)
			{
				_spriteByItemType.Add(i.ItemType, i.Sprite);
			}

			_isInitialize = true;
		}

		public bool IsInitialized()
		{
			return _isInitialize;
		}

		public void Release()
		{
			_isInitialize = false;

			_spriteByItemType.Clear();
		}

		public Sprite GetFieldItem(FieldItemType itemType)
		{
			if (_spriteByItemType.TryGetValue(itemType, out Sprite spr))
			{
				return spr;
			}

			return _spriteByItemType[FieldItemType.None];
		}

#if UNITY_EDITOR
		[Button]
		public void RefreshResourcesBinding()
		{
			_fieldItemSpriteSetList.Clear();

			Dictionary<string, FieldItemType> lowerStrByEnum = new();
			foreach (FieldItemType t in Enum.GetValues(typeof(FieldItemType)))
			{
				string name = "spr_item_" + t.ToString().ToLower();
				lowerStrByEnum.Add(name, t);
			}

			string prefabPath = "Sprites/FieldItems/";
			var sprites = AssetLoader.GetAssetsFromPath<Sprite>(prefabPath);
			foreach (var spr in sprites)
			{
				if (!lowerStrByEnum.TryGetValue(spr.name, out FieldItemType itemType))
				{
					_log.Warn($"There is no matched type for sprite : {spr.name}");
					continue;
				}

				_fieldItemSpriteSetList.Add(new FieldItemSpriteSet(itemType, spr));
			}
		}
#endif
	}
}
