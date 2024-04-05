#nullable enable
#pragma warning disable CS0649

using System.Collections;
using CTC.Networks.Synchronizations;
using CTC.Utils.Coroutines;
using TMPro;
using UnityEngine;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class TestCube : RemoteNetworkObject
	{
		[SerializeField] private SpriteRenderer _spawn;
		[SerializeField] private SpriteRenderer _trace;
		[SerializeField] private SpriteRenderer _despawn;
		[SerializeField] private TextMeshProUGUI _timer;

		private CoroutineRunner _lifeEvent;

		protected override void Awake()
		{
			base.Awake();
			_lifeEvent = new CoroutineRunner(this);
		}

		public partial void TestRPC(long someMessage)
		{
			//Debug.Log(someMessage);
		}

		public override void OnUpdate(float deltaTime)
		{
			_animationTime += deltaTime;
			_timer.text = AnimationTime.ToString("F1");
		}

		public override void OnEnter()
		{
			Debug.DrawRay(transform.position, Vector3.up, Color.cyan, 3.0f);
			_spawn.color = new Color(R, G, B);
			_trace.color = new Color(R, G, B);
			_despawn.color = new Color(R, G, B);

			_spawn.enabled = false;
			_trace.enabled = true;
			_despawn.enabled = false;
		}

		public override void OnSpawn()
		{
			Debug.DrawRay(transform.position, Vector3.up, Color.yellow, 3.0f);
			_spawn.color = new Color(R, G, B);
			_trace.color = new Color(R, G, B);
			_despawn.color = new Color(R, G, B);

			_spawn.enabled = true;
			_trace.enabled = false;
			_despawn.enabled = false;

			_lifeEvent.Start(onSpawn());
		}

		public override void OnLeave()
		{
			Destroy(gameObject);
		}

		public override void OnDespawn()
		{
			_spawn.enabled = false;
			_trace.enabled = false;
			_despawn.enabled = true;

			_lifeEvent.Start(onDestroyed());
		}

		IEnumerator onSpawn()
		{
			yield return CoroutineCache.GetWaitForSeconds(1f);
			_spawn.enabled = false;
			_trace.enabled = true;
			_despawn.enabled = false;
		}

		IEnumerator onDestroyed()
		{
			yield return CoroutineCache.GetWaitForSeconds(1f);
			Destroy(gameObject);
		}
	}
}
#pragma warning restore CS0649
