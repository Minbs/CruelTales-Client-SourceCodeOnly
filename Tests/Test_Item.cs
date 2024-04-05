using System;
using System.Collections;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Tests
{
	public enum ItemType
	{
		None = 0,
		Rice,
		Kimchi,
		RicecakeSoup,
		Jobchae,
		Jeon,
		Sanjeok,
		GuJeolPan,
		Fish,
		Galbi,
		Rawmeat,
		GalbiJjim,
		Shrimp
	}
	
	public class Test_Item : MonoBehaviour
	{
		public Test_DokzaInventory DokzaInventory;
		public SpriteRenderer ItemProxy;
		[field: SerializeField] public ItemType ItemType { get; private set; }
		
		private CoroutineRunner coroutineRunner;

		private void Awake()
		{
			coroutineRunner = new CoroutineRunner(this);
		}

		[Button]
		public void DoJump(AnimationCurve curve)
		{
			coroutineRunner.Start(jumpEnumerator(curve));
		}

		private IEnumerator jumpEnumerator(AnimationCurve curve)
		{
			Transform itemProxyTransform = ItemProxy.transform;
			float timer = 0f;
			Vector3 itemPos = itemProxyTransform.position;
			
			while (true)
			{
				itemPos = itemProxyTransform.position;
				itemPos.y = curve.Evaluate(timer);
				itemProxyTransform.position = itemPos;
				
				yield return null;
				
				timer += Time.deltaTime;
				if (timer >= 1f)
				{
					timer = 1f;
					itemPos = itemProxyTransform.position;
					itemPos.y = curve.Evaluate(timer);
					itemProxyTransform.position = itemPos;
					break;
				}
			}

			yield break;
		}
	}
}