using UnityEngine;
using UnityEngine.Serialization;

public class DisableBehaviourWhenBelowHealth : MonoBehaviour, HealthScript.IOnDamagedEvent, HealthScript.IOnHealedEvent
{
    public int enabledWhenAbove = 0;
    public bool inverted = false;

    [FormerlySerializedAs("target")]
    public Behaviour targetBehaviour;
    public GameObject targetGameObject;
    public ParticleSystem targetParticleSystem;

    public void OnDamaged(HealthScript.DamagedEvent data)
    {
        SetTargetBehaviourEnabled(data.currentHealth > enabledWhenAbove);
    }

    public void OnHealed(HealthScript.HealedEvent data)
    {
        SetTargetBehaviourEnabled(data.currentHealth > enabledWhenAbove);
    }

    void SetTargetBehaviourEnabled(bool enabled)
    {
        if (inverted)
        {
            enabled = !enabled;
        }

        if (targetBehaviour)
        {
            targetBehaviour.enabled = enabled;
        }

        if (targetGameObject)
        {
            targetGameObject.SetActive(enabled);
        }

        if (targetParticleSystem)
        {
            var emission = targetParticleSystem.emission;
            emission.enabled = enabled;
        }
    }
}
