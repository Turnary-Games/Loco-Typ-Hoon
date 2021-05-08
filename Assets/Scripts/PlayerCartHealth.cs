using UnityEngine;

public class PlayerCartHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth = 50;

    /// <param name="damage">Damage to deal to player. Negative numbers are ignored.</param>
    public void DealDamage(int damage)
    {
        if (damage > 0)
        {
            currentHealth -= damage;
            SendMessage(nameof(IOnDamagedEvent.OnDamaged), new DamagedEvent
            {
                damage = damage,
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
}
