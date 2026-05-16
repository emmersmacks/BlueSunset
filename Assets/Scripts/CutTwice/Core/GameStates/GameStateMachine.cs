using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Infrastructure.Events;

namespace CutTwice.GameStates
{
    /// <summary>
    /// MonoBehaviour singleton that manages game states and routes Update/FixedUpdate calls.
    /// Automatically subscribes to GameOverEvent and transitions to EndGameState.
    /// </summary>
    public class GameStateMachine
    {
        private Dictionary<Type, IGameState> _states;
        private IGameState _currentState;
        
        public static GameStateMachine Instance { get; private set; }

        public GameStateMachine(List<IGameState> states)
        {
            _states = states.ToDictionary(s => s.GetType(), s => s);
            _currentState = new BootstrapState();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("There are two statement instances of GameStateMachine");
            }
        }

        public async UniTask SetStateAsync<T>(CancellationToken ct) where T : IGameState
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = _states[typeof(T)];

            if (_currentState != null)
            {
                await _currentState.Enter(ct);
            }
        }
    }
}

