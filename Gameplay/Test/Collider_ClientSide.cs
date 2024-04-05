using CTC.Networks.SyncObjects.SyncObjects;
using UnityEngine;

namespace CTC.Gameplay.Test
{
	public class Collider_ClientSide : MonoBehaviour
	{
		public PlayerCharacter PlayerCharacter;

		public bool IsLocal()
		{
			return ReferenceEquals(PlayerCharacter, null) || PlayerCharacter.IsLocal;
		}
	}
}