/*
 * Generated File : Remote_PlayerState
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
	public partial class PlayerState : IRemoteSynchronizable
	{
		[SyncVar]
		private UserId _userId = new();
		public UserId UserId => _userId;
		private Action<UserId>? _onUserIdChanged;
		public event Action<UserId> OnUserIdChanged
		{
			add => _onUserIdChanged += value;
			remove => _onUserIdChanged -= value;
		}
		[SyncVar]
		private NetStringShort _username = new();
		public NetStringShort Username => _username;
		private Action<NetStringShort>? _onUsernameChanged;
		public event Action<NetStringShort> OnUsernameChanged
		{
			add => _onUsernameChanged += value;
			remove => _onUsernameChanged -= value;
		}
		[SyncVar]
		private Faction _faction;
		public Faction Faction => _faction;
		private Action<Faction>? _onFactionChanged;
		public event Action<Faction> OnFactionChanged
		{
			add => _onFactionChanged += value;
			remove => _onFactionChanged -= value;
		}
		[SyncVar]
		private bool _isEliminated;
		public bool IsEliminated => _isEliminated;
		private Action<bool>? _onIsEliminatedChanged;
		public event Action<bool> OnIsEliminatedChanged
		{
			add => _onIsEliminatedChanged += value;
			remove => _onIsEliminatedChanged -= value;
		}
		[SyncVar]
		private bool _isHost;
		public bool IsHost => _isHost;
		private Action<bool>? _onIsHostChanged;
		public event Action<bool> OnIsHostChanged
		{
			add => _onIsHostChanged += value;
			remove => _onIsHostChanged -= value;
		}
		[SyncVar]
		private bool _isReady;
		public bool IsReady => _isReady;
		private Action<bool>? _onIsReadyChanged;
		public event Action<bool> OnIsReadyChanged
		{
			add => _onIsReadyChanged += value;
			remove => _onIsReadyChanged -= value;
		}
		[SyncVar]
		private bool _isMapLoaded;
		public bool IsMapLoaded => _isMapLoaded;
		private Action<bool>? _onIsMapLoadedChanged;
		public event Action<bool> OnIsMapLoadedChanged
		{
			add => _onIsMapLoadedChanged += value;
			remove => _onIsMapLoadedChanged -= value;
		}
		[SyncObject]
		private readonly CostumeSet _selectedCostume;
		public CostumeSet SelectedCostume => _selectedCostume;
		private Action<CostumeSet>? _onSelectedCostumeChanged;
		public event Action<CostumeSet> OnSelectedCostumeChanged
		{
			add => _onSelectedCostumeChanged += value;
			remove => _onSelectedCostumeChanged -= value;
		}
		[SyncObject]
		private readonly CostumeSet _currentCostume;
		public CostumeSet CurrentCostume => _currentCostume;
		private Action<CostumeSet>? _onCurrentCostumeChanged;
		public event Action<CostumeSet> OnCurrentCostumeChanged
		{
			add => _onCurrentCostumeChanged += value;
			remove => _onCurrentCostumeChanged -= value;
		}
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public PlayerState()
		{
			_selectedCostume = new(this);
			_currentCostume = new(this);
		}
		public PlayerState(IDirtyable owner)
		{
			_owner = owner;
			_selectedCostume = new(this);
			_currentCostume = new(this);
		}
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
		public void ClearDirtyReliable() { }
		public void ClearDirtyUnreliable() { }
		public void SerializeSyncReliable(IPacketWriter writer) { }
		public void SerializeSyncUnreliable(IPacketWriter writer) { }
		public void InitializeMasterProperties() { }
		public bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					if (!_userId.TryDeserialize(reader)) return false;
					_onUserIdChanged?.Invoke(_userId);
				}
				if (dirtyReliable_0[1])
				{
					if (!_username.TryDeserialize(reader)) return false;
					_onUsernameChanged?.Invoke(_username);
				}
				if (dirtyReliable_0[2])
				{
					if (!reader.TryReadByte(out var _factionValue)) return false;
					_faction = (Faction)_factionValue;
					_onFactionChanged?.Invoke(_faction);
				}
				if (dirtyReliable_0[3])
				{
					if (!reader.TryReadBoolean(out _isEliminated)) return false;
					_onIsEliminatedChanged?.Invoke(_isEliminated);
				}
				if (dirtyReliable_0[4])
				{
					if (!reader.TryReadBoolean(out _isHost)) return false;
					_onIsHostChanged?.Invoke(_isHost);
				}
				if (dirtyReliable_0[5])
				{
					if (!reader.TryReadBoolean(out _isReady)) return false;
					_onIsReadyChanged?.Invoke(_isReady);
				}
				if (dirtyReliable_0[6])
				{
					if (!reader.TryReadBoolean(out _isMapLoaded)) return false;
					_onIsMapLoadedChanged?.Invoke(_isMapLoaded);
				}
				if (dirtyReliable_0[7])
				{
					if (!_selectedCostume.TryDeserializeSyncReliable(reader)) return false;
					_onSelectedCostumeChanged?.Invoke(_selectedCostume);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					if (!_currentCostume.TryDeserializeSyncReliable(reader)) return false;
					_onCurrentCostumeChanged?.Invoke(_currentCostume);
				}
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!_userId.TryDeserialize(reader)) return false;
			if (!_username.TryDeserialize(reader)) return false;
			if (!reader.TryReadByte(out var _factionValue)) return false;
			_faction = (Faction)_factionValue;
			if (!reader.TryReadBoolean(out _isEliminated)) return false;
			if (!reader.TryReadBoolean(out _isHost)) return false;
			if (!reader.TryReadBoolean(out _isReady)) return false;
			if (!reader.TryReadBoolean(out _isMapLoaded)) return false;
			if (!_selectedCostume.TryDeserializeEveryProperty(reader)) return false;
			if (!_currentCostume.TryDeserializeEveryProperty(reader)) return false;
			return true;
		}
		public void InitializeRemoteProperties()
		{
			_userId = new();
			_username = new();
			_faction = (Faction)0;
			_isEliminated = false;
			_isHost = false;
			_isReady = false;
			_isMapLoaded = false;
			_selectedCostume.InitializeRemoteProperties();
			_currentCostume.InitializeRemoteProperties();
		}
		public void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					UserId.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[1])
				{
					NetStringShort.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[2])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[4])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[7])
				{
					_selectedCostume.IgnoreSyncReliable(reader);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					_currentCostume.IgnoreSyncReliable(reader);
				}
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0.AnyTrue())
			{
				if (dirtyReliable_0[0])
				{
					UserId.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[1])
				{
					NetStringShort.IgnoreStatic(reader);
				}
				if (dirtyReliable_0[2])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[3])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[4])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[5])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[6])
				{
					reader.Ignore(1);
				}
				if (dirtyReliable_0[7])
				{
					CostumeSet.IgnoreSyncStaticReliable(reader);
				}
			}
			BitmaskByte dirtyReliable_1 = reader.ReadBitmaskByte();
			if (dirtyReliable_1.AnyTrue())
			{
				if (dirtyReliable_1[0])
				{
					CostumeSet.IgnoreSyncStaticReliable(reader);
				}
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
