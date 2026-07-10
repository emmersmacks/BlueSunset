using System.Collections.Generic;
using System.Threading;
using CutTwice.App;
using CutTwice.Core.Addressables;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using CascadeDI;
using CascadeDI.Builder;
using CascadeDI.Container;
using UnityEngine;

namespace CutTwice.Core.Initialization
{
    public abstract class Bootstrap : MonoBehaviour
    {
        public bool InstantiateAppBootstrap;
        public IContainer Container;
        
        protected CompositionRoot CompositionRoot;
        
        protected abstract CompositionRoot CreateCompositionRoot();
        
        private void Awake()
        {
            AwakeAsync().Forget(Debug.LogException);
        }

        private async UniTask AwakeAsync()
        {
            var appBootstrap = FindAnyObjectByType<AppBootstrap>();
            if (InstantiateAppBootstrap && appBootstrap == null)
            {
                var bootstrap = await AddressablesAsyncLoader.LoadAssetAsync<GameObject>("AppBootstrap", destroyCancellationToken);
                var bootstrapObj =  Instantiate(bootstrap);
                appBootstrap = bootstrapObj.GetComponent<AppBootstrap>();
            }
            
            CompositionRoot = CreateCompositionRoot();

            var builder = appBootstrap.Container == null ? new ContainerBuilder() : appBootstrap.Container.CreateChildBuilder();
            var lifecycleManager = LifecycleManagerUtils.CreateLifecycleManager(gameObject.name);
            
            CompositionRoot.Compose(builder, lifecycleManager);
            Container = builder.Build();
            var scope = Container.CreateScope();
            
            var lifecicleObjects = scope.Resolve<List<ILifecycleObject>>();
            lifecycleManager.Register(lifecicleObjects);
            await lifecycleManager.InitAsync(destroyCancellationToken);

            await InitAsync(scope, destroyCancellationToken);
        }

        protected virtual UniTask InitAsync(IScope scope, CancellationToken ct) { return UniTask.CompletedTask; }

        private void OnDestroy()
        {
            CompositionRoot = null;
        }
    }
}