using System;
using System.Collections.Generic;
using CutTwice.Gameplay.Runtime.Map.Presets;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Map.Nodes
{
    [Serializable]
    public class LocationNode : MapNode
    {
        public LocationPreset Preset;

        [Header("Map-specific overrides")]
        public List<string> ItemIdsForSale = new();
    }
}
