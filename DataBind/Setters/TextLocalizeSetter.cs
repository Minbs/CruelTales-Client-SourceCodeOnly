using CTC.Globalizations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CTC.DataBind.Setters
{
	[AddComponentMenu("Custom Data Bind/Setters/[CDB] Text Localize Setter")]
	public class TextLocalizeSetter : MonoBehaviour, IComponentResettable
	{
		[field: SerializeField] public string TextKey { get; protected set; }
		[field: SerializeField] public TextMeshProUGUI Reference;

		public void Reset()
		{
			ResetComponent();
		}

		[Button]
		public void ResetComponent()
		{
			TextKey = gameObject.name.Replace(Global.DataBind.ComponentPrefix.TEXT, "");
			Reference = GetComponent<TextMeshProUGUI>();
			Reference.text = Localizer.GetTextAsDevelop(TextKey);
		}

		public void OnEnable()
		{
			registerLocalization();
		}

		public void OnDisable()
		{
			unregisterLocalization();
		}

		private void registerLocalization()
		{
			Localizer.OnLanguageChanged += setText;
			setText();
		}

		private void unregisterLocalization()
		{
			Localizer.OnLanguageChanged -= setText;
		}

		private void setText()
		{
			Reference.text = Localizer.GetText(TextKey);
		}
	}
}
