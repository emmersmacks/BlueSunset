using CutTwice.Core.RivletUI;
using CutTwice.Core.StaticNames;
using UnityEngine.SceneManagement;

namespace CutTwice.UI.Game.GameOver.MenuExitButton
{
    public class MenuExitButtonController : WindowControllerBase<MenuExitButtonView>
    {
        public MenuExitButtonController(MenuExitButtonView view) : base(view)
        {
            view.Button.onClick.AddListener(ExitMenu);
        }

        private void ExitMenu()
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
        }
    }
}