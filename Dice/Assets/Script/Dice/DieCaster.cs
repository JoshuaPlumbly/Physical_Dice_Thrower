using System;
using UnityEngine;

namespace Plumbly.Dice
{
    public class DieCaster : MonoBehaviour
    {
        [SerializeField] DieInt _die;
        [SerializeField] float _launchVelcity;

        public void CastDie(Action<int> callback)
        {
            Vector3 launchForce = transform.forward * _launchVelcity;
            _die.gameObject.SetActive(true);
            _die.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _die.Cast(launchForce, callback);
        }
    }
}