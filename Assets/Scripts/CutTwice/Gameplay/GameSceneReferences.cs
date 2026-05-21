using System;
using CutTwice.Gameplay.Runtime.Road;
using CutTwice.UI.Game.GameHUD;
using CutTwice.UI.Game.GameOver;
using UnityEngine;
using UnityEngine.Rendering;

namespace CutTwice.Infrastructure.Scenes.Game
{
    [Serializable]
    public class GameSceneReferences
    {
        public Volume PostProcessing;
        public Transform Player;
        public Transform PlayerInitialPosition;
        public InfiniteRoadPresenter InfiniteRoadPresenter;
        
        public GameHUDView GameHUDView;
        public GameOverView GameOverView;
    }
}