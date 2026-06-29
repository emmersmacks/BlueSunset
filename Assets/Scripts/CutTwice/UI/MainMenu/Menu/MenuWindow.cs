using System;
using System.Collections.Generic;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Menu.StartGameButton;
using CascadeDI.Builder;
using CutTwice.Core.Factory;

namespace CutTwice.UI.MainMenu.Menu
{
    public class MenuWindow : WindowBase<MenuWindowView>
    {
        public MenuWindow(MenuWindowView windowView, IWindowFactory windowFactory) 
            : base(windowView, windowFactory)
        {
        }

        public override void Compose(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(MenuButtonsView), _windowView.MenuButtonsView);
            builder.RegisterSingletonWithLifetime<MenuButtonsController>(new List<Type> { typeof(IWindowController)});
        }
    }
}