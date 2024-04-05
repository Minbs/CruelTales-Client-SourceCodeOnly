namespace CTC.DataBind.Setters
{
	using Slash.Unity.DataBind.Core.Presentation;
	using Slash.Unity.DataBind.Foundation.Setters;
	using TMPro;
	using UnityEngine;

	[AddComponentMenu("Custom Data Bind/Setters/[CDB] Text Mesh Setter")]
	public class TextMeshSetter : ComponentSingleSetter<TextMeshProUGUI, string>
	{
		protected override void UpdateTargetValue(TextMeshProUGUI target, string value)
		{
			target.text = value;
		}

		protected override void Reset()
		{
			base.Reset();
			ResetComponent();
		}

		public void ResetComponent()
		{
			this.Data.Path = gameObject.name.Replace(Global.DataBind.ComponentPrefix.TEXT, string.Empty);
		}
	}
}
