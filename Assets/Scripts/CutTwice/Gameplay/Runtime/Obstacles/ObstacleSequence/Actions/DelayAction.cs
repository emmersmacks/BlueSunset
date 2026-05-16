using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace CutTwice.ObstacleSequence.Actions
{
    public class DelayAction : ISequenceActionRuntime
    {
        [Serializable]
        public struct Parameters
        {
            public float Value;
        }

        private readonly Parameters _parameters;
        
        public DelayAction(Parameters parameters)
        {
            _parameters = parameters;
        }

        UniTask ISequenceActionRuntime.Init(CancellationToken ct) { return UniTask.CompletedTask; }

        async UniTask ISequenceActionRuntime.Run(CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_parameters.Value), cancellationToken: ct);
        }
    }
}