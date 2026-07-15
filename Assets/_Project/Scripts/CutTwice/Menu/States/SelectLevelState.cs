using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.SelectLevel;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.States
{
    public class SelectLevelState : IMenuState
    {
        private readonly IEventBus _eventBus;

        public SelectLevelState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _eventBus.Publish(new PushWindowRequest<SelectLevelWindow>());
            _eventBus.Publish(new SwitchCameraEvent { CameraType = MenuCameraType.SelectLevel });
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
        }
    }
}
