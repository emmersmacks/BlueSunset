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

        public ShopState(IEventBus eventBus, IFadeService fadeService)
        {
            _eventBus = eventBus;
            _fadeService = fadeService;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            await _fadeService.FadeOutAsync(ct);

            _eventBus.Publish(new PushWindowRequest<ShopWindow>());
            _eventBus.Publish(new SwitchCameraEvent { CameraType = MenuCameraType.Shop });

            await _fadeService.FadeInAsync(ct);
        }

        public void Exit()
        {
        }
    }
}
