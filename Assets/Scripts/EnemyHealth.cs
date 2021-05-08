using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth = 50;

    /// <param name="damage">Damage to deal to enemy. Negative numbers are ignored.</param>
    public void DealDamage(int damage)
    {
        if (damage > 0)
        {
            currentHealth -= damage;
        }
    }
}
