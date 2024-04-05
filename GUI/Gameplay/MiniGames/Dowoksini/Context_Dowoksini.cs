using CTC.DataBind.Contexts;
using CTC.Gameplay.MiniGames.Dowoksini;
using Slash.Unity.DataBind.Core.Data;
using System.Linq;

namespace CTC.GUI.Gameplay.MiniGames.Dowoksini
{
	public class Context_Dowoksini : ContextWithView<View_Dowoksini>
	{
		private readonly Property<string> goalTitleProperty = new();
		private readonly Property<string> goalContentProperty = new();
		private readonly Property<int> redTeamScoreProperty = new();
		private readonly Property<int> blueTeamScoreProperty = new();
		private readonly Property<float> remainTimeProperty = new();

		public string GoalTitle
		{
			get => goalTitleProperty.Value;
			set => goalTitleProperty.Value = value;
		}

		public string GoalContent
		{
			get => goalContentProperty.Value;
			set => goalContentProperty.Value = value;
		}

		public int RedTeamScore
		{
			get => redTeamScoreProperty.Value;
			set => redTeamScoreProperty.Value = value;
		}

		public int BlueTeamScore
		{
			get => blueTeamScoreProperty.Value;
			set => blueTeamScoreProperty.Value = value;
		}

		public float RemainTime
		{
			get => remainTimeProperty.Value;
			set => remainTimeProperty.Value = value;
		}
	}
}
