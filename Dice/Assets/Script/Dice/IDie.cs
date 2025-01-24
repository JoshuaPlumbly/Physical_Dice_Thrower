using System;
using System.Collections;
using UnityEngine;

namespace Plumbly.Dice
{
    public interface IDie<T>
    {
        public void Cast(Vector3 force, Action<T> callback);
    }
}