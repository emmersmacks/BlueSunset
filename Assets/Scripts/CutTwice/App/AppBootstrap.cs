using System.Threading;
using CutTwice.Core.Initialization;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using CascadeDI;

namespace CutTwice.App
{
    public class AppBootstrap : Bootstrap
    {
        protected override CompositionRoot CreateCompositionRoot()
        {
            return new AppCompositionRoot();
        }

        protected override UniTask InitAsync(IScope scope, CancellationToken cancellationToken)
        {
            var appLifecycleManager = scope.Resolve<RuntimeLifecycleManager>();
            DontDestroyOnLoad(appLifecycleManager.gameObject);
            DontDestroyOnLoad(gameObject);
            return UniTask.CompletedTask;
        }
    }
}