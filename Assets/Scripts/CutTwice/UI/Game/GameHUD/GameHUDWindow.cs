using UnityEngine;

namespace CutTwice.UI
{
    public class GameHUDWindow : WindowBase
    {
        public GameHUDWindow(GameObject windowObject, TimePanelController timePanelController) : base(windowObject)
        {
            Register(timePanelController);
        }
    }
}