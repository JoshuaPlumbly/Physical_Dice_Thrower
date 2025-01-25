using System.Collections.Generic;
using UnityEngine;

public class ComponentPool<T> where T : Component
{
    public static Dictionary<string, ComponentPool<T>> _existingPools = new Dictionary<string, ComponentPool<T>>();

    private T _prefab;
    private Queue<T> _available = new Queue<T>();

    public ComponentPool(T prefab)
    {
        _prefab = prefab;
        _available = new Queue<T>();
    }

    public void EnqueueObject(T item)
    {
        if (!item.gameObject.activeSelf)
            return;

        item.transform.position = Vector3.zero;
        _available.Enqueue(item);
        item.gameObject.SetActive(false);
    }

    public T DequeueObject()
    {
        if (_available.TryDequeue(out var item))
        {
            return (T)item;
        }

        return EnqueueNewInstance(_prefab);
    }

    public T EnqueueNewInstance(T item)
    {
        T newInstance = Object.Instantiate(item);
        newInstance.gameObject.SetActive(false);
        newInstance.transform.position = Vector3.zero;
        _available.Enqueue(newInstance);
        return newInstance;
    }

    public static ComponentPool<T> SetupPool(T pooledItemPrefab, int startingPooSize, string dictionryKey)
    {
        if (_existingPools.TryGetValue(dictionryKey, out ComponentPool<T> pool))
        {
            return pool;
        }

        ComponentPool<T> newObjectPool = new ComponentPool<T>(pooledItemPrefab);
        _existingPools.Add(dictionryKey, newObjectPool);

        for (int i = 0; i < startingPooSize; i++)
        {
            T newInstance = Object.Instantiate(pooledItemPrefab);
            newInstance.gameObject.SetActive(false);
            newInstance.transform.position = Vector3.zero;
            newObjectPool.EnqueueObject(newInstance);
        }

        return newObjectPool;
    }
}