using System;
using CutTwice.App.Fade;
using CutTwice.App.LoadingScreen;
using CutTwice.Gameplay.Runtime.Map;

namespace CutTwice.App
{
    [Serializable]
    public class AppSceneReferences
    {
        public LoadingScreenView LoadingScreen;
        public FadeView FadeView;
        public MapDefinition HardcodedAdventureMap;
    }
}
