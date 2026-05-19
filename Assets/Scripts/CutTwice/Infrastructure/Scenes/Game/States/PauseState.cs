using System.Threading;
using CutTwice.Core.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Infrastructure.Scenes.Game.States
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

