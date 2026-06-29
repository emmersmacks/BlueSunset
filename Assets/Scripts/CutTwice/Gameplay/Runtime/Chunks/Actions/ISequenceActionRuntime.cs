using System.Threading;
using CutTwice.Gameplay.Runtime.Chunks.ModuleLoader.Dto;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CutTwice.Gameplay.Runtime.Chunks.Actions
{
    public interface ISequenceActionRuntime
    {
        public UniTask Init(JObject dtoParameters, CancellationToken ct);
        public UniTask Run(CancellationToken ct);
    }
}