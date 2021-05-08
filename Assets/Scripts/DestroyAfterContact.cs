using UnityEngine;

public class DestroyAfterContact : MonoBehaviour
{
    public bool onCollision = true;
    public bool onTrigger = false;

    public void OnCollisionEnter(Collision _)
    {
        if (onCollision)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider _)
    {
        if (onTrigger)
        {
            Destroy(gameObject);
        }
    }
}
