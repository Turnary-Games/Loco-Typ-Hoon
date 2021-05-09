using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;

    public AudioSource fireAudioSource;

    public LineRenderer fireRay;

    public Transform fireFrom;

    public Animator cannonAnimator;

    public Animator harpoonAnimator;

    public float projectileSpeed = 15;

    public float reloadTime = 10.0f;

    public bool cannonIsFlippedToTheLeft;

    public void Update()
    {
        UpdateFireRayEnabled();
    }

    public void OnEnable()
    {
        UpdateFireRayEnabled();
    }

    public void OnDisable()
    {
        UpdateFireRayEnabled();
    }

    public void Fire()
    {
        if (!enabled)
        {
            return;
        }

        if (!fireFrom)
        {
            Debug.LogWarning("No transform to fire from.", this);
        }
        else if (!projectilePrefab)
        {
            Debug.LogWarning("No projectile prefab to clone when firing.", this);
        }
        else if (!IsFlippingCannon() && !IsShooting())
        {
            var clone = Instantiate(projectilePrefab, fireFrom.position, fireFrom.rotation);
            var body = clone.GetComponentInChildren<Rigidbody>();
            harpoonAnimator.Play("Base Layer.Shoot", 0, 0.0f);
            fireAudioSource.Play();
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
        if (!enabled)
        {
            return;
        }

        if (!IsFlippingCannon())
        {
            if (cannonIsFlippedToTheLeft)
            {
                cannonIsFlippedToTheLeft = false;
                cannonAnimator.Play("Base Layer.Rotate Left", 0);
                Debug.Log("Flip to the left");
            }
            else
            {
                cannonIsFlippedToTheLeft = true;
                cannonAnimator.Play("Base Layer.Rotate Right", 0);
                Debug.Log("Flip to the right");
            }
        }
    }

    void UpdateFireRayEnabled()
    {
        fireRay.enabled = enabled && !IsFlippingCannon() && !IsShooting();
    }

    bool IsFlippingCannon()
    {
        return cannonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1;
    }

    bool IsShooting()
    {
        return harpoonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1;
    }
}
