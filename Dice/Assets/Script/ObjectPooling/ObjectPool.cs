using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public static Dictionary<string, ObjectPool> _existingPools = new Dictionary<string, ObjectPool>();

    private PoolableObject _prefab;
    private Queue<PoolableObject> _available = new Queue<PoolableObject>();

    public ObjectPool(PoolableObject prefab)
    {
        _prefab = prefab;
        _available = new Queue<PoolableObject>();
    }

    public void EnqueueObject(PoolableObject item)
    {
        if (!item.gameObject.activeSelf)
            return;

        item.transform.position = Vector3.zero;
        _available.Enqueue(item);
        item.gameObject.SetActive(false);
    }

    public PoolableObject DequeueObject()
    {
        if (_available.TryDequeue(out var item))
        {
            return (PoolableObject)item;
        }

        return CreateNewEnqueuedInstance();
    }

    public PoolableObject CreateNewEnqueuedInstance()
    {
        PoolableObject newInstance = CreateNewInstance();
        _available.Enqueue(newInstance);
        newInstance.AssignToPool(this);
        return newInstance;
    }

    public PoolableObject CreateNewInstance()
    {
        PoolableObject newInstance = Object.Instantiate(_prefab);
        newInstance.gameObject.SetActive(false);
        newInstance.transform.position = Vector3.zero;
        return newInstance;
    }

    public void CreateNewEnqueuedInstances(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewEnqueuedInstance();
        }
    }

    public static ObjectPool SetupPool(PoolableObject pooledItemPrefab, int startingPooSize, string dictionryKey)
    {
        if (_existingPools.TryGetValue(dictionryKey, out ObjectPool pool))
        {
            return pool;
        }

        ObjectPool newObjectPool = new ObjectPool(pooledItemPrefab);
        _existingPools.Add(dictionryKey, newObjectPool);
        newObjectPool.CreateNewEnqueuedInstances(startingPooSize);

        return newObjectPool;
    }
}