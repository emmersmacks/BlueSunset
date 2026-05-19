using System.Threading;
using CutTwice.Core.GameStates;
using Cysharp.Threading.Tasks;

namespace CutTwice.Infrastructure.Scenes.Game.States
{
    public class GameplayState : IGameState
    {
        public UniTask Enter(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        public void Exit() { }
    }
}

