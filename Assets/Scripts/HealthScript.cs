using UnityEngine;

public abstract class HealthScript : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth = 50;
    public bool skipSendMessageWhenNonPositiveDamage = true;
    public bool skipSendMessageWhenNonPositiveHeal = true;

    public virtual void Start()
    {
        // just to force inspector to have an "enabled" checkbox
    }

    /// <param name="damage">Damage to deal. Negative numbers are ignored.</param>
    public virtual void DealDamage(int damage)
    {
        if (!enabled)
        {
            return;
        }

        if (damage > 0 || !skipSendMessageWhenNonPositiveDamage)
        {
            var previousHealth = currentHealth;
            if (damage > 0)
            {
                currentHealth = Mathf.Max(0, currentHealth - damage);
            }
            SendMessageUpwards(nameof(IOnDamagedEvent.OnDamaged), new DamagedEvent
            {
                damage = damage,
                previousHealth = previousHealth,
                currentHealth = currentHealth,
                maxHealth = maxHealth,
            }, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <param name="damage">Heal amount to apply. Negative numbers are ignored.</param>
    public virtual void ApplyHealing(int heal)
    {
        if (!enabled)
        {
            return;
        }

        if (heal > 0 || !skipSendMessageWhenNonPositiveHeal)
        {
            var previousHealth = currentHealth;
            if (heal > 0)
            {
                currentHealth = Mathf.Min(maxHealth, currentHealth + heal);
            }
            SendMessageUpwards(nameof(IOnHealedEvent.OnHealed), new HealedEvent
            {
                heal = heal,
                previousHealth = previousHealth,
                currentHealth = currentHealth,
                maxHealth = maxHealth,
            }, SendMessageOptions.DontRequireReceiver);
        }
    }

    public struct DamagedEvent
    {
        public int maxHealth;
        public int previousHealth;
        public int currentHealth;
        public int damage;
    }

    public interface IOnDamagedEvent
    {
        void OnDamaged(DamagedEvent data);
    }


    public struct HealedEvent
    {
        public int maxHealth;
        public int previousHealth;
        public int currentHealth;
        public int heal;
    }

    public interface IOnHealedEvent
    {
        void OnHealed(HealedEvent data);
    }
}
