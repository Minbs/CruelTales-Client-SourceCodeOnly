using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace CTC.GUI.MiniGameSelect.Item
{
	public class Context_VoteMiniGameItem : Context
	{
		private readonly Property<Sprite> playerIconProperty = new();
		public Sprite PlayerIcon
		{
			get => playerIconProperty.Value;
			set => playerIconProperty.Value = value;
		}

		private readonly Property<string> playerNameProperty = new();
		public string PlayerName
		{
			get => playerNameProperty.Value;
			set => playerNameProperty.Value = value;
		}

		public int MiniGameVoteIndex { get; set; }// 투표한 게임의 번호
	}
}
