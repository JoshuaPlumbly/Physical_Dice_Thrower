using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private Die _die;
    [SerializeField] private Vector3 _lauchVelocity;

    private void Awake()
    {
        _die = GameObject.Instantiate(_die);
        _die.gameObject.SetActive(false);
    }

    public void RollDie(System.Action<int> callback)
    {
        _die.transform.position = transform.position;
        _die.transform.rotation = Random.rotation;
        _die.Rigidbody.AddForce(_lauchVelocity, ForceMode.VelocityChange);
        _die.gameObject.SetActive(true);
        _die.EvaluateWhenStopped(callback);
    }
}
