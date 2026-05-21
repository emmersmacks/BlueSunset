using CutTwice.Core.EventBus;
using CutTwice.Core.RivletUI;

namespace CutTwice.UI.Common.UIBackButton
{
    public class UIBackButtonController : WindowControllerBase<UIBackButtonView>
    {
        private readonly IEventBus _eventBus;

        public UIBackButtonController(UIBackButtonView view, IEventBus eventBus) : base(view)
        {
            _eventBus = eventBus;
            View.BackButton.onClick.AddListener(Back);
        }

        private void Back()
        {
            _eventBus.Publish(new PopWindowRequest());
        }
    }
}