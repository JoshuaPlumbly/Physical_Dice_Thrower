using System;
using System.Collections;
using UnityEngine;

namespace Plumbly.Dice
{
    public enum SideOfCoin
    {
        Upside,
        Downside,
        Edge
    }

    public class Coin : PoolableObject, IDie<SideOfCoin>
    {
        [SerializeField] private float _edgeAngle;
        [SerializeField] private float _waitTimeForDieToStop = 1f;

        private Rigidbody _rigidbody;
        private ObjectPool _pool;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            DieRollManager.DespawnSpawnedAssets += Despawn;
        }

        private void OnDestroy()
        {
            DieRollManager.DespawnSpawnedAssets -= Despawn;
        }

        public override void AssignToPool(ObjectPool objectPool)
        {
            _pool = objectPool;
        }

        private void Despawn()
        {
            if (_pool != null)
            {
                _pool.EnqueueObject(this);
                return;
            }

            GameObject.Destroy(gameObject);
        }

        public void Cast(Vector3 launchVelocity, Action<SideOfCoin> callback)
        {
            _rigidbody.AddForce(launchVelocity, ForceMode.VelocityChange);
            transform.rotation = UnityEngine.Random.rotation;
            StartCoroutine(CastCoroutine(callback));
        }

        public IEnumerator CastCoroutine(Action<SideOfCoin> callback)
        {
            Vector3 oldPosition = transform.position;
            Quaternion oldRotation = transform.rotation;
            float timeStillFor = 0f;
            bool isFinishedMoving = false;

            while (!isFinishedMoving)
            {
                if (Vector3.Distance(oldPosition, transform.position) < 0.01f && Quaternion.Angle(transform.rotation, oldRotation) < 0.01f)
                {
                    timeStillFor += Time.deltaTime;

                    if (timeStillFor >= _waitTimeForDieToStop)
                    {
                        callback(GetUpwardsFace());
                        isFinishedMoving = true;
                    }
                }
                else
                {
                    timeStillFor = 0f;
                }

                oldPosition = transform.position;
                oldRotation = transform.rotation;
                yield return null;
            }
        }

        private SideOfCoin GetUpwardsFace()
        {
            var dot = Vector3.Dot(transform.up, Vector3.up);

            if (dot > _edgeAngle)
                return SideOfCoin.Upside;
            else if (dot < -_edgeAngle)
                return SideOfCoin.Downside;
            else
                return SideOfCoin.Edge;
        }
    }
}