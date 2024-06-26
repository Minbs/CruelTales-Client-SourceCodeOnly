/*
 * Generated File : Remote_MapVoteController
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
	public partial class MapVoteController : IRemoteSynchronizable
	{
		[SyncRpc(dir: SyncDirection.FromRemote)]
		public partial void Client_VoteMap(int mapIndex);
		[SyncObject]
		private readonly SyncList<GameSceneIdentity> _nextMapVoteList;
		public SyncList<GameSceneIdentity> NextMapVoteList => _nextMapVoteList;
		private Action<SyncList<GameSceneIdentity>>? _onNextMapVoteListChanged;
		public event Action<SyncList<GameSceneIdentity>> OnNextMapVoteListChanged
		{
			add => _onNextMapVoteListChanged += value;
			remove => _onNextMapVoteListChanged -= value;
		}
		[SyncVar]
		private GameSceneIdentity _nextMap = new();
		public GameSceneIdentity NextMap => _nextMap;
		private Action<GameSceneIdentity>? _onNextMapChanged;
		public event Action<GameSceneIdentity> OnNextMapChanged
		{
			add => _onNextMapChanged += value;
			remove => _onNextMapChanged -= value;
		}
		[SyncObject]
		private readonly SyncDictionary<UserId, NetInt32> _mapIndexByUserId;
		public SyncDictionary<UserId, NetInt32> MapIndexByUserId => _mapIndexByUserId;
		private Action<SyncDictionary<UserId, NetInt32>>? _onMapIndexByUserIdChanged;
		public event Action<SyncDictionary<UserId, NetInt32>> OnMapIndexByUserIdChanged
		{
			add => _onMapIndexByUserIdChanged += value;
			remove => _onMapIndexByUserIdChanged -= value;
		}
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public MapVoteController()
		{
			_nextMapVoteList = new(this);
			_mapIndexByUserId = new(this, capacity: 8);
		}
		public MapVoteController(IDirtyable owner)
		{
			_owner = owner;
			_nextMapVoteList = new(this);
			_mapIndexByUserId = new(this, capacity: 8);
		}
		private BitmaskByte _dirtyReliable_0 = new();
		protected bool _isDirtyReliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		public void MarkDirtyReliable()
		{
			_isDirtyReliable = true;
			_owner.MarkDirtyReliable();
		}
		protected bool _isDirtyUnreliable;
		public bool IsDirtyUnreliable => _isDirtyUnreliable;
		public void MarkDirtyUnreliable()
		{
			_isDirtyUnreliable = true;
			_owner.MarkDirtyUnreliable();
		}
		public partial void Client_VoteMap(int mapIndex)
		{
			Client_VoteMapiCallstack.Add(mapIndex);
			_dirtyReliable_0[0] = true;
			MarkDirtyReliable();
		}
		private List<int> Client_VoteMapiCallstack = new(4);
		public void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Client_VoteMapiCallstack.Clear();
		}
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				byte count = (byte)Client_VoteMapiCallstack.Count;
				writer.Put(count);
				for (int i = 0; i < count; i++)
				{
					var arg = Client_VoteMapiCallstack[i];
					writer.Put(arg);
				}
			}
		}
		public void SerializeSyncUnreliable(IPacketWriter writer) { }
		public void InitializeMasterProperties() { }
		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!_nextMapVoteList.TryDeserializeSyncReliable(reader)) return false;
				_onNextMapVoteListChanged?.Invoke(_nextMapVoteList);
			}
			if (dirtyReliable_0[1])
			{
				if (!_nextMap.TryDeserialize(reader)) return false;
				_onNextMapChanged?.Invoke(_nextMap);
			}
			if (dirtyReliable_0[2])
			{
				if (!_mapIndexByUserId.TryDeserializeSyncReliable(reader)) return false;
				_onMapIndexByUserIdChanged?.Invoke(_mapIndexByUserId);
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_nextMapVoteList.TryDeserializeEveryProperty(reader)) return false;
			if (!_nextMap.TryDeserialize(reader)) return false;
			if (!_mapIndexByUserId.TryDeserializeEveryProperty(reader)) return false;
			return true;
		}
		public void InitializeRemoteProperties()
		{
			_nextMapVoteList.InitializeRemoteProperties();
			_nextMap = new();
			_mapIndexByUserId.InitializeRemoteProperties();
		}
		public void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				_nextMapVoteList.IgnoreSyncReliable(reader);
			}
			if (dirtyReliable_0[1])
			{
				GameSceneIdentity.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				_mapIndexByUserId.IgnoreSyncReliable(reader);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				SyncList<GameSceneIdentity>.IgnoreSyncStaticReliable(reader);
			}
			if (dirtyReliable_0[1])
			{
				GameSceneIdentity.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				SyncDictionary<UserId, NetInt32>.IgnoreSyncStaticReliable(reader);
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
