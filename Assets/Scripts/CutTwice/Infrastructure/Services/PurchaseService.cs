using System.Threading;
using CutTwice.Infrastructure.Scenes.Game;
using Cysharp.Threading.Tasks;
using YG;

namespace CutTwice.Infrastructure.Services
{
    public class PurchaseService : IService
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

        public UniTask DestroyAsync()
        {
            YG2.onPurchaseSuccess -= OnPurchaseSuccess;
            return default;
        }
    }
}