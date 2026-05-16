using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.GameStates
{
    public class BootstrapState : IGameState
    {
        public UniTask Enter(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        public void Exit() { }
    }
}