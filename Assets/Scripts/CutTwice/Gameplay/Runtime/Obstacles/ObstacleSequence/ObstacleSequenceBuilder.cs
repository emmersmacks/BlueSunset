using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CutTwice.Controllers;
using CutTwice.Core.Factory;
using CutTwice.Game;
using CutTwice.Infrastructure.Factories;
using CutTwice.ObstacleSequence.Actions;
using CutTwice.ObstacleSequence.ModuleLoader.Dto;
using CutTwice.ObstacleSequence.Services;
using Cysharp.Threading.Tasks;

namespace CutTwice.ObstacleSequence
{
    public class ObstacleSequenceBuilder
    {
        private readonly IObstacleSequenceService _service;
        private readonly InfiniteRoadController _infiniteRoadController;
        private readonly TrafficFactory _trafficFactory;
        private readonly DeerFactory _deerFactory;
        private readonly RuntimeLifecycleManager _lifecycleManager;

        private Dictionary<string, SequenceActionDto[]> _allChunks;
        
        public ObstacleSequenceBuilder(IObstacleSequenceService service, InfiniteRoadController infiniteRoadController,
            TrafficFactory trafficFactory, DeerFactory deerFactory, RuntimeLifecycleManager lifecycleManager)
        {
            _service = service;
            _infiniteRoadController = infiniteRoadController;
            _trafficFactory = trafficFactory;
            _deerFactory = deerFactory;
            _lifecycleManager = lifecycleManager;
        }

        public async UniTask Init(CancellationToken ct)
        {
            var allChunks = await _service.LoadAllChunksAsync(ct);
            _allChunks = allChunks.ToDictionary(k => k.Id, v => v.Actions);
        }
        
        public ObstacleSequenceModuleRuntime BuildModule(SequenceModuleDto module)
        {
            var moduleRuntime = new ObstacleSequenceModuleRuntime();
            var actions = new List<ISequenceActionRuntime>();
            
            foreach(var chunkRef in module.Sequences)
            {
                var actionDtoArr = _allChunks[chunkRef.Id];
                foreach (var actionDto in actionDtoArr)
                {
                    switch (actionDto.Type)
                    {
                        case ActionType.Delay:
                            actions.Add(new DelayAction(actionDto.Parameters.ToObject<DelayAction.Parameters>()));
                            break;
                        case ActionType.SpawnTraffic:
                            var spawnParameters = actionDto.Parameters.ToObject<SpawnTrafficAction.Parameters>();
                            actions.Add(new SpawnTrafficAction(spawnParameters, _trafficFactory, _lifecycleManager));
                            break;
                        case ActionType.SpawnDeer:
                            var spawnOnTileParameters = actionDto.Parameters.ToObject<SpawnDeerAction.Parameters>();
                            actions.Add(new SpawnDeerAction(spawnOnTileParameters, _infiniteRoadController, _deerFactory, _lifecycleManager));
                            break;
                    }
                }
            }
            
            moduleRuntime.Commands = actions.ToArray();
            return moduleRuntime;
        }
    }
}