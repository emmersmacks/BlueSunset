using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.Core.GameStates
{
    public interface IGameState
    {
        /// <summary>
        /// Called when the state becomes active
        /// </summary>
        UniTask Enter(CancellationToken ct);

        /// <summary>
        /// Called when the state is exited
        /// </summary>
        void Exit();
    }
}

