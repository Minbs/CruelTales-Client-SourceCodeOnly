using CTC.GUI;

namespace CTC.MainMenu
{
	public class Navigation_MainMenu : ViewNavigation
	{
		public void OnSceneLoaded()
		{
			this.Push<View_MainMenu>();
		}

		public void OpenServerBrowser()
		{
		}

		public void StartGame()
		{

		}

		public void OpenOption()
		{
			this.Push<View_Option>();
		}

		public void OpenQuitGameDialog()
		{

		}
	}
}
