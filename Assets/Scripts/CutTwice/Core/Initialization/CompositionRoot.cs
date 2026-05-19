using CutTwice.Core.Lifecycle;

namespace CutTwice.Core.Initialization
{
    public abstract class CompositionRoot
    {
        public abstract void Compose(RuntimeLifecycleManager lifecycleManager);
    }
}