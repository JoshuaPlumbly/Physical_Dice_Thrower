using UnityEngine;
using Plumbly.Dice;

public class DiceThrower : ProjectileLancher<Die>
{
    public enum LaunchTorqueMode {LaunchTorqueA, LaunchTorqueB, Random_Between_A_And_B };

    [SerializeField] private float _waitTimeForDiceToStop = 0.02f;

    [Header("Launch Torque Force")]
    [SerializeField] private LaunchTorqueMode _launchTorqueMode = LaunchTorqueMode.Random_Between_A_And_B;
    [SerializeField] private Vector3 _launchTorqueA = Vector3.zero;
    [SerializeField] private Vector3 _launchTorqueB = Vector3.zero;

    [Header("Launch Angle")]
    [SerializeField] private float _randomSpreadRadius;

    private void Awake()
    {
        _projectile = GameObject.Instantiate(_projectile);
        _projectile.gameObject.SetActive(false);

        _projectile.transform.position = _launchPoint.position;
        _projectile.transform.rotation = _launchPoint.rotation;
    }

    public void RollDie(System.Action<int> callback)
    {
        Vector3 launchForce = SpreadFire(transform.forward, _randomSpreadRadius);

        Vector3 launchTorque = 
            _launchTorqueMode == LaunchTorqueMode.LaunchTorqueA ? _launchTorqueA :
            _launchTorqueMode == LaunchTorqueMode.LaunchTorqueB ? _launchTorqueB : 
            RandomRotationBetween(_launchTorqueA, _launchTorqueB);

        Launch(_launchPoint.position, Random.rotation, launchForce, launchTorque);

        if (!_projectile.gameObject.activeSelf)
            Debug.LogWarning($"{_projectile.name} is not enabled.");
        else
            StartCoroutine(_projectile.EvaluateWhenStoppedCoroutine(callback, _waitTimeForDiceToStop));
    }

    public Vector3 RandomRotationBetween(Vector3 vectorA, Vector3 vectorB)
    {
        return new Vector3(
            Mathf.Lerp(vectorA.x, vectorB.x, Random.Range(0f, 1f)),
            Mathf.Lerp(vectorA.y, vectorB.y, Random.Range(0f, 1f)),
            Mathf.Lerp(vectorA.z, vectorB.z, Random.Range(0f, 1f)));
    }
}