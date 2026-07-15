using System.Threading;
using Cysharp.Threading.Tasks;

namespace CutTwice.Services
{
    public interface IFadeService
    {
        UniTask FadeOutAsync(CancellationToken ct, float duration = 0.3f);

        UniTask FadeInAsync(CancellationToken ct, float duration = 0.3f);
    }
}
