using System.Collections;
using UnityEngine;

namespace Plumbly.Dice
{
    public class Die : RigidbodyProjectile
    {
        [System.Serializable]
        public struct DiceFace
        {
            public Vector3 DotVector;
            public int FaceValue;
        }

        [SerializeField] private DiceFace[] _faces;

        private int Evaluate()
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

        public bool EvaluateWhenStopped(System.Action<int> callback, float waitTimeForDiceToStop)
        {
            if (!enabled)
                return false;

            StartCoroutine(EvaluateWhenStoppedCoroutine(callback, waitTimeForDiceToStop));
            return true;
        }

        public IEnumerator EvaluateWhenStoppedCoroutine(System.Action<int> callback, float waitTimeForDiceToStop)
        {
            var oldPosition = transform.position;
            var oldRotation = transform.rotation;
            var stillFor = 0f;
            var finished = false;

            while (!finished)
            {
                if (Vector3.Distance(oldPosition, transform.position) < 0.01f && Quaternion.Angle(transform.rotation, oldRotation) < 0.01f)
                {
                    stillFor += Time.deltaTime;

                    if (stillFor >= waitTimeForDiceToStop)
                    {
                        callback(Evaluate());
                        finished = true;
                    }
                }
                else
                {
                    stillFor = 0f;
                }

                oldPosition = transform.position;
                oldRotation = transform.rotation;
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < _faces.Length; i++)
            {
                var worldSpace = transform.localToWorldMatrix.MultiplyVector(_faces[i].DotVector);
                Gizmos.DrawLine(transform.position, transform.position + worldSpace);
            }
        }
    }
}