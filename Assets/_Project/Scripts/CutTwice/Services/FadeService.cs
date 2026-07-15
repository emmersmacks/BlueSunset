using System;
using System.Threading;
using CutTwice.App.Fade;
using CutTwice.Core.Lifecycle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CutTwice.Services
{
    // UniTask's DOTween integration (ToUniTask/AwaitForComplete) requires the
    // UNITASK_DOTWEEN_SUPPORT define, which this project's DOTween install doesn't set,
    // so tweens are awaited manually via WaitUntil instead.
    public class FadeService : IFadeService, IService, IInitializable
    {
        private readonly CanvasGroup _canvasGroup;

        public FadeService(FadeView fadeView)
        {
            _canvasGroup = fadeView.CanvasGroup;
        }

        public UniTask InitAsync(CancellationToken ct)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            return UniTask.CompletedTask;
        }

        public async UniTask FadeOutAsync(CancellationToken ct, float duration = 0.3f)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOKill();
            var tween = _canvasGroup.DOFade(1f, duration);
            await AwaitTweenAsync(tween, ct);
        }

        public async UniTask FadeInAsync(CancellationToken ct, float duration = 0.3f)
        {
            _canvasGroup.DOKill();
            var tween = _canvasGroup.DOFade(0f, duration);
            await AwaitTweenAsync(tween, ct);
            _canvasGroup.blocksRaycasts = false;
        }

        private static async UniTask AwaitTweenAsync(Tween tween, CancellationToken ct)
        {
            try
            {
                await UniTask.WaitUntil(() => !tween.IsActive(), cancellationToken: ct);
            }
            catch (OperationCanceledException)
            {
                if (tween.IsActive())
                {
                    tween.Kill();
                }
                throw;
            }
        }
    }
}
