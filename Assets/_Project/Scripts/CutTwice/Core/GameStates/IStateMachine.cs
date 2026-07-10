using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.Core.GameStates
{
    public interface IStateMachine
    {
        public UniTask SetStateAsync<T>(CancellationToken ct) where T : IState;
    }
}