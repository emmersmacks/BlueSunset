using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Menu;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.States
{
    public class MainMenuState : IMenuState
    {
        private readonly IEventBus _eventBus;

        public MainMenuState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _eventBus.Publish(new PushWindowRequest<MenuWindow>());
            _eventBus.Publish(new SwitchCameraEvent { CameraType = MenuCameraType.Main });
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
        }
    }
}
