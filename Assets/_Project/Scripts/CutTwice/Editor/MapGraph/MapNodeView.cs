using CutTwice.Gameplay.Runtime.Map;
using CutTwice.Gameplay.Runtime.Map.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CutTwice.Editor.MapGraph
{
    public class MapNodeView : Node
    {
        public MapNode Node { get; }
        public Port InputPort { get; }
        public Port OutputPort { get; }

        private readonly MapGraphView _owner;
        private readonly Label _rootBadge;

        public MapNodeView(MapNode node, MapGraphView owner)
        {
            Node = node;
            _owner = owner;

            title = BuildTitle(node);

            titleContainer.style.backgroundColor = node is CrossroadNode
                ? new StyleColor(new Color(0.55f, 0.35f, 0.1f))
                : new StyleColor(new Color(0.15f, 0.35f, 0.55f));

            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "In";
            inputContainer.Add(InputPort);

            OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            OutputPort.portName = "Out";
            outputContainer.Add(OutputPort);

            _rootBadge = new Label("ROOT")
            {
                style =
                {
                    color = Color.yellow,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginLeft = 6
                }
            };
            titleContainer.Add(_rootBadge);

            RefreshExpandedState();
            RefreshPorts();
        }

        public void RefreshRootBadge(string rootInstanceId)
        {
            _rootBadge.style.display = Node.InstanceId == rootInstanceId ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (Node is LocationNode)
            {
                evt.menu.AppendAction("Set as Root", _ => _owner.SetRoot(this), DropdownMenuAction.AlwaysEnabled);
            }
        }

        private static string BuildTitle(MapNode node)
        {
            var presetName = node switch
            {
                LocationNode location => location.Preset != null ? location.Preset.DisplayName : null,
                CrossroadNode crossroad => crossroad.Preset != null ? crossroad.Preset.DisplayName : null,
                _ => null
            };

            var kind = node is CrossroadNode ? "Crossroad" : "Location";
            var shortId = node.InstanceId.Length >= 6 ? node.InstanceId.Substring(0, 6) : node.InstanceId;
            return string.IsNullOrEmpty(presetName) ? $"{kind} ({shortId})" : presetName;
        }
    }
}
