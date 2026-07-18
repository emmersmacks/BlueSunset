using System;
using System.Collections.Generic;
using CascadeDI.Builder;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Shop;

namespace CutTwice.Location.Building
{
    public class ShopCompositionRoot : CompositionRoot
    {
        public ShopWindowView ShopWindow;
        
        public override void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager)
        {
            // UI
            builder.RegisterSingleton(typeof(ShopWindowView), ShopWindow);
            builder.RegisterSingletonWithLifetime<ShopWindow>(new List<Type>{ typeof(IWindow) });
        }
    }
}