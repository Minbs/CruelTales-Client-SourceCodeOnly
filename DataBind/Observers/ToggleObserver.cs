namespace CTC.DataBind.Observers
{
	using Slash.Unity.DataBind.Foundation.Observers;
	using UnityEngine.UI;

	/// <summary>
	///   Observes value changes of the text of an input field.
	/// </summary>
	public class ToggleObserver : ComponentDataObserver<Toggle, bool>
	{
		/// <inheritdoc />
		protected override void AddListener(Toggle target)
		{
			target.onValueChanged.AddListener(this.OnInputFieldValueChanged);
		}

		/// <inheritdoc />
		protected override bool GetValue(Toggle target)
		{
			return target.isOn;
		}

		/// <inheritdoc />
		protected override void RemoveListener(Toggle target)
		{
			target.onValueChanged.RemoveListener(this.OnInputFieldValueChanged);
		}

		private void OnInputFieldValueChanged(bool newValue)
		{
			this.OnTargetValueChanged();
		}
	}
}