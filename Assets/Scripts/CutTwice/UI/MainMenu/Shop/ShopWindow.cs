using CutTwice.Core.RivletUI;
using CutTwice.UI.Common.UIBackButton;
using UnityEngine;

namespace CutTwice.UI.MainMenu.Shop
{
    public class ShopWindow : WindowBase
    {
        public ShopWindow(GameObject windowObject, UIBackButtonController uiBackButtonController) : base(windowObject)
        {
            Register(uiBackButtonController);
        }
    }
}