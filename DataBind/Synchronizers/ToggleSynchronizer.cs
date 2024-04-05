namespace CTC.DataBind.Synchronizers
{
    using CTC.DataBind.Observers;
    using Slash.Unity.DataBind.Foundation.Synchronizers;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Synchronizer for the text of a <see cref="Toggle"/>.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Synchronizers/[CDB] Toggle Synchronizer (Unity)")]
    public class ToggleSynchronizer : ComponentDataSynchronizer<Toggle, string>
    {
        private ToggleObserver observer;

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
                this.observer = new ToggleObserver
				{
                    Target = target
                };
                this.observer.ValueChanged += this.OnObserverValueChanged;
            }
        }

        protected override void SetTargetValue(Toggle target, string newContextValue)
        {
            target.isOn = bool.TryParse(newContextValue, out var value) ? value : false;
        }

        private void OnObserverValueChanged()
        {
            this.OnComponentValueChanged(this.Target.isOn);
        }
    }
}