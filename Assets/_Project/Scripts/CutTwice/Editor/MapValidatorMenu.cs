using CutTwice.Gameplay.Runtime.Map;
using UnityEditor;
using UnityEngine;

namespace CutTwice.Editor
{
    public static class MapValidatorMenu
    {
        [MenuItem("Tools/Map/Validate Selected Map")]
        public static void ValidateSelectedMap()
        {
            if (Selection.activeObject is not MapDefinition map)
            {
                Debug.LogWarning("Выберите ассет MapDefinition в Project-окне, чтобы его провалидировать.");
                return;
            }

            var result = MapValidator.Validate(map);

            foreach (var error in result.Errors)
            {
                Debug.LogError($"[{map.name}] {error}", map);
            }

            foreach (var warning in result.Warnings)
            {
                Debug.LogWarning($"[{map.name}] {warning}", map);
            }

            if (result.IsValid)
            {
                Debug.Log($"[{map.name}] Карта валидна" + (result.Warnings.Count > 0 ? $" (предупреждений: {result.Warnings.Count})." : "."), map);
            }
        }
    }
}
