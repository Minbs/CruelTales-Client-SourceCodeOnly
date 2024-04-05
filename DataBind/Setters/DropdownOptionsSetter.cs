namespace CTC.DataBind.Setters
{
	using Slash.Unity.DataBind.Foundation.Setters;
	using TMPro;
	using UnityEngine;
	using System.Collections;
	using System.Linq;

	[AddComponentMenu("Custom Data Bind/Setters/[CDB] Drop Down Options Setter")]
	public class DropdownOptionsSetter : ComponentSingleSetter<TMP_Dropdown, IEnumerable>
	{
		protected override void UpdateTargetValue(TMP_Dropdown target, IEnumerable value)
		{
			target.ClearOptions();
			if (value != null)
			{
				target.AddOptions(value.OfType<TMP_Dropdown.OptionData>().ToList());
			}
		}
	}
}
