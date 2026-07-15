using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.Core.GameStates
{
    public class MenuStateMachine : StateMachineBase<IMenuState>
    {
        private bool _isTransitioning;

        public MenuStateMachine(List<IMenuState> states) : base(states)
        {
        }

        public async UniTask TransitionToAsync<T>(CancellationToken ct) where T : IMenuState
        {
            if (_isTransitioning)
            {
                return;
            }

            _isTransitioning = true;
            try
            {
                await SetStateAsync<T>(ct);
            }
            finally
            {
                _isTransitioning = false;
            }
        }
    }
}
