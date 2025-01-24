using System;
using UnityEngine;

namespace Plumbly.Dice
{
    public class DieCaster : MonoBehaviour
    {
        [SerializeField] DieInt _sixSidedDiePrefab;
        [SerializeField] float _launchVelcity;

        private void Awake()
        {
            ObjectPooler.SetupPool(_sixSidedDiePrefab, 10, "sixSidedDie");
        }

        public void CastNewDie(Action<int> callback)
        {
            Vector3 launchForce = transform.forward * _launchVelcity;
            DieInt newDie = ObjectPooler.DequeueObject<DieInt>("sixSidedDie");
            newDie.gameObject.SetActive(true);
            newDie.transform.SetPositionAndRotation(transform.position, transform.rotation);
            newDie.Cast(launchForce, callback);
        }
    }
}