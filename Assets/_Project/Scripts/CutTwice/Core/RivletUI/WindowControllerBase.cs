using System;

namespace CutTwice.Core.RivletUI
{
    /// <summary>
    /// Base implementation for window controllers. TView is the runtime view instance, TWindow is the strongly-typed window id.
    /// Controllers do not create views — views should be created elsewhere and passed into the controller.
    /// </summary>
    public abstract class WindowControllerBase<TView> : IWindowController
        where TView : class, IWindowView
    {
        protected readonly TView View;

        protected WindowControllerBase(TView view)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
        }

        public virtual void Show(object payload = null)
        {
            OnShow(payload);
        }
        
        protected virtual void OnShow(object payload = null)
        {
        }

        public virtual void Hide()
        {
            OnHide();
        }
        
        protected virtual void OnHide()
        {
        }
    }
}


