using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.StaticNames;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutTwice.Infrastructure.Scenes.Game.GlobalStates
{
    public class GameState : IGlobalState
    {
        public UniTask Enter(IStateMachine stateMachine, CancellationToken ct)
        {
            SceneManager.LoadScene(SceneNames.Game);
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
            
        }
    }
}