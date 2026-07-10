using System;
using System.Collections.Generic;
using CutTwice.Core.EventBus;
using CutTwice.Core.Factory;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Menu.Initializers;
using CutTwice.UI.MainMenu.Credits;
using CutTwice.UI.MainMenu.Leaderboard;
using CutTwice.UI.MainMenu.Menu;
using CutTwice.UI.MainMenu.Shop;
using CascadeDI.Builder;

namespace CutTwice.Menu
{
    public class MenuCompositionRoot : CompositionRoot
    {
        private readonly MenuSceneReferences _sceneReferences;

        public MenuCompositionRoot(MenuSceneReferences sceneReferences)
        {
            _sceneReferences = sceneReferences;
        }
        
        public override void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager)
        {
            builder.RegisterSingleton<RuntimeLifecycleManager>(lifecycleManager);
            
            var eventBus = new EventBus();
            builder.RegisterSingleton<IEventBus>(eventBus);

            // UI
            builder.RegisterSingleton(typeof(MenuWindowView), _sceneReferences.MenuWindow);
            builder.RegisterSingletonWithLifetime<MenuWindow>(new List<Type>{ typeof(IWindow) });

            builder.RegisterSingleton(typeof(CreditsWindowView), _sceneReferences.CreditsWindow);
            builder.RegisterSingletonWithLifetime<CreditsWindow>(new List<Type>{ typeof(IWindow) });

            builder.RegisterSingleton(typeof(ShopWindowView), _sceneReferences.ShopWindow);
            builder.RegisterSingletonWithLifetime<ShopWindow>(new List<Type>{ typeof(IWindow) });
            
            builder.RegisterSingleton(typeof(LeaderboardWindowView), _sceneReferences.LeaderboardWindow);
            builder.RegisterSingletonWithLifetime<LeaderboardWindow>(new List<Type>{ typeof(IWindow) });
            
            builder.RegisterSingleton<IWindowFactory, WindowFactory>();
            builder.RegisterSingletonWithLifetime<UIManager>();
            
            // Initializer
            builder.RegisterSingletonWithLifetime<MenuInitializer>();
        }
    }
}