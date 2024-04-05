//using System;
//using System.Collections;
//using CruelTales.GUI;
//using Legacy.CruelTales.GUI.View.Gameplay;
//using UnityEngine;

//namespace Legacy.CruelTales.GUI.Navigation.Gameplay
//{
//	[Obsolete("Legacy")]
//	public class Navigation_IngameHUD : ViewNavigation
//	{
//		public void Awake()
//		{
//			StartCoroutine(openLobbyInformation());
//		}

//		private IEnumerator openLobbyInformation()
//		{
//			yield return new WaitForSeconds(0.5f);
//			this.Push<View_LobbyInformation>();
//		}
//	}
//}
