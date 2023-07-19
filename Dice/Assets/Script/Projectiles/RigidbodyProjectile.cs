using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class RigidbodyProjectile : MonoBehaviour, IProjectile
{
    public Rigidbody _rigidbody;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 position, Quaternion rotation, Vector3 launchVelocity, Vector3 launchTorque)
    {
        _rigidbody.position = position;
        _rigidbody.rotation = rotation;
        gameObject.SetActive(true);
        _rigidbody.AddForce(launchVelocity, ForceMode.VelocityChange);
        _rigidbody.AddTorque(launchTorque, ForceMode.VelocityChange);
    }
}
