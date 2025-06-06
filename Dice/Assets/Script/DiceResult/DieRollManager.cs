using Plumbly.Dice;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DieRollManager : MonoBehaviour
{

    [SerializeField] DieInt _sixSidedDiePrefab;
    [SerializeField] Coin _coinPrefab;
    [SerializeField] float _launchVelcity;

    private ObjectPool _sixSidedDie;
    private ObjectPool _coin;

    public static event Action DespawnSpawnedAssets;
    public Action<string> OnResultFinished;

    private void Awake()
    {
        _sixSidedDie = ObjectPool.SetupPool(_sixSidedDiePrefab, 10, "sixSidedDie");
        _coin = ObjectPool.SetupPool(_coinPrefab, 10, "coin");
    }

    public void CastNewDie(Action<int> callback)
    {
        Vector3 launchForce = transform.forward * _launchVelcity;
        DieInt newDie = (DieInt)_sixSidedDie.DequeueObject();
        newDie.gameObject.SetActive(true);
        newDie.transform.SetPositionAndRotation(transform.position, transform.rotation);
        newDie.Cast(launchForce, callback);
    }

    public void FlipNewCoin(Action<SideOfCoin> callback)
    {
        Vector3 launchForce = transform.forward * _launchVelcity;
        Coin newDie = (Coin)_coin.DequeueObject();
        newDie.gameObject.SetActive(true);
        newDie.transform.SetPositionAndRotation(transform.position, transform.rotation);
        newDie.Cast(launchForce, callback);
    }

    public void RollDice()
    {
        void Callback(int result)
        {
            OnResultFinished?.Invoke(result.ToString());
        }

        CastNewDie(Callback);
    }

    public void FlipCoint()
    {
        void Callback(SideOfCoin result)
        {
            OnResultFinished?.Invoke(result.ToString());
        }

        FlipNewCoin(Callback);
    }

    public void ClearAssets()
    {
        DespawnSpawnedAssets?.Invoke();
    }
}