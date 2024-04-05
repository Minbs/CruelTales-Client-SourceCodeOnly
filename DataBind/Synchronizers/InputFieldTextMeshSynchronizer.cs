namespace CTC.DataBind.Synchronizers
{
    using CTC.DataBind.Observers;
    using Slash.Unity.DataBind.Foundation.Synchronizers;
    using TMPro;
    using UnityEngine;

	/// <summary>
	///   Synchronizer for the text of a <see cref="TMP_InputField"/>.
	/// </summary>
	[AddComponentMenu("Data Bind/UnityUI/Synchronizers/[CDB] Input Field Text Mesh Synchronizer (Unity)")]
    public class InputFieldTextMeshSynchronizer : ComponentDataSynchronizer<TMP_InputField, string>
    {
        /// <summary>
        ///     If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).
        /// </summary>
        [Tooltip(
            "If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).")]
        public bool OnlyUpdateValueOnEndEdit;

        private InputFieldTextMeshObserver observer;

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            if (this.observer != null)
            {
                this.observer.ValueChanged -= this.OnObserverValueChanged;
                this.observer = null;
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            var target = this.Target;
            if (target != null)
            {
                this.observer = new InputFieldTextMeshObserver
				{
                    OnlyUpdateValueOnEndEdit = this.OnlyUpdateValueOnEndEdit,
                    Target = target
                };
                this.observer.ValueChanged += this.OnObserverValueChanged;
            }
        }

        /// <inheritdoc />
        protected override void SetTargetValue(TMP_InputField target, string newContextValue)
        {
            target.text = newContextValue;
        }

        private void OnObserverValueChanged()
        {
            this.OnComponentValueChanged(this.Target.text);
        }
    }
}