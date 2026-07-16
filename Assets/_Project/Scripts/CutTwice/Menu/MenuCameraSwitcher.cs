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

        private readonly MenuSceneReferences _sceneReferences;

        public MenuCameraSwitcher(MenuSceneReferences sceneReferences)
        {
            _sceneReferences = sceneReferences;
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            SetActiveCamera(_sceneReferences.IdleVirtualCamera);
            return UniTask.CompletedTask;
        }

        // Starts the Cinemachine blend toward the target vcam without waiting for it to finish -
        // callers that need the blend to land exactly when some other transition (e.g. a fade)
        // completes should follow up with CutBlend() once that other transition is done.
        public void SwitchTo(MenuCameraType cameraType)
        {
            var target = GetCamera(cameraType);
            if (target == null)
            {
                return;
            }

            SetActiveCamera(target);
        }

        // Instantly snaps the brain to the currently active vcam, cutting short any in-progress blend.
        public void CutBlend()
        {
            var brain = _sceneReferences.CinemachineBrain;
            if (brain != null)
            {
                brain.ActiveBlend = null;
            }
        }

        private CinemachineVirtualCamera GetCamera(MenuCameraType cameraType)
        {
            return cameraType switch
            {
                MenuCameraType.Main => _sceneReferences.IdleVirtualCamera,
                MenuCameraType.SelectLevel => _sceneReferences.PlayVirtualCamera,
                MenuCameraType.Shop => _sceneReferences.ShopVirtualCamera,
                _ => null
            };
        }

        private void SetActiveCamera(CinemachineVirtualCamera active)
        {
            foreach (var camera in new[] { _sceneReferences.IdleVirtualCamera, _sceneReferences.PlayVirtualCamera, _sceneReferences.ShopVirtualCamera })
            {
                if (camera == null)
                {
                    continue;
                }

                camera.Priority = camera == active ? ActivePriority : InactivePriority;
            }
        }
    }
}
