using CTC.GUI;
using UnityEngine;

namespace CTC.Tests.Control
{
	public class Navigation_KeyBind : ViewNavigation
	{
		public void OnSceneLoaded()
		{
			this.Push<View_KeyBind>();
		}

		public void OpenServerBrowser()
		{
		}

		public void StartGame()
		{

		}

		public void OpenOption()
		{
			Debug.Log("Navigation_KeyBind" + " Option Opened");
		}

		public void OpenQuitGameDialog()
		{

		}
	}
}