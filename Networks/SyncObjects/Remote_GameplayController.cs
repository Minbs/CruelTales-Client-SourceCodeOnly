/*
 * Generated File : Remote_GameplayController
 * 
 * This code has been generated by the CodeGenerator.
 * Do not modify the code arbitrarily.
 */

#nullable enable
#pragma warning disable CS0649

using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common;
using CT.Common.DataType;
using CT.Common.Exceptions;
using CT.Common.Gameplay;
using CT.Common.Quantization;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools;
using CT.Common.DataType.Input;
using CT.Common.DataType.Primitives;
using CT.Common.DataType.Synchronizations;
using CT.Common.Gameplay.Dueoksini;
using CT.Common.Gameplay.Infos;
using CT.Common.Gameplay.PlayerCharacterStates;
using CT.Common.Gameplay.Players;
using CT.Common.Gameplay.RedHood;
using CT.Common.Tools.CodeGen;
using CT.Common.Tools.Collections;
using CT.Common.Tools.ConsoleHelper;
using CT.Common.Tools.Data;
using CT.Common.Tools.FSM;
using CT.Common.Tools.GetOpt;
using CT.Common.Tools.SharpJson;
using CTC.Networks.Synchronizations;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	[Serializable]
	public partial class GameplayController
	{
		public override NetworkObjectType Type => NetworkObjectType.GameplayController;
		[SyncObject(dir: SyncDirection.Bidirection)]
		private readonly RoomSessionManager _roomSessionManager;
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_ReadyToSync(JoinRequestToken token);
		[SyncVar]
		private ServerRuntimeOption _serverRuntimeOption = new();
		public ServerRuntimeOption ServerRuntimeOption => _serverRuntimeOption;
		private Action<ServerRuntimeOption>? _onServerRuntimeOptionChanged;
		public event Action<ServerRuntimeOption> OnServerRuntimeOptionChanged
		{
			add => _onServerRuntimeOptionChanged += value;
			remove => _onServerRuntimeOptionChanged -= value;
		}
		[SyncVar]
		private GameSystemState _gameSystemState;
		public GameSystemState GameSystemState => _gameSystemState;
		private Action<GameSystemState>? _onGameSystemStateChanged;
		public event Action<GameSystemState> OnGameSystemStateChanged
		{
			add => _onGameSystemStateChanged += value;
			remove => _onGameSystemStateChanged -= value;
		}
		private Action<RoomSessionManager>? _onRoomSessionManagerChanged;
		public event Action<RoomSessionManager> OnRoomSessionManagerChanged
		{
			add => _onRoomSessionManagerChanged += value;
			remove => _onRoomSessionManagerChanged -= value;
		}
		[SyncObject]
		private readonly EffectController _effectController;
		public EffectController EffectController => _effectController;
		private Action<EffectController>? _onEffectControllerChanged;
		public event Action<EffectController> OnEffectControllerChanged
		{
			add => _onEffectControllerChanged += value;
			remove => _onEffectControllerChanged -= value;
		}
		[SyncObject]
		private readonly SoundController _soundController;
		public SoundController SoundController => _soundController;
		private Action<SoundController>? _onSoundControllerChanged;
		public event Action<SoundController> OnSoundControllerChanged
		{
			add => _onSoundControllerChanged += value;
			remove => _onSoundControllerChanged -= value;
		}
		public GameplayController()
		{
			_roomSessionManager = new(this);
			_effectController = new(this);
			_soundController = new(this);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		public partial void Client_ReadyToSync(JoinRequestToken token)
		{
			Client_ReadyToSyncJCallstack.Add(token);
			_dirtyReliable_0[1] = true;
			MarkDirtyReliable();
		}
		private List<JoinRequestToken> Client_ReadyToSyncJCallstack = new(4);
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			_roomSessionManager.ClearDirtyReliable();
			Client_ReadyToSyncJCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0[0] = _roomSessionManager.IsDirtyReliable;
			BitmaskByte dirtyReliable_0 = _dirtyReliable_0;
			int dirtyReliable_0_pos = writer.OffsetSize(sizeof(byte));
			if (_dirtyReliable_0[0])
			{
				_roomSessionManager.SerializeSyncReliable(writer);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = (byte)Client_ReadyToSyncJCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_ReadyToSyncJCallstack[i];
					arg.Serialize(writer);
				}
			}
			if (dirtyReliable_0.AnyTrue())
			{
				writer.PutTo(dirtyReliable_0, dirtyReliable_0_pos);
			}
			else
			{
				writer.SetSize(dirtyReliable_0_pos);
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void InitializeMasterProperties()
		{
			_roomSessionManager.InitializeRemoteProperties();
		}
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!_serverRuntimeOption.TryDeserialize(reader)) return false;
				_onServerRuntimeOptionChanged?.Invoke(_serverRuntimeOption);
			}
			if (dirtyReliable_0[1])
			{
				if (!reader.TryReadByte(out var _gameSystemStateValue)) return false;
				_gameSystemState = (GameSystemState)_gameSystemStateValue;
				_onGameSystemStateChanged?.Invoke(_gameSystemState);
			}
			if (dirtyReliable_0[2])
			{
				if (!_roomSessionManager.TryDeserializeSyncReliable(reader)) return false;
				_onRoomSessionManagerChanged?.Invoke(_roomSessionManager);
			}
			if (dirtyReliable_0[3])
			{
				if (!_effectController.TryDeserializeSyncReliable(reader)) return false;
				_onEffectControllerChanged?.Invoke(_effectController);
			}
			if (dirtyReliable_0[4])
			{
				if (!_soundController.TryDeserializeSyncReliable(reader)) return false;
				_onSoundControllerChanged?.Invoke(_soundController);
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_serverRuntimeOption.TryDeserialize(reader)) return false;
			if (!reader.TryReadByte(out var _gameSystemStateValue)) return false;
			_gameSystemState = (GameSystemState)_gameSystemStateValue;
			if (!_roomSessionManager.TryDeserializeEveryProperty(reader)) return false;
			if (!_effectController.TryDeserializeEveryProperty(reader)) return false;
			if (!_soundController.TryDeserializeEveryProperty(reader)) return false;
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_serverRuntimeOption = new();
			_gameSystemState = (GameSystemState)0;
			_roomSessionManager.InitializeRemoteProperties();
			_effectController.InitializeRemoteProperties();
			_soundController.InitializeRemoteProperties();
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				ServerRuntimeOption.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[2])
			{
				_roomSessionManager.IgnoreSyncReliable(reader);
			}
			if (dirtyReliable_0[3])
			{
				_effectController.IgnoreSyncReliable(reader);
			}
			if (dirtyReliable_0[4])
			{
				_soundController.IgnoreSyncReliable(reader);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				ServerRuntimeOption.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[1])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[2])
			{
				RoomSessionManager.IgnoreSyncStaticReliable(reader);
			}
			if (dirtyReliable_0[3])
			{
				EffectController.IgnoreSyncStaticReliable(reader);
			}
			if (dirtyReliable_0[4])
			{
				SoundController.IgnoreSyncStaticReliable(reader);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
