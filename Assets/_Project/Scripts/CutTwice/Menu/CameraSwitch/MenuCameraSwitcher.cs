using System.Threading;
using Cinemachine;
using CutTwice.Core.EventBus;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu
{
    public class MenuCameraSwitcher : IInitializable
    {
        private const int ActivePriority = 20;
        private const int InactivePriority = 10;

        private readonly CameraSwitchContext _cameraContext;

        public MenuCameraSwitcher(CameraSwitchContext cameraContext)
        {
            _cameraContext = cameraContext;
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            SetActiveCamera(_cameraContext.GetCamera(MenuCameraType.Main));
            return UniTask.CompletedTask;
        }

        public void SwitchTo(MenuCameraType cameraType)
        {
            var target = _cameraContext.GetCamera(cameraType);
            if (target == null)
            {
                return;
            }

            SetActiveCamera(target);
        }

        public void CutBlend()
        {
            var brain = _cameraContext.CinemachineBrain;
            if (brain != null)
            {
                brain.ActiveBlend = null;
            }
        }

        private void SetActiveCamera(CinemachineVirtualCamera active)
        {
            foreach (var cameraData in _cameraContext.Cameras)
            {
                cameraData.Camera.Priority = cameraData.Camera == active ? ActivePriority : InactivePriority;
            }
        }
    }
}
