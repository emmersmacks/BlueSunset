using System;
using System.Collections.Generic;
using CutTwice.Core.EventBus;
using CutTwice.Core.Factory;
using CutTwice.Core.GameStates;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Gameplay.Factories;
using CutTwice.Gameplay.GameStates;
using CutTwice.Gameplay.Modes;
using CutTwice.Gameplay.Runtime.Chunks;
using CutTwice.Gameplay.Runtime.Chunks.Actions;
using CutTwice.Gameplay.Runtime.Chunks.ModuleLoader;
using CutTwice.Gameplay.Runtime.Chunks.Services;
using CutTwice.Gameplay.Runtime.Hazards.Components;
using CutTwice.Gameplay.Runtime.Interactables.Components;
using CutTwice.Gameplay.Runtime.Player.Components;
using CutTwice.Gameplay.Runtime.Road.Components;
using CutTwice.Gameplay.Runtime.Scenario;
using CutTwice.Gameplay.Runtime.Scenario.Stages;
using CutTwice.Gameplay.Runtime.Sound.Components;
using CutTwice.UI.Game.GameHUD;
using CutTwice.UI.Game.GameOver;
using CascadeDI.Builder;

namespace CutTwice.Gameplay
{
    public class GameCompositionRoot : CompositionRoot
    {
        private readonly GameSceneReferences _gameSceneReferences;

        public GameCompositionRoot(GameSceneReferences gameSceneReferences)
        {
            _gameSceneReferences = gameSceneReferences;
        }

        public override void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager)
        {
            builder.RegisterSingleton<RuntimeLifecycleManager>(lifecycleManager);

            ComposeGameplayModule(builder, lifecycleManager);
            ComposeUIModule(builder);
            
            // Initialization
            builder.RegisterSingletonWithLifetime<GameInitializer>();
        }

