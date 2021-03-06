﻿using SCore.Framework;
using SCore.SceneLoading;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SCore.ObjectPool
{
    /// <summary>
    /// Static class provided service for ObjectPool pattern realisation and control
    /// Extend GameObjects methods
    /// When we return object in pool we need reinit all components with ILoadableContent Clear realisation
    /// </summary>
    public sealed class ObjectPool : MonoBehaviourSingleton<ObjectPool>
    {
        //TODO replace singletone by zenject
        [System.Serializable]
        public class StartupPool
        {
            public int size;
            public GameObject prefab;
        }

        //DEPENDENCIES

        [Inject] private DiContainer _container;
        [Inject] private ISceneLoadingHandler _sceneLoadingHandler;

        //PUBLIC STATIC
        public enum StartupPoolMode { Awake, Start, CallManually };

        private static ObjectPool _instance;
        private static List<GameObject> tempList = new List<GameObject>();

        //PUBLIC EVENTS

        //PUBLIC VARIABLES
        public StartupPoolMode startupPoolMode;

        public StartupPool[] startupPools;
        public GameObject[] ReturnInPoolOnSceneLoading;

        //PRIVATE STATIC

        //PRIVATE VARIABLES
        private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

        private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
        private bool startupPoolsCreated;

        private void Awake()
        {
            _instance = this;
            if (startupPoolMode == StartupPoolMode.Awake)
                CreateStartupPools();
        }

        private void Start()
        {
            if (startupPoolMode == StartupPoolMode.Start)
                CreateStartupPools();
            if (_sceneLoadingHandler != null )
                _sceneLoadingHandler.LoadBeginEvent += OnLoadingSceneBegin;
        }

        private void OnLoadingSceneBegin()
        {
            if (ReturnInPoolOnSceneLoading != null && ReturnInPoolOnSceneLoading.Length > 0)
            {
                for (int i = 0; i < ReturnInPoolOnSceneLoading.Length; i++)
                {
                    if (ReturnInPoolOnSceneLoading[i] != null)
                        RecycleAll(ReturnInPoolOnSceneLoading[i]);
                }
            }
        }

        public static void CreateStartupPools()
        {
            if (!instance.startupPoolsCreated)
            {
                instance.startupPoolsCreated = true;
                var pools = instance.startupPools;
                if (pools != null && pools.Length > 0)
                    for (int i = 0; i < pools.Length; ++i)
                        CreatePool(pools[i].prefab, pools[i].size);
            }
        }

        public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
        {
            if (prefab != null)
                CreatePool(prefab.gameObject, initialPoolSize);
            else
                Debug.LogError("ObjectPool:CreatePool prefab is null!");
        }

        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            if (prefab != null && !instance.pooledObjects.ContainsKey(prefab))
            {
                var list = new List<GameObject>();
                instance.pooledObjects.Add(prefab, list);

                if (initialPoolSize > 0)
                {
                    bool active = prefab.activeSelf;
                    prefab.SetActive(false);
                    Transform parent = instance.transform;
                    while (list.Count < initialPoolSize)
                    {
                        GameObject obj = Instance._container.InstantiatePrefab(prefab);
                        obj.transform.SetParent(parent, false);
                        list.Add(obj);
                    }
                    prefab.SetActive(active);
                }
            }
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            if (prefab != null)
            {
                List<GameObject> list;
                Transform trans;
                GameObject obj;
                if (instance.pooledObjects.TryGetValue(prefab, out list))
                {
                    obj = null;
                    if (list.Count > 0)
                    {
                        while (obj == null && list.Count > 0)
                        {
                            obj = list[0];
                            list.RemoveAt(0);
                        }
                        if (obj != null)
                        {
                            trans = obj.transform;
                            trans.SetParent(parent, false);
                            trans.localPosition = position;
                            trans.localRotation = rotation;
                            obj.SetActive(true);
                            instance.spawnedObjects.Add(obj, prefab);
                            return obj;
                        }
                    }
                    obj = NewObject(prefab, parent, position, rotation);
                    return obj;
                }
                else
                {
                    CreatePool(prefab, 1);
                    obj = NewObject(prefab, parent, position, rotation);
                    return obj;
                }
            }
            else
            {
                Debug.LogError("ObjectPool: Prefab is null!");
                return null;
            }
        }

        public static GameObject NewObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            Transform trans;
            GameObject obj = null;

            if (prefab != null)
            {
                obj = Instance._container.InstantiatePrefab(prefab);
                trans = obj.transform;
                trans.SetParent(parent, false);
                trans.localPosition = position;
                trans.localRotation = rotation;
                instance.spawnedObjects.Add(obj, prefab);
            }
            else
            {
                Debug.LogError("ObjectPool:NewObject Prefab is null!");
            }

            return obj;
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj) where T : Component
        {
            if (obj != null)
                Recycle(obj.gameObject);
            else
                Debug.LogWarning("ObjectPool:Recycle obj or GameObject is null!");
        }

        public static void Recycle(GameObject obj)
        {
            if (obj != null)
            {
                GameObject prefab;
                if (instance.spawnedObjects.TryGetValue(obj, out prefab))
                    Recycle(obj, prefab);
            }
            else
            {
                Debug.LogWarning("ObjectPool:Recycle obj or GameObject is null!");
            }
        }

        private static void Recycle(GameObject obj, GameObject prefab)
        {
            if (obj != null && prefab != null)
            {
                instance.pooledObjects[prefab].Add(obj);
                instance.spawnedObjects.Remove(obj);

                //Call all IObjectPoolReusable to ClearForReuse
                IObjectPoolReusable[] allReusableComponents = obj.GetComponents<IObjectPoolReusable>();
                foreach (IObjectPoolReusable reusableComponent in allReusableComponents)
                {
                    reusableComponent.ClearForReuse();
                }
                obj.transform.SetParent(instance.transform, false);
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("ObjectPool:Recycle obj or GameObject is null!");
            }
        }

        public static void RecycleAll<T>(T prefab) where T : Component
        {
            RecycleAll(prefab.gameObject);
        }

        public static void RecycleAll(GameObject prefab)
        {
            foreach (var item in instance.spawnedObjects)
                if (item.Value == prefab)
                    tempList.Add(item.Key);
            for (int i = 0; i < tempList.Count; ++i)
                Recycle(tempList[i]);
            tempList.Clear();
        }

        public static void RecycleAll()
        {
            tempList.AddRange(instance.spawnedObjects.Keys);
            for (int i = 0; i < tempList.Count; ++i)
                Recycle(tempList[i]);
            tempList.Clear();
        }

        public static bool IsSpawned(GameObject obj)
        {
            return instance.spawnedObjects.ContainsKey(obj);
        }

        public static int CountPooled<T>(T prefab) where T : Component
        {
            return CountPooled(prefab.gameObject);
        }

        public static int CountPooled(GameObject prefab)
        {
            List<GameObject> list;
            if (instance.pooledObjects.TryGetValue(prefab, out list))
                return list.Count;
            return 0;
        }

        public static int CountSpawned<T>(T prefab) where T : Component
        {
            return CountSpawned(prefab.gameObject);
        }

        public static int CountSpawned(GameObject prefab)
        {
            int count = 0;
            foreach (var instancePrefab in instance.spawnedObjects.Values)
                if (prefab == instancePrefab)
                    ++count;
            return count;
        }

        public static int CountAllPooled()
        {
            int count = 0;
            foreach (var list in instance.pooledObjects.Values)
                count += list.Count;
            return count;
        }

        public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
                list = new List<GameObject>();
            if (!appendList)
                list.Clear();
            List<GameObject> pooled;
            if (instance.pooledObjects.TryGetValue(prefab, out pooled))
                list.AddRange(pooled);
            return list;
        }

        public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
                list = new List<T>();
            if (!appendList)
                list.Clear();
            List<GameObject> pooled;
            if (instance.pooledObjects.TryGetValue(prefab.gameObject, out pooled))
                for (int i = 0; i < pooled.Count; ++i)
                    list.Add(pooled[i].GetComponent<T>());
            return list;
        }

        public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
                list = new List<GameObject>();
            if (!appendList)
                list.Clear();
            foreach (var item in instance.spawnedObjects)
                if (item.Value == prefab)
                    list.Add(item.Key);
            return list;
        }

        public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
                list = new List<T>();
            if (!appendList)
                list.Clear();
            var prefabObj = prefab.gameObject;
            foreach (var item in instance.spawnedObjects)
                if (item.Value == prefabObj)
                    list.Add(item.Key.GetComponent<T>());
            return list;
        }

        public static void DestroyPooled(GameObject prefab)
        {
            List<GameObject> pooled;
            if (instance.pooledObjects.TryGetValue(prefab, out pooled))
            {
                for (int i = 0; i < pooled.Count; ++i)
                    GameObject.Destroy(pooled[i]);
                pooled.Clear();
            }
        }

        public static void DestroyPooled<T>(T prefab) where T : Component
        {
            DestroyPooled(prefab.gameObject);
        }

        public static void DestroyAll(GameObject prefab)
        {
            RecycleAll(prefab);
            DestroyPooled(prefab);
        }

        public static void DestroyAll<T>(T prefab) where T : Component
        {
            DestroyAll(prefab.gameObject);
        }

        public static ObjectPool instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = Object.FindObjectOfType<ObjectPool>();
                if (_instance != null)
                    return _instance;

                var obj = new GameObject("ObjectPool");
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                _instance = obj.AddComponent<ObjectPool>();
                return _instance;
            }
        }
    }

    public static class ObjectPoolExtensions
    {
        public static void CreatePool<T>(this T prefab) where T : Component
        {
            ObjectPool.CreatePool(prefab, 0);
        }

        public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
        {
            ObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static void CreatePool(this GameObject prefab)
        {
            ObjectPool.CreatePool(prefab, 0);
        }

        public static void CreatePool(this GameObject prefab, int initialPoolSize)
        {
            ObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, position, rotation);
        }

        public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, position, rotation);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab, Transform parent) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, parent, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab)
        {
            return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(this T obj) where T : Component
        {
            ObjectPool.Recycle(obj);
        }

        public static void Recycle(this GameObject obj)
        {
            ObjectPool.Recycle(obj);
        }

        public static void RecycleAll<T>(this T prefab) where T : Component
        {
            ObjectPool.RecycleAll(prefab);
        }

        public static void RecycleAll(this GameObject prefab)
        {
            ObjectPool.RecycleAll(prefab);
        }

        public static int CountPooled<T>(this T prefab) where T : Component
        {
            return ObjectPool.CountPooled(prefab);
        }

        public static int CountPooled(this GameObject prefab)
        {
            return ObjectPool.CountPooled(prefab);
        }

        public static int CountSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPool.CountSpawned(prefab);
        }

        public static int CountSpawned(this GameObject prefab)
        {
            return ObjectPool.CountSpawned(prefab);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPool.GetSpawned(prefab, list, appendList);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPool.GetSpawned(prefab, list, false);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab)
        {
            return ObjectPool.GetSpawned(prefab, null, false);
        }

        public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, list, appendList);
        }

        public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, list, false);
        }

        public static List<T> GetSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, null, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPool.GetPooled(prefab, list, appendList);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPool.GetPooled(prefab, list, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab)
        {
            return ObjectPool.GetPooled(prefab, null, false);
        }

        public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPool.GetPooled(prefab, list, appendList);
        }

        public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPool.GetPooled(prefab, list, false);
        }

        public static List<T> GetPooled<T>(this T prefab) where T : Component
        {
            return ObjectPool.GetPooled(prefab, null, false);
        }

        public static void DestroyPooled(this GameObject prefab)
        {
            ObjectPool.DestroyPooled(prefab);
        }

        public static void DestroyPooled<T>(this T prefab) where T : Component
        {
            ObjectPool.DestroyPooled(prefab.gameObject);
        }

        public static void DestroyAll(this GameObject prefab)
        {
            ObjectPool.DestroyAll(prefab);
        }

        public static void DestroyAll<T>(this T prefab) where T : Component
        {
            ObjectPool.DestroyAll(prefab.gameObject);
        }
    }
}