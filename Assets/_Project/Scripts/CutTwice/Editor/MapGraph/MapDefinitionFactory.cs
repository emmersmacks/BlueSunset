using CutTwice.Gameplay.Runtime.Map;
using CutTwice.Gameplay.Runtime.Map.Nodes;
using UnityEditor;
using UnityEngine;

namespace CutTwice.Editor.MapGraph
{
    public static class MapDefinitionFactory
    {
        [MenuItem("Assets/Create/CutTwice/Map/MapDefinition")]
        public static void CreateMapDefinition()
        {
            var map = ScriptableObject.CreateInstance<MapDefinition>();

            var settings = MapEditorSettings.Load();
            var rootNode = new LocationNode { Preset = settings != null ? settings.DefaultRootPreset : null };
            map.Nodes.Add(rootNode);
            map.RootInstanceId = rootNode.InstanceId;

            ProjectWindowUtil.CreateAsset(map, "New MapDefinition.asset");
        }
    }
}
