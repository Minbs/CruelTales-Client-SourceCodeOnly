#nullable enable
#pragma warning disable CS0649

using System.Collections.Generic;
using CT.Common.Gameplay;
using CTC.Globalizations;
using CTC.Networks.Synchronizations;
using KaNet.Physics;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class Interactor : RemoteNetworkObject
	{
		[field: SerializeField]
		public TextKey InfoTextKey { get; protected set; } = TextKey.Void;

		public virtual partial void Server_InteractResult(InteractResultType result)
		{
			var sceneController = GameplayController.SceneController;
			var miniGameController = sceneController as MiniGameControllerBase;
			miniGameController?.ApplyInteractResult(result, ProgressTime, InfoTextKey);
			// TODO : Input things
		}

		public override void OnCreated()
		{
			GameplayController.OnInteractorCreated(this);
		}

		public override void OnDestroyed()
		{
			GameplayController.OnInteractorDestroyed(this);
			base.OnDestroyed();
		}

		public bool IsCollideWith(PlayerCharacter player)
		{
			if (Interactable == false)
				return false;

			List<int>? hits;
			InteractorColliderShapeType shapeType = Size.ShapeType;
			if (shapeType == InteractorColliderShapeType.Box)
			{
				if (!PhysicsWorld.Raycast(Position, Size.Width, Size.Height,
										  out hits, PhysicsLayerMask.Player))
				{
					return false;
				}
			}
			else if (shapeType == InteractorColliderShapeType.Circle)
			{
				if (!PhysicsWorld.Raycast(Position, Size.Radius,
										  out hits, PhysicsLayerMask.Player))
				{
					return false;
				}
			}
			else
			{
				return false;
			}

			foreach (int id in hits)
			{
				if (player.Identity.Id == id)
					return true;
			}

			return false;
		}

		public virtual void OnTarget(bool isTarget) { }
	}
}
