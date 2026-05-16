using System;
using System.Threading;
using CutTwice.Common;
using CutTwice.GameStates;
using CutTwice.ObstacleSequence;
using CutTwice.ObstacleSequence.ModuleLoader.Dto;
using CutTwice.ObstacleSequence.Services;
using CutTwice.UI.MainMenu;
using Cysharp.Threading.Tasks;
using Infrastructure.Events;
using UnityEngine;
using YG;

namespace CutTwice.Game
{
    public class GameSession : IInitializable, IDisposable, ITickable
    {
        private readonly IObstacleSequenceService _service;
        private readonly ObstacleSequenceBuilder _builder;
        private readonly ObstacleRuntimeController _runtime;

        public float SessionTime;
        
        private bool _gameStarted = false;
        private CancellationTokenSource _sessionCancellationTokenSource;
        private CancellationToken _destroyCancellationToken;
        
        public GameSession(IObstacleSequenceService service, ObstacleSequenceBuilder builder, ObstacleRuntimeController runtime)
        {
            _service = service;
            _builder = builder;
            _runtime = runtime;
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            _destroyCancellationToken = ct;
            _sessionCancellationTokenSource = new CancellationTokenSource();
            EventBus.Subscribe<GameOverEvent>(OnGameOverRequested);
            StartSequenceAsync(new SequenceModulePreviewDto { Name = "DefaultSequenceModule" }, _sessionCancellationTokenSource.Token).Forget(Debug.LogException);
            return UniTask.CompletedTask;
        }

        private async UniTask StartSequenceAsync(SequenceModulePreviewDto modulePreviewDto, CancellationToken ct)
        {
            _gameStarted = true;
            
            var dto = await _service.LoadModuleAsync(modulePreviewDto, ct);
            
            await _builder.Init(ct);
            var sequence = _builder.BuildModule(dto);

            await _runtime.Init(sequence, ct);
            await _runtime.Run(ct);
        }

        public void Tick()
        {
            if(_gameStarted)
            {
                SessionTime += Time.deltaTime;
            }
        }

        private void OnGameOverRequested(GameOverEvent obj)
        {
            _gameStarted = false;
            
            _sessionCancellationTokenSource.Cancel();
            _sessionCancellationTokenSource.Dispose();
            _sessionCancellationTokenSource = null;
            
            GameStateMachine.Instance.SetStateAsync<EndGameState>(_destroyCancellationToken).Forget(Debug.LogException);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<GameOverEvent>(OnGameOverRequested);

            if (_sessionCancellationTokenSource != null)
            {
                _sessionCancellationTokenSource.Cancel();
                _sessionCancellationTokenSource.Dispose();
                _sessionCancellationTokenSource = null;
            }
        }
    }
}