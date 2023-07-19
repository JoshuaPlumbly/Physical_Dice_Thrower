using UnityEngine;

public abstract class ProjectileLancher<T> : MonoBehaviour where T : IProjectile
{
    [SerializeField] protected T _projectile;
    [SerializeField] protected Transform _launchPoint;
    [SerializeField] protected Vector3 _launchForce;
    [SerializeField] protected Vector3 _launchTorque;

    public void Launch()
    {
        Launch(_launchPoint.position, _launchPoint.rotation, _launchForce, _launchTorque);
    }

    public void Launch(Vector3 position, Quaternion rotation, Vector3 launchForce, Vector3 torque)
    {
        if (_projectile == null)
            return;

        _projectile.Launch(position, rotation, launchForce, torque);
    }

    public static Vector3 SpreadFire(Vector3 direction, float spreadRadius)
    {
        direction.x += Random.Range(-spreadRadius, spreadRadius);
        direction.y += Random.Range(-spreadRadius, spreadRadius);
        direction.z += 1f;
        direction.Normalize();
        return direction;
    }
}