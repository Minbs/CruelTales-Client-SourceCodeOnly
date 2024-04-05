namespace CTC.DataBind
{
	/// <summary>컴포넌트를 재설정합니다. 일반적으로 에디터에서 값을 재설정할 때 사용합니다.</summary>
	public interface IComponentResettable
	{
		/// <summary>컴포넌트를 재설정합니다.</summary>
		public void ResetComponent();
	}
}
