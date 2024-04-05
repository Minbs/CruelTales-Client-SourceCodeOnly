#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CT.Common.DataType;
using CT.Common.Exceptions;
using CT.Common.Serialization;
using CT.Common.Tools;
using CT.Logger;
using CT.Networks;
using CT.Networks.Extensions;
using CT.Networks.Runtimes;
using CT.Packets;
using CTC.Globalizations;
using CTC.Networks.Packets;
using CTC.SystemCore;
using LiteNetLib;
using Slash.Unity.DataBind.Core.Utils;
using UnityEngine;

namespace CTC.Networks
{
	[Serializable]
	public class GameServerEndPoint
	{
		public string IPEndPoint = "127.0.0.1";
		public int Port = 60128;
		public GameInstanceGuid GameInstanceGuid = new(1);
		public int Password = -1;
	}

	public struct DummyUserInfo
	{
		public UserId UserId;
		public UserToken UserToken;
		public string Username;

		public override string ToString()
		{
			return $"[{nameof(UserId)}:{UserId}]" +
				$"[{nameof(UserToken)}:{UserToken}]" +
				$"[{nameof(Username)}:{Username}]";
		}
	}

	public class DisconnectMessage
	{
		public DisconnectReasonType Reason { get; private set; }
		public string Detail { get; private set; } = string.Empty;
		public bool IsDisconnectByClient { get; private set; }

		public void Reset()
		{
			Reason = DisconnectReasonType.None;
			Detail = string.Empty;
			IsDisconnectByClient = false;
		}

		public void Set(DisconnectReasonType reason, string detail)
		{
			Reason = reason;
			Detail = detail;
			IsDisconnectByClient = true;
		}
	}

