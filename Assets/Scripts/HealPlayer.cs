using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    [Range(0, 50)]
    public int heal = 5;

    public void Start()
    {
        // Having this method forces the inspector to show "enabled" checkmark
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!enabled)
        {
            return;
        }

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
