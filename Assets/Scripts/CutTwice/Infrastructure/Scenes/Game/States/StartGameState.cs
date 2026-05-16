using System.Threading;
using CutTwice.Common;
using CutTwice.Core.RivletUI;
using CutTwice.UI;
using CutTwice.UI.Game.GameOver;
using Cysharp.Threading.Tasks;
using Infrastructure.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutTwice.GameStates
{
    public class StartGameState : IGameState
    {
        public async UniTask Enter(CancellationToken ct)
        {
            SceneManager.LoadScene(SceneNames.Game);
            
            Time.timeScale = 1f;
            
            // TODO: Move To Audio System
            //Mixer.TransitionToSnapshots(new []{ Normal, Crash, Menu }, new []{ 0f, 0f, 1f }, 0);

            await GameStateMachine.Instance.SetStateAsync<GameplayState>(ct);
        }

        public void Exit()
        {
        }
    }
}

