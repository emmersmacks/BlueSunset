using System;
using System.Collections.Generic;
using System.Linq;
using CutTwice.Gameplay.Runtime.Map;
using CutTwice.Gameplay.Runtime.Map.Nodes;
using CutTwice.Gameplay.Runtime.Map.Presets;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CutTwice.Editor.MapGraph
{
    public class MapGraphView : GraphView
    {
        public event Action<SerializedProperty> SelectionChanged;

        private MapDefinition _map;
        private SerializedObject _serializedMap;
        private bool _suppressChangeHandling;

        private readonly Dictionary<MapNode, MapNodeView> _nodeViews = new();
        private readonly Dictionary<string, MapNodeView> _viewsByInstanceId = new();
        private readonly Dictionary<Edge, MapConnection> _edgeToConnection = new();
        private readonly Dictionary<MapConnection, Edge> _connectionToEdge = new();

        public MapGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            graphViewChanged = OnGraphViewChanged;
        }

        public void Load(SerializedObject serializedMap)
        {
            _serializedMap = serializedMap;
            _map = (MapDefinition)serializedMap.targetObject;
            Rebuild();
        }

        public void Unload()
        {
            _serializedMap = null;
            _map = null;

            _suppressChangeHandling = true;
            DeleteElements(graphElements.ToList());
            _suppressChangeHandling = false;

            _nodeViews.Clear();
            _viewsByInstanceId.Clear();
            _edgeToConnection.Clear();
            _connectionToEdge.Clear();
        }

        public void Relayout()
        {
            ApplyLayout();
        }

        public void SetRoot(MapNodeView nodeView)
        {
            if (nodeView.Node is not LocationNode)
            {
                EditorUtility.DisplayDialog("Invalid Root", "Only Location nodes can be the map's root.", "OK");
                return;
            }

            _serializedMap.Update();
            var rootProp = _serializedMap.FindProperty(nameof(MapDefinition.RootInstanceId));
            rootProp.stringValue = nodeView.Node.InstanceId;
            _serializedMap.ApplyModifiedProperties();

            foreach (var view in _nodeViews.Values)
            {
                view.RefreshRootBadge(_map.RootInstanceId);
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            // evt.target for an empty-canvas click is not guaranteed to be this exact GraphView instance
            // (it can be an internal child such as contentViewContainer) - only skip when the click was
            // on an existing graph element (node/edge), which already has its own contextual menu entries.
            if (evt.target is GraphElement || _map == null)
            {
                return;
            }

            AppendCreateNodeActions<LocationPreset>(evt, "Create Location Node", AddLocationNode);
            AppendCreateNodeActions<CrossroadPreset>(evt, "Create Crossroad Node", AddCrossroadNode);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()
                .Where(p => p.direction != startPort.direction && p.node != startPort.node)
                .ToList();
        }

        // Appends one action per matching preset asset as "<menuPath>/<preset display name>" directly on
        // evt.menu (a UI Toolkit DropdownMenu). Deliberately does NOT use GenericMenu.ShowAsContext() here -
        // that API expects an active IMGUI Event and silently does nothing when invoked from a UI Toolkit
        // dropdown-action callback, which is exactly the context this method runs in.
        private static void AppendCreateNodeActions<TPreset>(ContextualMenuPopulateEvent evt, string menuPath, Action<TPreset> onSelected)
            where TPreset : NodePreset
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(TPreset).Name}");

            if (guids.Length == 0)
            {
                evt.menu.AppendAction($"{menuPath}/No {typeof(TPreset).Name} assets found", _ => { }, _ => DropdownMenuAction.Status.Disabled);
                return;
            }

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var preset = AssetDatabase.LoadAssetAtPath<TPreset>(path);
                var label = string.IsNullOrEmpty(preset.DisplayName) ? preset.name : preset.DisplayName;
                evt.menu.AppendAction($"{menuPath}/{label}", _ => onSelected(preset), DropdownMenuAction.AlwaysEnabled);
            }
        }

        private void AddLocationNode(LocationPreset preset)
        {
            AddNode(new LocationNode { Preset = preset });
        }

        private void AddCrossroadNode(CrossroadPreset preset)
        {
            AddNode(new CrossroadNode { Preset = preset });
        }

        private void AddNode(MapNode node)
        {
            var undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Add Map Node");

            _serializedMap.Update();
            var nodesProp = _serializedMap.FindProperty(nameof(MapDefinition.Nodes));
            var index = nodesProp.arraySize;
            nodesProp.InsertArrayElementAtIndex(index);
            nodesProp.GetArrayElementAtIndex(index).managedReferenceValue = node;
            _serializedMap.ApplyModifiedProperties();

            Undo.CollapseUndoOperations(undoGroup);

            var addedNode = _map.Nodes[^1];
            var view = new MapNodeView(addedNode, this);
            view.RefreshRootBadge(_map.RootInstanceId);
            _nodeViews[addedNode] = view;
            _viewsByInstanceId[addedNode.InstanceId] = view;
            AddElement(view);

            ApplyLayout();
        }

        private void Rebuild()
        {
            _suppressChangeHandling = true;
            DeleteElements(graphElements.ToList());
            _suppressChangeHandling = false;

            _nodeViews.Clear();
            _viewsByInstanceId.Clear();
            _edgeToConnection.Clear();
            _connectionToEdge.Clear();

            if (_map == null)
            {
                return;
            }

            foreach (var node in _map.Nodes)
            {
                if (node == null)
                {
                    continue;
                }

                var view = new MapNodeView(node, this);
                view.RefreshRootBadge(_map.RootInstanceId);
                _nodeViews[node] = view;
                _viewsByInstanceId[node.InstanceId] = view;
                AddElement(view);
            }

            foreach (var connection in _map.Connections)
            {
                if (!_viewsByInstanceId.TryGetValue(connection.NodeAId, out var a) ||
                    !_viewsByInstanceId.TryGetValue(connection.NodeBId, out var b))
                {
                    continue;
                }

                var edge = a.OutputPort.ConnectTo(b.InputPort);
                _edgeToConnection[edge] = connection;
                _connectionToEdge[connection] = edge;
                AddElement(edge);
            }

            ApplyLayout();
        }

        private void ApplyLayout()
        {
            if (_map == null)
            {
                return;
            }

            var layout = MapGraphAutoLayout.ComputeLayout(_map);
            foreach (var pair in _viewsByInstanceId)
            {
                if (layout.TryGetValue(pair.Key, out var rect))
                {
                    pair.Value.SetPosition(rect);
                }
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (_suppressChangeHandling)
            {
                return change;
            }

            var undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Edit Map Graph");

            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    var connection = CreateConnection(edge);
                    if (connection != null)
                    {
                        _edgeToConnection[edge] = connection;
                        _connectionToEdge[connection] = edge;
                    }
                }
            }

            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    switch (element)
                    {
                        case Edge edge when _edgeToConnection.TryGetValue(edge, out var connection):
                            RemoveConnection(connection);
                            break;
                        case MapNodeView nodeView:
                            RemoveNode(nodeView.Node);
                            break;
                    }
                }
            }

            Undo.CollapseUndoOperations(undoGroup);

            return change;
        }

        private MapConnection CreateConnection(Edge edge)
        {
            if (edge.output?.node is not MapNodeView sourceView || edge.input?.node is not MapNodeView targetView)
            {
                return null;
            }

            _serializedMap.Update();
            var connectionsProp = _serializedMap.FindProperty(nameof(MapDefinition.Connections));
            var index = connectionsProp.arraySize;
            connectionsProp.InsertArrayElementAtIndex(index);
            var element = connectionsProp.GetArrayElementAtIndex(index);
            element.FindPropertyRelative(nameof(MapConnection.NodeAId)).stringValue = sourceView.Node.InstanceId;
            element.FindPropertyRelative(nameof(MapConnection.NodeBId)).stringValue = targetView.Node.InstanceId;
            element.FindPropertyRelative(nameof(MapConnection.Direction)).enumValueIndex = (int)ConnectionDirection.Forward;
            element.FindPropertyRelative(nameof(MapConnection.DisplayName)).stringValue = string.Empty;
            _serializedMap.ApplyModifiedProperties();

            return _map.Connections[^1];
        }

        private void RemoveConnection(MapConnection connection)
        {
            _serializedMap.Update();
            var connectionsProp = _serializedMap.FindProperty(nameof(MapDefinition.Connections));
            var index = _map.Connections.IndexOf(connection);
            if (index >= 0)
            {
                connectionsProp.DeleteArrayElementAtIndex(index);
                _serializedMap.ApplyModifiedProperties();
            }

            if (_connectionToEdge.TryGetValue(connection, out var edge))
            {
                _connectionToEdge.Remove(connection);
                _edgeToConnection.Remove(edge);
            }
        }

        private void RemoveNode(MapNode node)
        {
            _serializedMap.Update();

            var connectionsProp = _serializedMap.FindProperty(nameof(MapDefinition.Connections));
            for (var i = connectionsProp.arraySize - 1; i >= 0; i--)
            {
                var element = connectionsProp.GetArrayElementAtIndex(i);
                var a = element.FindPropertyRelative(nameof(MapConnection.NodeAId)).stringValue;
                var b = element.FindPropertyRelative(nameof(MapConnection.NodeBId)).stringValue;
                if (a == node.InstanceId || b == node.InstanceId)
                {
                    connectionsProp.DeleteArrayElementAtIndex(i);
                }
            }

            var rootProp = _serializedMap.FindProperty(nameof(MapDefinition.RootInstanceId));
            if (rootProp.stringValue == node.InstanceId)
            {
                rootProp.stringValue = string.Empty;
            }

            var nodesProp = _serializedMap.FindProperty(nameof(MapDefinition.Nodes));
            var index = _map.Nodes.IndexOf(node);
            if (index >= 0)
            {
                nodesProp.DeleteArrayElementAtIndex(index);
            }

            _serializedMap.ApplyModifiedProperties();

            _nodeViews.Remove(node);
            _viewsByInstanceId.Remove(node.InstanceId);
        }

        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            NotifySelectionChanged();
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            NotifySelectionChanged();
        }

        public override void ClearSelection()
        {
            base.ClearSelection();
            NotifySelectionChanged();
        }

        private void NotifySelectionChanged()
        {
            if (_serializedMap == null || selection.Count == 0)
            {
                SelectionChanged?.Invoke(null);
                return;
            }

            SerializedProperty property = selection[0] switch
            {
                MapNodeView nodeView => FindNodeProperty(nodeView.Node),
                Edge edge when _edgeToConnection.TryGetValue(edge, out var connection) => FindConnectionProperty(connection),
                _ => null
            };

            SelectionChanged?.Invoke(property);
        }

        private SerializedProperty FindNodeProperty(MapNode node)
        {
            _serializedMap.Update();
            var nodesProp = _serializedMap.FindProperty(nameof(MapDefinition.Nodes));
            var index = _map.Nodes.IndexOf(node);
            return index >= 0 ? nodesProp.GetArrayElementAtIndex(index) : null;
        }

        private SerializedProperty FindConnectionProperty(MapConnection connection)
        {
            _serializedMap.Update();
            var connectionsProp = _serializedMap.FindProperty(nameof(MapDefinition.Connections));
            var index = _map.Connections.IndexOf(connection);
            return index >= 0 ? connectionsProp.GetArrayElementAtIndex(index) : null;
        }
    }
}
