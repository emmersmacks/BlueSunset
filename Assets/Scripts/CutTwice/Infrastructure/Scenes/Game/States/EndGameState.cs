using System;
using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Game.GameOver;
using Cysharp.Threading.Tasks;

namespace CutTwice.Infrastructure.Scenes.Game.States
{
    public class EndGameState : IGameState
    {
        public async UniTask Enter(CancellationToken ct)
        {
            EventBus.Publish(new OpenWindowRequest<GameOverWindow>());
            // TODO: Move To Service
            //YG2.onGetLeaderboard += OnGetLeaderboard;
            //YG2.GetLeaderboard("time");
            await PlayEndSoundAndTransition();
        }
        
        //private void OnGetLeaderboard(LBData data)
        //{
        //    YG2.onGetLeaderboard -= OnGetLeaderboard;
        //    if (data.currentPlayer.score < (int)TimeScore)
        //    {
        //        YG2.SetLBTimeConvert("time", (int)TimeScore);
        //    }
        //}

        private async UniTask PlayEndSoundAndTransition()
        {
            // TODO: Move To Audio System
            //CrashEffect.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(1.5));
            // TODO: Move To Audio System
            //gm.Mixer.TransitionToSnapshots(new[] { gm.Normal, gm.Crash, gm.Menu }, new[] { 0f, 1f, 0f }, 0);
        }

        public void Exit()
        {
        }
    }
}

