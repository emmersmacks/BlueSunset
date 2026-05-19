using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.StaticNames;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.Infrastructure.Scenes.Menu.States
{
    public class MainMenuState : IGameState
    {
        public UniTask Enter(CancellationToken ct)
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
            return UniTask.CompletedTask;
        }

        public void Exit() { }
    }
}