using CutTwice.Core.Factory;
using CutTwice.Core.Lifecycle;
using CutTwice.Gameplay.Runtime.Obstacles;
using CutTwice.Gameplay.Runtime.Obstacles.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Gameplay.Factories
{
    public class DeerFactory : GameObjectFactory
    {
        protected override string PrefabKey => "Obstacles/Deer";
        public override UniTask<Context> Create(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var deerContext = new DeerContext();
            
            deerContext.GameObject = InstantiatePrefab(_prefab, position, rotation, parent);
            
            var raycastStripPresenter = deerContext.GameObject.GetComponent<RaycastStripPresenter>();
            deerContext.RaycastStripController = new RaycastStripController(raycastStripPresenter);
            
            return UniTask.FromResult((Context)deerContext);
        }
    }
}