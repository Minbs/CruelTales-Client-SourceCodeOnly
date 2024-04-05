using System.Numerics;
using CT.Common.Gameplay;
using CTC.Tests;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class EffectController
	{
		public GameplayController GameplayController { get; private set; }
		public EffectManager EffectManager { get; private set; }

		public void Initialize(GameplayController gameplayController)
		{
			GameplayController = gameplayController;
			EffectManager = GameplayController.GameplayManager.EffectManager;
		}

		public partial void Play(EffectType effect, Vector2 position, float duration)
		{
			EffectManager.SpawnEffect(effect, position.ToUnityVector2(), duration);
		}

		public partial void Play3D(EffectType effect, Vector3 position, float duration)
		{
			EffectManager.SpawnEffect(effect, position.ToUnityVector3(), duration);
		}
	}
}