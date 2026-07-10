using System.Threading;
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

        public StartGameState(IEventBus eventBus, AudioSnapshotService audioSnapshotService)
        {
            _eventBus = eventBus;
            _audioSnapshotService = audioSnapshotService;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
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

