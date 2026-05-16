using System;
using CutTwice.Controllers;
using CutTwice.UI;
using CutTwice.UI.Game.GameOver;
using UnityEngine;
using UnityEngine.Rendering;

namespace CutTwice.Game
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