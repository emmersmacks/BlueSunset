using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.Menu;
using CutTwice.Services;
using CutTwice.UI.MainMenu.SelectLevel;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.States
{
    public class SelectLevelState : IMenuState
    {
        private readonly IEventBus _eventBus;
        private readonly IFadeService _fadeService;
        private readonly MenuCameraSwitcher _cameraSwitcher;

        public SelectLevelState(IEventBus eventBus, IFadeService fadeService, MenuCameraSwitcher cameraSwitcher)
        {
            _eventBus = eventBus;
            _fadeService = fadeService;
            _cameraSwitcher = cameraSwitcher;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            var fadeTask = _fadeService.FadeOutAsync(ct);
            var switchCameraTask = _cameraSwitcher.SwitchToAsync(MenuCameraType.SelectLevel, ct);
            await UniTask.WhenAll(fadeTask, switchCameraTask);
            
            _eventBus.Publish(new PushWindowRequest<SelectLevelWindow>());
            await _fadeService.FadeInAsync(ct);
        }

        public void Exit()
        {
        }
    }
}
