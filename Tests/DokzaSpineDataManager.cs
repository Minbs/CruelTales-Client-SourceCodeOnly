using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CT.Common.Gameplay.Players;
using CT.Logger;
using CTC.SystemCore;
using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

namespace CTC.Tests
{
	public class DokzaSpineDataManager : MonoBehaviour, IManager
	{
		// Publics
		public SkeletonDataAsset DokzaSkeletonDataAsset;
		
		// Privates
		[ShowInInspector] private Dictionary<int, Skin> _skinSQLDic = new Dictionary<int, Skin>();

		public bool IsSQLInit
		{
			get { return _skinSQLDic is { Count: > 0 }; }
		}

		// Const Variables
		private const string DBNAME = "/SkinSQLDB.db";
		private const string TABLENAME = "SpineSkinSQL";
		private string DBCONNECTIONSTRING
		{
			get
			{
				return $"Data Source ={Application.streamingAssetsPath}{DBNAME}";
			}
		}
		
		// Log
		private static readonly ILog _log = LogManager.GetLogger(typeof(DokzaSpineDataManager));
		
		
		public void Initialize()
		{
			readSkinDB();
			initDokzaAnimSkinDic();
			initDokzaBoneNameDic();
		}

		public void Release()
		{
			_skinSQLDic.Clear();
		}

		/// <summary>
		/// SQL DB를 통해 내부에 데이터를 캐싱합니다.
		/// </summary>
		private void readSkinDB()
		{
			var skeletonData = DokzaSkeletonDataAsset.GetSkeletonData(true);

			_skinSQLDic.Clear();
		    
			IDbConnection dbConnection = new SqliteConnection(DBCONNECTIONSTRING);
			dbConnection.Open();

			IDbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = $"SELECT * FROM {TABLENAME}";

			IDataReader dataReader = dbCommand.ExecuteReader();

			while (dataReader.Read())
			{
				int skinID = dataReader.GetInt32(0);
				string skinPath = dataReader.GetString(1);
				_skinSQLDic.Add(skinID, skeletonData.FindSkin(skinPath));
			}
		}

		private List<Skin> _returnSkinList = new();
		/// <summary>
		/// 요청한 Skin ID를 기반으로 확실한 Skin Path를 리턴받습니다.
		/// </summary>
		/// <param name="requestSkinIDList"></param>
		/// <returns></returns>
		public List<Skin> GetSkinList(List<int> requestSkinIDList, bool debug = false)
		{
			if (!IsSQLInit)
			{
				_log.Warn("SpineSQLManager에서 Dictionary가 초기화 되지 않았습니다.");
				readSkinDB();
			}
			
			_returnSkinList.Clear();

			foreach (var skinID in requestSkinIDList)
			{
				if (_skinSQLDic.TryGetValue(skinID, out Skin skin))
				{
					_returnSkinList.Add(skin);
				}
				
				if(debug)
					_log.Debug(skin);
			}
			
			return _returnSkinList;
		}

		/// <summary>
		/// Debug.Log를 사용해 현재 DB를 읽고 모든 스킨 목록을 출력합니다.
		/// </summary>
		[Button]
		private void printSkinDB()
		{
			if(!IsSQLInit)
				readSkinDB();

			string log = "";
			foreach (var VARIABLE in _skinSQLDic)
			{
				log += VARIABLE.Key.ToString() + ", " + VARIABLE.Value.ToString() + "\n";
			}
			
			Debug.Log(log);
		}
		
		/// <summary>
		/// *** Spine SQL DB를 초기화합니다. ***
		/// </summary>
		/// <param name="skeletonDataAsset"></param>
		[PropertySpace(50f), Button(ButtonSizes.Medium)]
		private void UNSAFE_ResetSQLDB(SkeletonDataAsset skeletonDataAsset)
		{
			IDbConnection dbConnection = new SqliteConnection(DBCONNECTIONSTRING);
			dbConnection.Open();

			IDbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = $"DELETE FROM {TABLENAME}";
			dbCommand.ExecuteReader();

			List<string> _skeletonDataSkinList = new();
			foreach (var VARIABLE in skeletonDataAsset.GetSkeletonData(true).Skins)
			{
				if(VARIABLE.ToString().Contains("/"))
					_skeletonDataSkinList.Add(VARIABLE.ToString());
			}

			Dictionary<string, int> _skinTypeDic = new();
			int _skinTypeIDX = 1000001;
			foreach (var VARIABLE in _skeletonDataSkinList)
			{
				string skinType = VARIABLE.Split("/")[0];
				if (!_skinTypeDic.Keys.Contains(skinType))
				{
					_skinTypeDic.Add(skinType, _skinTypeIDX);
					_skinTypeIDX += 1000000;
				}
			}

			foreach (var VARIABLE in _skeletonDataSkinList)
			{
				if (_skinTypeDic.Keys.Contains(VARIABLE.Split("/")[0]))
				{
					int itemID = _skinTypeDic[VARIABLE.Split("/")[0]];
					string name = VARIABLE;
					
					IDbCommand insertCommand = dbConnection.CreateCommand();
					insertCommand.CommandText =
						$"INSERT INTO {TABLENAME} (ID, NAME) VALUES ({itemID}, '{name}')";
					insertCommand.ExecuteReader();
					
					_skinTypeDic[VARIABLE.Split("/")[0]]++;
				}
			}
		}

