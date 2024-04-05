#nullable enable
#pragma warning disable CS0649

using System.Collections;
using System.Numerics;
using CT.Common.DataType;
using CTC.Gameplay;
using CTC.Networks.Synchronizations;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class CameraController : RemoteNetworkObject
	{
		public CameraManager Camera { get; private set; }
		public PlayerCharacter? TargetPlayer { get; private set; }

		public CoroutineRunner _cannotFindTargetRunner { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			Camera = FindObjectOfType<CameraManager>();
			OnTargetIdChanged += onTargetIDChanged;

			_cannotFindTargetRunner = new CoroutineRunner(this);
		}

		public override void OnCreated()
		{
			Camera.SetProperties(FollowSpeed);
			onTargetIDChanged(TargetId);
		}

		private void onTargetIDChanged(NetworkIdentity targetId)
		{
			// Target이 사라졌습니다.
			if (targetId == 0)
			{
				Camera.ReleaseForce();
				return;
			}

			_cannotFindTargetRunner.Start(bindTarget());
		}

		/// <summary>
		/// 바인딩을 시도합니다.
		/// 바인딩을 지정한 시간동안 시도하지만 객체를 찾을 수 없다면 서버에 객체의 위치를 요청합니다.
		/// 바인딩이 성공할 때 까지 시도합니다.
		/// </summary>
		/// <returns></returns>
		private IEnumerator bindTarget()
		{
			const float TRY_TIME_SEC = 0.5f;
			const float INTERVAL = 0.016f;
			const int REPEAT_TIME = (int)(TRY_TIME_SEC / INTERVAL);

			if (!ReferenceEquals(TargetPlayer, null))
			{
				TargetPlayer.OnDestroy -= onPlayerCharacterDestroyed;
			}
			TargetPlayer = null;

			for (int i = 0; i < REPEAT_TIME; i++)
			{
				yield return null;
				if (tryBindTarget())
				{
					_cannotFindTargetRunner.Stop();
					yield break;
				}
			}

			Client_CannotFindBindTarget();
			_cannotFindTargetRunner.Start(bindTarget());

			bool tryBindTarget()
			{
				if (!WorldManager.TryGetNetworkObject(TargetId, out var playerCharacter) ||
					playerCharacter is not PlayerCharacter target)
				{
					if (Camera == null)
						return false;

					Camera.ReleaseForce();
					return false;
				}

				TargetPlayer = target;
				TargetPlayer.OnDestroy += onPlayerCharacterDestroyed;
				Camera.BindTarget(TargetPlayer.transform);
				return true;
			}
		}

		private void onPlayerCharacterDestroyed(RemoteNetworkObject playerCharacter)
		{
			playerCharacter.OnDestroy -= onPlayerCharacterDestroyed;
			Camera.ReleaseTarget(playerCharacter.transform);
		}

		public partial void Server_MoveTo(Vector2 position)
		{
			Camera.MoveTo(position.ToUnityVector2(), isInstant: true);
		}

		public partial void Server_LookAt(Vector2 position)
		{
			Camera.MoveTo(position.ToUnityVector2(), isInstant: false);
		}

		public partial void Server_LookAt(Vector2 position, float time)
		{
			Camera.LookAt(position.ToUnityVector2(), time);
		}

		public partial void Server_Shake()
		{
			var dir = RandomHelper.NextVector2().ToUnityVector2();
			Camera.ShakeCam( 1);
		}

		public partial void Server_Zoom(float zoom)
		{
			Camera.SetZoomTarget(zoom);
		}
	}
}

#pragma warning restore CS0649