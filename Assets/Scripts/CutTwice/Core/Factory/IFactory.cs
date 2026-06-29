using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Core.Factory
{
    public interface IGameObjectFactory
    {
        GameObject InstantiatePrefab(GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null);
        
        public UniTask<Context> Create(
            Vector3 position,
            Quaternion rotation,
            Transform parent = null);

        void Destroy(GameObject gameObject);
    }
}