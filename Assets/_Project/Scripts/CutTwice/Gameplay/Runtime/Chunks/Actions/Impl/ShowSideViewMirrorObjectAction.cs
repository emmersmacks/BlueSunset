using System.Threading;
using CutTwice.Gameplay.Runtime.Chunks.ModuleLoader.Dto;
using CutTwice.Gameplay.Runtime.Hazards.Components;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CutTwice.Gameplay.Runtime.Chunks.Actions
{
    [SequenceAction(ActionType.SideviewObject)]
    public class ShowSideViewMirrorObjectAction : ISequenceActionRuntime
    {
        private readonly SideviewMirrorHazardController _sideviewMirrorHazardController;
        
        public ShowSideViewMirrorObjectAction(SideviewMirrorHazardController sideviewMirrorHazardController)
        {
            _sideviewMirrorHazardController = sideviewMirrorHazardController;
        }

        public UniTask Init(JObject parameters, CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        public UniTask Run(CancellationToken ct)
        {
            _sideviewMirrorHazardController.StartHazard();
            return UniTask.CompletedTask;
        }
    }
}