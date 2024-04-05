/*
 * Generated File : Remote_Teleporter
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
	public partial class Teleporter
	{
		public override NetworkObjectType Type => NetworkObjectType.Teleporter;
		[SyncVar]
		protected TeleporterShapeType _teleporterShape;
		public TeleporterShapeType TeleporterShape => _teleporterShape;
		protected Action<TeleporterShapeType>? _onTeleporterShapeChanged;
		public event Action<TeleporterShapeType> OnTeleporterShapeChanged
		{
			add => _onTeleporterShapeChanged += value;
			remove => _onTeleporterShapeChanged -= value;
		}
		public Teleporter()
		{
		}
		public override void ClearDirtyReliable()
		{
			_isDirtyReliable = false;
			_dirtyReliable_0.Clear();
			Client_TryInteractCallstackCount = 0;
			Client_TryCancelCallstackCount = 0;
		}
		public override void ClearDirtyUnreliable() { }
		public override void SerializeSyncReliable(IPacketWriter writer)
		{
			_dirtyReliable_0.Serialize(writer);
			if (_dirtyReliable_0[0])
			{
				writer.Put((byte)Client_TryInteractCallstackCount);
			}
			if (_dirtyReliable_0[1])
			{
				writer.Put((byte)Client_TryCancelCallstackCount);
			}
		}
		public override void SerializeSyncUnreliable(IPacketWriter writer) { }
		public override void InitializeMasterProperties() { }
		public override bool TryDeserializeSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				if (!reader.TryReadByte(out var _behaviourTypeValue)) return false;
				_behaviourType = (InteractionBehaviourType)_behaviourTypeValue;
				_onBehaviourTypeChanged?.Invoke(_behaviourType);
			}
			if (dirtyReliable_0[1])
			{
				if (!_size.TryDeserialize(reader)) return false;
				_onSizeChanged?.Invoke(_size);
			}
			if (dirtyReliable_0[2])
			{
				if (!_currentSubjectId.TryDeserialize(reader)) return false;
				_onCurrentSubjectIdChanged?.Invoke(_currentSubjectId);
			}
			if (dirtyReliable_0[3])
			{
				if (!reader.TryReadSingle(out _progressTime)) return false;
				_onProgressTimeChanged?.Invoke(_progressTime);
			}
			if (dirtyReliable_0[4])
			{
				if (!reader.TryReadSingle(out _cooltime)) return false;
				_onCooltimeChanged?.Invoke(_cooltime);
			}
			if (dirtyReliable_0[5])
			{
				if (!reader.TryReadBoolean(out _interactable)) return false;
				_onInteractableChanged?.Invoke(_interactable);
			}
			if (dirtyReliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					if (!reader.TryReadByte(out var resultValue)) return false;
					InteractResultType result = (InteractResultType)resultValue;
					Server_InteractResult(result);
				}
			}
			if (dirtyReliable_0[7])
			{
				if (!reader.TryReadByte(out var _teleporterShapeValue)) return false;
				_teleporterShape = (TeleporterShapeType)_teleporterShapeValue;
				_onTeleporterShapeChanged?.Invoke(_teleporterShape);
			}
			return true;
		}
		public override bool TryDeserializeSyncUnreliable(IPacketReader reader) => true;
		public override bool TryDeserializeEveryProperty(IPacketReader reader)
		{
			if (!reader.TryReadByte(out var _behaviourTypeValue)) return false;
			_behaviourType = (InteractionBehaviourType)_behaviourTypeValue;
			if (!_size.TryDeserialize(reader)) return false;
			if (!_currentSubjectId.TryDeserialize(reader)) return false;
			if (!reader.TryReadSingle(out _progressTime)) return false;
			if (!reader.TryReadSingle(out _cooltime)) return false;
			if (!reader.TryReadBoolean(out _interactable)) return false;
			if (!reader.TryReadByte(out var _teleporterShapeValue)) return false;
			_teleporterShape = (TeleporterShapeType)_teleporterShapeValue;
			return true;
		}
		public override void InitializeRemoteProperties()
		{
			_behaviourType = (InteractionBehaviourType)0;
			_size = new();
			_currentSubjectId = new();
			_progressTime = 0;
			_cooltime = 0;
			_interactable = false;
			_teleporterShape = (TeleporterShapeType)0;
		}
		public override void IgnoreSyncReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				InteractorSize.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				NetworkIdentity.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[4])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[5])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
				}
			}
			if (dirtyReliable_0[7])
			{
				reader.Ignore(1);
			}
		}
		public new static void IgnoreSyncStaticReliable(IPacketReader reader)
		{
			BitmaskByte dirtyReliable_0 = reader.ReadBitmaskByte();
			if (dirtyReliable_0[0])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[1])
			{
				InteractorSize.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[2])
			{
				NetworkIdentity.IgnoreStatic(reader);
			}
			if (dirtyReliable_0[3])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[4])
			{
				reader.Ignore(4);
			}
			if (dirtyReliable_0[5])
			{
				reader.Ignore(1);
			}
			if (dirtyReliable_0[6])
			{
				byte count = reader.ReadByte();
				for (int i = 0; i < count; i++)
				{
					reader.Ignore(1);
				}
			}
			if (dirtyReliable_0[7])
			{
				reader.Ignore(1);
			}
		}
		public override void IgnoreSyncUnreliable(IPacketReader reader) { }
		public new static void IgnoreSyncStaticUnreliable(IPacketReader reader) { }
	}
}
#pragma warning restore CS0649
