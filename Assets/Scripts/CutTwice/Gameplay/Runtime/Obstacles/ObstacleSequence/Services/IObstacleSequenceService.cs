using System.Threading;
using CutTwice.Gameplay.Runtime.Obstacles.ObstacleSequence.ModuleLoader.Dto;
using CutTwice.Infrastructure.Services;
using Cysharp.Threading.Tasks;

namespace CutTwice.Gameplay.Runtime.Obstacles.ObstacleSequence.Services
{
    public interface IObstacleSequenceService : IService
    {
        UniTask<SequenceModuleDto> LoadModuleAsync(SequenceModulePreviewDto module, CancellationToken ct);
        UniTask<SequenceChunkDto[]> LoadAllChunksAsync(CancellationToken ct);
    }
}