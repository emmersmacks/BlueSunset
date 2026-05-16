using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.ObstacleSequence.Actions
{
    public interface ISequenceActionRuntime
    {
        public UniTask Init(CancellationToken ct);
        public UniTask Run(CancellationToken ct);
    }
}