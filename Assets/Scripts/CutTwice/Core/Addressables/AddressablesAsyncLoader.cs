using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CutTwice.Common
{
    public static class AddressablesAsyncLoader
    {
        public static async UniTask<IList<T>> LoadAssetsAsync<T>(string key, CancellationToken ct)
        {
            var handle = Addressables.LoadAssetsAsync<T>(key);
            
            try
            {
                return await handle.ToUniTask(cancellationToken: ct);
            }
            catch (OperationCanceledException e)
            {
                Addressables.Release(handle);
                Debug.Log(e);
                throw;
            }
        }
        
        public static async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken ct)
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            
            try
            {
                return await handle.ToUniTask(cancellationToken: ct);
            }
            catch (OperationCanceledException e)
            {
                Addressables.Release(handle);
                Debug.Log(e);
                throw;
            }
        }
        
    }
}