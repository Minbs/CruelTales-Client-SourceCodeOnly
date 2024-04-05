using System;
using CTC.Gameplay.Test;
using CTC.Networks.SyncObjects.SyncObjects;
using UnityEngine;

namespace CTC.Graphics
{
	public class VolumeChanger_Collider : MonoBehaviour
	{
		public VolumeManager VolumeManager;
		public int ChangeIdx;
		
		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent<Collider_ClientSide>(out Collider_ClientSide clientCollider))
				return;

			if (!clientCollider.IsLocal())
				return;
			
			VolumeManager.ChangeVolumeTo(ChangeIdx);
		}
	}
}