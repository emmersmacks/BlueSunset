using System;
using System.Threading;
using CutTwice.Gameplay.Runtime.Chunks.ModuleLoader.Dto;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CutTwice.Gameplay.Runtime.Chunks.Actions
{
    [SequenceAction(ActionType.Delay)]
    public class DelayAction : ISequenceActionRuntime
    {
        [Serializable]
        public struct Parameters
        {
            public float Value;
        }

        private Parameters _parameters;

        UniTask ISequenceActionRuntime.Init(JObject parameters, CancellationToken ct)
        {
            _parameters = parameters.ToObject<Parameters>();
            return UniTask.CompletedTask;
        }

        async UniTask ISequenceActionRuntime.Run(CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_parameters.Value), cancellationToken: ct);
        }
    }
}