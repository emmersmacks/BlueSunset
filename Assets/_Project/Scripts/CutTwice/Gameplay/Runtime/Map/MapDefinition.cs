using System.Collections.Generic;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Map
{
    [CreateAssetMenu(fileName = "MapDefinition", menuName = "CutTwice/Map/MapDefinition")]
    public class MapDefinition : ScriptableObject
    {
        [Header("Shop Metadata")]
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;

        [Header("Graph")]
        [SerializeReference] public List<MapNode> Nodes = new();
        public List<MapConnection> Connections = new();
        public string RootInstanceId;

        public IEnumerable<(MapConnection Connection, string TargetId)> GetOutgoing(string nodeId)
        {
            foreach (var connection in Connections)
            {
                if (connection.NodeAId == nodeId && connection.Direction != ConnectionDirection.Backward)
                {
                    yield return (connection, connection.NodeBId);
                }
                else if (connection.NodeBId == nodeId && connection.Direction != ConnectionDirection.Forward)
                {
                    yield return (connection, connection.NodeAId);
                }
            }
        }
    }
}
