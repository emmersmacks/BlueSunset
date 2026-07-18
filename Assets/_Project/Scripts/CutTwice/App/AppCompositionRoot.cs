using System;
using System.Collections.Generic;
using CutTwice.App.Fade;
using CutTwice.App.GlobalStates;
using CutTwice.App.LoadingScreen;
using CutTwice.Core.GameStates;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Gameplay;
using CutTwice.Gameplay.GlobalStates;
using CutTwice.Gameplay.Modes;
using CutTwice.Gameplay.Runtime.Map;
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
            
            // UI
            builder.RegisterSingleton(typeof(LoadingScreenView), _sceneReferences.LoadingScreen);
            builder.RegisterSingletonWithLifetime<LoadingScreenController>();
            builder.RegisterSingleton(typeof(FadeView), _sceneReferences.FadeView);

            // Services
            builder.RegisterSingletonWithLifetime<PurchaseService>();
            builder.RegisterSingletonWithLifetime<AudioSnapshotService>();
            builder.RegisterSingletonWithLifetime<FadeService>(new List<Type>{ typeof(IFadeService) });

            // AppStateMachine
            builder.RegisterSingleton<IGlobalState, GlobalBootstrapState>();
            builder.RegisterSingleton<IGlobalState, GlobalMainMenuState>();
            builder.RegisterSingleton<IGlobalState, GlobalGameState>();
            builder.RegisterSingleton<IGlobalState, GlobalLocationState>();
            builder.RegisterSingleton<GlobalStateMachine>();

            // Player data
            PlayerData.Load();

            // Game mode
            builder.RegisterSingleton<GameModeContext>(new GameModeContext());

            // Map progress
            var mapProgressService = new MapProgressService();
            if (_sceneReferences.HardcodedAdventureMap != null)
            {
                mapProgressService.SelectMap(_sceneReferences.HardcodedAdventureMap);
            }
            builder.RegisterSingleton<MapProgressService>(mapProgressService);
            builder.RegisterSingleton<AdventureFlowService>();

            builder.RegisterSingletonWithLifetime<AppInitializer>();
        }
    }
}