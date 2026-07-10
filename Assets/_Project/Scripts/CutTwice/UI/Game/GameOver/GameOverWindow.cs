using System;
using System.Collections.Generic;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Game.GameOver.MenuExitButton;
using CutTwice.UI.Game.GameOver.RestartButton;
using CascadeDI.Builder;
using CutTwice.Core.Factory;

namespace CutTwice.UI.Game.GameOver
{
    public class GameOverWindow : WindowBase<GameOverWindowView>
    {
        public GameOverWindow(GameOverWindowView windowView, IWindowFactory windowFactory) 
            : base(windowView, windowFactory) { }

        public override void Compose(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(MenuExitButtonView), _windowView.ExitMenuButtonView);
            builder.RegisterSingletonWithLifetime<MenuExitButtonController>(new List<Type> { typeof(IWindowController)});
            
            builder.RegisterSingleton(typeof(RestartButtonView), _windowView.RestartButtonView);
            builder.RegisterSingletonWithLifetime<RestartButtonController>(new List<Type> { typeof(IWindowController)});
        }
    }
}