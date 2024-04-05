#nullable enable

using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CTC.GUI.Gameplay.Common.PointArrow;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class WolfCharacter : PlayerCharacter
	{
		[SerializeField]
		private View_PointArrow _pointArrowView;

		public override void OnUpdate(float stepTime)
		{
			base.OnUpdate(stepTime);

			if (IsLocal)
			{
				if (WorldManager.TryGetNetworkObjectSetBy(NetworkObjectType.RedHoodCharacter, out var set))
				{
					if (GameplayController.SceneController == null)
						return;
					var mapData = GameplayController.SceneController.MapData;;

					Vector3 curPos = transform.position;
					Span<Vector3> dirs = stackalloc Vector3[set.Count];
					int dirCount = 0;
					foreach (PlayerCharacter p in set)
					{
						Vector3 distance;
						distance = p.transform.position - curPos;
						if (distance.magnitude < 8)
							continue;

						if (Section != p.Section)
						{
							SectionDirection secDir = new() { From = Section, To = p.Section };
							ushort combindedValue = secDir.GetCombinedValue();
							if (mapData.SectionDirectionTable.TryGetValue(combindedValue, out var dir))
							{
								distance = dir.ToUnityVector3() - curPos;
							}
						}

						dirs[dirCount++] = distance;
					}
					_pointArrowView.OnUpdate(dirs, dirCount);
				}
				else
				{
					_pointArrowView.Clear();
				}
			}
			else
			{
				_pointArrowView.Clear();
			}
		}

		public override void Server_BroadcastOrderTest(int userId, int fromSever)
		{
			Debug.Log("TO Wolf");

			if ((int)UserId.Id != userId)
				return;

			switch (fromSever)
			{
				case 1:
					Debug.Log("Wolf Default SkinSet Enabled");
					DokzaSkinHandler.ApplySkin(new List<int>(DokzaSkinHandler.DEFAULT_WOLFSKINSET));
					break;
			}
		}
	}
}