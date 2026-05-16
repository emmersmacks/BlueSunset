using CutTwice.UI.Common.UIBackButton;
using UnityEngine;

namespace CutTwice.UI.MainMenu.Leaderboard
{
    public class LeaderboardWindow : WindowBase
    {
        public LeaderboardWindow(GameObject windowObject, UIBackButtonController uiBackButtonController) : base(windowObject)
        {
            Register(uiBackButtonController);
        }
    }
}