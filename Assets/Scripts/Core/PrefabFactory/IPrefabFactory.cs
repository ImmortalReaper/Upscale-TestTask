using UnityEngine;

namespace Core.PrefabFactory
{
    public interface IPrefabFactory
    {
        public GameObject Create(GameObject prefab);
        public GameObject Create(GameObject prefab, Vector3 position, Transform parent = null);
        public GameObject Create<T>(T prefab) where T : Component;
        public GameObject Create<T>(T prefab, Vector3 position, Transform parent = null) where T : Component;
        public GameObject Create(string prefabName);
        public GameObject Create(string prefabName, Vector3 position, Transform parent = null);
    }
}
