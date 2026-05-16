using UnityEngine;

namespace CutTwice.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class OneShotSoundPresenter : MonoBehaviour
    {
        [Tooltip("Start playback from this time (seconds)")]
        public float StartTimeInSeconds;
    }
}