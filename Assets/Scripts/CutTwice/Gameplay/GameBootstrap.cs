using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.Initialization;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Game.GameHUD;
using Cysharp.Threading.Tasks;

namespace CutTwice.Infrastructure.Scenes.Game
{
    public class GameBootstrap : Bootstrap
    {
        public GameSceneReferences SceneReferences;
        
        protected override CompositionRoot CreateCompositionRoot()
        {
            return new GameCompositionRoot(SceneReferences);
        }
    }
}