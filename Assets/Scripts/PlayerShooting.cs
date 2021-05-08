using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;

    public LineRenderer fireRay;

    public Transform fireFrom;

    public Animator cannonAnimator;

    public Animator harpoonAnimator;

    public float projectileSpeed = 15;

    public float reloadTime = 10.0f;

    private bool right = true;

public void Update()
    {
        if (!FlippingCannon() && !Shooting())
            fireRay.enabled = true;
        else
            fireRay.enabled = false;
    }

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
        else if (!FlippingCannon() && !Shooting())
        {
            var clone = Instantiate(projectilePrefab, fireFrom.position, fireFrom.rotation);
            var body = clone.GetComponentInChildren<Rigidbody>();
            harpoonAnimator.Play("Base Layer.Shoot", 0, 0.0f);
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

public void Flip()
    {
        if (!FlippingCannon())
        {
            if (right)
            {
                right = false;
                cannonAnimator.Play("Base Layer.Rotate Left", 0);
            }
            else
            {
                right = true;
                cannonAnimator.Play("Base Layer.Rotate Right", 0);
            }
        }
    }

bool FlippingCannon()
    {
        return cannonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1;
    }

bool Shooting()
    {
        return harpoonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1;
    }

}