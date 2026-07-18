using CutTwice.Gameplay.Runtime.Map.Presets;
using UnityEditor;
using UnityEngine;

namespace CutTwice.Editor.MapGraph
{
    [CreateAssetMenu(fileName = "MapEditorSettings", menuName = "CutTwice/Map/Editor/MapEditorSettings")]
    public class MapEditorSettings : ScriptableObject
    {
        public LocationPreset DefaultRootPreset;

        public static MapEditorSettings Load()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(MapEditorSettings)}");
            return guids.Length == 0 ? null : AssetDatabase.LoadAssetAtPath<MapEditorSettings>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }
    }
}
