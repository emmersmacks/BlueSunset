using UnityEngine;
using UnityEngine.Serialization;

namespace CutTwice.Gameplay.Runtime.Obstacles.Components
{
    /// <summary>
    /// Detects collisions by casting a strip (line) of raycasts between the object's current and previous positions.
    /// Useful for sweeping thin geometry across movement to detect fast collisions.
    /// </summary>
    public class RaycastStripPresenter : MonoBehaviour
    {
        [FormerlySerializedAs("origin")]
        [Header("Strip Settings (local space)")]
        [Tooltip("Center of the raycast strip in local space.")]
        [SerializeField]
        public Vector3 Origin = Vector3.zero;

        [FormerlySerializedAs("count")]
        [Tooltip("Number of rays in the strip (evenly distributed). Minimum 1.")]
        [SerializeField]
        public int Count = 5;

        [FormerlySerializedAs("width")]
        [Tooltip("Total width of the strip in world units.")]
        [SerializeField]
        public float Width = 1f;

        [FormerlySerializedAs("direction")]
        [Tooltip("Local direction that defines the strip orientation (e.g. Vector3.right for horizontal strip).")]
        [SerializeField]
        public Vector3 Direction = Vector3.right;

        [FormerlySerializedAs("minMovementThreshold")]
        [Header("Detection Settings")]
        [Tooltip("Minimum movement magnitude to perform raycasts. If the object moved less than this since the last check, casts are skipped.")]
        [SerializeField]
        public float MinMovementThreshold = 0.001f;

        [FormerlySerializedAs("layerMask")]
        [Tooltip("Layer mask for raycasts.")]
        [SerializeField]
        public LayerMask LayerMask = ~0;

        [FormerlySerializedAs("triggerInteraction")]
        [Tooltip("Whether to include trigger colliders in detection.")]
        [SerializeField]
        public QueryTriggerInteraction TriggerInteraction = QueryTriggerInteraction.Ignore;

        [FormerlySerializedAs("debugDraw")]
        [Header("Debug")]
        [Tooltip("Draw debug lines for raycasts in the scene view.")]
        [SerializeField]
        public bool DebugDraw = true;

        [FormerlySerializedAs("debugColorNoHit")] [SerializeField]
        public Color DebugColorNoHit = Color.green;

        [FormerlySerializedAs("debugColorHit")] [SerializeField]
        public Color DebugColorHit = Color.red;
    }
}

