using System.Collections.Generic;

using UnityEngine;

namespace CTC.Data
{
	[CreateAssetMenu(fileName = "ProcessStartOption", menuName = "Static Data/ProcessStartOptionData", order = 0)]
	public class ProcessStartOptionData : ScriptableObject
	{
		[field: SerializeField]
		public uint AppID { get; protected set; } = 480;

		[field: SerializeField]
		public bool IsDebugMode { get; protected set; } = false;

		[field: SerializeField]
		public SceneType InitialSceneType { get; protected set; } = SceneType.scn_game_initial;

		[field: SerializeField]
		public NetworkPlatform RunningPlatform { get; protected set; } = NetworkPlatform.Standalone;

		[field: SerializeField]
		public List<string> EditorProcessArguments = new();
	}
}

