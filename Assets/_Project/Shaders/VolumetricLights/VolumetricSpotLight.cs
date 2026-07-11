using UnityEngine;

/// <summary>
/// Объёмный световой конус для SpotLight.
/// Повесьте на объект со SpotLight — скрипт сгенерирует конусный меш,
/// подгонит его под угол и дальность света и будет синхронизировать цвет.
/// Материал должен использовать шейдер Custom/URP/VolumetricSpotCone.
/// Для мягкого пересечения с геометрией в URP Asset должна быть включена Depth Texture.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(Light))]
public class VolumetricSpotLight : MonoBehaviour
{
    [Tooltip("Материал с шейдером Custom/URP/VolumetricSpotCone")]
    public Material coneMaterial;

    [Header("Форма конуса")]
    [Range(0.1f, 1f)]
    [Tooltip("Доля дальности света, которую занимает видимый конус")]
    public float rangeMultiplier = 0.8f;

    [Range(0.5f, 1.2f)]
    [Tooltip("Множитель угла: <1 — конус уже светового пятна, выглядит естественнее")]
    public float angleMultiplier = 0.9f;

    [Min(0f)]
    [Tooltip("Радиус у источника: 0 — острый конус, >0 — усечённый (как у фонаря или прожектора)")]
    public float sourceRadius = 0.1f;

    [Range(8, 64)]
    public int segments = 32;

    [Header("Вид")]
    [Range(0f, 5f)]
    public float intensity = 1f;

    [Tooltip("Брать цвет из компонента Light")]
    public bool syncColorWithLight = true;

    private Light _light;
    private GameObject _coneGO;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;
    private Material _materialInstance;

    private float _lastAngle, _lastRange, _lastRangeMult, _lastAngleMult, _lastSourceRadius;
    private int _lastSegments;

    private static readonly int ColorId     = Shader.PropertyToID("_Color");
    private static readonly int IntensityId = Shader.PropertyToID("_Intensity");

    private void OnEnable()
    {
        _light = GetComponent<Light>();
        CreateConeObject();
        RebuildMesh();
    }

    private void OnDisable()
    {
        if (_coneGO != null) DestroyImmediate(_coneGO);
        if (_mesh != null) DestroyImmediate(_mesh);
        if (_materialInstance != null) DestroyImmediate(_materialInstance);
    }

    private void LateUpdate()
    {
        if (_light == null || _light.type != LightType.Spot)
            return;

        if (!Mathf.Approximately(_lastAngle, _light.spotAngle) ||
            !Mathf.Approximately(_lastRange, _light.range) ||
            !Mathf.Approximately(_lastRangeMult, rangeMultiplier) ||
            !Mathf.Approximately(_lastAngleMult, angleMultiplier) ||
            !Mathf.Approximately(_lastSourceRadius, sourceRadius) ||
            _lastSegments != segments)
        {
            RebuildMesh();
        }

        SyncMaterial();
    }

    private void CreateConeObject()
    {
        if (_coneGO != null) return;

        _coneGO = new GameObject("VolumetricCone") { hideFlags = HideFlags.HideAndDontSave };
        _coneGO.transform.SetParent(transform, false);

        _meshFilter = _coneGO.AddComponent<MeshFilter>();
        _meshRenderer = _coneGO.AddComponent<MeshRenderer>();
        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _meshRenderer.receiveShadows = false;
        _meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;

        _mesh = new Mesh { name = "VolumetricConeMesh" };
        _meshFilter.sharedMesh = _mesh;
    }

    private void SyncMaterial()
    {
        if (coneMaterial == null) return;

        if (_materialInstance == null || _meshRenderer.sharedMaterial == null)
        {
            _materialInstance = new Material(coneMaterial) { hideFlags = HideFlags.HideAndDontSave };
            _meshRenderer.sharedMaterial = _materialInstance;
        }

        if (syncColorWithLight)
        {
            _materialInstance.SetColor(ColorId, _light.color);
        }
        _materialInstance.SetFloat(IntensityId, intensity * Mathf.Clamp01(_light.intensity));
    }

    private void RebuildMesh()
    {
        if (_light == null || _mesh == null) return;

        _lastAngle = _light.spotAngle;
        _lastRange = _light.range;
        _lastSegments = segments;
        _lastRangeMult = rangeMultiplier;
        _lastAngleMult = angleMultiplier;
        _lastSourceRadius = sourceRadius;

        float height = _light.range * rangeMultiplier;
        float halfAngleRad = Mathf.Min(89f, _light.spotAngle * 0.5f * angleMultiplier) * Mathf.Deg2Rad;

        // Усечённый конус: у источника кольцо радиуса sourceRadius,
        // дальний радиус растёт от него по углу света
        float nearRadius = Mathf.Max(0f, sourceRadius);
        float farRadius  = nearRadius + Mathf.Tan(halfAngleRad) * height;

        // UV: v = 0 у источника, v = 1 на дальнем краю — шейдер гасит луч по v.
        int ringVerts = segments + 1; // +1 вершина для замыкания UV-шва
        var vertices = new Vector3[ringVerts * 2];
        var normals  = new Vector3[ringVerts * 2];
        var uvs      = new Vector2[ringVerts * 2];
        var tris     = new int[segments * 6];

        // Наклон боковой поверхности определяется разницей радиусов
        float deltaRadius = farRadius - nearRadius;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float ang = t * Mathf.PI * 2f;
            float cos = Mathf.Cos(ang);
            float sin = Mathf.Sin(ang);

            // Гладкая наружная нормаль боковой поверхности усечённого конуса —
            // от неё зависит мягкость краёв в шейдере (edge fade).
            // При deltaRadius = 0 вырождается в нормаль цилиндра (cos, sin, 0).
            Vector3 n = new Vector3(cos * height, sin * height, -deltaRadius).normalized;

            // Кольцо у источника
            vertices[i] = new Vector3(cos * nearRadius, sin * nearRadius, 0f);
            normals[i]  = n;
            uvs[i]      = new Vector2(t, 0f);

            // Кольцо на дальнем краю
            vertices[ringVerts + i] = new Vector3(cos * farRadius, sin * farRadius, height);
            normals[ringVerts + i]  = n;
            uvs[ringVerts + i]      = new Vector2(t, 1f);
        }

        for (int i = 0; i < segments; i++)
        {
            int ti = i * 6;
            int a = i, b = i + 1;
            int c = ringVerts + i, d = ringVerts + i + 1;

            tris[ti + 0] = a; tris[ti + 1] = c; tris[ti + 2] = b;
            tris[ti + 3] = b; tris[ti + 4] = c; tris[ti + 5] = d;
        }

        _mesh.Clear();
        _mesh.vertices  = vertices;
        _mesh.normals   = normals;
        _mesh.uv        = uvs;
        _mesh.triangles = tris;
        _mesh.RecalculateBounds();
    }
}