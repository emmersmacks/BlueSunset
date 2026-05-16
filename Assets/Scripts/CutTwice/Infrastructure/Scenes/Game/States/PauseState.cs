using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.GameStates
{
    public class PauseState : IGameState
    {
        public UniTask Enter(CancellationToken ct)
        {
            Time.timeScale = 0f;
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}

