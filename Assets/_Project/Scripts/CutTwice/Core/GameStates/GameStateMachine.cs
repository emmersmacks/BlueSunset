using System.Collections.Generic;

namespace CutTwice.Core.GameStates
{
    public class GameStateMachine : StateMachineBase<IGameState>
    {
        public GameStateMachine(List<IGameState> states) : base(states)
        {
        }
    }
}