/*
 * Generated File : Remote_SoundController
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
	public partial class SoundController : IRemoteSynchronizable
	{
		[SyncRpc]
		public partial void Play();
		[AllowNull] public IDirtyable _owner;
		public void BindOwner(IDirtyable owner) => _owner = owner;
		public SoundController()
		{
		}
		public SoundController(IDirtyable owner)
		{
			_owner = owner;
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
			if (dirtyReliable_0[0])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					Play();
				}
			}
			return true;
		}
		public bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public bool TryDeserializeEveryProperty(IPacketReader reader) => true;
		public void InitializeRemoteProperties() { }
		public void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
		}
		public static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
		}
		public void IgnoreSyncUnreliable(IPacketReader reader) { }
		public static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
