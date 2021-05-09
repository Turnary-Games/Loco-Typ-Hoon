using UnityEngine;

public abstract class HealthScript : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth = 50;
    public bool skipSendMessageWhenNonPositiveDamage = true;
    public bool skipSendMessageWhenNonPositiveHeal = true;

    /// <param name="damage">Damage to deal. Negative numbers are ignored.</param>
    public virtual void DealDamage(int damage)
    {
        if (damage > 0 || !skipSendMessageWhenNonPositiveDamage)
        {
            if (damage > 0)
            {
                currentHealth = Mathf.Max(0, currentHealth - damage);
            }
            SendMessageUpwards(nameof(IOnDamagedEvent.OnDamaged), new DamagedEvent
            {
                damage = damage,
                currentHealth = currentHealth,
                maxHealth = maxHealth,
            }, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <param name="damage">Heal amount to apply. Negative numbers are ignored.</param>
    public virtual void ApplyHealing(int heal)
    {
        if (heal > 0 || !skipSendMessageWhenNonPositiveHeal)
        {
            if (heal > 0)
            {
                currentHealth = Mathf.Min(maxHealth, currentHealth + heal);
            }
            SendMessageUpwards(nameof(IOnHealedEvent.OnHealed), new HealedEvent
            {
                heal = heal,
                currentHealth = currentHealth,
                maxHealth = maxHealth,
            }, SendMessageOptions.DontRequireReceiver);
        }
    }

    public struct DamagedEvent
    {
        public int maxHealth;
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
        public int currentHealth;
        public int heal;
    }

    public interface IOnHealedEvent
    {
        void OnHealed(HealedEvent data);
    }
}
