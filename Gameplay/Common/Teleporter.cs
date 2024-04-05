#nullable enable
#pragma warning disable CS0649

using CT.Common.Gameplay;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class Teleporter : Interactor
	{
		[SerializeField]
		private GameObject Model;

		public override void OnCreated()
		{
			base.OnCreated();
			Model.SetActive(TeleporterShape != TeleporterShapeType.None);
		}
	}
}
