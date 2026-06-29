using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using CascadeDI;
using Newtonsoft.Json.Linq;

namespace CutTwice.Gameplay.Runtime.Chunks.Actions
{
    public class ActionFactory
    {
        private readonly IScope _scope;

        public ActionFactory(IScope scope)
        {
            _scope = scope;
        }
        
        public async UniTask<ISequenceActionRuntime> CreateAsync(Type type, JObject parameters, CancellationToken ct)
        {
            var action = _scope.Resolve(type) as ISequenceActionRuntime;
            await action.Init(parameters, ct);
            return action;
        }
    }
}

