using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Core.AssetLoader
{
    public class AddressablesAssetLoaderService : IAddressablesAssetLoaderService
    {
        private readonly Dictionary<string, Object> _loadedAssets = new();
        private IAddressablesAssetLoaderService _addressablesAssetLoaderServiceImplementation;

        public int LoadedAssetsCount => _loadedAssets.Count;
    
        public async UniTask<TAsset> LoadAssetAsync<TAsset>(string address) where TAsset : Object
        {
            try
            {
                if (_loadedAssets.TryGetValue(address, out var asset))
                    return asset as TAsset;
                
                var operation = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TAsset>(address);
                await operation.Task;

                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    var result = operation.Result;
                    _loadedAssets[address] = result;
                    return result;
                }
                else
                {
                    Debug.LogError($"Failed to load asset at address: {address}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading asset at address {address}: {ex.Message}");
                return null;
            }
        }
    
        public TAsset LoadAsset<TAsset>(string address) where TAsset : Object
        {
            try
            {
                if (_loadedAssets.TryGetValue(address, out var asset))
                    return asset as TAsset;
            
                AsyncOperationHandle<TAsset> handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TAsset>(address);
                TAsset result = handle.WaitForCompletion();
                _loadedAssets.Add(address, result);
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading asset at address {address}: {ex.Message}");
                return null;
            }
        }
    
        public bool ReleaseAsset(string address)
        {
            if (_loadedAssets.TryGetValue(address, out var asset))
            {
                UnityEngine.AddressableAssets.Addressables.Release(asset);
                _loadedAssets.Remove(address);
                return true;
            }
        
            return false;
        }

        public void ReleaseAllAssets()
        {
            int count = _loadedAssets.Count;
            foreach (var pair in _loadedAssets)
            {
                if (pair.Value != null)
                {
                    UnityEngine.AddressableAssets.Addressables.Release(pair.Value);
                }
            }
            _loadedAssets.Clear();
        }
    
        public bool IsAssetLoaded(string address)
        {
            return _loadedAssets.ContainsKey(address);
        }
    }
}
