using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [Range(0, 50)]
    public int damage = 5;

    public bool hasDealtDamage;
    public bool canOnlyDealDamageOnce = true;

    public string playerTag = "Player";

    public void OnTriggerEnter(Collider collider)
    {
        if (hasDealtDamage && canOnlyDealDamageOnce)
        {
            return;
        }

        if (!collider.attachedRigidbody)
        {
            return;
        }

        var body = collider.attachedRigidbody;
        if (!body.CompareTag(playerTag))
        {
            return;
        }

        var health = body.GetComponentInParent<PlayerCartHealth>();
        if (!health)
        {
            Debug.LogWarning("Could not find player health script to deal damage to.", this);
            return;
        }

        health.DealDamage(damage);

        hasDealtDamage = true;
    }
}
