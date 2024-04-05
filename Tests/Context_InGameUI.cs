using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;






namespace CTC.Tests
{
	public class Context_InGameUI : ContextWithView<View_InGameUI>
	{
		private readonly Property<string> titleProperty = new Property<string>();
		public string Title
		{
			get => titleProperty.Value;
			set => titleProperty.Value = value;
		}
		
		private readonly Property<string> contentProperty = new Property<string>();
		public string Content
		{
			get => contentProperty.Value;
			set => contentProperty.Value = value;
		}
		
		// 생각해보니 딱히 넣을 이벤트가 없음낄낄
	}
}