using System;
using CutTwice.Gameplay.Runtime.Map.Presets;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Map.Nodes
{
    public enum BiomeType
    {
        Forest
    }

    [Serializable]
    public class CrossroadNode : MapNode
    {
        public CrossroadPreset Preset;

        [Header("Map-specific overrides")]
        public BiomeType Biome;
    }
}
