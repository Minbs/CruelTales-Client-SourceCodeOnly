using System.Collections;
using CT.Common.Gameplay;
using CTC.SystemCore;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Gameplay.Proxies
{
	public class FieldItemProxy : MonoBehaviour
	{
		[SerializeField]
		private AnimationCurve _jumpCurve;

		[SerializeField]
		private Transform _modelTransform;

		[SerializeField]
		private SpriteRenderer _renderer;

		[SerializeField] 
		private SpriteRenderer _outlineRenderer;

		[field: SerializeField]
		public FieldItemType FieldItem { get; private set; }

		private CoroutineRunner _jumpRunner;
		private CoroutineRunner _fadeInRunner;
		
		private void Awake()
		{
			_jumpRunner = new CoroutineRunner(this);
			_fadeInRunner = new CoroutineRunner(this);
		}

		public void Reset()
		{
			
		}

		public void Initialize(FieldItemType itemType)
		{
			FieldItem = itemType;
			Sprite spr = GlobalService
				.GameplayManager
				.GameplayResourcesManager
				.GetFieldItem(FieldItem);
			_renderer.sprite = spr;

			_outlineRenderer.sprite = spr;
			_outlineRenderer.color = new Color(0f, 0f, 0f, 0f);
		}

		[Button]
		public void OnSpawn()
		{
			_jumpRunner.Start(jumpEnumerator());
			_fadeInRunner.Start(fadeInEnumerator());
		}

		[Button]
		public void OnOutline(bool isOn)
		{
			Color blackColor = Color.black;
			blackColor.a = isOn ? 1f : 0f;
			_outlineRenderer.color = blackColor;
		}
		
		private IEnumerator fadeInEnumerator()
		{
			for (float alpha = 0; alpha <= 1f; alpha += Time.deltaTime * 3.33f)
			{
				_renderer.color = new Color(1f, 1f, 1f, alpha);
				yield return null;
			}
		}
		
		private IEnumerator jumpEnumerator()
		{
			float timer = 0f;
			while (true)
			{
				Vector3 itemPos = _modelTransform.position;
				itemPos.y = _jumpCurve.Evaluate(timer);
				_modelTransform.position = itemPos;

				yield return null;

				timer += Time.deltaTime;
				if (timer >= 1f)
				{
					timer = 1f;
					itemPos = _modelTransform.position;
					itemPos.y = _jumpCurve.Evaluate(timer);
					_modelTransform.position = itemPos;
					break;
				}
			}
		}
	}
}