using System;
using System.Collections.Generic;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay;
using UnityEngine;

namespace CTC.Gameplay.Proxies
{
	[Serializable]
	public struct DueoksiniTablePivot
	{
		public FieldItemType FieldItem;
		public FieldItemProxy Proxy;
	}

	public class DueoksiniTableProxy : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer _renderer;

		[SerializeField]
		private Sprite _redTeamTable;

		[SerializeField]
		private Sprite _blueTeamTable;

		[SerializeField]
		private List<DueoksiniTablePivot> _itemPivots = new();
		private Dictionary<FieldItemType, List<FieldItemProxy>> _pivotTable = new();

		public void Initialized()
		{
			foreach (var pivot in _itemPivots)
			{
				FieldItemType itemType = pivot.FieldItem;
				FieldItemProxy pivotTransform = pivot.Proxy;

				if (!_pivotTable.ContainsKey(itemType))
				{
					_pivotTable.Add(itemType, new List<FieldItemProxy>());
				}

				_pivotTable[itemType].Add(pivotTransform);
			}
		}

		public void SetFaction(Faction fation)
		{
			_renderer.sprite = fation == Faction.Red ?
				_redTeamTable : _blueTeamTable;
		}

		public void OnItemChanged(SyncDictionary<NetInt32, NetByte> itemCountTable)
		{
			foreach (var typeIndex in itemCountTable.Keys)
			{
				FieldItemType itemType = (FieldItemType)typeIndex.Value;
				int count = itemCountTable[typeIndex];
				if (_pivotTable.TryGetValue(itemType, out var proxies))
				{
					foreach (var proxy in proxies)
					{
						FieldItemType proxyItem = (count-- > 0) ?
							itemType : FieldItemType.None;
						proxy.Initialize(proxyItem);
					}
				}
			}
		}
	}
}