        private void ComposeGameplayModule(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager)
        {
            // Services
            var eventBus = new EventBus();
            builder.RegisterSingleton<IEventBus>(eventBus);
            builder.RegisterSingleton<RuntimeLifecycleManager>(lifecycleManager);
            
            // Gameplay
            var playerInputController = lifecycleManager.Register(new PlayerInputController(_gameSceneReferences.PlayerCamera));
            builder.RegisterSingleton(typeof(PlayerInputController), playerInputController);
            var playerCarPresenter = _gameSceneReferences.Player.GetComponent<PlayerCarPresenter>();
            var playerCarController = lifecycleManager.Register(new PlayerCarController(playerCarPresenter, playerInputController));
            var playerSleepPresenter = _gameSceneReferences.Player.GetComponent<PlayerSleepPresenter>();
            var playerSleepController = lifecycleManager.Register(new PlayerSleepController(playerSleepPresenter));
            builder.RegisterSingleton<PlayerSleepController>(playerSleepController);
            var steeringInterferencePresenter = _gameSceneReferences.Player.GetComponent<SteeringInterferencePresenter>();
            var steeringInterferenceController = lifecycleManager.Register(new SteeringInterferenceController(steeringInterferencePresenter, playerSleepController));
            
            var infiniteRoadController = lifecycleManager.Register(new InfiniteRoadController(_gameSceneReferences.InfiniteRoadPresenter));
            builder.RegisterSingleton<InfiniteRoadController>(infiniteRoadController);
            
            var rotateBackviewMirrorController = lifecycleManager.Register(new RotateMirrorController(_gameSceneReferences.RotateBackviewMirrorPresenter, playerInputController));
            var backviewReflectionObjectController = lifecycleManager.Register(new ReflectionObjectController(_gameSceneReferences.BackviewReflectionObjectPresenter, rotateBackviewMirrorController));
            var backviewMirrorHazardController = lifecycleManager.Register(new BackviewMirrorHazardController(_gameSceneReferences.BackviewMirrorHazardPresenter, backviewReflectionObjectController, rotateBackviewMirrorController, eventBus));
            builder.RegisterSingleton(typeof(BackviewMirrorHazardController), backviewMirrorHazardController);
            
            var rotateSideviewMirrorController = lifecycleManager.Register(new RotateMirrorController(_gameSceneReferences.RotateSideviewMirrorPresenter, playerInputController));
            var sideviewReflectionObjectController = lifecycleManager.Register(new ReflectionObjectController(_gameSceneReferences.SideviewReflectionObjectPresenter, rotateSideviewMirrorController));
            var sideviewMirrorHazardController = lifecycleManager.Register(new SideviewMirrorHazardController(_gameSceneReferences.SideviewMirrorHazardPresenter, sideviewReflectionObjectController, rotateSideviewMirrorController, eventBus));
            var sideviewHazardSoundLoopController = lifecycleManager.Register(new MusicLoopController(_gameSceneReferences.LeftSideHazardLoopSoundPresenter));
            builder.RegisterSingleton(typeof(SideviewMirrorHazardController), sideviewMirrorHazardController);


            // Game States
            builder.RegisterSingletonWithLifetime<EndGameState>(new List<Type>{ typeof(IGameState) });
            builder.RegisterSingletonWithLifetime<GameLoopState>(new List<Type>{ typeof(IGameState) });
            builder.RegisterSingletonWithLifetime<PauseGameState>(new List<Type>{ typeof(IGameState) });
            builder.RegisterSingletonWithLifetime<StartGameState>(new List<Type>{ typeof(IGameState) });
            builder.RegisterSingletonWithLifetime<GameStateMachine>();
            
            // Orchestration
            
            // --- Scenario
            var initialStage = lifecycleManager.Register(new InitialStage(_gameSceneReferences.Player, _gameSceneReferences.PlayerInitialPosition));
            var openEyeStage = lifecycleManager.Register(new OpenEyeStage(_gameSceneReferences.PostProcessing));
            var scenarioSystem = new ScenarioSystem(_gameSceneReferences.ScenarioManagerSettings, playerInputController, new ScenarioStage[]
            {
                initialStage,
                openEyeStage,
            });
            lifecycleManager.Register(scenarioSystem);
            builder.RegisterSingleton<ScenarioSystem>(scenarioSystem);

            // --- Obstacles
            builder.RegisterTransientWithLifetime<ISequenceActionRuntime, DelayAction>(typeof(DelayAction));
            builder.RegisterTransientWithLifetime<ISequenceActionRuntime, ShowBackViewMirrorObjectAction>(typeof(ShowBackViewMirrorObjectAction));
            builder.RegisterTransientWithLifetime<ISequenceActionRuntime, ShowSideViewMirrorObjectAction>(typeof(ShowSideViewMirrorObjectAction));
            builder.RegisterTransientWithLifetime<ISequenceActionRuntime, SpawnDeerAction>(typeof(SpawnDeerAction));
            builder.RegisterTransientWithLifetime<ISequenceActionRuntime, SpawnTrafficAction>(typeof(SpawnTrafficAction));
            
            builder.RegisterSingleton<ActionFactory>();
            builder.RegisterSingletonWithLifetime<TrafficGameObjectFactory>();
            builder.RegisterSingletonWithLifetime<DeerGameObjectFactory>();
            builder.RegisterSingletonWithLifetime<ISequenceModuleLoader, AddressablesModuleLoader>();
            builder.RegisterSingletonWithLifetime<IObstacleSequenceService, ObstacleSequenceService>();
            builder.RegisterSingletonWithLifetime<ObstacleSequenceBuilder>();
            builder.RegisterSingletonWithLifetime<ObstacleRuntimeController>();
            
            // Session
            builder.RegisterSingletonWithLifetime<GameSession>();
            builder.RegisterSingletonWithLifetime<DistanceTracker>();
        }

        private void ComposeUIModule(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(GameHUDWindowView), _gameSceneReferences.gameHUDWindowView);
            builder.RegisterSingletonWithLifetime<GameHUDWindow>(new List<Type>{ typeof(IWindow) });
            
            builder.RegisterSingleton(typeof(GameOverWindowView), _gameSceneReferences.gameOverWindowView);
            builder.RegisterSingletonWithLifetime<GameOverWindow>(new List<Type>{ typeof(IWindow) });

            builder.RegisterSingleton<IWindowFactory, WindowFactory>();
            builder.RegisterSingletonWithLifetime<UIManager>();
        }
    }
}