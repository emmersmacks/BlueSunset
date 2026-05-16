using System.Threading;
using CutTwice.Common.Infrastructure;
using CutTwice.Core.RivletUI;
using CutTwice.UI;
using Cysharp.Threading.Tasks;
using Infrastructure.Events;

namespace CutTwice.Game
{
    public class GameBootstrap : Bootstrap
    {
        public GameSceneReferences SceneReferences;
        
        protected override CompositionRoot CreateCompositionRoot()
        {
            return new GameCompositionRoot(SceneReferences);
        }

        protected override UniTask InitAsync(CancellationToken ct)
        {
            EventBus.Publish(new OpenWindowRequest<GameHUDWindow>());
            return UniTask.CompletedTask;
        }
    }
}