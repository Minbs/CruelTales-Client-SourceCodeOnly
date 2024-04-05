//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//namespace CruelTales.GUI.Component
//{
//	public class InputFieldContentTypeByToggle : MonoBehaviour
//	{
//		[field: SerializeField]
//		public TMP_InputField InputFieldReference { get; private set; }

//		[field: SerializeField]
//		public Toggle TargetToggle { get; private set; }

//		[field: SerializeField]
//		public TMP_InputField.ContentType OnType { get; private set; } = TMP_InputField.ContentType.Standard;

//		[field: SerializeField]
//		public TMP_InputField.ContentType OffType { get; private set; } = TMP_InputField.ContentType.Password;

//		public void Reset()
//		{
//			InputFieldReference = GetComponent<TMP_InputField>();
//		}

//		private void OnEnable()
//		{
//			TargetToggle.onValueChanged.AddListener(onToggleChanged);
//		}

//		private void OnDisable()
//		{
//			TargetToggle.onValueChanged.RemoveListener(onToggleChanged);
//		}

//		private void onToggleChanged(bool value)
//		{
//			InputFieldReference.contentType = value ? OnType : OffType;
//			InputFieldReference.textComponent.SetAllDirty();
//		}
//	}
//}
