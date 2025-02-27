﻿using System;
using System.Collections;
using UnityEngine;

namespace Plumbly.Dice
{
    public class Die<T> : PoolableObject, IDie<T>
    {
        [SerializeField] private DiceFace<T>[] _faces;
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

        public void Cast(Vector3 launchVelocity, Action<T> callback)
        {
            _rigidbody.AddForce(launchVelocity, ForceMode.VelocityChange);
            transform.rotation = UnityEngine.Random.rotation;
            StartCoroutine(CastCoroutine(callback));
        }

        public IEnumerator CastCoroutine(Action<T> callback)
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

        private T GetUpwardsFace()
        {
            var bestDot = -1f;
            var bestFaceIndex = 0;

            for (int i = 0; i < _faces.Length; i++)
            {
                var dotToCheck = _faces[i].DotVector;
                var dotToCheckWorldSpace = transform.localToWorldMatrix.MultiplyVector(dotToCheck);
                var dot = Vector3.Dot(dotToCheckWorldSpace, Vector3.up);

                if (dot > bestDot)
                {
                    bestDot = dot;
                    bestFaceIndex = i;
                }
            }

            return _faces[bestFaceIndex].FaceValue;
        }
    }
}