using System;
using System.Collections.Generic;
using System.Linq;

namespace CutTwice.Gameplay.Runtime.Map
{
    public readonly struct MapTransition
    {
        public MapConnection Connection { get; }
        public MapNode Target { get; }

        public MapTransition(MapConnection connection, MapNode target)
        {
            Connection = connection;
            Target = target;
        }
    }

    public class MapRuntimeState
    {
        public MapDefinition Map { get; }
        public MapNode CurrentNode { get; private set; }
        public IReadOnlyList<MapNode> History => _history;
        public bool IsCompleted { get; private set; }

        private readonly Dictionary<string, MapNode> _byId;
        private readonly Dictionary<string, int> _visitCounts = new();
        private readonly List<MapNode> _history = new();

        public MapRuntimeState(MapDefinition map)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
            _byId = map.Nodes.ToDictionary(node => node.InstanceId);

            if (!_byId.TryGetValue(map.RootInstanceId, out var root))
            {
                throw new InvalidOperationException($"RootInstanceId '{map.RootInstanceId}' does not resolve to a node in '{map.name}'.");
            }

            CurrentNode = root;
            Enter(CurrentNode);
        }

        public IReadOnlyList<MapTransition> AvailableExits =>
            Map.GetOutgoing(CurrentNode.InstanceId)
                .Select(o => new MapTransition(o.Connection, _byId[o.TargetId]))
                .ToList();

        public bool CanContinue => !IsCompleted;

        public void MoveTo(MapTransition transition)
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("Cannot move — the map has already been completed.");
            }

            if (!AvailableExits.Any(t => t.Connection == transition.Connection))
            {
                throw new ArgumentException("The given transition is not a valid exit from CurrentNode.", nameof(transition));
            }

            CurrentNode = transition.Target;
            Enter(CurrentNode);
        }

        private void Enter(MapNode node)
        {
            _history.Add(node);
            _visitCounts[node.InstanceId] = _visitCounts.GetValueOrDefault(node.InstanceId) + 1;
            IsCompleted = EvaluateCompletion(node);
        }

        private bool EvaluateCompletion(MapNode node)
        {
            var exits = Map.GetOutgoing(node.InstanceId).ToList();
            if (exits.Count == 0)
            {
                return true;
            }

            if (exits.Count == 1)
            {
                var targetVisited = _visitCounts.GetValueOrDefault(exits[0].TargetId) > 0;
                var maxReached = node.MaxPassCount > 0 && _visitCounts[node.InstanceId] >= node.MaxPassCount;
                if (targetVisited && maxReached)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
