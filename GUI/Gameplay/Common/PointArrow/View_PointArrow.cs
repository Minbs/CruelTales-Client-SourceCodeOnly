using System;
using System.Collections.Generic;
using CT.Logger;
using CTC.GUI.Gameplay.Overlay;
using CTC.Utils;
using UnityEngine;

namespace CTC.GUI.Gameplay.Common.PointArrow
{
	public class View_PointArrow : ViewBase
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(View_PlayerInfoList));

		public GameObject PointArrowItem;
		public Transform PointArrowTransform;

		private Queue<Item_PointArrow> _pointAllows = new();
		public int ArrowCount => _pointAllows.Count;

		private MonoObjectPoolService _objectPool;

		private void Awake()
		{
			_objectPool = new MonoObjectPoolService(PointArrowTransform);
		}

		public void OnUpdate(Span<Vector3> directions, int count)
		{
			int gap = count - ArrowCount;
			if (gap < 0)
			{
				releaseItem(-gap);
			}
			else if (gap > 0)
			{
				createItem(gap);
			}
			
			int i = 0;

			foreach (var p in _pointAllows)
			{
				if (i >= count)
					break;
				p.SetDirection(directions[i++]);
			}
		}

		public void Clear()
		{
			releaseItem(ArrowCount);
		}

		private void createItem(int count)
		{
			for (int i = 0; i < count; i++)
			{
				GameObject go = _objectPool.CreateObject(PointArrowItem);
				var point = go.GetComponent<Item_PointArrow>();
				go.GetComponent<RectTransform>().localPosition = Vector3.zero;
				_pointAllows.Enqueue(point);
			}
		}

		private void releaseItem(int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (!_pointAllows.TryDequeue(out var point))
					break;
				_objectPool.Release(point.gameObject);
			}
		}
	}
}
