using UnityEngine;

namespace Plumbly.Dice
{
    [System.Serializable]
    public struct DiceFace<T>
    {
        public Vector3 DotVector;
        public T FaceValue;
    }
}