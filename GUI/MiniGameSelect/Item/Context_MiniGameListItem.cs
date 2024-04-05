using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace CTC.GUI.MiniGameSelect.Item
{
	public class Context_MiniGameListItem : Context
	{
		private View_MiniGameSelect _miniGameSelectView;

		private readonly Property<Sprite> miniGameSpriteProperty = new();
		public Sprite MiniGameSprite
		{
			get => miniGameSpriteProperty.Value;
			set => miniGameSpriteProperty.Value = value;
		}

		private readonly Property<string> miniGameNameProperty = new();
		public string MiniGameName
		{
			get => miniGameNameProperty.Value;
			set => miniGameNameProperty.Value = value;
		}

		private readonly Property<int> miniGameIndexProperty = new();
		public int MiniGameIndex
		{
			get => miniGameIndexProperty.Value;
			set => miniGameIndexProperty.Value = value;
		}

		private readonly Property<int> miniGameVoteCountProperty = new();
		public int MiniGameVoteCount
		{
			get => miniGameVoteCountProperty.Value;
			set => miniGameVoteCountProperty.Value = value;
		}

		private readonly Property<bool> isSelectableProperty = new();
		public bool IsSelectable
		{
			get => isSelectableProperty.Value;
			set => isSelectableProperty.Value = value;
		}

		public void BindMiniGameSelectView(View_MiniGameSelect miniGameSelectView)
		{
			_miniGameSelectView = miniGameSelectView;
		}

		public void GUI_OnClick_MiniGame()
		{
			if (ReferenceEquals(_miniGameSelectView, null))
				return;

			_miniGameSelectView.OnClick_MiniGame(this);
		}
	}
}
