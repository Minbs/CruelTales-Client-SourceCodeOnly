using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Infos;
using CT.Common.Tools.Data;
using CT.Logger;
using CT.Networks;
using CTC.Locators;
using CTC.Physics;
using KaNet.Physics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Gameplay
{
	public class GameSceneMapController : MonoBehaviour
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(GameSceneMapController));

		[field: SerializeField]
		public GameModeType Mode;

		[field: SerializeField]
		public GameMapType Map { get; private set; }

		[field: SerializeField]
		public GameMapThemeType Theme { get; private set; }

		[SerializeField]
		private KaCollider[] _geomertyColliderArray;
		public IReadOnlyList<KaCollider> GeometryColliders => _geomertyColliderArray;

		public GameSceneMapData GetGameSceneMapData()
		{
			// Set this objects
			_geomertyColliderArray = GetComponentsInChildren<KaCollider>();

			// Create game scene map data
			GameSceneMapData data = new();

			// Set properties
			data.GameSceneIdentity = new GameSceneIdentity()
			{
				Mode = Mode,
				Map = Map,
				Theme = Theme,
			};

			// Set Interactors
			var interactors = GetComponentsInChildren<InteractorGizmo>();
			foreach (var interactor in interactors)
			{ 
				var info = interactor.CreateInfo();
				var table = data.InteractorTable;
				if (!table.ContainsKey(info.InteractorType))
				{
					table.Add(info.InteractorType, new List<InteractorInfo>());
				}
				data.InteractorTable[info.InteractorType].Add(info);
			}
			
			// Set Pivots
			var pivots = GetComponentsInChildren<PivotGizmo>();
			foreach (var pivot in pivots)
			{
				PivotInfo info = pivot.CreateInfo();
				if (pivot.Index >= GlobalNetwork.SYSTEM_PIVOT_INDEX_LIMIT)
				{
					data.PivotInfos.Add(info);
					continue;
				}

				// Set Spawn Positions
				Faction curFaction = (Faction)info.Index;
				if (!data.SpawnPositionTable.ContainsKey(curFaction))
				{
					data.SpawnPositionTable.Add(curFaction, new());
				}
				data.SpawnPositionTable[curFaction].Add(info.Position);
			}

			// Set Areas
			var areas = GetComponentsInChildren<AreaGizmo>();
			foreach (var area in areas)
			{
				var info = area.CreateInfo();
				data.AreaInfos.Add(info);
			}

			// Set Colliders
			List<ColliderInfo> environmentColliders = new();
			var colliders = _geomertyColliderArray;
			foreach (var c in colliders)
			{
				ColliderInfo info = c.CreateColliderInfo();
				if (!info.IsStatic)
					throw new ArgumentException("There is non-static collider exist!");
				environmentColliders.Add(info);
			}
			data.EnvironmentColliders = environmentColliders;

			// Set Section Direction Table
			string secDirPrefix = "SectionDirection_";
			var secDirs = this.FindComponents<Transform>((go) => go.IsMatch(secDirPrefix));
			foreach (var t in secDirs)
			{
				string[] dirToken = t.name.Replace(secDirPrefix, "").Split("to");
				byte from = byte.Parse(dirToken[0]);
				byte to = byte.Parse(dirToken[1]);
				SectionDirection secDir = new() { From = from, To = to };
				System.Numerics.Vector2 position = t.position.ToNativeVector2();
				data.SectionDirectionTable.Add(secDir.GetCombinedValue(), position);
			}

			return data;
		}

#if UNITY_EDITOR

		[Button(ButtonHeight = 30)]
		public void CreateGameSceneMapData()
		{
			var data = GetGameSceneMapData();

			if (!Global.Editor.Path.IsExistServerProject)
			{
				throw new Exception("There is no server project exist");
			}

			// Try save minigame data
			string minigameDataPath =
				Path.Combine(Global.Editor.Path.ServerProject,
							 $@"Data/MiniGameMapData/{Map}.json");

			var result = JsonHandler.TryWriteObject(minigameDataPath, data, true);
			if (result.ResultType != JobResultType.Success)
			{
				throw result.Exception;
			}
			else
			{
				_log.Info($"Save {Map} data succeeded!");
			}
		}

#endif
	}
}
