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

        [MenuItem("Tools/3D Noise Texture")]
        public static void CreateTexture3D()
        {
            // Configure the texture
            const int size = 32;
            const TextureFormat format = TextureFormat.RGBA32;

            // Noise setup. "frequency" is how many noise cells repeat across the texture and must
            // stay an integer <= 256 at every octave for the result to tile seamlessly (required
            // since the fog shader samples this texture with a repeat wrap mode).
            const int seed = 1337;
            const int frequency = 4;
            const int octaves = 4;
            const float persistence = 0.5f;
            const float lacunarity = 2f;

            // Create the texture and apply the configuration
            Texture3D texture = new Texture3D(size, size, size, format, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Trilinear
            };

            // Each channel uses its own permutation table so R/G/B/A end up as uncorrelated
            // noise fields instead of the same pattern repeated four times
            int[][] channelPermutations =
            {
                BuildPermutationTable(seed),
                BuildPermutationTable(seed + 1),
                BuildPermutationTable(seed + 2),
                BuildPermutationTable(seed + 3)
            };

            // Create a 3-dimensional array to store color data
            Color[] colors = new Color[size * size * size];

            // Populate the array with fractal (multi-octave) 3D Perlin noise
            for (int z = 0; z < size; z++)
            {
                int zOffset = z * size * size;
                float w = (float)z / size;
                for (int y = 0; y < size; y++)
                {
                    int yOffset = y * size;
                    float v = (float)y / size;
                    for (int x = 0; x < size; x++)
                    {
                        float u = (float)x / size;

                        colors[x + yOffset + zOffset] = new Color(
                            FractalNoise3D(u, v, w, frequency, octaves, persistence, lacunarity, channelPermutations[0]),
                            FractalNoise3D(u, v, w, frequency, octaves, persistence, lacunarity, channelPermutations[1]),
                            FractalNoise3D(u, v, w, frequency, octaves, persistence, lacunarity, channelPermutations[2]),
                            FractalNoise3D(u, v, w, frequency, octaves, persistence, lacunarity, channelPermutations[3]));
                    }
                }
            }

            // Copy the color values to the texture
            texture.SetPixels(colors);

            // Apply the changes to the texture and upload the updated texture to the GPU
            texture.Apply();

            // Save the texture to your Unity Project
            AssetDatabase.CreateAsset(texture, "Assets/New3DTexture.asset");
        }

        // Fractal Brownian motion: sums several octaves of noise at increasing frequency and
        // decreasing amplitude for a more natural, cloud-like result than a single noise layer.
        private static float FractalNoise3D(float u, float v, float w, int baseFrequency, int octaves,
            float persistence, float lacunarity, int[] perm)
        {
            float total = 0f;
            float amplitude = 1f;
            float maxAmplitude = 0f;
            int frequency = baseFrequency;

            for (int i = 0; i < octaves; i++)
            {
                total += PerlinNoise3D(u * frequency, v * frequency, w * frequency, frequency, perm) * amplitude;
                maxAmplitude += amplitude;
                amplitude *= persistence;
                frequency = Mathf.RoundToInt(frequency * lacunarity);
            }

            return total / maxAmplitude;
        }

        // Classic (Ken Perlin, "improved noise") 3D gradient noise with periodic wrapping so the
        // result tiles seamlessly every `repeat` units.
        private static float PerlinNoise3D(float x, float y, float z, int repeat, int[] perm)
        {
            x %= repeat;
            y %= repeat;
            z %= repeat;

            int xi = Mathf.FloorToInt(x) & 255;
            int yi = Mathf.FloorToInt(y) & 255;
            int zi = Mathf.FloorToInt(z) & 255;

            float xf = x - Mathf.Floor(x);
            float yf = y - Mathf.Floor(y);
            float zf = z - Mathf.Floor(z);

            float u = Fade(xf);
            float v = Fade(yf);
            float w = Fade(zf);

            int aaa = perm[perm[perm[xi] + yi] + zi];
            int aba = perm[perm[perm[xi] + Inc(yi, repeat)] + zi];
            int aab = perm[perm[perm[xi] + yi] + Inc(zi, repeat)];
            int abb = perm[perm[perm[xi] + Inc(yi, repeat)] + Inc(zi, repeat)];
            int baa = perm[perm[perm[Inc(xi, repeat)] + yi] + zi];
            int bba = perm[perm[perm[Inc(xi, repeat)] + Inc(yi, repeat)] + zi];
            int bab = perm[perm[perm[Inc(xi, repeat)] + yi] + Inc(zi, repeat)];
            int bbb = perm[perm[perm[Inc(xi, repeat)] + Inc(yi, repeat)] + Inc(zi, repeat)];

            float x1 = Mathf.Lerp(Grad(aaa, xf, yf, zf), Grad(baa, xf - 1, yf, zf), u);
            float x2 = Mathf.Lerp(Grad(aba, xf, yf - 1, zf), Grad(bba, xf - 1, yf - 1, zf), u);
            float y1 = Mathf.Lerp(x1, x2, v);

            x1 = Mathf.Lerp(Grad(aab, xf, yf, zf - 1), Grad(bab, xf - 1, yf, zf - 1), u);
            x2 = Mathf.Lerp(Grad(abb, xf, yf - 1, zf - 1), Grad(bbb, xf - 1, yf - 1, zf - 1), u);
            float y2 = Mathf.Lerp(x1, x2, v);

            // Result is in [-1, 1]; remap to [0, 1] so it can be stored in a color channel
            return (Mathf.Lerp(y1, y2, w) + 1f) * 0.5f;
        }

        private static int Inc(int value, int repeat)
        {
            value++;
            if (value >= repeat) value -= repeat;
            return value;
        }

        private static float Fade(float t) => t * t * t * (t * (t * 6f - 15f) + 10f);

        private static float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        // Builds a shuffled 0..255 permutation table (doubled to 512 entries to avoid
        // extra wrapping in the lookups above) - the seed determines the noise pattern.
        private static int[] BuildPermutationTable(int seed)
        {
            var perm = new int[256];
            for (int i = 0; i < 256; i++) perm[i] = i;

            var random = new System.Random(seed);
            for (int i = 255; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (perm[i], perm[j]) = (perm[j], perm[i]);
            }

            var doubled = new int[512];
            for (int i = 0; i < 512; i++) doubled[i] = perm[i & 255];
            return doubled;
        }
    }
}
