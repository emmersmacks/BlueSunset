using CutTwice.Core.RivletUI;
using CutTwice.UI.Game.GameOver.MenuExitButton;
using CutTwice.UI.Game.GameOver.RestartButton;
using UnityEngine;

namespace CutTwice.UI.Game.GameOver
{
    public class GameOverWindow : WindowBase
    {
        public GameOverWindow(GameObject windowObject, MenuExitButtonController exitMenuButtonController,
            RestartButtonController restartButtonController) : base(windowObject)
        {
            Register(exitMenuButtonController);
            Register(restartButtonController);
        }
    }
}