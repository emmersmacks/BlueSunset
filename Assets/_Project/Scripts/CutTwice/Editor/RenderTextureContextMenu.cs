using System.IO;
using UnityEditor;
using UnityEngine;

namespace CutTwice.Editor
{
    public class RenderTextureContextMenu
    {
        // Добавим пункт в контекстное меню для RenderTexture в проекте
        [MenuItem("Assets/Сохранить RenderTexture как PNG", true)]
        private static bool ValidateSaveAsPNG()
        {
            // Проверяем, что выделен именно RenderTexture
            return Selection.activeObject is RenderTexture;
        }

        [MenuItem("Assets/Сохранить RenderTexture как PNG")]
        private static void SaveRenderTextureAsPNG()
        {
            RenderTexture rt = Selection.activeObject as RenderTexture;
            if (rt == null)
            {
                Debug.LogError("Выбранный объект не является RenderTexture");
                return;
            }

            int width = rt.width;
            int height = rt.height;

            // Создаем временный RenderTexture и копируем содержимое
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            
            Color[] pixels = tex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].r = Mathf.LinearToGammaSpace(pixels[i].r);
                pixels[i].g = Mathf.LinearToGammaSpace(pixels[i].g);
                pixels[i].b = Mathf.LinearToGammaSpace(pixels[i].b);
                // Альфа-канал не трогаем
            }
            tex.SetPixels(pixels);
            tex.Apply();

            RenderTexture.active = currentRT;

            // Сохраняем в файл рядом с ассетом
            string assetPath = AssetDatabase.GetAssetPath(rt);
            string directory = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath) + "_screenshot.png";
            string fullPath = Path.Combine(directory, fileName);

            byte[] pngData = tex.EncodeToPNG();
            File.WriteAllBytes(fullPath, pngData);
            Debug.Log($"Скриншот RenderTexture сохранён: {fullPath}");

            // Обновим проект, чтобы файл появился в Unity
            AssetDatabase.Refresh();

            Object.DestroyImmediate(tex);
        }
    }
}