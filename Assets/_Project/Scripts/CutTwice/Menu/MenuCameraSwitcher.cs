using System.Threading;
using Cinemachine;
using CutTwice.Core.EventBus;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using UnityEngine;

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

        public async UniTask SwitchToAsync(MenuCameraType cameraType, CancellationToken ct)
        {
            var target = GetCamera(cameraType);
            if (target == null)
            {
                return;
            }

            SetActiveCamera(target);
            await AwaitBlendCompleteAsync(ct);
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

        private async UniTask AwaitBlendCompleteAsync(CancellationToken ct)
        {
            var brain = _sceneReferences.CinemachineBrain;
            if (brain == null)
            {
                return;
            }
            
            await UniTask.WaitUntil(() => !brain.IsBlending, cancellationToken: ct);
            Debug.Log(brain.IsBlending);
        }
    }
}
