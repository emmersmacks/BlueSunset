using System.Collections.Generic;
using CutTwice.Core.GameStates;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Infrastructure.Scenes.App.States;
using CutTwice.Infrastructure.Scenes.Game;
using CutTwice.Infrastructure.Scenes.Game.GlobalStates;
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

            // Event bus
            var eventBus = lifecycleManager.Register(new Core.EventBus.EventBus());

            // UI
            var uiManager = lifecycleManager.Register(new UIManager(eventBus));

            // AppStateMachine
            var bootstrapState = lifecycleManager.Register(new BootstrapState());
            var mainMenuState = lifecycleManager.Register(new MainMenuState());
            var gameState = lifecycleManager.Register(new GameState());

            var stateMachine = lifecycleManager.Register(new GlobalStateMachine(new List<IGlobalState>()
            {
                bootstrapState,
                mainMenuState,
                gameState,
            }));
            
            // Player data
            PlayerData.Load();
        }
    }
}