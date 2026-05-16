using System.Threading;
using CutTwice.Game;
using CutTwice.ObstacleSequence.ModuleLoader.Dto;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.GameStates
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

