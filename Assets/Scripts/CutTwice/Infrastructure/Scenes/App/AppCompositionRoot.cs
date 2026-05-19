using System.Collections.Generic;
using CutTwice.Core.GameStates;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Infrastructure.Scenes.App.States;
using CutTwice.Infrastructure.Scenes.Game;
using CutTwice.Infrastructure.Scenes.Game.States;
using CutTwice.Infrastructure.Scenes.Menu.States;
using CutTwice.Infrastructure.Services;

namespace CutTwice.Infrastructure.Scenes.App
{
    public class AppCompositionRoot : CompositionRoot
    {
        public override void Compose(RuntimeLifecycleManager lifecycleManager)
        {
            // Services
            var purchaseService = lifecycleManager.Register(new PurchaseService());

            // UI
            var uiManager = lifecycleManager.Register(new UIManager());

            // GameStateMachine
            var bootstrapState = lifecycleManager.Register(new BootstrapState());
            var endGameState = lifecycleManager.Register(new EndGameState());
            var gameplayState = lifecycleManager.Register(new GameplayState());
            var mainMenuState = lifecycleManager.Register(new MainMenuState());
            var pauseState = lifecycleManager.Register(new PauseState());
            var startGameState = lifecycleManager.Register(new StartGameState());

            var stateMachine = lifecycleManager.Register(new GameStateMachine(new List<IGameState>()
            {
                bootstrapState,
                endGameState,
                gameplayState,
                mainMenuState,
                pauseState,
                startGameState
            }));
            
            // Player data
            PlayerData.Load();
        }
    }
}