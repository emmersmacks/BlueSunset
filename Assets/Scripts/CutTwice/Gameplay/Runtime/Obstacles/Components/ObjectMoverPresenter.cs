using UnityEngine;
using UnityEngine.Serialization;

namespace CutTwice.Gameplay.Runtime.Obstacles.Components
{
    public class ObjectMoverPresenter : MonoBehaviour
    {
        [FormerlySerializedAs("moveDirection")] [Header("Movement Settings")]
        public Vector3 MoveDirection = Vector3.forward;

        [FormerlySerializedAs("movementCurve")] [Tooltip("The Animation Curve that defines the movement over time.")]
        public AnimationCurve MovementCurve = AnimationCurve.Linear(0,0,1,1);

        [FormerlySerializedAs("duration")] [Tooltip("The duration of the movement in seconds.")]
        public float Duration = 1f;
    }
}
