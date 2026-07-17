using System.Text;
using CutTwice.Gameplay.Runtime.Map;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CutTwice.Editor.MapGraph
{
    public class MapGraphEditorWindow : EditorWindow
    {
        private MapDefinition _map;
        private SerializedObject _serializedMap;

        private ObjectField _mapField;
        private Label _validationLabel;
        private MapGraphView _graphView;
        private MapNodeDetailsPanel _detailsPanel;

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is not MapDefinition asset)
            {
                return false;
            }

            Open(asset);
            return true;
        }

        [MenuItem("Tools/Map/Open Map Graph Editor")]
        public static void OpenEmpty()
        {
            var window = GetWindow<MapGraphEditorWindow>();
            window.titleContent = new GUIContent("Map Graph");

            if (Selection.activeObject is MapDefinition selected)
            {
                window.LoadMap(selected);
            }
        }

        public static void Open(MapDefinition map)
        {
            var window = GetWindow<MapGraphEditorWindow>();
            window.titleContent = new GUIContent("Map Graph");
            window.LoadMap(map);
        }

        public void CreateGUI()
        {
            var toolbar = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    paddingTop = 4,
                    paddingBottom = 4,
                    paddingLeft = 4,
                    paddingRight = 4
                }
            };

            _mapField = new ObjectField("Map") { objectType = typeof(MapDefinition), allowSceneObjects = false };
            _mapField.style.flexGrow = 1;
            _mapField.RegisterValueChangedCallback(evt => LoadMap(evt.newValue as MapDefinition));
            toolbar.Add(_mapField);

            toolbar.Add(new Button(() => _graphView?.Relayout()) { text = "Re-layout" });
            toolbar.Add(new Button(RunValidation) { text = "Validate" });

            rootVisualElement.Add(toolbar);

            _validationLabel = new Label { style = { whiteSpace = WhiteSpace.Normal, paddingLeft = 4, paddingRight = 4 } };
            rootVisualElement.Add(_validationLabel);

            var splitView = new TwoPaneSplitView(1, 320, TwoPaneSplitViewOrientation.Horizontal)
            {
                style = { flexGrow = 1 }
            };
            rootVisualElement.Add(splitView);

            _graphView = new MapGraphView { style = { flexGrow = 1 } };
            _graphView.SelectionChanged += property => _detailsPanel.Bind(property);
            splitView.Add(_graphView);

            _detailsPanel = new MapNodeDetailsPanel();
            splitView.Add(_detailsPanel);

            if (_map != null)
            {
                LoadMap(_map);
            }
        }

        private void LoadMap(MapDefinition map)
        {
            _map = map;
            _mapField?.SetValueWithoutNotify(map);

            if (_graphView == null)
            {
                // CreateGUI hasn't run yet; it will call LoadMap again once the UI exists.
                return;
            }

            _detailsPanel.Bind(null);
            _validationLabel.text = string.Empty;

            if (map == null)
            {
                _serializedMap = null;
                _graphView.Unload();
                return;
            }

            _serializedMap = new SerializedObject(map);
            _graphView.Load(_serializedMap);
        }

        private void RunValidation()
        {
            if (_map == null)
            {
                _validationLabel.text = "No map loaded.";
                return;
            }

            var result = MapValidator.Validate(_map);
            var text = new StringBuilder();

            foreach (var error in result.Errors)
            {
                text.AppendLine("Error: " + error);
            }

            foreach (var warning in result.Warnings)
            {
                text.AppendLine("Warning: " + warning);
            }

            if (result.IsValid && result.Warnings.Count == 0)
            {
                text.Append("Map is valid.");
            }

            _validationLabel.text = text.ToString();
        }
    }
}
