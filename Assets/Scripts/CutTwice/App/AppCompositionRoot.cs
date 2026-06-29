using CutTwice.App.GlobalStates;
using CutTwice.Core.GameStates;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Gameplay;
using CutTwice.Gameplay.GlobalStates;
using CutTwice.Menu.GlobalStates;
using CutTwice.Services;
using CascadeDI.Builder;

namespace CutTwice.App
{
    public class AppCompositionRoot : CompositionRoot
    {
        public override void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager)
        {
            
            builder.RegisterSingleton<RuntimeLifecycleManager>(lifecycleManager);
            
            // Services
            builder.RegisterSingleton<PurchaseService>();

            // AppStateMachine
            builder.RegisterSingleton<IGlobalState, BootstrapState>();
            builder.RegisterSingleton<IGlobalState, MainMenuState>();
            builder.RegisterSingleton<IGlobalState, GameState>();
            builder.RegisterSingleton<GlobalStateMachine>();
            
            // Player data
            PlayerData.Load();
            
            builder.RegisterSingletonWithLifetime<AppInitializer>();
        }
    }
}