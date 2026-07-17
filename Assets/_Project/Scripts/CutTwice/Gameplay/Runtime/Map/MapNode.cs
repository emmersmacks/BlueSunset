using System;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Map
{
    [Serializable]
    public abstract class MapNode
    {
        [Tooltip("Stable id of this placement within the map; referenced by MapConnection and MapDefinition.RootInstanceId. Auto-generated on creation, do not edit by hand.")]
        public string InstanceId = Guid.NewGuid().ToString("N");

        [Header("Map-specific overrides")]
        [Tooltip("How many times this node can be visited before a looping path through it is treated as the end of the map. 0 or less = unlimited.")]
        public int MaxPassCount = 1;
    }
}
