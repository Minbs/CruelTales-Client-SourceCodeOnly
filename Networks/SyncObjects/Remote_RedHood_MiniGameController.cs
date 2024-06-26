/*
 * Generated File : Remote_RedHood_MiniGameController
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
	public partial class RedHood_MiniGameController
	{
		public override NetworkObjectType Type => NetworkObjectType.RedHood_MiniGameController;
		public RedHood_MiniGameController()
		{
		}
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Client_OnSceneLoadedCallstackCount = 0;
			Client_ReadyGamebCallstack.Clear();
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put((byte)Client_OnSceneLoadedCallstackCount);
			}
			if (_dirtyReliable_0[1])
			{
				byte count = (byte)Client_ReadyGamebCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_ReadyGamebCallstack[i];
					writer.Put(arg);
				}
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
				if (dirtyReliable_0[0])
				{
					if (!_gameSceneIdentity.TryDeserialize(reader)) return false;
					_onGameSceneIdentityChanged?.Invoke(_gameSceneIdentity);
				}
				if (dirtyReliable_0[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity gameScene = new();
						if (!gameScene.TryDeserialize(reader)) return false;
						Server_TryLoadSceneAll(gameScene);
					}
				}
				if (dirtyReliable_0[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity gameScene = new();
						if (!gameScene.TryDeserialize(reader)) return false;
						Server_TryLoadScene(gameScene);
					}
				}
				if (dirtyReliable_0[3])
				{
					if (!reader.TryReadByte(out var _gameplayStateValue)) return false;
					_gameplayState = (GameplayState)_gameplayStateValue;
					_onGameplayStateChanged?.Invoke(_gameplayState);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadSingle(out _gameTime)) return false;
					_onGameTimeChanged?.Invoke(_gameTime);
				}
				if (dirtyReliable_0[5])
				{
					if (!_eliminatedPlayers.TryDeserializeSyncReliable(reader)) return false;
					_onEliminatedPlayersChanged?.Invoke(_eliminatedPlayers);
				}
				if (dirtyReliable_0[6])
				{
					if (!_mapVoteController.TryDeserializeSyncReliable(reader)) return false;
					_onMapVoteControllerChanged?.Invoke(_mapVoteController);
				}
				if (dirtyReliable_0[7])
				{
					if (!_teamScoreByFaction.TryDeserializeSyncReliable(reader)) return false;
					_onTeamScoreByFactionChanged?.Invoke(_teamScoreByFaction);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadSingle(out float missionShowTime)) return false;
						if (!reader.TryReadSingle(out float countdown)) return false;
						Server_GameStartCountdown(missionShowTime, countdown);
					}
				}
				if (dirtyReliable_1[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadSingle(out float timeLeft)) return false;
						Server_GameStart(timeLeft);
					}
				}
				if (dirtyReliable_1[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						Server_FeverTimeStart();
					}
				}
				if (dirtyReliable_1[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadSingle(out float freezeTime)) return false;
						Server_GameEnd(freezeTime);
					}
				}
				if (dirtyReliable_1[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadSingle(out float resultTime)) return false;
						Server_ShowResult(resultTime);
					}
				}
				if (dirtyReliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadByte(out var cutSceneTypeValue)) return false;
						ExecutionCutSceneType cutSceneType = (ExecutionCutSceneType)cutSceneTypeValue;
						if (!reader.TryReadSingle(out float playTime)) return false;
						Server_ShowExecution(cutSceneType, playTime);
					}
				}
				if (dirtyReliable_1[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadSingle(out float mapVoteTime)) return false;
						Server_StartVoteMap(mapVoteTime);
					}
				}
				if (dirtyReliable_1[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity nextMap = new();
						if (!nextMap.TryDeserialize(reader)) return false;
						if (!reader.TryReadSingle(out float showTime)) return false;
						Server_ShowVotedNextMap(nextMap, showTime);
					}
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						if (!reader.TryReadSingle(out float timeLeft)) return false;
						Server_SyncTimer(timeLeft);
					}
				}
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_gameSceneIdentity.TryDeserialize(reader)) return false;
			if (!reader.TryReadByte(out var _gameplayStateValue)) return false;
			_gameplayState = (GameplayState)_gameplayStateValue;
			if (!reader.TryReadSingle(out _gameTime)) return false;
			if (!reader.TryReadSingle(out _currentTime)) return false;
			if (!_eliminatedPlayers.TryDeserializeEveryProperty(reader)) return false;
			if (!_mapVoteController.TryDeserializeEveryProperty(reader)) return false;
			if (!_teamScoreByFaction.TryDeserializeEveryProperty(reader)) return false;
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_gameSceneIdentity = new();
			_gameplayState = (GameplayState)0;
			_gameTime = 0;
			_currentTime = 0;
			_eliminatedPlayers.InitializeRemoteProperties();
			_mapVoteController.InitializeRemoteProperties();
			_teamScoreByFaction.InitializeRemoteProperties();
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
				if (dirtyReliable_0[0])
				{
					GameSceneIdentity.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity.IgnoreStatic(reader);
					}
				}
				if (dirtyReliable_0[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity.IgnoreStatic(reader);
					}
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[5])
				{
					_eliminatedPlayers.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_0[6])
				{
					_mapVoteController.IgnoreSyncReliable(reader);
				}
				if (dirtyReliable_0[7])
				{
					_teamScoreByFaction.IgnoreSyncReliable(reader);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_1[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(1);
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity.IgnoreStatic(reader);
						reader.Ignore(4);
					}
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
			}
		}
		public new static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte masterDirty = reader.ReadBitmaskByte();
			if (masterDirty[0])
			{
				BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
				if (dirtyReliable_0[0])
				{
					GameSceneIdentity.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity.IgnoreStatic(reader);
					}
				}
				if (dirtyReliable_0[2])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity.IgnoreStatic(reader);
					}
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[4])
				{
					reader.Ignore(4);
				}
				if (dirtyReliable_0[5])
				{
					SyncList<UserId>.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_0[6])
				{
					MapVoteController.IgnoreSyncStaticReliable(reader);
				}
				if (dirtyReliable_0[7])
				{
					SyncDictionary<NetByte, NetInt16>.IgnoreSyncStaticReliable(reader);
				}
			}
			if (masterDirty[1])
			{
				BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
				if (dirtyReliable_1[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[1])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[2])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_1[3])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[4])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[5])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(1);
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[6])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
				if (dirtyReliable_1[7])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						GameSceneIdentity.IgnoreStatic(reader);
						reader.Ignore(4);
					}
				}
			}
			if (masterDirty[2])
			{
				BitmaskByte dirtyReliable_2 = reader.ReadBitmaskByte();
				if (dirtyReliable_2[0])
				{
					byte count = reader.ReadByte();
					for (int i = 0; i < count; i++)
					{
						reader.Ignore(4);
					}
				}
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
