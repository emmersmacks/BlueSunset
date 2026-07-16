using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.Services;
using CutTwice.UI.MainMenu.Shop;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.States
{
    public class ShopState : IMenuState
    {
        private readonly IEventBus _eventBus;
        private readonly IFadeService _fadeService;
        private readonly MenuCameraSwitcher _cameraSwitcher;

        public ShopState(IEventBus eventBus, IFadeService fadeService, MenuCameraSwitcher cameraSwitcher)
        {
            _eventBus = eventBus;
            _fadeService = fadeService;
            _cameraSwitcher = cameraSwitcher;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            var fadeTask = _fadeService.FadeOutAsync(ct);
            var switchCameraTask = _cameraSwitcher.SwitchToAsync(MenuCameraType.Shop, ct);
            await UniTask.WhenAll(fadeTask, switchCameraTask);

            _eventBus.Publish(new PushWindowRequest<ShopWindow>());
            await _fadeService.FadeInAsync(ct);
        }

        public void Exit()
        {
        }
    }
}
