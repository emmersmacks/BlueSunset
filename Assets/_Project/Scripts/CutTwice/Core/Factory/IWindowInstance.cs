using System.Collections.Generic;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;

namespace CutTwice.Core.Factory
{
    public interface IWindowInstance
    {
        List<IWindowController> Controllers { get; }
        List<ILifecycleObject> LifecycleObjects { get; }
        void Release();
    }
}


