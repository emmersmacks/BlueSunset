using System.Threading;
using CutTwice.ObstacleSequence.ModuleLoader.Dto;
using CutTwice.Services;
using Cysharp.Threading.Tasks;

namespace CutTwice.ObstacleSequence.Services
{
    public interface IObstacleSequenceService : IService
    {
        UniTask<SequenceModuleDto> LoadModuleAsync(SequenceModulePreviewDto module, CancellationToken ct);
        UniTask<SequenceChunkDto[]> LoadAllChunksAsync(CancellationToken ct);
    }
}