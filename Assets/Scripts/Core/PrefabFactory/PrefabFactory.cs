using Core.AssetLoader;
using UnityEngine;
using Zenject;

namespace Core.PrefabFactory
{
    public class PrefabsFactory : IPrefabFactory
    {
        private IAddressablesAssetLoaderService _addressablesAssetLoaderService;
        private DiContainer _diContainer;

        public PrefabsFactory(IAddressablesAssetLoaderService addressablesAssetLoaderService, DiContainer container)
        {
            _addressablesAssetLoaderService = addressablesAssetLoaderService;
            _diContainer = container;
        }

        public GameObject Create(GameObject prefab)
        {
            return _diContainer.InstantiatePrefab(prefab);
        }
        public GameObject Create(GameObject prefab, Vector3 position, Transform parent = null)
        {
            return _diContainer.InstantiatePrefab(prefab);
        }
        public GameObject Create<T>(T prefab) where T : Component
        {
            return _diContainer.InstantiatePrefab(prefab);
        }
        public GameObject Create<T>(T prefab, Vector3 position, Transform parent = null) where T : Component
        {
            return _diContainer.InstantiatePrefab(prefab);
        }
        public GameObject Create(string prefabName)
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab);
        }
        public GameObject Create(string prefabName, Vector3 position, Transform parent = null)
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, parent);
        }
    }
}
