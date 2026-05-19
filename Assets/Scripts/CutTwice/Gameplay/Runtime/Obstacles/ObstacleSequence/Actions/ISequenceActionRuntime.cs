using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.Gameplay.Runtime.Obstacles.ObstacleSequence.Actions
{
    public interface ISequenceActionRuntime
    {
        public UniTask Init(CancellationToken ct);
        public UniTask Run(CancellationToken ct);
    }
}