	public class NetworkManager : MonoBehaviour, IManager, IJobHandler
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(NetworkManager));

		// State
		private readonly EnumNotifier<UserSessionState> _sessionState = new();
		public event Action<UserSessionState>? OnSessionStateChanged
		{
			add => _sessionState.OnDataChanged += value;
			remove => _sessionState.OnDataChanged -= value;
		}
		public UserSessionState SessionState => _sessionState.Value;
		private RemoteWorldManager? _remoteWorldManager;
		public RemoteWorldManager? RemoteWorldManager => _remoteWorldManager;

		// Networks
		[AllowNull] private EventBasedNetListener _listener;
		[AllowNull] private NetManager _netManager;
		private NetPeer? _serverPeer;

		// Packets
		private PacketPool _packetPool = new();
		[AllowNull] private ConcurrentByteBufferPool _byteBufferPool;
		[AllowNull] private JobQueue<ByteBuffer> _jobQueue;

		// Handle
		private SystemEventHandle? _networkHandle;
		private DisconnectMessage _disconnectMessage = new();

		#region Debug
		public GameServerEndPoint GameServerEndPoint => 
			GlobalService.UserDataManager.UserData.InputCache.ServerEndPoint;
		private DummyUserInfo _userInfo;
		public UserId UserId => _userInfo.UserId;
		#endregion

		public ServerRuntimeOption ServerRuntimeOption { get; private set; }

		public void Initialize()
		{
			// Initialize properties
			_sessionState.Value = UserSessionState.NoConnection;

			_listener = new EventBasedNetListener();
			_netManager = new NetManager(_listener);
			_netManager.ReuseAddress = true;
			_netManager.DisconnectTimeout = GlobalNetwork.DisconnectTimeout;

			// Bind events
			_listener.NetworkReceiveEvent += OnReceived;
			_listener.PeerConnectedEvent += OnConnected;
			_listener.PeerDisconnectedEvent += OnDisconnected;

			// Start network manager
			_netManager.Start();
			_log.Info($"Start and initialize NetworkManager...");

			// packets
			int packetCapacity = 64;
			_byteBufferPool = new(1024 * 8, packetCapacity);
			_jobQueue = new JobQueue<ByteBuffer>(onProcessPacket, packetCapacity);

			// Set server runtime option to default
			ServerRuntimeOption = new()
			{
				PhysicsStepTime = GlobalNetwork.DEFAULT_PHYSICS_STEP_TIME
			};
		}

		public void SetDummyUserInfo()
		{
			// Setup dummy user info
			_userInfo = new DummyUserInfo();
			_userInfo.UserId = GlobalService.BackendManager.UserId;
			_userInfo.Username = GlobalService.BackendManager.Username;
			_userInfo.UserToken = GlobalService.BackendManager.UserToken;
			_log.Info($"Dummy user info : {_userInfo}");
		}

		public void Release()
		{
			_netManager.Stop();
			_log.Info($"Stop NetworkManager...");
		}

		public void Update()
		{
			Flush();
		}

		public bool Test_TryConnect()
		{
			_log.Info($"[Test] Try connect to game server with endpoint");
			return TryConnect(GameServerEndPoint.IPEndPoint,
							  GameServerEndPoint.Port);
		}

		public bool TryConnect(string address, int port)
		{
			SetDummyUserInfo();

			// Check it's already connected
			if (_networkHandle != null)
			{
				GlobalService.StaticGUI
					.OpenSystemDialogPopup(isTemporary: true,
										   TextKey.Dialog_Error_Title.GetText(),
										   TextKey.Dialog_AlreadyInMatchmaking.GetText(),
										   responseCallback: null,
										   DialogResult.OK);
				return false;
			}

			// Events
			var mainScene = (MainMenuSceneController)GlobalService.CurrentScene;
			mainScene.OnMatchmakingStart();

			_disconnectMessage.Reset();

			// Set session state
			_sessionState.Value = UserSessionState.TryConnecting;

			// Set system event handle
			_networkHandle = GlobalService.SystemEventManager.CreateHandle();
			_networkHandle.Push(() =>
			{
				_netManager.Connect(address, port, "TestServer");
			},
			(result) =>
			{
				if (result == SystemEventResult.Completed)
					_log.Info($"Try connect is copleted!");
				else
					_log.Error($"Try connect callback result : {result}");
			});
			return true;
		}

		public void Disconnect(DisconnectReasonType disconnectReason = DisconnectReasonType.None,
							   string disconnectReasonDetail = "")
		{
			_disconnectMessage.Set(disconnectReason, disconnectReasonDetail);
			_networkHandle?.OnCancel();
			_netManager.DisconnectAll();
		}

		public void SendReliable(IPacketWriter writer,
								 byte channelNumber = 0)
		{
			_serverPeer?.Send(writer.ByteSegment.Array,
							  writer.ByteSegment.Offset,
							  writer.Size,
							  channelNumber,
							  DeliveryMethod.ReliableOrdered);
		}

		public void SendUnreliable(IPacketWriter writer,
								   byte channelNumber = 0)
		{
			_serverPeer?.Send(writer.ByteSegment.Array,
							  writer.ByteSegment.Offset,
							  writer.Size,
							  channelNumber,
							  DeliveryMethod.Unreliable);
		}

		private void OnReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
		{
			if (_netManager.IsRunning)

			try
			{
				var receiveSegment = reader.GetRemainingBytesSegment();
				var buffer = _byteBufferPool.Get();
				buffer.Put(receiveSegment, receiveSegment.Count);
				_jobQueue.Push(buffer);
			}
			catch
			{
				_log.Error($"Disconnect! Failed to receive packet!");
				Disconnect(DisconnectReasonType.ClientError_FailedToReceivePacket);
			}
		}

		public void Clear()
		{
			_jobQueue?.Flush();
			_jobQueue?.Clear();
		}

		public void Flush()
		{
			_netManager?.PollEvents();
			_jobQueue?.Flush();
		}

		public void onProcessPacket(ByteBuffer packetBuffer)
		{
			try
			{
				while (packetBuffer.CanRead(sizeof(PacketType)))
				{
					PacketType packetType = packetBuffer.ReadPacketType();

					if (PacketDispatcher.IsCustomPacket(packetType))
					{
						PacketDispatcher.DispatchRaw(packetType, packetBuffer, this);
					}
					else
					{
						if (!_packetPool.TryReadPacket(packetType, packetBuffer, out var packet))
						{
							_log.Error($"Cannot parse packet. Type : {packetType}");
							Disconnect(DisconnectReasonType.ClientError_CannotHandlePacket);
							break;
						}

						PacketDispatcher.Dispatch(packet, this);
						_packetPool.Return(packet);
					}
				}
				_byteBufferPool.Return(packetBuffer);
			}
			catch (Exception e)
			{
				// Byte buffer will reset when it's call by pool getter
				_byteBufferPool.Return(packetBuffer);
				Debug.LogException(e);
				throw new NetworkRuntimeException("Failed to process packet in runtime!");
				//_log.Error($"Disconnect! Failed to handle packet!");
				//_log.Error(e.Message);
				//_log.Error(e.StackTrace);
				//Disconnect(DisconnectReasonType.ClientError_CannotHandlePacket);
			}
		}

		private ByteBuffer _connectionBuffer = new(1024);
		private void OnConnected(NetPeer peer)
		{
			// Set system event handle
			_networkHandle?.OnCompleted();
			_networkHandle = null;

			// Set session state
			_sessionState.Value = UserSessionState.TryEnterGameInstance;

			_serverPeer = peer;
			_log.Info($"Success to connect to the server. Server endpoint : {peer.EndPoint}");

			// Send request to server to enter the game
			var enterPacket = _packetPool.GetPacket<CS_Req_TryEnterGameInstance>();
			enterPacket.MatchTo = GameServerEndPoint.GameInstanceGuid;
			enterPacket.Token = _userInfo.UserToken;
			enterPacket.UserDataInfo = new UserDataInfo()
			{
				UserId = _userInfo.UserId,
				Username = _userInfo.Username,
				UserCostume = new DokzaCostume()
				{
					Body = 1,
					Head = 2,
				}
			};
			enterPacket.Password = GameServerEndPoint.Password;

			_log.Info($"Try request join game to server... : {_userInfo}");

			_connectionBuffer.ResetWriter();
			_connectionBuffer.Put(enterPacket);

			_serverPeer.Send(_connectionBuffer.ByteSegment.Array, 0,
							 _connectionBuffer.Size,
							 DeliveryMethod.ReliableOrdered);

			_packetPool.Return(enterPacket);
		}

		public void ServerAck_TryEnterGameInstance()
		{
			// Check it's valid connect action
			if (SessionState != UserSessionState.TryEnterGameInstance)
			{
				string title = TextKey.Dialog_UnknownError.GetText();
				GlobalService.StaticGUI
					.OpenSystemDialogPopup(isTemporary: true, title,
										   $"Server okay to enter the game, but client doesn't have connection.",
										   responseCallback: null,
										   DialogResult.OK);

				if (SessionState != UserSessionState.NoConnection)
				{
					Disconnect(DisconnectReasonType.None);
				}
				return;
			}

			// Set session state
			_sessionState.Value = UserSessionState.TryReadyToSync;

			// Events
			var mainScene = (MainMenuSceneController)GlobalService.CurrentScene;
			mainScene.OnMatchmakingEnd();

			// Load scene
			GlobalService.SceneManager
				.SystemHandle_LoadSceneAsync(SceneType.scn_game_gameplay, onSceneLoaded: () =>
				{
					var netManager = GlobalService.NetworkManager;

					if (netManager.SessionState != UserSessionState.TryReadyToSync)
					{
						string errorMessage = $"Server okay to enter the game, but client doesn't have connection.";

						_log.Error(errorMessage);
						if (netManager.SessionState != UserSessionState.NoConnection)
							Disconnect(DisconnectReasonType.ClientError_WrongConnectionFlow);

						_disconnectMessage.Set(DisconnectReasonType.ClientError_WrongConnectionFlow,
											   errorMessage);

						GlobalService.SceneManager
							.SystemHandle_LoadSceneAsync(SceneType.scn_game_main_menu, onSceneLoaded: () =>
							{
								openDisconnectedPopup(_disconnectMessage);
							});

						return;
					}

					var currentScene = GlobalService.CurrentScene as GameplaySceneController;
					if (currentScene == null)
					{
						_log.Fatal($"There is no {nameof(GameplaySceneController)}!");
						Disconnect(DisconnectReasonType.ClientError_NullReference);
						return;
					}

					// Try to send ready packet to server
					sendReq_TryReadyToSync();
			});

			void sendReq_TryReadyToSync()
			{
				var currentScene = (GameplaySceneController)GlobalService.CurrentScene;
				this._remoteWorldManager = currentScene.GameplayManager.WorldManager;

				_log.Info("Try request ready to sync");

				var readyPacket = _packetPool.GetPacket<CS_Req_ReadyToSync>();
				ByteBuffer writer = new ByteBuffer(20);
				writer.Put(readyPacket);
				SendReliable(writer);
				_packetPool.Return(readyPacket);
			}
		}

		private void OnDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
		{
			_remoteWorldManager?.Clear();

			// Set system event handle
			_networkHandle?.OnCancel();
			_networkHandle = null;

			// Set session state
			_sessionState.Value = UserSessionState.NoConnection;

			// Parse disconnect message
			DisconnectReasonType disconnectedReason = disconnectInfo.AdditionalData.IsNull ? 
				LiteNetLibExtension.ConvertEnum(disconnectInfo.Reason) :
				(DisconnectReasonType)disconnectInfo.AdditionalData.GetByte();

			if (!_disconnectMessage.IsDisconnectByClient)
			{
				_disconnectMessage.Set(disconnectedReason, string.Empty);
			}

			// Log
			_log.Warn($"Disconnected from the server. Disconnect reason : {disconnectedReason}");

			// Popup disconnect reason on main menu
			bool isMainMenu = GlobalService.CurrentScene is MainMenuSceneController;
			if (isMainMenu || SessionState == UserSessionState.TryEnterGameInstance)
			{
				var mainScene = (MainMenuSceneController)GlobalService.CurrentScene;
				mainScene.OnMatchmakingEnd();
				openDisconnectedPopup(_disconnectMessage);
				return;
			}

			GlobalService.SceneManager
				.SystemHandle_LoadSceneAsync(SceneType.scn_game_main_menu, onSceneLoaded: () =>
				{
					openDisconnectedPopup(_disconnectMessage);
				});
		}

		public void OnSynchronized(ServerRuntimeOption serverRuntimeOption)
		{
			_sessionState.Value = UserSessionState.InGameplay;
			this.ServerRuntimeOption = serverRuntimeOption;
		}

		private static StringBuilder _popupContentSb = new(512);
		private static void openDisconnectedPopup(DisconnectMessage disconnectMessage)
		{
			DisconnectReasonType reason = disconnectMessage.Reason;

			if (disconnectMessage.IsDisconnectByClient &&
				(reason == DisconnectReasonType.DisconnectPeerCalled ||
				reason == DisconnectReasonType.Client_GameEnd ||
				reason == DisconnectReasonType.None))
			{
				return;
			}

			string title = TextKey.Dialog_Disconnected_Title.GetText();
			string content = StringExtension.TryFormat(TextKey.Dialog_Disconnected_Content.GetText(), reason.ToString());
			_popupContentSb.AppendLine(content);
			_popupContentSb.Append(Global.UnityTexts.NewLine);
			_popupContentSb.Append(disconnectMessage.Detail);

			GlobalService.StaticGUI
				.OpenSystemDialogPopup(isTemporary: true, title,
									   _popupContentSb.ToString(),
									   responseCallback: null,
									   DialogResult.OK);
			_popupContentSb.Clear();
		}
	}
}
