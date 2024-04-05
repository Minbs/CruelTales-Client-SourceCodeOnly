namespace CTC.DataBind.Setters
{
	using Slash.Unity.DataBind.Foundation.Setters;
	using UnityEngine;
	using UnityEngine.UI;

	[AddComponentMenu("Custom Data Bind/Setters/[CDB] Raw Image Setter")]
	public class RawImageSetter : ComponentSingleSetter<RawImage, Texture2D>
	{
		protected override void UpdateTargetValue(RawImage target, Texture2D value)
		{
			target.texture = value;
		}
	}
}
