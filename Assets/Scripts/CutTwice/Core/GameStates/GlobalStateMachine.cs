using System;
using System.Collections.Generic;
using CutTwice.App.GlobalStates;

namespace CutTwice.Core.GameStates
{
    /// <summary>
    /// MonoBehaviour singleton that manages game states and routes Update/FixedUpdate calls.
    /// Automatically subscribes to GameOverEvent and transitions to EndGameState.
    /// </summary>
    public class GlobalStateMachine : StateMachineBase<IGlobalState>
    {
        public GlobalStateMachine(List<IGlobalState> states) : base(states)
        {
            _currentState = new BootstrapState();
        }
    }
}

