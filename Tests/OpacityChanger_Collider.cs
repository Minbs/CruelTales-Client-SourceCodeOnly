using System;
using UnityEngine;

namespace CTC.Tests
{
	public class OpacityChanger_Collider : OpacityChanger
	{
		public Collider TargetCollider;
		public bool EnterToFadeOut = true;
		
		private void OnTriggerEnter(Collider other)
		{
			if (ReferenceEquals(TargetCollider, null) || (other != TargetCollider))
				return;
			
			LerpOpacity(EnterToFadeOut);
		}

		private void OnTriggerExit(Collider other)
		{
			if (ReferenceEquals(TargetCollider, null) || (other != TargetCollider))
				return;
			
			LerpOpacity(!EnterToFadeOut);
		}
	}
}