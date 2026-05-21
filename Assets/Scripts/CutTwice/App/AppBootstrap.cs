using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.Initialization;
using CutTwice.Infrastructure.Scenes.Menu.States;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.Infrastructure.Scenes.App
{
    public class AppBootstrap : Bootstrap
    {
        protected override CompositionRoot CreateCompositionRoot()
        {
            return new AppCompositionRoot();
        }

        protected override async UniTask InitAsync(CancellationToken cancellationToken)
        {
            DontDestroyOnLoad(gameObject);

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                await GlobalStateMachine.Instance.SetStateAsync<MainMenuState>(destroyCancellationToken);
            }
        }
    }
}