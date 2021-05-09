using UnityEngine;

public class DisableBehaviourWhenDead : MonoBehaviour, HealthScript.IOnDamagedEvent
{
    public Behaviour target;

    public void OnDamaged(HealthScript.DamagedEvent data)
    {
        if (data.currentHealth == 0)
        {
            SetTargetBehaviourEnabled(false);
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
