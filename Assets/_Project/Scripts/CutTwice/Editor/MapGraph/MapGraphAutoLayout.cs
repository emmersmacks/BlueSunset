using System.Collections.Generic;
using CutTwice.Gameplay.Runtime.Map;
using UnityEngine;

namespace CutTwice.Editor.MapGraph
{
    // Pure layout computation, no GraphView/UI dependency: positions are never persisted,
    // they're recomputed from Nodes/Connections every time the graph view is (re)built.
    public static class MapGraphAutoLayout
    {
        private const float ColumnSpacing = 260f;
        private const float RowSpacing = 140f;
        private const float NodeWidth = 200f;
        private const float NodeHeight = 100f;

        public static Dictionary<string, Rect> ComputeLayout(MapDefinition map)
        {
            var positions = new Dictionary<string, Rect>();
            if (map == null || map.Nodes == null)
            {
                return positions;
            }

            var byId = new Dictionary<string, MapNode>();
            foreach (var node in map.Nodes)
            {
                if (node != null && !string.IsNullOrEmpty(node.InstanceId))
                {
                    byId[node.InstanceId] = node;
                }
            }

            var placed = new HashSet<string>();
            var columns = new List<List<string>>();

            void PlaceComponent(string startId)
            {
                var depth = new Dictionary<string, int> { [startId] = 0 };
                var order = new List<string> { startId };
                var queue = new Queue<string>();
                queue.Enqueue(startId);
                placed.Add(startId);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    var currentDepth = depth[current];

                    foreach (var (_, targetId) in map.GetOutgoing(current))
                    {
                        if (!byId.ContainsKey(targetId) || placed.Contains(targetId))
                        {
                            continue;
                        }

                        depth[targetId] = currentDepth + 1;
                        placed.Add(targetId);
                        order.Add(targetId);
                        queue.Enqueue(targetId);
                    }
                }

                foreach (var id in order)
                {
                    var depthValue = depth[id];
                    while (columns.Count <= depthValue)
                    {
                        columns.Add(new List<string>());
                    }

                    columns[depthValue].Add(id);
                }
            }

            // Layer the graph reachable from the root first (proper BFS depth from the map's entry point)...
            if (!string.IsNullOrEmpty(map.RootInstanceId) && byId.ContainsKey(map.RootInstanceId))
            {
                PlaceComponent(map.RootInstanceId);
            }

            // ...then treat every still-unplaced node as the root of its own component. This covers both
            // mid-authoring maps (no root chosen yet) and nodes disconnected from the main graph — every
            // node always ends up with a deterministic position, never left at a stale/arbitrary spot.
            foreach (var id in byId.Keys)
            {
                if (!placed.Contains(id))
                {
                    PlaceComponent(id);
                }
            }

            for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                var column = columns[columnIndex];
                for (var rowIndex = 0; rowIndex < column.Count; rowIndex++)
                {
                    var x = columnIndex * ColumnSpacing;
                    var y = rowIndex * RowSpacing;
                    positions[column[rowIndex]] = new Rect(x, y, NodeWidth, NodeHeight);
                }
            }

            return positions;
        }
    }
}
