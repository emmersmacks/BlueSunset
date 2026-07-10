using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.StaticNames;
using CutTwice.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CutTwice.Menu.GlobalStates
{
    public class MainMenuState : IGlobalState
    {
        private readonly AudioSnapshotService _audioSnapshotService;

        public MainMenuState(AudioSnapshotService audioSnapshotService)
        {
            _audioSnapshotService = audioSnapshotService;
        }

        public UniTask EnterAsync(IStateMachine stateMachine, CancellationToken ct)
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
            _audioSnapshotService.TransitionTo(AudioSnapshot.Menu);
            return UniTask.CompletedTask;
        }

        public void Exit() { }
    }
}