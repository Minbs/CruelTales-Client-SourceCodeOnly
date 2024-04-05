using System;
using System.Collections.Generic;
using CT.Common.Gameplay;
using CTC.GUI.Components.Timer;
using CTC.GUI.MiniGameSelect.Item;
using CTC.Utils;
using Cysharp.Threading.Tasks.Triggers;
using Sirenix.OdinInspector;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace CTC.GUI.MiniGameSelect
{
	[Serializable]
	public struct MIniGameInfo
	{
		public Sprite Sprite;
		public string Name;
	}

	public class View_MiniGameSelect : ViewBaseWithContext
	{


		[field: SerializeField]
		private List<MIniGameInfo> _MiniGamesList = new();

		[field: SerializeField]
		private List<GameObject> _miniGameSelectItemList = new();
		private Dictionary<int, Context_MiniGameListItem> _contextItemByIndex = new();

		#region PlayerVote
		[TabGroup("Player Vote")]
		public GameObject PlayerVoteIcon;
		[TabGroup("Player Vote")]
		public Transform MiniGameVoteLayout;
		//	[TabGroup("Player Vote")]
		//	public int PlayerVoteIconVerticalSpace; // 플레이어 투표 아이콘 간격
		private Dictionary<string, (GameObject GameObject, Context_VoteMiniGameItem Context)> _contextVoteItemByName = new(); // 투표한 플레이어 테이블
		#endregion

		[field: TitleGroup("Timer"), SerializeField]
		public ClockTimer ClockTimer;
		public TextTimer TextTimer;


		private MonoObjectPoolService _objectPool;
		protected override void Awake()
		{
			base.Awake();
			_objectPool = new MonoObjectPoolService(MiniGameVoteLayout);
			InitializeTimer(60);
			SetVoteTimeLeft(60);
		}

		protected override void Start()
		{
			base.Start();
			int count = 0;

			foreach (var item in _miniGameSelectItemList)
			{
				var context = item.GetComponent<ContextHolder>().Context as Context_MiniGameListItem;
				if (_MiniGamesList.Count <= 0)
				{
					//context.MiniGameSprite = miniGameInfo.Sprite;
					context.MiniGameName = "게임 준비중";
					context.MiniGameIndex = count;
					context.IsSelectable = false;
				}
				else
				{
					int rand = UnityEngine.Random.Range(0, _MiniGamesList.Count);
					MIniGameInfo miniGameInfo = _MiniGamesList[rand];

					if (context != null)
					{
						context.MiniGameSprite = miniGameInfo.Sprite;
						context.MiniGameName = miniGameInfo.Name;
						context.MiniGameIndex = count;
						context.IsSelectable = true;
						_contextItemByIndex.Add(count, context);
						context.BindMiniGameSelectView(this);
					}

					_MiniGamesList.RemoveAt(rand);
					count++;
				}
			}
		}

		protected override void onBeginShow()
		{
			//ClockTimer.Initialize(, 0);
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				AddPlayerVoteIcon(0, "A");
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				AddPlayerVoteIcon(1, "A");
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				AddPlayerVoteIcon(1, "D");
			}
		}

		public void OnClick_MiniGame(Context_MiniGameListItem context)
		{
			AddPlayerVoteIcon(context.MiniGameIndex, "나");
			Debug.Log(context.MiniGameName + "클릭");
		}

		public void AddPlayerVoteIcon(int miniGameIndex, string playerName)
		{
			if (_contextItemByIndex.TryGetValue(miniGameIndex, out Context_MiniGameListItem context))
			{
				if (_contextVoteItemByName.TryGetValue(playerName, out var contextTuple)) // 해당 플레이어가 이미투표했다면
				{
					if (contextTuple.Context.MiniGameVoteIndex == miniGameIndex) // 같은 미니게임 투표
						return;

					_contextItemByIndex[contextTuple.Context.MiniGameVoteIndex].MiniGameVoteCount--; // 원래 투표했던 게임 투표 수 차감
					_contextVoteItemByName.Remove(playerName);
					_objectPool.Release(contextTuple.GameObject);
				}

				// 오브젝트 풀링 및 Context 할당
				GameObject go = _objectPool.CreateObject(PlayerVoteIcon, Vector3.zero, Quaternion.identity);
				go.transform.SetParent(MiniGameVoteLayout.GetChild(miniGameIndex).Find("Layout_PlayerVoteIcon"));
				var contextHolder = go.GetComponent<ContextHolder>();
				contextHolder.Context = new Context_VoteMiniGameItem();
				var playerInfo = contextHolder.Context as Context_VoteMiniGameItem;
				playerInfo.PlayerName = playerName;
				playerInfo.MiniGameVoteIndex = miniGameIndex;

				_contextVoteItemByName.Add(playerName, (go, playerInfo));


				context.MiniGameVoteCount++;
			}
		}

		/*
	   [TabGroup("Player Vote"), Button]
	   public void BindPlayerPlayerVoteIconPivot()
	   {
		   _playerVoteIconPivot.Clear();
		   Transform layout = transform.Find("Layout_MiniGame");
		   for(int i = 0; i < layout.childCount; i++)
		   {
			   RectTransform pivot = layout.GetChild(i).Find("Pivot_PlayerVoteIcon").GetComponent<RectTransform>();
			   _playerVoteIconPivot.Add(pivot);
		   }
	   }
	   */

		public void InitializeTimer(float startTime)
		{
			ClockTimer.Initialize(startTime);
			TextTimer.Initialize(startTime);
		}

		public void SetVoteTimeLeft(float timeLeft)
		{
			// TODO : timer
			ClockTimer.SynchronizeTimer(timeLeft);
			TextTimer.SynchronizeTimer(timeLeft);

			if (!ClockTimer.IsRunning || !TextTimer.IsRunning)
			{
				ClockTimer.StartTimer();
				TextTimer.StartTimer();
			}
		}
	}
}
