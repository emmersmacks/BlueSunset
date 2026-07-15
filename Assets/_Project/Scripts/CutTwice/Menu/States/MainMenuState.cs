using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.Services;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.States
{
    public class MainMenuState : IMenuState
    {
        private readonly IEventBus _eventBus;
        private readonly IFadeService _fadeService;

        public MainMenuState(IEventBus eventBus, IFadeService fadeService)
        {
            _eventBus = eventBus;
            _fadeService = fadeService;
        }

        public async UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            await _fadeService.FadeOutAsync(ct);

            _eventBus.Publish(new PushWindowRequest<MenuWindow>());
            _eventBus.Publish(new SwitchCameraEvent { CameraType = MenuCameraType.Main });

            await _fadeService.FadeInAsync(ct);
        }

        public void Exit()
        {
        }
    }
}
