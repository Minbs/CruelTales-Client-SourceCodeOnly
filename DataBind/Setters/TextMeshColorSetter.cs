namespace CruelTales.DataBind.Setters
{
	using Slash.Unity.DataBind.Foundation.Setters;
	using TMPro;
	using UnityEngine;

	[AddComponentMenu("Custom Data Bind/Setters/[CDB] Text Mesh Color Setter")]
	public class TextMeshColorSetter : ComponentSingleSetter<TextMeshProUGUI, Color>
	{
		protected override void UpdateTargetValue(TextMeshProUGUI target, Color value)
		{
			target.color = value;
		}
	}
}
