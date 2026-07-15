using System;
using System.Threading;
using Cinemachine;
using CutTwice.Core.EventBus;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu
{
    public class MenuCameraSwitcher : IInitializable, IDisposable
    {
        private const int ActivePriority = 20;
        private const int InactivePriority = 10;

        private readonly MenuSceneReferences _sceneReferences;
        private readonly IEventBus _eventBus;

        public MenuCameraSwitcher(MenuSceneReferences sceneReferences, IEventBus eventBus)
        {
            _sceneReferences = sceneReferences;
            _eventBus = eventBus;
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            SetActiveCamera(_sceneReferences.IdleVirtualCamera, _sceneReferences.PlayVirtualCamera);
            _eventBus.Subscribe<SwitchCameraEvent>(OnCameraSwitchRequested);
            return UniTask.CompletedTask;
        }

        private void OnCameraSwitchRequested(SwitchCameraEvent evt)
        {
            switch (evt.CameraType)
            {
                case MenuCameraType.SelectLevel:
                {
                    SetActiveCamera(_sceneReferences.PlayVirtualCamera, _sceneReferences.IdleVirtualCamera);
                    break;
                }
                case MenuCameraType.Main:
                {
                    SetActiveCamera(_sceneReferences.IdleVirtualCamera, _sceneReferences.PlayVirtualCamera);
                    break;
                }
            }
            
        }

        private static void SetActiveCamera(CinemachineVirtualCamera active, CinemachineVirtualCamera inactive)
        {
            active.Priority = ActivePriority;
            inactive.Priority = InactivePriority;
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<SwitchCameraEvent>(OnCameraSwitchRequested);
        }
    }
}
