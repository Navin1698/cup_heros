using System.Collections.Generic;
using UnityEngine;

namespace OrbRaiders.Core
{
    public class ObjectPool<T> where T : Component
    {
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly Queue<T> poolQueue = new Queue<T>();

        public ObjectPool(T prefab, int initialSize, Transform parentTransform = null)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;

            for (int i = 0; i < initialSize; i++)
            {
                T instance = Object.Instantiate(prefab, parentTransform);
                instance.gameObject.SetActive(false);
                poolQueue.Enqueue(instance);
            }
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T instance;
            if (poolQueue.Count > 0)
            {
                instance = poolQueue.Dequeue();
            }
            else
            {
                instance = Object.Instantiate(prefab, parentTransform);
            }

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void ReturnToPool(T instance)
        {
            instance.gameObject.SetActive(false);
            poolQueue.Enqueue(instance);
        }

        public void Clear()
        {
            while (poolQueue.Count > 0)
            {
                T instance = poolQueue.Dequeue();
                if (instance != null)
                {
                    Object.Destroy(instance.gameObject);
                }
            }
        }
    }

    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        private readonly Dictionary<string, object> genericPools = new Dictionary<string, object>();
        private readonly Dictionary<GameObject, GameObject> prefabLookup = new Dictionary<GameObject, GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public ObjectPool<T> GetOrCreatePool<T>(T prefab, int initialCount = 10) where T : Component
        {
            string key = prefab.name;
            if (!genericPools.TryGetValue(key, out object pool))
            {
                GameObject poolParent = new GameObject($"Pool_{key}");
                poolParent.transform.SetParent(transform);
                var newPool = new ObjectPool<T>(prefab, initialCount, poolParent.transform);
                genericPools[key] = newPool;
                return newPool;
            }
            return (ObjectPool<T>)pool;
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            var pool = GetOrCreatePool(prefab);
            T instance = pool.Get(position, rotation);
            prefabLookup[instance.gameObject] = prefab.gameObject;
            return instance;
        }

        public void Despawn<T>(T instance, T prefab) where T : Component
        {
            if (instance == null) return;
            var pool = GetOrCreatePool(prefab);
            pool.ReturnToPool(instance);
        }
    }
}
