using CTC.GUI;
using CTC.GUI.Customize;
using CTC.SystemCore;

namespace CTC.MainMenu
{
	public class View_MainMenu : ViewBaseWithContext
	{
		public Context_MainMenu BindedContext { get; private set; }
		public Navigation_MainMenu Navigation { get; private set; }

		private View_Customize? _costume;

		protected override void onBeginShow()
		{
			base.onBeginShow();
			this.BindedContext = this.CurrentContext as Context_MainMenu;
			Navigation = (Navigation_MainMenu)ParentNavigation;
		}

		public void Initialize()
		{

		}

		public void OnClick_MatchingGame()
		{
			GlobalService.NetworkManager.Test_TryConnect();
		}

		public void OnClick_TestTryConnect()
		{
			// TODO : Try connect to server
		}

		public void OnClick_Closet()
		{
			if (_costume != null)
			{
				Navigation.PopByObject(_costume.gameObject);
			}

			_costume = Navigation.Push<View_Customize>();
			_costume.Initialize(onClosetClose);

			void onClosetClose()
			{
				if (_costume != null)
				{
					Navigation.PopByObject(_costume.gameObject);
					_costume = null;
				}
			}
		}

		public void OnClick_Close()
		{
			ProcessHandler.Instance.StopProcess();
		}

		public void OnClick_Setting()
		{

		}

		public void OnClick_Option()
		{
			Navigation.OpenOption();
		}

		public void OnClick_GotoTestScene()
		{
			GlobalService.SceneManager
				.SystemHandle_LoadSceneAsync(SceneType.scn_game_test_develop);
		}
	}
}
