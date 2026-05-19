using System.Threading;
using CutTwice.Core.GameStates;
using Cysharp.Threading.Tasks;

namespace CutTwice.Infrastructure.Scenes.App.States
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