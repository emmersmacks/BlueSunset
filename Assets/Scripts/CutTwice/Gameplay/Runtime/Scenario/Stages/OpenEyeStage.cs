using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace CutTwice.Gameplay.Runtime.Scenario.Stages
{
    public class OpenEyeStage : ScenarioStage
    {
        private Volume _postProcessingVolume;
        
        private Beautify.Universal.Beautify _beautify;

        private float _openProgress;

        public OpenEyeStage(Volume postProcessingVolume)
        {
            _postProcessingVolume = postProcessingVolume;
        }
        
        public override UniTask InitAsync(CancellationToken ct)
        {
            _postProcessingVolume.profile.TryGet(out _beautify);
            return UniTask.CompletedTask;
        }

        public override void StageStart()
        {
            _beautify.vignettingBlink.value = 1f;
        }

        public override void StageEnd()
        {
            _beautify.vignettingInnerRing.value = _beautify.vignettingBlink.value = 0f;
        }

        public override void StageUpdate()
        {
            _openProgress += Time.deltaTime;
            
            var effectIntensity = math.max(0, 1 - _openProgress);
            _beautify.vignettingInnerRing.value = _beautify.vignettingBlink.value = effectIntensity;
        }

        public override bool StageComplete()
        {
            return _openProgress >= 0.8f;
        }
    }
}