using System;
using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.Services;
using CutTwice.UI.Game.GameOver;
using Cysharp.Threading.Tasks;

namespace CutTwice.Gameplay.GameStates
{
    public class EndGameState : IGameState
    {
        private readonly CutTwice.Core.EventBus.IEventBus _eventBus;
        private readonly AudioSnapshotService _audioSnapshotService;

        public EndGameState(CutTwice.Core.EventBus.IEventBus eventBus, AudioSnapshotService audioSnapshotService)
        {
            _eventBus = eventBus;
            _audioSnapshotService = audioSnapshotService;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _eventBus.Publish(new OpenWindowRequest<GameOverWindow>());
            // TODO: Move To Service
            //YG2.onGetLeaderboard += OnGetLeaderboard;
            //YG2.GetLeaderboard("time");
            await PlayEndSoundAndTransitionAsync();
        }
        
        //private void OnGetLeaderboard(LBData data)
        //{
        //    YG2.onGetLeaderboard -= OnGetLeaderboard;
        //    if (data.currentPlayer.score < (int)TimeScore)
        //    {
        //        YG2.SetLBTimeConvert("time", (int)TimeScore);
        //    }
        //}

        private async UniTask PlayEndSoundAndTransitionAsync()
        {
            // TODO: Move To Audio System
            //CrashEffect.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(1.5));
            _audioSnapshotService.TransitionTo(AudioSnapshot.GameOver, 1f);
        }

        public void Exit()
        {
        }
    }
}

