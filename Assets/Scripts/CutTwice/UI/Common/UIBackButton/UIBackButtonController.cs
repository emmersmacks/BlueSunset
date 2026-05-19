using CutTwice.Core.EventBus;
using CutTwice.Core.RivletUI;

namespace CutTwice.UI.Common.UIBackButton
{
    public class UIBackButtonController : WindowControllerBase<UIBackButtonView>   {
        public UIBackButtonController(UIBackButtonView view) : base(view)
        {
            View.BackButton.onClick.AddListener(Back);
        }

        private void Back()
        {
            EventBus.Publish(new PopWindowRequest());
        }
    }
}