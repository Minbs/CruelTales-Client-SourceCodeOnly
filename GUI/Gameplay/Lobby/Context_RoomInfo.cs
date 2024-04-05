using CTC.Networks.SyncObjects.SyncObjects;
using Slash.Unity.DataBind.Core.Data;

namespace CTC.GUI.Gameplay.Lobby
{
	public class Context_RoomInfo : Context
	{
		private readonly Property<string> roomNameProperty = new();
		public string RoomName
		{
			get => roomNameProperty.Value;
			set => roomNameProperty.Value = value;
		}

		private readonly Property<string> roomDiscriptionProperty = new();
		public string RoomDiscription
		{
			get => roomDiscriptionProperty.Value;
			set => roomDiscriptionProperty.Value = value;
		}

		private readonly Property<string> playerCountProperty = new();
		public string PlayerCount
		{
			get => playerCountProperty.Value;
			set => playerCountProperty.Value = value;
		}
	}
}
