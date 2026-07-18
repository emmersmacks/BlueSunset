using System;
using System.Collections.Generic;
using CascadeDI.Builder;
using CutTwice.Core.Factory;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.UI.Location.LocationButtons;

namespace CutTwice.UI.Location
{
    public class LocationWindow : WindowBase<LocationWindowView>
    {
        public LocationWindow(LocationWindowView windowView, IWindowFactory windowFactory) : base(windowView, windowFactory)
        {
        }

        public override void Compose(IContainerBuilder builder)
        {
            builder.RegisterSingleton(typeof(LocationButtonsView), _windowView.LocationButtonsView);
            builder.RegisterSingletonWithLifetime<LocationButtonsController>(new List<Type> { typeof(IWindowController)});
        }
    }
}