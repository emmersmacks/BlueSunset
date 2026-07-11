using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Gameplay.GlobalStates;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.UI.Game.GameOver.RestartButton
{
    public class RestartButtonController : WindowControllerBase<RestartButtonView>, IInitializable
    {
        private readonly GlobalStateMachine _globalStateMachine;
        private CancellationToken _cancellationToken;
        
        public RestartButtonController(RestartButtonView view, GlobalStateMachine globalStateMachine) : base(view)
        {
            _globalStateMachine = globalStateMachine;
            View.RestartButton.onClick.AddListener(RestartGame);
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            _cancellationToken = ct;
            return UniTask.CompletedTask;
        }

        private void RestartGame()
        {
            _globalStateMachine.SetStateAsync<GlobalGameState>(_cancellationToken).Forget(Debug.LogException);
        }
    }
}