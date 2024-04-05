using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Tests
{
	public class Test_DowokItemTable : MonoBehaviour
	{
		public Transform[] ItemAnchors;

		private Dictionary<Transform, Test_Item> _itemTableDic = new();

		private void Awake()
		{
			foreach (var VARIABLE in ItemAnchors)
			{
				_itemTableDic.Add(VARIABLE, null);
			}
		}

		/// <summary>
		///	밥상에 아이템 올리기를 시도합니다.
		/// 실패 시 null을 반환합니다.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>아이템 위치 Transform</returns>
		[Button]
		public Transform TryPutItem(Test_Item item)
		{
			if (ReferenceEquals(item, null))
				return null;
			
			foreach (var VARIABLE in _itemTableDic)
			{
				if (ReferenceEquals(VARIABLE.Value, null))
				{
					Test_Item createdItem = GameObject.Instantiate(item, VARIABLE.Key);
					createdItem.transform.localPosition = Vector3.zero;
					_itemTableDic[VARIABLE.Key] = createdItem;
					return VARIABLE.Key;
				}
			}

			return null;
		}

		/// <summary>
		/// 밥상 위 아이템 제거를 시도합니다.
		/// </summary>
		/// <param name="itemSlot">아이템 위치 Transform</param>
		/// <returns></returns>
		[Button]
		private bool TryRemoveItem(Transform itemSlot)
		{
			if (ReferenceEquals(itemSlot, null))
				return false;

			if (_itemTableDic.TryGetValue(itemSlot, out Test_Item item))
			{
				GameObject.Destroy(item.gameObject);
				_itemTableDic[itemSlot] = null;
				return true;
			}
			
			return false;
		}
	}
}