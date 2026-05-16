using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace CutTwice.ObstacleSequence
{
    public class ObstacleRuntimeController
    {
        private ObstacleSequenceModuleRuntime _sequenceModuleRuntime;
        
        public async Task Init(ObstacleSequenceModuleRuntime sequence, CancellationToken ct)
        {
            foreach (var command in sequence.Commands)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                
                await command.Init(ct);
            }
            
            _sequenceModuleRuntime = sequence;
        }

        public async UniTask Run(CancellationToken ct)
        {
            foreach (var command in _sequenceModuleRuntime.Commands)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                
                await command.Run(ct);
            }
        }
    }
}