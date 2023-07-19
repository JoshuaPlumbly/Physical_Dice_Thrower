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

    public class Coin : RigidbodyProjectile
    {
        public SideOfCoin Evaluate(float edgeAngleRev = 0)
        {
            var dot = Vector3.Dot(transform.up, Vector3.up);

            if (dot > edgeAngleRev)
                return SideOfCoin.Upside;
            else if (dot < -edgeAngleRev)
                return SideOfCoin.Downside;
            else
                return SideOfCoin.Edge;
        }

        public void EvaluateWhenStopped(System.Action<SideOfCoin> callback, float waitTimeForDiceToStop)
        {
            StartCoroutine(EvaluateWhenStoppedCoroutine(callback, waitTimeForDiceToStop));
        }

        public IEnumerator EvaluateWhenStoppedCoroutine(System.Action<SideOfCoin> callback, float waitTimeForDiceToStop)
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
    }
}