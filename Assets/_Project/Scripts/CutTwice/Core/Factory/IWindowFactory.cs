using System.Threading;
using Cysharp.Threading.Tasks;
using CascadeDI.Builder;

namespace CutTwice.Core.Factory
{
    public interface IWindowFactory
    {
        UniTask<IWindowInstance> CreateAsync(string name, System.Action<IContainerBuilder> compose, CancellationToken ct);
    }
}


