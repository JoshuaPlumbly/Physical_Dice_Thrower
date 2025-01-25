using System;
using UnityEngine;

namespace Plumbly.Dice
{
    public class DieCaster : MonoBehaviour
    {
        [SerializeField] DieInt _sixSidedDie;
        [SerializeField] float _launchVelcity;

        public void CastDie(Action<int> callback)
        {
            Vector3 launchForce = transform.forward * _launchVelcity;
            _sixSidedDie.gameObject.SetActive(true);
            _sixSidedDie.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _sixSidedDie.Cast(launchForce, callback);
        }
    }
}