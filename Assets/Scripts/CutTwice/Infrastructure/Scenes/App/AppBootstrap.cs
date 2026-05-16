using System.Threading;
using CutTwice.Common.Infrastructure;
using CutTwice.Game;
using CutTwice.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.App
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
                await GameStateMachine.Instance.SetStateAsync<MainMenuState>(destroyCancellationToken);
            }
        }
    }
}