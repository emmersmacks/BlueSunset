using CutTwice.Core.EventBus;
using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Menu;

namespace CutTwice.UI.MainMenu.SelectLevel.SmoothBackButton
{
    public class SmoothBackButtonController : WindowControllerBase<SmoothBackButtonView>
    {
        private readonly IEventBus _eventBus;

        public SmoothBackButtonController(SmoothBackButtonView view, IEventBus eventBus) : base(view)
        {
            _eventBus = eventBus;
            view.BackButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            _eventBus.Publish(new PushWindowRequest<MenuWindow>());
            _eventBus.Publish(new SwitchCameraEvent { CameraType = MenuCameraType.Main });
        }
    }
}