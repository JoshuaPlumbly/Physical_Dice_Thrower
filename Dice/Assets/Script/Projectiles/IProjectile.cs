using UnityEngine;

public interface IProjectile
{
    public void Launch(Vector3 position, Quaternion rotation, Vector3 launchVelocity, Vector3 launchTorque);
}
