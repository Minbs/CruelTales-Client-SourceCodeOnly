using CTC.GUI;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.DataBind.Contexts
{
	public abstract class ContextWithView : Context
	{
		public abstract void BindView(ViewBase view);
	}

	public class ContextWithView<T> : ContextWithView where T : ViewBase
	{
		public T CurrentView { get; private set; }

		public override void BindView(ViewBase view)
		{
			CurrentView = view as T;
		}
	}
}
