using System;
using CTC.Gameplay.Test;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace CTC.Gameplay
{
	public class LightChanger_Collider : MonoBehaviour
	{
		public LightChangeManager LightChangeManager = null;
		public Light LightToReplace = null;
		public int IndexToReplace = 0;
		
		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<Collider_ClientSide>(out Collider_ClientSide clientCollider))
				return;

			if (!clientCollider.IsLocal())
				return;
		
			if(LightToReplace != null)
				LightChangeManager.ChangeLightTo(LightToReplace);
			else
				LightChangeManager.ChangeLightTo(IndexToReplace);
		}
	}
}