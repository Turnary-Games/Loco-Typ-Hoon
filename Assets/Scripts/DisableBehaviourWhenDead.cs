using UnityEngine;

public class DisableBehaviourWhenDead : MonoBehaviour, HealthScript.IOnDamagedEvent, HealthScript.IOnHealedEvent
{
    public Behaviour target;
    public bool disableWhenDied = true;
    public bool enableWhenResurrected = true;

    public void OnDamaged(HealthScript.DamagedEvent data)
    {
        if (data.currentHealth == 0 && disableWhenDied)
        {
            SetTargetBehaviourEnabled(false);
        }
    }

    public void OnHealed(HealthScript.HealedEvent data)
    {
        if (data.previousHealth == 0 && data.currentHealth > 0 && enableWhenResurrected)
        {
            SetTargetBehaviourEnabled(true);
        }
    }

    void SetTargetBehaviourEnabled(bool enabled)
    {
        if (!target)
        {
            Debug.LogWarning($"The '{nameof(target)}' must be set.", this);
            return;
        }

        target.enabled = enabled;
    }
}
