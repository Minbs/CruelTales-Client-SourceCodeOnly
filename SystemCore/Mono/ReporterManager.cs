using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTC.SystemCore
{
	public class ReporterManager : MonoBehaviour, IManager
	{
		[SerializeField]
		private GameObject _reporterObject;

		public void Initialize()
		{
		}

		public void Release()
		{
		}
	}
}
