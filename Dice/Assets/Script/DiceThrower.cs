using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceThrower : MonoBehaviour
{
    public enum LaunchTorqueMode {A,B, Random_Between_A_And_B };

    [SerializeField] private Die _die;

    [Header("Launch Force")]
    [SerializeField] private float _launchForce = 0;
    [SerializeField] private ForceMode _launchForceMode = ForceMode.VelocityChange;

    [Header("Launch Torque Force")]
    [SerializeField] private Vector3 _launchTorqueForceA = Vector3.zero;
    [SerializeField] private Vector3 _launchTorqueForceB = Vector3.zero;
    [SerializeField] private LaunchTorqueMode _launchTorqueMode = LaunchTorqueMode.Random_Between_A_And_B;
    [SerializeField] private ForceMode _launchTorqueForceMode = ForceMode.VelocityChange;

    [Header("Launch Angle")]
    [SerializeField] private float _randomSpreadRadius;

    private void Awake()
    {
        _die = GameObject.Instantiate(_die);
        _die.gameObject.SetActive(false);
    }

    public void RollDie(System.Action<int> callback)
    {
        Vector3 launchDirection = GenerateLaunchDirection(transform.forward, _randomSpreadRadius);

        Vector3 launchTorque = 
            _launchTorqueMode == LaunchTorqueMode.A ? _launchTorqueForceA :
            _launchTorqueMode == LaunchTorqueMode.B ? _launchTorqueForceB : 
            GenerateRandomVector3(_launchTorqueForceA, _launchTorqueForceB);

        _die.transform.position = transform.position;
        _die.transform.rotation = Random.rotation;
        _die.gameObject.SetActive(true);
        _die.Rigidbody.AddForce(launchDirection * _launchForce, _launchForceMode);
        _die.Rigidbody.AddTorque(launchTorque, _launchTorqueForceMode);
        _die.EvaluateWhenStopped(callback);
    }

    public Vector3 GenerateLaunchDirection(Vector3 direction, float spreadRadius)
    {
        direction.x += Random.Range(-spreadRadius, spreadRadius);
        direction.y += Random.Range(-spreadRadius, spreadRadius);
        direction.z += 1f;
        direction.Normalize();
        return direction;
    }

    public Vector3 GenerateRandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Lerp(min.x, max.x, Random.Range(0f, 1f)),
            Mathf.Lerp(min.y, max.y, Random.Range(0f, 1f)),
            Mathf.Lerp(min.z, max.z, Random.Range(0f, 1f)));
    }
}
