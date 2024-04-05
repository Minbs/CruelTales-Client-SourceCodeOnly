using CTC.DataBind.Contexts;
using UnityEngine;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.MiniGameSelect
{
	public class Context_MiniGameVoteResult : ContextWithView<View_MiniGameVoteResult>
	{
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
	}
}
