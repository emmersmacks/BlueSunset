using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.Core.Lifecycle
{
    /// <summary>
    /// Simple interface for components that require parameterless initialization.
    /// </summary>
    public interface IInitializable : ILifecycleObject
    {
        UniTask InitAsync(CancellationToken ct);
    }
}

