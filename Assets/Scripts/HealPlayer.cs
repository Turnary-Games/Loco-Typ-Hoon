using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    [Range(0, 50)]
    public int heal = 5;

    public void OnTriggerEnter(Collider collider)
    {
        var body = collider.attachedRigidbody;
        if (!body)
        {
            return;
        }

        var health = body.GetComponentInParent<PlayerCartHealth>();
        if (!health)
        {
            return;
        }

        health.ApplyHealing(heal);
    }
}
