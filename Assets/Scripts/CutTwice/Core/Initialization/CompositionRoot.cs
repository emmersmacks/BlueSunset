using System.Collections.Generic;
using CutTwice.Game;

namespace CutTwice.Common.Infrastructure
{
    public abstract class CompositionRoot
    {
        public abstract void Compose(RuntimeLifecycleManager lifecycleManager);
    }
}