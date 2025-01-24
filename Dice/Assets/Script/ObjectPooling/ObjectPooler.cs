using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler
{
    public static Dictionary<string, Component> _poolLookup = new Dictionary<string, Component>();
    public static Dictionary<string, Queue<Component>> _poolDictionary = new Dictionary<string, Queue<Component>>();

    public static void EnqueueObject<T>(T item, string key) where T : Component
    {
        if (!item.gameObject.activeSelf) 
            return;

        item.transform.position = Vector3.zero;
        _poolDictionary[key].Enqueue(item);
        item.gameObject.SetActive(false);
    }

    public static T DequeueObject<T>(string key)where T : Component
    {
        if (_poolDictionary[key].TryDequeue(out var item))
        {
            return (T)item;
        }

        return (T)EnqueueNewInstance(_poolLookup[key], key);
    }

    public static T EnqueueNewInstance<T>(T item, string key) where T : Component
    {
        T newInstance = Object.Instantiate(item);
        newInstance.gameObject.SetActive(false);
        newInstance.transform.position = Vector3.zero;
        _poolDictionary[key].Enqueue(newInstance);
        return newInstance;
    }

    public static void SetupPool<T>(T pooledItemPrefab, int startingPooSize, string dictionryKey) where T : Component
    {
        _poolDictionary.Add(dictionryKey, new Queue<Component>());
        _poolLookup.Add(dictionryKey, pooledItemPrefab);
        var pool = _poolDictionary[dictionryKey];

        for (int i = 0; i < startingPooSize; i++)
        {
            T newInstance = Object.Instantiate(pooledItemPrefab);
            newInstance.gameObject.SetActive(false);
            newInstance.transform.position = Vector3.zero;
            pool.Enqueue(newInstance);
        }
    }
}