		#region DokzaAnimationSkinBone
		
		private Dictionary<DokzaAnimationState, Skin> _dokzaAnimSkinDic = new();

		private void initDokzaAnimSkinDic()
		{
			var skeletonData = DokzaSkeletonDataAsset.GetSkeletonData(true);
			
			_dokzaAnimSkinDic.Add(DokzaAnimationState.Event_RedHood_Clean1, 
				skeletonData.FindSkin("Animation/Redhood_mission/Clean1"));
			
			_dokzaAnimSkinDic.Add(DokzaAnimationState.Event_RedHood_Flower,
				skeletonData.FindSkin("Animation/Redhood_mission/Flower"));
			
			_dokzaAnimSkinDic.Add(DokzaAnimationState.Event_RedHood_Food,
				skeletonData.FindSkin("Animation/Redhood_mission/Food"));
			
			_dokzaAnimSkinDic.Add(DokzaAnimationState.Event_RedHood_Herb,
				skeletonData.FindSkin("Animation/Redhood_mission/Herb"));
			
			_dokzaAnimSkinDic.Add(DokzaAnimationState.Event_RedHood_Stump,
				skeletonData.FindSkin("Animation/Redhood_mission/Stump"));
		}

		/// <summary>
		/// 미리 캐싱한 Animation을 위한 Skin을 Return합니다.
		/// </summary>
		/// <param name="animState"></param>
		/// <param name="animSkin"></param>
		/// <returns></returns>
		public bool TryGetSkinBone(DokzaAnimationState animState, out Skin animSkin)
		{
			return _dokzaAnimSkinDic.TryGetValue(animState, out animSkin);
		}
		
		#endregion

		
		
		#region DokzaAnimationBoneFollow

		private Dictionary<DokzaAnimationState, string> _dokzaBoneNameDic = new();

		private void initDokzaBoneNameDic()
		{
			_dokzaBoneNameDic.Add(DokzaAnimationState.Event_RedHood_Clean1, "Clean1_effect");
		}

		public bool TryGetBoneName(DokzaAnimationState animState, out string bonePath)
		{
			return _dokzaBoneNameDic.TryGetValue(animState, out bonePath);
		}

		#endregion



		#region DokzaAnimationPath
		
		/// <summary>
		/// bool은 IsFront를 의미
		/// </summary>
		private Dictionary<Tuple<DokzaAnimationState, bool>, string> _dokzaAnimationPathDic = new()
		{
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Idle, true), "Idle_Front" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Idle, false), "Idle_Back" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Walk, true), "Walk_Front" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Walk, false), "Walk_Back" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Run, true), "Run_Front" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Run, false), "Run_Back" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_Hammer, true), "Action_Push" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_Hammer, false), "Action_Push" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_WolfCatch, true), "Action_wolf_attack_side" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Action_WolfCatch, false), "Action_wolf_attack_up" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Knockback, true), "Action_Pushed" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Knockback, false), "Action_Pushed" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Event_RedHood_Bird, true), "Redhood_mission/Bird" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Event_RedHood_Bird, false), "Redhood_mission/Bird" },
			
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Event_RedHood_Clean1, true), "Redhood_mission/Clean1" },
			{ new Tuple<DokzaAnimationState, bool>(DokzaAnimationState.Event_RedHood_Clean1, false), "Redhood_mission/Clean1" },
		};
		
		public bool TryGetDokzaAnimPath(DokzaAnimationState state, ProxyDirection direction, out string path)
		{
			return _dokzaAnimationPathDic.TryGetValue(Tuple.Create(state, direction.IsDown()), out path);
		}

		#endregion
	}
}