using System;
using CutTwice.Gameplay.Runtime.Hazards.Components;
using CutTwice.Gameplay.Runtime.Interactables.Components;
using CutTwice.Gameplay.Runtime.Road.Components;
using CutTwice.Gameplay.Runtime.Scenario;
using CutTwice.Gameplay.Runtime.Sound.Components;
using CutTwice.UI.Game.GameHUD;
using CutTwice.UI.Game.GameOver;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace CutTwice.Gameplay
{
    [Serializable]
    public class GameSceneReferences
    {
        public Volume PostProcessing;
        public Transform Player;
        public Transform PlayerInitialPosition;
        
        public InfiniteRoadPresenter InfiniteRoadPresenter;
        public RotateMirrorPresenter RotateBackviewMirrorPresenter;
        public RotateMirrorPresenter RotateSideviewMirrorPresenter;
        public ReflectionObjectPresenter BackviewReflectionObjectPresenter;
        public ReflectionObjectPresenter SideviewReflectionObjectPresenter;
        public SideviewMirrorHazardPresenter SideviewMirrorHazardPresenter;
        public BackviewMirrorHazardPresenter BackviewMirrorHazardPresenter;
        
        public MusicLoopPresenter LeftSideHazardLoopSoundPresenter;

        public GameHUDWindowView gameHUDWindowView;
        public GameOverWindowView gameOverWindowView;
        
        public ScenarioManagerSettings ScenarioManagerSettings;
        public Camera PlayerCamera;
    }
}