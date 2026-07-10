using System;
using System.Collections.Generic;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CascadeDI;

namespace CutTwice.Core.Factory
{
    public class WindowInstance : IWindowInstance
    {
        public List<IWindowController> Controllers { get; }
        public List<ILifecycleObject> LifecycleObjects { get; }

        private readonly IScope _scope;
        private readonly RuntimeLifecycleManager _lifecycleManager;

        public WindowInstance(List<IWindowController> controllers, List<ILifecycleObject> lifecycleObjects, IScope scope, RuntimeLifecycleManager lifecycleManager)
        {
            Controllers = controllers ?? new List<IWindowController>();
            LifecycleObjects = lifecycleObjects ?? new List<ILifecycleObject>();
            _scope = scope;
            _lifecycleManager = lifecycleManager;
        }

        public void Release()
        {
            if (LifecycleObjects != null && _lifecycleManager != null)
            {
                foreach (var obj in LifecycleObjects)
                {
                    _lifecycleManager.Unregister(obj);
                }
            }

            if (_scope is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}


