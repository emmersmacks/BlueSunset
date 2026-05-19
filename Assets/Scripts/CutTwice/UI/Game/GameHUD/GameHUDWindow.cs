using CutTwice.Core.RivletUI;
using CutTwice.UI.Game.GameHUD.SleepBar;
using CutTwice.UI.Game.GameHUD.TimePanel;
using UnityEngine;

namespace CutTwice.UI.Game.GameHUD
{
    public class GameHUDWindow : WindowBase
    {
        public GameHUDWindow(GameObject windowObject, TimePanelController timePanelController,
            SleepBarController sleepBarController) : base(windowObject)
        {
            Register(timePanelController);
            Register(sleepBarController);
        }
    }
}