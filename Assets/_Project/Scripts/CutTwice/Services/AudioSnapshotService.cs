using System.Threading;
using CutTwice.Core.Addressables;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.StaticNames;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace CutTwice.Services
{
    public enum AudioSnapshot { Menu, Game, GameOver }

    public class AudioSnapshotService : IService, IInitializable
    {
        private AudioMixer _mixer;
        
        public async UniTask InitAsync(CancellationToken ct)
        {
            _mixer = await AddressablesAsyncLoader.LoadAssetAsync<AudioMixer>(AddressableLabels.MainAudioMixer, ct);
        }
        
        public void TransitionTo(AudioSnapshot snapshot, float transitionDuration = 0f)
        {
            var target = _mixer.FindSnapshot(snapshot.ToString());
            if (target == null)
            {
                Debug.LogWarning($"AudioSnapshotService: snapshot '{snapshot}' not found in mixer '{_mixer.name}'.");
                return;
            }
            target.TransitionTo(transitionDuration);
        }
    }
}
