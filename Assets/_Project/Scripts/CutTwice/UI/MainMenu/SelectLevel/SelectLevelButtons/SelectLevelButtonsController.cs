using System;
using System.Threading;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Core.RivletUI;
using CutTwice.Gameplay.GlobalStates;
using CutTwice.Gameplay.Modes;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.UI.MainMenu.SelectLevel.SelectLevelButtons
{
    public class SelectLevelButtonsController : WindowControllerBase<SelectLevelButtonsView>, IDisposable, IInitializable
    {
        private const float StoryModeDistanceMeters = 1000f;
        
        private readonly GlobalStateMachine _globalStateMachine;
        private readonly GameModeContext _gameModeContext;
        private CancellationToken _cancellationToken;

        public SelectLevelButtonsController(SelectLevelButtonsView view, GlobalStateMachine globalStateMachine, GameModeContext gameModeContext) : base(view)
        {
            _globalStateMachine = globalStateMachine;
            _gameModeContext = gameModeContext;
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
            _gameModeContext.SetMode(new DistanceModeConfig(StoryModeDistanceMeters));
            _globalStateMachine.SetStateAsync<GlobalGameState>(_cancellationToken).Forget(Debug.LogException);
        }

        private void OnAdventureModeButtonClicked()
        {
            _gameModeContext.SetMode(new EndlessModeConfig());
            _globalStateMachine.SetStateAsync<GlobalGameState>(_cancellationToken).Forget(Debug.LogException);
        }

        public void Dispose()
        {
            View.AdventureModeButton.onClick.RemoveListener(OnAdventureModeButtonClicked);
            View.StoryModeButton.onClick.RemoveListener(OnStoryModeButtonClicked);
        }
    }
}