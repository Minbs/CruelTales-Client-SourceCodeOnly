using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;
using Slash.Unity.DataBind.Scripts.Customs;

namespace CTC.GUI.Option
{
	public class Context_DisplayOption : Context
	{
		private Property<Collection<ContextValueString>> resolutionDropdown = new(new Collection<ContextValueString>());
		private readonly Property<int> selectedIndex = new();

		public Context_DisplayOption()
		{
			this.ResolutionsProperty = new Collection<ContextValueString>
			{
				new ContextValueString("1"),
				new ContextValueString("2"),
			};
		}
		public Collection<ContextValueString> ResolutionsProperty
		{
			get => this.resolutionDropdown.Value;
			set => this.resolutionDropdown.Value = value;
		}

		public int SelectedIndex
		{
			get => this.selectedIndex.Value;
			set => this.selectedIndex.Value = value;
		}

		public void SelectItem(int index)
		{
			this.SelectedIndex = index;
		}
	}
}
