using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Players;
using CTC.Tests;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace CTC.Tests
{
	public class Test_DokzaInventory : MonoBehaviour
	{
		public AnimationCurve Curve;
		public Transform InventoryAnchor = null;
		public Test_Item[] ItemArray;

		public Test_Item CurrentHoldingItem { get; private set; } = null;
		public bool IsHoldingItem => !ReferenceEquals(CurrentHoldingItem, null);

		// Internal
		private Dictionary<ItemType, Test_Item> _itemDic = new();

		public void Awake()
		{
			if(ReferenceEquals(InventoryAnchor, null))
				Debug.LogError("There is no Anchor");

			foreach (var VARIABLE in ItemArray)
			{
				_itemDic.Add(VARIABLE.ItemType, VARIABLE);
			}
		}
		
		/// <summary>
		/// 아이템을 듭니다.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		[Button]
		public bool TryLayUpItem(ItemType itemType)
		{
			if (_itemDic.TryGetValue(itemType, out var item))
			{
				LayDownItem();
				
				var createdItem = GameObject.Instantiate(item, InventoryAnchor);
				createdItem.transform.localPosition = Vector3.zero;

				CurrentHoldingItem = createdItem;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// 아이템을 내려놓습니다.
		/// </summary>
		[Button]
		public void LayDownItem()
		{
			if (ReferenceEquals(CurrentHoldingItem, null))
				return;

			CurrentHoldingItem.DokzaInventory = this;
			CurrentHoldingItem.transform.parent = null;
			CurrentHoldingItem.DoJump(Curve);
			
			CurrentHoldingItem = null;
		}
	}
}