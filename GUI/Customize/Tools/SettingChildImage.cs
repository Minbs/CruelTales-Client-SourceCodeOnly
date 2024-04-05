using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CTC.Assets.Scripts.GUI.Tools
{
	public class SettingChildImage : MonoBehaviour
	{
		public List<Color> colors = new();
		

		public Image NoneImage;

		[Button]
		public void SetHairSkinColor()
		{
			int count = 0;

			string h = "#ff6f70 #ff6f70 #6c0e10 #e8459a #c81472 #ffd526 #ffac05 #ff6a05 #ff6a05 #ff6a05 #432d29 #00c528 #2a9da7 #0083ff #122564 #9843d0 #421164 #f1f1f1 #7b7b7b #313131";
			string[] hexas = h.Split(' ');


			foreach (string s in hexas)
			{
				Color color;
				ColorUtility.TryParseHtmlString(s ,out color);
				transform.GetChild(count).GetComponent<Image>().color = color;
				count++;
			}
		}
	}
}
