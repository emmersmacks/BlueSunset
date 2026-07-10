using UnityEngine;

namespace CutTwice.Core.Lifecycle
{
    public static class LifecycleManagerUtils
    {
        public static RuntimeLifecycleManager CreateLifecycleManager(string tag)
        {
            var obj = new GameObject($"LifecycleManager_{tag}");
            return obj.AddComponent<RuntimeLifecycleManager>();
        }
    }
}