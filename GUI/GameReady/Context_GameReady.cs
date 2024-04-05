using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.GameReady
{
	public class Context_GameReady : ContextWithView<View_GameReady>
	{
		private readonly Property<string> miniGameNameProperty = new();
		public string MiniGameName
		{
			get => miniGameNameProperty.Value;
			set => miniGameNameProperty.Value = value;
		}

		private readonly Property<int> miniGameChapterProperty = new();
		public int MiniGameChapter
		{
			get => miniGameChapterProperty.Value;
			set => miniGameChapterProperty.Value = value;
		}
	}
}
