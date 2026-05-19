using CutTwice.Core.RivletUI;
using CutTwice.UI.MainMenu.Menu.StartGameButton;
using UnityEngine;

namespace CutTwice.UI.MainMenu.Menu
{
    public class MenuWindow : WindowBase
    {
        public MenuWindow(GameObject windowObject, MenuButtonsController menuButtonsController) : base(windowObject)
        {
            Register(menuButtonsController);
        }
    }
}