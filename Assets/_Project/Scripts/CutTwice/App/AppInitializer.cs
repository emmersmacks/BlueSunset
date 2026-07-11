using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Menu.GlobalStates;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.App
{
    public class AppInitializer : IInitializable
    {
        private readonly GlobalStateMachine _globalStateMachine;

        public AppInitializer(GlobalStateMachine globalStateMachine)
        {
            _globalStateMachine = globalStateMachine;
        }

        public async UniTask InitAsync(CancellationToken ct)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                await _globalStateMachine.SetStateAsync<GlobalMainMenuState>(ct);
            }
        }
    }
}