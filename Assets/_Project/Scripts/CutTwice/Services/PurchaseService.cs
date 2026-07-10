using System;
using System.Threading;
using CutTwice.Core.Lifecycle;
using CutTwice.Gameplay;
using Cysharp.Threading.Tasks;
using YG;

namespace CutTwice.Services
{
    public class PurchaseService : IService, IInitializable, IDisposable
    {
        public UniTask InitAsync(CancellationToken ct)
        {
            YG2.onPurchaseSuccess += OnPurchaseSuccess;
            return default;
        }
        
        private void OnPurchaseSuccess(string id)
        {
            YG2.SetState($"payment_{id}", 1);
            PlayerData.SteeringWheelId = id;
            PlayerData.Save();
        }

        public void Dispose()
        {
            YG2.onPurchaseSuccess -= OnPurchaseSuccess;
        }
    }
}