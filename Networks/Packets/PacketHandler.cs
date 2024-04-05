using System;
using CT.Common.Serialization;
using CT.Logger;
using CT.Packets;

namespace CTC.Networks.Packets
{
	public static class PacketHandler
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(PacketHandler));

		internal static void Handle_SC_Ack_TryEnterGameInstance(PacketBase receivedPacket, NetworkManager networkManager)
		{
			networkManager.ServerAck_TryEnterGameInstance();
		}

		internal static void Handle_SC_Sync_MasterSpawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterSpawn");
			networkManager.RemoteWorldManager.OnMasterSpawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterDespawn(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterDespawn");
			networkManager.RemoteWorldManager.OnMasterDespawn(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterEnter(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterEnter");
			networkManager.RemoteWorldManager.OnMasterEnter(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterLeave(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterLeave");
			networkManager.RemoteWorldManager.OnMasterLeave(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterPhysics(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterMovement");
			networkManager.RemoteWorldManager.OnMasterPhysics(receivedPacket);
		}

		internal static void Handle_SC_Sync_MasterReliable(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterReliable");
			networkManager.RemoteWorldManager.OnMasterReliable(receivedPacket);

		}
		internal static void Handle_SC_Sync_MasterUnreliable(IPacketReader receivedPacket, NetworkManager networkManager)
		{
			//_log.Info("Handle_SC_Sync_MasterUnreliable");
			networkManager.RemoteWorldManager.OnMasterUnreliable(receivedPacket);
		}
	}
}