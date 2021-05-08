using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;

    public Transform fireFrom;

    public float projectileSpeed = 15;


public void Fire()
    {
        if (!fireFrom)
        {
            Debug.LogWarning("No transform to fire from.", this);
        }
        else if (!projectilePrefab)
        {
            Debug.LogWarning("No projectile prefab to clone when firing.", this);
        }
        else
        {
            var clone = Instantiate(projectilePrefab, fireFrom.position, fireFrom.rotation);
            var body = clone.GetComponentInChildren<Rigidbody>();
            if (!body)
            {
                Debug.LogWarning("Projectile was spawned, but could not set its velocity because didnt find its Rigidbody.", this);
            }
            else
            {
                body.velocity = fireFrom.forward * projectileSpeed;
            }
        }
    }

}
