using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public abstract void AssignToPool(ObjectPool objectPool);
}