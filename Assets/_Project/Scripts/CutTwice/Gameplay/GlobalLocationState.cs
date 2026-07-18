using System.Threading;
using CutTwice.App.LoadingScreen;
using CutTwice.Core.GameStates;
using CutTwice.Core.StaticNames;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.Gameplay.GlobalStates
{
    public class GlobalLocationState : IGlobalState
    {
        private readonly LoadingScreenController _loadingScreenController;

        public GlobalLocationState(LoadingScreenController loadingScreenController)
        {
            _loadingScreenController = loadingScreenController;
        }

        public UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _loadingScreenController.Show();
            SceneManager.LoadScene(SceneNames.Location);
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
        }
    }
}
