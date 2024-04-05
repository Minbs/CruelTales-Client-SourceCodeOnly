using Sirenix.OdinInspector;
using UnityEngine;

namespace CTC.Tests
{
	/// <summary>
	/// Dokza의 각종 데이터 저장용입니다.
	/// </summary>
	[CreateAssetMenu(fileName = "DokzaPlayData", menuName = "ScriptableObject/DokzaPlayData")]
	public class DokzaPlayData : ScriptableObject
	{
		[field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
		[field: SerializeField] public float WalkSpeed { get; private set; } = 2f;
		[field: SerializeField] public float PushPower { get; private set; } = 12f;
		[field: SerializeField] public float PushDecreasePower { get; private set; } = 13f;
		[field: SerializeField] public float PushedPower { get; private set; } = 12f;
		[field: SerializeField] public float PushedDecreasePower { get; private set; } = 13f;
	}
}