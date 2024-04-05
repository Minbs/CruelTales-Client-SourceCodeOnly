#nullable enable
#pragma warning disable CS0649

using CT.Common.DataType;
using CT.Logger;
using CTC.GUI.MiniGames;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class CustomLobby_SceneController : SceneControllerBase
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(CustomLobby_SceneController));

		// Reference
		[field: SerializeField]
		public Lobby_Navigation Lobby_Navigation { get; private set; }

		public override void OnCreated()
		{
			base.OnCreated();
			Lobby_Navigation.Initialize(this);
		}

		public override void OnDestroyed()
		{
			Lobby_Navigation.Dispose();
			base.OnDestroyed();
		}

		public virtual partial void Server_TryStartGameCallback(StartGameResultType result)
		{
			Lobby_Navigation.OnTryStartGameCallback(result);
		}

		public virtual partial void Server_StartGameCountdown(float second)
		{
			Lobby_Navigation.OnGameStartCountdown(second);
		}

		public virtual partial void Server_CancelStartGameCountdown()
		{
			Lobby_Navigation.OnCancelGameStartCountdown();
		}
	}
}

#pragma warning restore CS0649
