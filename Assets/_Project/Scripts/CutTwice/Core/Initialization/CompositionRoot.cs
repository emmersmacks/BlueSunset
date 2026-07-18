using CutTwice.Core.Lifecycle;
using CascadeDI.Builder;
using UnityEngine;

namespace CutTwice.Core.Initialization
{
    public abstract class CompositionRoot : MonoBehaviour
    {
        public abstract void Compose(IContainerBuilder builder, RuntimeLifecycleManager lifecycleManager);
    }
}