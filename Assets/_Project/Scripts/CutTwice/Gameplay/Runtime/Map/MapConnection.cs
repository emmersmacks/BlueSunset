using System;

namespace CutTwice.Gameplay.Runtime.Map
{
    public enum ConnectionDirection
    {
        Forward,
        Backward,
        Bidirectional
    }

    [Serializable]
    public class MapConnection
    {
        public string NodeAId;
        public string NodeBId;
        public ConnectionDirection Direction = ConnectionDirection.Forward;
        public string DisplayName;
    }
}
