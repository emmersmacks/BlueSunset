using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.Core.StaticNames;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutTwice.Infrastructure.Scenes.Menu.States
{
    public class MainMenuState : IGlobalState
    {
        private readonly EventBus _eventBus;

        public MainMenuState(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public UniTask Enter(IStateMachine stateMachine, CancellationToken ct)
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
            return UniTask.CompletedTask;
        }

        public void Exit() { }
    }
}