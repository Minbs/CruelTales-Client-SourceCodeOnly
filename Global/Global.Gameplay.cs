using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class Global
{
	public static class Physics
	{
		public static class Mask
		{
			public static readonly int Entity = GetMaskByLayerData(Layer.Entity);
			public static readonly int Item = GetMaskByLayerData(Layer.Item);
			public static readonly int Ground = GetMaskByLayerData(Layer.Environment, Layer.Invisible);
			public static readonly int Hitscan = GetMaskByLayerData(Layer.Environment, Layer.Entity, Layer.Item);
			public static readonly int Sound = GetMaskByLayerData(Layer.Sound);
		}

		public static class Layer
		{
			public readonly struct LayerData
			{
				public readonly string Name;
				public readonly int Index;
				public readonly int Mask;

				public LayerData(string layerName)
				{
					Name = layerName;
					Index = LayerMask.NameToLayer(layerName);
					Mask = GetMaskByIndex(Index);
				}

				public static implicit operator int(LayerData value) => value.Index;
			}


			public static readonly LayerData Environment = new(nameof(Environment));
			public static readonly LayerData Invisible = new(nameof(Invisible));
			public static readonly LayerData Entity = new(nameof(Entity));
			public static readonly LayerData Item = new(nameof(Item));
			public static readonly LayerData Hitscan = new(nameof(Hitscan));
			public static readonly LayerData Sound = new(nameof(Sound));
		}

		public static int GetMaskByLayerData(params Layer.LayerData[] layers)
		{
			var layerNames = layers.Select((layer) => layer.Name).ToArray();
			return LayerMask.GetMask(layerNames);
		}

		public static int GetMaskByIndex(int layerIndex) => 1 << layerIndex;

		public static bool IsMatch(int lhsLayerMask, int rhsLayerMask)
			=> (lhsLayerMask & rhsLayerMask) != 0;
	}

	public static class ObjectPrefix
	{
		public const string Collider = "Col_";
		public const string SoundCollider = "Sudcol_";
	}
}

public class SoundInfo
{
	public string Sound_1;
	public string Sound_2;
	public string Sound_3;
}