using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using CutTwice.Core.Lifecycle;
using CutTwice.Gameplay.Runtime.Chunks.Actions;
using CutTwice.Gameplay.Runtime.Chunks.ModuleLoader.Dto;
using CutTwice.Gameplay.Runtime.Chunks.Services;
using Cysharp.Threading.Tasks;

namespace CutTwice.Gameplay.Runtime.Chunks
{
    public class ObstacleSequenceBuilder : IInitializable
    {
        private readonly IObstacleSequenceService _service;
        private readonly ActionFactory _actionFactory;
        
        private readonly Dictionary<ActionType, Type> _registry;

        private Dictionary<string, SequenceActionDto[]> _allChunks;
        
        public ObstacleSequenceBuilder(IObstacleSequenceService service, ActionFactory actionFactory)
        {
            _service = service;
            _actionFactory = actionFactory;
            _registry = BuildRegistry();
        }

        public async UniTask InitAsync(CancellationToken ct)
        {
            var allChunks = await _service.LoadAllChunksAsync(ct);
            _allChunks = allChunks.ToDictionary(k => k.Id, v => v.Actions);
        }
        
        public async UniTask<ObstacleSequenceModuleRuntime> BuildModuleAsync(SequenceModuleDto module, CancellationToken ct)
        {
            var moduleRuntime = new ObstacleSequenceModuleRuntime();
            var chunks = new List<SequenceChunkRuntime>();
            
            foreach(var chunkRef in module.Sequences)
            {
                var chunkRuntime = new SequenceChunkRuntime
                {
                    Name = chunkRef.Id
                };

                var actionDtoArr = _allChunks[chunkRef.Id];
                foreach (var actionDto in actionDtoArr)
                {
                    var action = await _actionFactory.CreateAsync(_registry[actionDto.Type], actionDto.Parameters, ct);

                    if (action == null)
                    {
                        throw new Exception("Unknown action type: " + actionDto.Type);
                    }
                    
                    chunkRuntime.Actions.Add(action);
                }
                chunks.Add(chunkRuntime);
            }
            
            moduleRuntime.Chunks = chunks.ToArray();
            return moduleRuntime;
        }

        private static Dictionary<ActionType, Type> BuildRegistry()
        {
            var interfaceType = typeof(ISequenceActionRuntime);

            var result = new Dictionary<ActionType, Type>();

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        return e.Types.Where(t => t != null)!;
                    }
                });

            foreach (var type in types)
            {
                if (type.IsAbstract ||
                    type.IsInterface ||
                    !interfaceType.IsAssignableFrom(type))
                {
                    continue;
                }

                var attribute = type.GetCustomAttribute<SequenceActionAttribute>();

                if (attribute == null)
                {
                    continue;
                }

                if (!result.TryAdd(attribute.ActionType, type))
                {
                    throw new InvalidOperationException(
                        $"Duplicate ActionType '{attribute.ActionType}' found. " +
                        $"Type '{type.FullName}' conflicts with '{result[attribute.ActionType].FullName}'.");
                }
            }

            return result;
        }
    }
}