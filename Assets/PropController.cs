using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : DamageDealer
{
    public string playerTag = "Player";
    
    public Animator propAnimator;
    
    public void OnTriggerEnter(Collider collider)
    {
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

        DealDamage(health, damage);
        propAnimator.Play("Base Layer.Remove", 0, 0.0f);

    }
}
