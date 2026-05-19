using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.Initialization;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;

namespace CutTwice.Infrastructure.Scenes.Menu
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