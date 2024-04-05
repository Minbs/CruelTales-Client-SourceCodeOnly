namespace CTC.DataBind.Commands
{
	using Slash.Unity.DataBind.UI.Unity.Commands;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;

	[AddComponentMenu("Custom Data Bind/Commands/[CDB] Button Click Command Binder")]
	public class ButtonClickCommandBinder : UnityEventCommand<Button>, IComponentResettable
	{
		protected override void Reset()
		{
			base.Reset();
			ResetComponent();
		}

		public void ResetComponent()
		{
			Path = gameObject.name.Replace
			(
				Global.DataBind.ComponentPrefix.BUTTON,
				Global.DataBind.Path.GUI_ON_CLICK
			);
		}

		protected override UnityEvent GetEvent(Button target)
		{
			return target.onClick;
		}
	}
}