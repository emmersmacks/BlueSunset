using System.Threading;
using CutTwice.Common.Infrastructure;
using CutTwice.Core.RivletUI;
using CutTwice.Game;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;
using Infrastructure.Events;

namespace CutTwice.Menu
{
    public class MenuBootstrap : Bootstrap
    {
        public MenuSceneReferences SceneReferences;
        
        protected override CompositionRoot CreateCompositionRoot()
        {
            return new MenuCompositionRoot(SceneReferences);
        }

        protected override UniTask InitAsync(CancellationToken ct)
        {
            EventBus.Publish(new OpenWindowRequest<MenuWindow>());
            return UniTask.CompletedTask;
        }
    }
}