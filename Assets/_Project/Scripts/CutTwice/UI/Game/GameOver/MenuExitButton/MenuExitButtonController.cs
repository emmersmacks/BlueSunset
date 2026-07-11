using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Menu.GlobalStates;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.UI.Game.GameOver.MenuExitButton
{
    public class MenuExitButtonController : WindowControllerBase<MenuExitButtonView>, IInitializable
    {
        private readonly GlobalStateMachine _globalStateMachine;
        private CancellationToken _cancellationToken;

        public MenuExitButtonController(MenuExitButtonView view, GlobalStateMachine globalStateMachine) : base(view)
        {
            _globalStateMachine = globalStateMachine;
            view.Button.onClick.AddListener(ExitMenu);
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            _cancellationToken = ct;
            return UniTask.CompletedTask;
        }

        private void ExitMenu()
        {
            _globalStateMachine.SetStateAsync<GlobalMainMenuState>(_cancellationToken).Forget(Debug.LogException);
        }
    }
}