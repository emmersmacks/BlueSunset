using CutTwice.UI.Common.UIBackButton;
using UnityEngine;

namespace CutTwice.UI.MainMenu.Credits
{
    public class CreditsWindow : WindowBase
    {
        public CreditsWindow(GameObject windowObject, UIBackButtonController uiBackButtonController) : base(windowObject)
        {
            Register(uiBackButtonController);
        }
    }
}