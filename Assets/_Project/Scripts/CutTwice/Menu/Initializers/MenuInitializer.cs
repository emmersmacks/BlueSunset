using System.Threading;
using CutTwice.App.LoadingScreen;
using CutTwice.Core.EventBus;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.Initializers
{
    public class MenuInitializer : IInitializable
    {
        private readonly IEventBus _eventBus;
        private readonly LoadingScreenController _loadingScreenController;

        public MenuInitializer(IEventBus eventBus, LoadingScreenController loadingScreenController)
        {
            _eventBus = eventBus;
            _loadingScreenController = loadingScreenController;
        }
        
        public UniTask InitAsync(CancellationToken ct)
        {
            _eventBus.Publish(new OpenWindowRequest<MenuWindow>());
            _loadingScreenController.Hide();
            return UniTask.CompletedTask;
        }
    }
}