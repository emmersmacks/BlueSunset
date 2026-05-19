using System.Threading;
using CutTwice.Core.Addressables;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Core.Factory
{
    public abstract class GameObjectFactory : IFactoryBase, IInitializable
    {
        protected abstract string PrefabKey { get; }
        
        protected GameObject _prefab;
        
        public GameObject InstantiatePrefab(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
        {
            return Object.Instantiate(
                prefab,
                position,
                rotation,
                parent);
        }

        public void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        public abstract UniTask<Context> Create(
            Vector3 position,
            Quaternion rotation,
            Transform parent = null);

        public async UniTask InitAsync(CancellationToken ct)
        {
            _prefab = await AddressablesAsyncLoader.LoadAssetAsync<GameObject>(PrefabKey, ct);
        }
    }
}