using System.Threading;
using CutTwice.App.LoadingScreen;
using CutTwice.Core.GameStates;
using CutTwice.Core.StaticNames;
using CutTwice.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.Menu.GlobalStates
{
    public class GlobalMainMenuState : IGlobalState
    {
        private readonly AudioSnapshotService _audioSnapshotService;
        private readonly LoadingScreenController _loadingScreenController;

        public GlobalMainMenuState(AudioSnapshotService audioSnapshotService, LoadingScreenController loadingScreenController)
        {
            _audioSnapshotService = audioSnapshotService;
            _loadingScreenController = loadingScreenController;
        }

        public UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            _loadingScreenController.Show();
            SceneManager.LoadScene(SceneNames.MainMenu);
            _audioSnapshotService.TransitionTo(AudioSnapshot.Menu);
            _loadingScreenController.Hide();
            return UniTask.CompletedTask;
        }

        public void Exit() { }
    }
}