using System;
using System.Threading;
using CutTwice.Game;
using CutTwice.Infrastructure.Factories;
using CutTwice.ObstacleSequence.Serialization;
using CutTwice.ObstacleSequence.Serialization.SimpleTypes;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace CutTwice.ObstacleSequence.Actions
{
    public class SpawnTrafficAction : ISequenceActionRuntime, IDisposable
    {
        [Serializable]
        public struct Parameters
        {
            [JsonConverter(typeof(SimplePositionJsonConverter))]
            public SimplePosition Position;
            
            public SimpleRotation Rotation;
        }
        
        private readonly Parameters _parameters;
        private readonly TrafficFactory _factory;
        private readonly RuntimeLifecycleManager _lifecycleManager;

        private TrafficContext _trafficContext;
        
        public SpawnTrafficAction(Parameters parameters, TrafficFactory factory,
            RuntimeLifecycleManager lifecycleManager)
        {
            _parameters = parameters;
            _factory = factory;
            _lifecycleManager = lifecycleManager;
        }
        
        UniTask ISequenceActionRuntime.Init(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        async UniTask ISequenceActionRuntime.Run(CancellationToken ct)
        {
            _trafficContext = await _factory.Create(_parameters.Position, _parameters.Rotation) as TrafficContext;
            
            // TODO: Simplify this
            await _lifecycleManager.RuntimeRegister(_trafficContext.ObjectMoverController, ct);
            await _lifecycleManager.RuntimeRegister(_trafficContext.RaycastStripController, ct);
            
            _trafficContext.ObjectMoverController.OnFinished += DespawnObject;
        }

        private void DespawnObject()
        {
            _lifecycleManager.Unregister(_trafficContext.ObjectMoverController);
            _lifecycleManager.Unregister(_trafficContext.RaycastStripController);
            Object.Destroy(_trafficContext.GameObject);
            _trafficContext = null;
        }

        public void Dispose()
        {
            _trafficContext.ObjectMoverController.OnFinished -= DespawnObject;
        }
    }
}