using System.Collections.Generic;
using System.Linq;
using CutTwice.Gameplay.Runtime.Map.Nodes;

namespace CutTwice.Gameplay.Runtime.Map
{
    public readonly struct MapValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public IReadOnlyList<string> Errors { get; }
        public IReadOnlyList<string> Warnings { get; }

        public MapValidationResult(IReadOnlyList<string> errors, IReadOnlyList<string> warnings)
        {
            Errors = errors;
            Warnings = warnings;
        }
    }

    public static class MapValidator
    {
        public static MapValidationResult Validate(MapDefinition map)
        {
            var errors = new List<string>();
            var warnings = new List<string>();

            var byId = new Dictionary<string, MapNode>();
            foreach (var node in map.Nodes)
            {
                if (node == null)
                {
                    errors.Add("Nodes contains a null entry.");
                    continue;
                }

                if (string.IsNullOrEmpty(node.InstanceId))
                {
                    errors.Add("A node has an empty InstanceId.");
                    continue;
                }

                if (!byId.TryAdd(node.InstanceId, node))
                {
                    errors.Add($"Duplicate InstanceId '{node.InstanceId}'.");
                }
            }

            foreach (var connection in map.Connections)
            {
                if (!byId.ContainsKey(connection.NodeAId))
                {
                    errors.Add($"Connection references missing NodeAId '{connection.NodeAId}'.");
                }

                if (!byId.ContainsKey(connection.NodeBId))
                {
                    errors.Add($"Connection references missing NodeBId '{connection.NodeBId}'.");
                }
            }

            MapNode root = null;
            if (string.IsNullOrEmpty(map.RootInstanceId) || !byId.TryGetValue(map.RootInstanceId, out root))
            {
                errors.Add("RootInstanceId does not resolve to a node in Nodes.");
            }
            else if (root is not LocationNode)
            {
                errors.Add("Root node must be a LocationNode.");
            }

            foreach (var pair in byId)
            {
                var id = pair.Key;
                var node = pair.Value;
                var outgoingCount = map.GetOutgoing(id).Count();
                switch (node)
                {
                    case LocationNode when outgoingCount > 1:
                        errors.Add($"LocationNode '{id}' has {outgoingCount} outgoing connections, expected at most 1.");
                        break;
                    case CrossroadNode when outgoingCount < 2:
                        errors.Add($"CrossroadNode '{id}' has {outgoingCount} outgoing connections, expected at least 2.");
                        break;
                }
            }

            if (root == null)
            {
                return new MapValidationResult(errors, warnings);
            }

            var reachable = CollectReachable(map, root.InstanceId);

            foreach (var id in byId.Keys)
            {
                if (!reachable.Contains(id))
                {
                    warnings.Add($"Node '{id}' is not reachable from the root.");
                }
            }

            var canReachSink = CollectCanReachSink(map, byId, reachable);
            foreach (var id in reachable)
            {
                if (!canReachSink.Contains(id))
                {
                    warnings.Add($"Node '{id}' cannot reach a classic dead end (0 outgoing connections) — if this is an intentional loop, make sure MaxPassCount is configured on the involved nodes.");
                }
            }

            return new MapValidationResult(errors, warnings);
        }

        private static HashSet<string> CollectReachable(MapDefinition map, string rootId)
        {
            var visited = new HashSet<string> { rootId };
            var queue = new Queue<string>();
            queue.Enqueue(rootId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var (_, targetId) in map.GetOutgoing(current))
                {
                    if (visited.Add(targetId))
                    {
                        queue.Enqueue(targetId);
                    }
                }
            }

            return visited;
        }

        private static HashSet<string> CollectCanReachSink(MapDefinition map, IReadOnlyDictionary<string, MapNode> byId, HashSet<string> reachable)
        {
            var sinks = reachable
                .Where(id => byId[id] is LocationNode && !map.GetOutgoing(id).Any())
                .ToList();

            var canReach = new HashSet<string>(sinks);
            var queue = new Queue<string>(sinks);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var id in reachable)
                {
                    if (canReach.Contains(id))
                    {
                        continue;
                    }

                    if (map.GetOutgoing(id).Any(o => o.TargetId == current))
                    {
                        canReach.Add(id);
                        queue.Enqueue(id);
                    }
                }
            }

            return canReach;
        }
    }
}
