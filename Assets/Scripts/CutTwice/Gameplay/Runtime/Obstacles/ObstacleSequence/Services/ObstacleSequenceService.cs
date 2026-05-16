using System;
using System.Collections.Generic;
using System.Threading;
using CutTwice.ObstacleSequence.ModuleLoader;
using CutTwice.ObstacleSequence.ModuleLoader.Dto;
using Cysharp.Threading.Tasks;

namespace CutTwice.ObstacleSequence.Services
{
    public class ObstacleSequenceService : IObstacleSequenceService
    {
        private SequenceModulePreviewDto[] _allModules;
        private ISequenceModuleLoader _sequenceModuleLoader;
        
        public ObstacleSequenceService(ISequenceModuleLoader sequenceModuleLoader)
        {
            _sequenceModuleLoader = sequenceModuleLoader;
        }

        public async UniTask InitAsync(CancellationToken ct)
        {
            _allModules = await _sequenceModuleLoader.LoadModulesListAsync(ct);
        }

        async UniTask<SequenceModuleDto> IObstacleSequenceService.LoadModuleAsync(SequenceModulePreviewDto module, CancellationToken ct)
        {
            return await _sequenceModuleLoader.LoadModuleAsync(module, ct);
        }

        async UniTask<SequenceChunkDto[]> IObstacleSequenceService.LoadAllChunksAsync(CancellationToken ct)
        {
            var sequenceChunks = new List<SequenceChunkDto>();
            foreach (ChunkType type in Enum.GetValues(typeof(ChunkType)))
            {
                var chunkList = await _sequenceModuleLoader.LoadChunkAsync(type, ct);
                sequenceChunks.AddRange(chunkList.Chunks);
            }
            return sequenceChunks.ToArray();
        }

        public UniTask DestroyAsync() { return default; }
    }
}