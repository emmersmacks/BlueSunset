using System.Threading;
using CutTwice.Core.GameStates;
using Cysharp.Threading.Tasks;

namespace CutTwice.Infrastructure.Scenes.App.States
{
    public class BootstrapState : IGlobalState

    {
    public UniTask Enter(IStateMachine stateMachine, CancellationToken ct)
    {
        return UniTask.CompletedTask;
    }

    public void Exit()
    {
    }
    }
}