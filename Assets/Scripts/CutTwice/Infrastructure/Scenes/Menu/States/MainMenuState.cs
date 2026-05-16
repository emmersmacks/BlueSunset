using System.Threading;
using CutTwice.Common;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Game.GameOver;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;
using Infrastructure.Events;
using UnityEngine.SceneManagement;

namespace CutTwice.GameStates
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