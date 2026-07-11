using System.Threading;
using CutTwice.App.LoadingScreen;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.Services;
using CutTwice.UI.Game.GameHUD;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Gameplay.GameStates
{
    public class StartGameState : IGameState
    {
        private readonly IEventBus _eventBus;
        private readonly AudioSnapshotService _audioSnapshotService;
        private readonly LoadingScreenController _loadingScreenController;

        public StartGameState(IEventBus eventBus, AudioSnapshotService audioSnapshotService, LoadingScreenController loadingScreenController)
        {
            _eventBus = eventBus;
            _audioSnapshotService = audioSnapshotService;
            _loadingScreenController = loadingScreenController;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _loadingScreenController.Hide();
            Time.timeScale = 1f;

            _audioSnapshotService.TransitionTo(AudioSnapshot.Game);

            _eventBus.Publish(new OpenWindowRequest<GameHUDWindow>());

            await stateMachine.SetStateAsync<GameLoopState>(ct);
        }

        public void Exit()
        {
        }
    }
}

