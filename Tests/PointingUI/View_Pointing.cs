using System;
using System.Collections.Generic;
using CTC.GUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Tests.PointingUI
{
	public class View_Pointing : ViewBaseWithContext
	{
		private const string SETUP_GROUP = "Setup Group";

		[SerializeField, TitleGroup(SETUP_GROUP)]
		private int PointingPoolLength = 10;
		
		[SerializeField, TitleGroup(SETUP_GROUP)]
		private GameObject PointingObject;

		[SerializeField, TitleGroup(SETUP_GROUP)]
		private GameObject ArrowAnchor;
		
		private const string TEST_GROUP = "Test Group";

		[SerializeField, TitleGroup(TEST_GROUP)]
		private Transform TargetTransform;

		[SerializeField, TitleGroup(TEST_GROUP)]
		private Camera MainCamera;
		
		
		private List<GameObject> _pointObjectList = new();
		
		protected override void Awake()
		{
			base.Awake();
			for (int i = 0; i < PointingPoolLength; i++)
			{
				_pointObjectList.Add(Instantiate(PointingObject, ArrowAnchor.transform));
			}

			for (int i = 0; i < PointingPoolLength; i++)
			{
				if (_pointObjectList[i].TryGetComponent(out RectTransform _rect))
				{
					_rect.anchoredPosition = Vector2.zero;
					_rect.gameObject.SetActive(false);
				}
			}
		}

		[Button]
		public void ExecutePointing(int _idx, Transform _target)
		{
			if (_idx < 0 || _idx >= PointingPoolLength)
				return;

			if (!_pointObjectList[_idx].activeSelf)
				_pointObjectList[_idx].SetActive(true);

			//Vector2 _gizonAnglr = Vector2.up;
			//Vector2 _
				
			//_pointObjectList[_idx].transform.rotation = Quaternion.Euler(0f,0f,  );
		}

		private void Update()
		{
			ArrowAnchor.transform.position = MainCamera.WorldToScreenPoint(TargetTransform.transform.position);
		}
	}
}