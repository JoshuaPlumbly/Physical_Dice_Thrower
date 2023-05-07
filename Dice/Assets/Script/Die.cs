using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    [System.Serializable]
    public struct DiceFace
    {
        public Vector3 DotVector;
        public int FaceValue;
    }

    [SerializeField] private DiceFace[] faces;
    [SerializeField] private float waitForDiceToStopFor = 0.5f;

    private IEnumerator evaluateWhenStoppedCoroutine;

    public int LastResult { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private int Evaluate()
    {
        var bestDot = -1f;
        var bestFaceIndex = 0;

        for (int i = 0; i < faces.Length; i++)
        {
            var dotToCheck = faces[i].DotVector;
            var dotToCheckWorldSpace = transform.localToWorldMatrix.MultiplyVector(dotToCheck);
            var dot = Vector3.Dot(dotToCheckWorldSpace, Vector3.up);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestFaceIndex = i;
            }
        }

        LastResult = faces[bestFaceIndex].FaceValue;
        return LastResult;
    }

    public void EvaluateWhenStopped(System.Action<int> callback)
    {
        if (evaluateWhenStoppedCoroutine != null)
            StopCoroutine(evaluateWhenStoppedCoroutine);

        evaluateWhenStoppedCoroutine = EvaluateWhenStoppedCoroutine(callback);
        StartCoroutine(evaluateWhenStoppedCoroutine);
    }

    public IEnumerator EvaluateWhenStoppedCoroutine(System.Action<int> callback)
    {
        var oldPosition = transform.position;
        var oldRotation = transform.rotation;
        var stillFor = 0f;
        var finished = false;
        
        while(!finished)
        {
            if (Vector3.Distance(oldPosition, transform.position) < 0.01f && Quaternion.Angle(transform.rotation, oldRotation) < 0.01f)
            {
                stillFor += Time.deltaTime;

                if (stillFor >= waitForDiceToStopFor)
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

        for (int i = 0; i < faces.Length; i++)
        {
            var worldSpace = transform.localToWorldMatrix.MultiplyVector(faces[i].DotVector);
            Gizmos.DrawLine(transform.position, transform.position + worldSpace);
        }
    }
}
