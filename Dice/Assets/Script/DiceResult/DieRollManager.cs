using Plumbly.Dice;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DieRollManager : MonoBehaviour
{
    [SerializeField] DieInt _sixSidedDiePrefab;
    [SerializeField] Coin _coinPrefab;
    [SerializeField] float _launchVelcity;
    [SerializeField] Text _resultText;

    private ObjectPool _sixSidedDie;
    private ObjectPool _coin;

    public static event Action DespawnSpawnedAssets;

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
        void ShowResult(int result)
        {
            _resultText.text = result.ToString();
        }

        _resultText.text = "";
        CastNewDie(ShowResult);
    }

    public void FlipCoint()
    {
        void ShowResult(SideOfCoin result)
        {
            _resultText.text = result.ToString();
        }

        _resultText.text = "";
        FlipNewCoin(ShowResult);
    }

    public void ClearAssets()
    {
        DespawnSpawnedAssets?.Invoke();
    }
}