using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Infrastructure.Scenes.Game.States;
using CutTwice.UI.MainMenu.Credits;
using CutTwice.UI.MainMenu.Leaderboard;
using CutTwice.UI.MainMenu.Shop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.UI.MainMenu.Menu.StartGameButton
{
    public class MenuButtonsController : WindowControllerBase<MenuButtonsView>, IInitializable
    {
        private CancellationToken _cancellationToken;

        public MenuButtonsController(MenuButtonsView view) : base(view)
        {
            View.StartButton.onClick.AddListener(StartGame);
            View.CreditsButton.onClick.AddListener(ShowCredits);
            View.ShopButton.onClick.AddListener(ShowShop);
            View.LeaderboardButton.onClick.AddListener(ShowLeaderboard);
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            _cancellationToken = ct;
            return UniTask.CompletedTask;
        }

        private void StartGame()
        {
            GameStateMachine.Instance.SetStateAsync<StartGameState>(_cancellationToken).Forget(Debug.LogException);
        }

        private void ShowCredits()
        {
            EventBus.Publish(new PushWindowRequest<CreditsWindow>());
        }
        
        private void ShowShop()
        {
            EventBus.Publish(new PushWindowRequest<ShopWindow>());
        }
        
        private void ShowLeaderboard()
        {
            EventBus.Publish(new PushWindowRequest<LeaderboardWindow>());
        }
    }
}