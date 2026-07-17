using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Map.Presets
{
    public abstract class NodePreset : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public Sprite Icon;
    }
}
