using System;
using System.Collections.Generic;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Common.UIBackButton;
using CascadeDI.Builder;
using CutTwice.Core.Factory;

namespace CutTwice.UI.MainMenu.Credits
{
    public class CreditsWindow : WindowBase<CreditsWindowView>
    {
        public CreditsWindow(CreditsWindowView windowView, IWindowFactory windowFactory) 
            : base(windowView, windowFactory) { }

        public override void Compose(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(UIBackButtonView), _windowView.BackButtonView);
            builder.RegisterSingletonWithLifetime<UIBackButtonController>(new List<Type> { typeof(IWindowController)});
        }
    }
}