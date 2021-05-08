using UnityEngine;

public class PlayerCartHealth : MonoBehaviour
{
    public int currentHealth = 50;

    /// <param name="damage">Damage to deal to player. Negative numbers are ignored.</param>
    public void DealDamage(int damage)
    {
        currentHealth -= Mathf.Max(0, damage);
    }
}
