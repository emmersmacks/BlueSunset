using System.Threading;
using CutTwice.Core.EventBus;
using CutTwice.Core.GameStates;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Shop;
using Cysharp.Threading.Tasks;

namespace CutTwice.Menu.States
{
    public class ShopState : IMenuState
    {
        private readonly IEventBus _eventBus;

        public ShopState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _eventBus.Publish(new PushWindowRequest<ShopWindow>());
            _eventBus.Publish(new SwitchCameraEvent { CameraType = MenuCameraType.Shop });
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
        }
    }
}
