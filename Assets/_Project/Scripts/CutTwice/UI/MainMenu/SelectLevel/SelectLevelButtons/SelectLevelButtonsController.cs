using System;
using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Gameplay.GlobalStates;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.UI.MainMenu.SelectLevel.SelectLevelButtons
{
    public class SelectLevelButtonsController : WindowControllerBase<SelectLevelButtonsView>, IDisposable, IInitializable
    {
        private readonly GlobalStateMachine _globalStateMachine;
        private CancellationToken _cancellationToken;

        public SelectLevelButtonsController(SelectLevelButtonsView view, GlobalStateMachine globalStateMachine) : base(view)
        {
            _globalStateMachine = globalStateMachine;
            View.AdventureModeButton.onClick.AddListener(OnAdventureModeButtonClicked);
            View.StoryModeButton.onClick.AddListener(OnStoryModeButtonClicked);
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            _cancellationToken = ct;
            return UniTask.CompletedTask;
        }

        private void OnStoryModeButtonClicked()
        {
            throw new Exception("Story mode is not implemented yet");
        }

        private void OnAdventureModeButtonClicked()
        {
            _globalStateMachine.SetStateAsync<GlobalGameState>(_cancellationToken).Forget(Debug.LogException);
        }

        public void Dispose()
        {
            View.AdventureModeButton.onClick.RemoveListener(OnAdventureModeButtonClicked);
            View.StoryModeButton.onClick.RemoveListener(OnStoryModeButtonClicked);
        }
    }
}