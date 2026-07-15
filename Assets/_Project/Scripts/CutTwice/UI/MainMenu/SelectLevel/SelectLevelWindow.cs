using System;
using System.Collections.Generic;
using CascadeDI.Builder;
using CutTwice.Core.Factory;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.SelectLevel.SmoothBackButton;

namespace CutTwice.UI.MainMenu.SelectLevel
{
    public class SelectLevelWindow : WindowBase<SelectLevelWindowView>
    {
        public SelectLevelWindow(SelectLevelWindowView windowView, IWindowFactory windowFactory) : base(windowView, windowFactory)
        {
        }

        public override void Compose(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(SmoothBackButtonView), _windowView.BackButtonView);
            builder.RegisterSingletonWithLifetime<SmoothBackButtonController>(new List<Type> { typeof(IWindowController)});
        }
    }
}