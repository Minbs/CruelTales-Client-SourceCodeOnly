using Slash.Unity.DataBind.Core.Data;

namespace MyTestDataBinding
{
	public class Test_DataBindingContext : Context
	{
		private readonly Property<string> mTest_Message = new();

		public string Test_Message
		{
			get => mTest_Message.Value;
			set => mTest_Message.Value = value;
		}
	}

}

