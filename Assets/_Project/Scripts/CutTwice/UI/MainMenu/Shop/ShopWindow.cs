using System;
using System.Collections.Generic;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Common.UIBackButton;
using CascadeDI.Builder;
using CutTwice.Core.Factory;

namespace CutTwice.UI.MainMenu.Shop
{
    public class ShopWindow : WindowBase<ShopWindowView>
    {
        public ShopWindow(ShopWindowView windowView, IWindowFactory windowFactory) 
            : base(windowView, windowFactory)
        {
        }

        public override void Compose(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(UIBackButtonView), _windowView.BackButtonView);
            builder.RegisterSingletonWithLifetime<UIBackButtonController>(new List<Type> { typeof(IWindowController)});
        }
    }
}