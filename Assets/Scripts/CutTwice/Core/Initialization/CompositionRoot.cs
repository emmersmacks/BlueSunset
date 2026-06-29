using CutTwice.Core.Lifecycle;
using CascadeDI.Builder;

namespace CutTwice.Core.Initialization
{
    public abstract class CompositionRoot
    {
        public abstract void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager);
    }
}