using CutTwice.App.GlobalStates;
using CutTwice.App.LoadingScreen;
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
        private readonly AppSceneReferences _sceneReferences;

        public AppCompositionRoot(AppSceneReferences sceneReferences)
        {
            _sceneReferences = sceneReferences;
        }

        public override void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager)
        {
            
            builder.RegisterSingleton<RuntimeLifecycleManager>(lifecycleManager);
            
            // Services
            builder.RegisterSingletonWithLifetime<PurchaseService>();
            builder.RegisterSingletonWithLifetime<AudioSnapshotService>();

            // UI
            builder.RegisterSingleton(typeof(LoadingScreenView), _sceneReferences.LoadingScreen);
            builder.RegisterSingletonWithLifetime<LoadingScreenController>();

            // AppStateMachine
            builder.RegisterSingleton<IGlobalState, GlobalBootstrapState>();
            builder.RegisterSingleton<IGlobalState, GlobalMainMenuState>();
            builder.RegisterSingleton<IGlobalState, GlobalGameState>();
            builder.RegisterSingleton<GlobalStateMachine>();
            
            // Player data
            PlayerData.Load();
            
            builder.RegisterSingletonWithLifetime<AppInitializer>();
        }
    }
}