using UnityEditor;
using UnityEngine;

namespace CutTwice.Editor
{
    public static class SimpleEditorTools
    {
        [MenuItem("Tools/Clear All PlayerPrefs")]
        public static void ClearAllPlayerPrefs()
        {
            if (EditorUtility.DisplayDialog(
                    "Очистить PlayerPrefs",
                    "Вы уверены, что хотите удалить все сохранённые PlayerPrefs?",
                    "Да", "Отмена"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("Все PlayerPrefs удалены.");
            }
        }
    }
}