//using System;
//using CruelTales.DataBind.Contexts;
//using Legacy.CruelTales.GUI.Contexts.Gameplay;
//using Legacy.CruelTales.GUI.View.Gameplay;
//using Slash.Unity.DataBind.Core.Data;

//namespace CruelTales.GUI.Contexts.Gameplay
//{
//	[Obsolete("Legacy")]
//	public class Context_LobbyInformation : ContextWithView<View_LobbyInformation>
//	{
//		private readonly Property<Collection<Context_UserStateLabel>> mUserLabelItems = new(new());

//		public Collection<Context_UserStateLabel> UserLabelItems
//		{
//			get => mUserLabelItems.Value;
//			set => mUserLabelItems.Value = value;
//		}

//		public void Initialize()
//		{
//			UserLabelItems.Clear();
//		}
//	}
//}